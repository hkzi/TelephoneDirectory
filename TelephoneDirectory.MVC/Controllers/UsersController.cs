using Microsoft.AspNetCore.Mvc;
using TelephoneDirectory.Business.Abstract;
using TelephoneDirectory.Entities.DTOs;
using TelephoneDirectory.MVC.Models;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using System.Net.Mail;
using System.Net;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Text;
using Microsoft.AspNetCore.Authorization;
using TelephoneDirectory.Core.Entities.Concrete;
using X.PagedList.Extensions;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Linq;
using Mono.TextTemplating;
using System.Drawing.Printing;


namespace TelephoneDirectory.MVC.Controllers
{
    [Authorize(AuthenticationSchemes = "Login")]
    [Authorize(Roles = "Moderator")]
    public class UsersController : Controller
    {
        private readonly IAuthService _authService;
        private readonly IUserService _userService;
        private readonly IUserOperationClaimService _userOperationClaimService;
        private IOperationClaimService _operationClaimService;
        private readonly ILogger<UsersController> _logger;
        private IConfiguration _configuration;
        public UsersController(IAuthService authService, IUserService userService, ILogger<UsersController> logger, IConfiguration configuration, IUserOperationClaimService userOperationClaimService, IOperationClaimService operationClaimService)
        {
            _authService = authService;
            _userService = userService;
            _logger = logger;
            _configuration = configuration;
            _userOperationClaimService = userOperationClaimService;
            _operationClaimService = operationClaimService;
        }
        public IActionResult Index()
        {
            return View();
        }
        [HttpGet]
        public IActionResult Index(int page = 1, int pageSize = 4)
        {
            // Kullanıcıları alıyoruz
            var users = _authService.GetAll();

            if (users == null || !users.Any())
            {
                // Kullanıcı bulunamadığında bir hata mesajıyla yönlendirme yapılabilir
                ViewBag.Message = "Kullanıcılar bulunamadı.";
                return View(Enumerable.Empty<ListUserViewRegisterModel>().ToPagedList(page, pageSize));
            }

            var userList = users.Select(user =>
            {
                var operationClaims = _userService.GetClaims(user); // Kullanıcının rollerini alıyoruz
                var roller = string.Join(", ", operationClaims.Select(c => c.Name)); // Rolleri virgülle birleştiriyoruz

                return new ListUserViewRegisterModel
                {
                    Id = user.Id,
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    Email = user.Email,
                    RolString = roller, // Rolleri birleştirilmiş string olarak döner
                    SelectedRoles = operationClaims.Select(c => new UserRoleViewModel // Roller detaylı döner
                    {
                        Id = c.Id,
                        Name = c.Name
                    }).ToList()
                };
            }).ToList(); // Listeye dönüştürüyoruz

            // Kullanıcı listesini sayfaya göre ayırıyoruz
            var pagedUserList = userList.ToPagedList(page, pageSize);

            return View(pagedUserList);
        }



        public IActionResult Add()
        {

            return View();

        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Add(User users)
        {
            if (ModelState.IsValid)
            {
                _userService.Add(users);
                return RedirectToAction("Index");
            }

            return View();

        }




        [AllowAnonymous]
        public IActionResult AccessDenied()
        {
            return View("Login");
        }
        [AllowAnonymous]
        public IActionResult Login()
        {
            // Login için boş bir UserListViewModel instance'ı oluşturuyoruz
            var loginViewModel = new UserListViewLoginModel
            {
                UserForLoginDtos = new UserForLoginDto() // Boş bir liste başlatıyoruz
            };
            return View(loginViewModel);

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [AllowAnonymous]
        public async Task<IActionResult> Login(UserListViewLoginModel loginViewModel)
        {

            if (string.IsNullOrEmpty(loginViewModel.UserForLoginDtos.Email) ||
          string.IsNullOrEmpty(loginViewModel.UserForLoginDtos.Password))
            {
                ModelState.AddModelError(string.Empty, "Email ve şifre gereklidir.");
                return View(loginViewModel);
            }

            var user = _authService.Login(loginViewModel.UserForLoginDtos.Email, loginViewModel.UserForLoginDtos.Password);
            if (user == null || !user.Success || user.Data == null)
            {
                _logger.LogWarning($"Login başarısız: Email: {loginViewModel.UserForLoginDtos.Email}");
                ModelState.AddModelError(string.Empty, "Geçersiz kullanıcı adı veya şifre.");
                return View(loginViewModel);
            }
            _logger.LogInformation($"User data: {user.Data}");
            var claims = new List<Claim>
    {
        new(ClaimTypes.Name, user.Data.Email),
        new(ClaimTypes.NameIdentifier, user.Data.Id.ToString())
    };
            if (user.Data.UserOperationClaims != null && user.Data.UserOperationClaims.Any())
            {
                var roles = user.Data.UserOperationClaims
                    .Where(uoc => !string.IsNullOrEmpty(uoc.OperationClaim.Name))
                    .Select(uoc => uoc.OperationClaim.Name);

                claims.AddRange(roles.Select(role => new Claim(ClaimTypes.Role, role)));
            }


            var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            var principal = new ClaimsPrincipal(identity);

            await HttpContext.SignInAsync("Login", principal);

            var tokenHandler = new JwtSecurityTokenHandler();
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["TokenOptions:SecurityKey"] ?? string.Empty));
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.AddHours(1),
                Issuer = _configuration["TokenOptions:Issuer"],
                Audience = _configuration["TokenOptions:Audience"],
                SigningCredentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            var jwtToken = tokenHandler.WriteToken(token);

            //return Ok(new
            //{
            //    Message = "Giriş başarılı!",
            //    Token = jwtToken,
            //    UserName = user.Data.Email,
            //    UserId = user.Data.Id,
            //    UserAuthenticated = User.Identity?.Name,
            //    IsModerator = User.IsInRole("Moderator")
            //});
            return RedirectToAction("Index", "Persons");
        }



        public IActionResult Register()
        {
            // Null kontrolü ekledik
            var registerViewModel = new UserListViewRegisterModel
            {
                UserForRegisterDtos = new UserForRegisterDto(),

            };
            return View(registerViewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Register(UserListViewRegisterModel registerViewModel, string sifreTekrarı, bool? kabul)
        {

            if (registerViewModel == null)
            {
                ViewBag.ErrorMessage = "Kullanıcı bilgileri geçerli değil.";
                return View();
            }
            else if (registerViewModel.UserForRegisterDtos == null)
            {
                ViewBag.ErrorMessage = "Kayıt verileri eksik.";
                return View(registerViewModel);
            }

            // Şifrelerin eşleşip eşleşmediğini kontrol ediyoruz
            if (string.IsNullOrEmpty(registerViewModel.UserForRegisterDtos.Password) ||
                string.IsNullOrEmpty(sifreTekrarı))
            {
                ViewBag.PasswordError = "Şifre alanları boş bırakılamaz!";
                return View(registerViewModel);
            }

            if (registerViewModel.UserForRegisterDtos.Password != sifreTekrarı)
            {
                ViewBag.PasswordError = "Şifreler uyuşmuyor!";
                return View(registerViewModel);
            }


            // Şartların kabul edilip edilmediğini kontrol ediyoruz
            if (kabul != true)
            {
                ViewBag.AcceptError = "Lütfen şartları kabul ettiğinizi onaylayın!";
                return View(registerViewModel);
            }

            //  Kullanıcının var olup olmadığını kontrol ediyoruz
            var userExists = _authService.UserExists(registerViewModel.UserForRegisterDtos.Email);
            if (userExists == null || !userExists.Success)
            {
                ViewBag.ErrorMessage = userExists?.Message ?? "Kullanıcı zaten mevcut.";
                return View(registerViewModel);
            }
            //if (!ModelState.IsValid)
            //{
            //    ViewBag.ErrorMessage = "Lütfen tüm alanları doğru şekilde doldurduğunuzdan emin olun.";
            //    return View(registerViewModel);
            //}

            var registerResult = _authService.Register(registerViewModel.UserForRegisterDtos, registerViewModel.UserForRegisterDtos.Password);
            if (registerResult == null || !registerResult.Success)
            {
                ViewBag.ErrorMessage = registerResult?.Message ?? "Kayıt işlemi başarısız.";
                return View(registerViewModel);
            }


            // Erişim tokeni oluşturuyoruz
            var tokenResult = _authService.CreateAccessToken(registerResult.Data);
            if (tokenResult == null || !tokenResult.Success)
            {
                ViewBag.ErrorMessage = tokenResult?.Message ?? "Erişim tokeni oluşturulamadı.";
                return View(registerViewModel);
            }

            //Başarılı giriş sonrası yönlendirme
            TempData["SuccessMessage"] = "Kayıt başarılı! Giriş yapabilirsiniz.";
            return RedirectToAction("Index", "Users");
        }
        [AllowAnonymous]
        public async Task<IActionResult> ForgetMyPassword()
        {
            var resetModel = new UserListViewLoginModel
            {
                UserForLoginDtos = new UserForLoginDto()
            };
            return View(resetModel);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        [AllowAnonymous]
        public async Task<IActionResult> ForgetMyPassword(UserListViewLoginModel resetModel)
        {
            var model = resetModel.UserForLoginDtos;

            if (string.IsNullOrEmpty(model?.Email))
            {
                ModelState.AddModelError("", "Lütfen geçerli bir e-posta adresi giriniz.");
                return View();
            }

            // Kullanıcının e-posta adresini veritabanında arayın
            var user = await _userService.GetUserByEmailAsync(model.Email);

            if (user == null)
            {
                ViewBag.ErrorMessage = "Böyle bir e-mail adresi bulunamadı.";
                return View();
            }

            // Rastgele bir şifre oluşturun ve kullanıcı modelinde güncelleyin
            Guid rastgele = Guid.NewGuid();
            string yeniSifre = rastgele.ToString().Substring(0, 8);
            user.Password = yeniSifre;

            // Şifreyi veritabanında güncelleyin
            await _userService.UpdateUserPasswordAsync(user);

            try
            {
                // E-posta gönderimi
                SmtpClient client = new SmtpClient("smtp.gmail.com", 587)
                {
                    EnableSsl = true,
                    Credentials = new NetworkCredential("halo3449@gmail.com", "diezyfynioyixioe")
                };

                MailMessage mail = new MailMessage
                {
                    From = new MailAddress("halo3449@gmail.com", "Şifre Sıfırlama"),
                    Subject = "Şifre Değiştirme İsteği",
                    IsBodyHtml = true,
                    Body = $"Merhaba {user.Email},<br/> Yeni şifreniz: {yeniSifre}"
                };
                mail.To.Add(user.Email);

                client.Send(mail);
            }
            catch (Exception ex)
            {
                // E-posta gönderim hatası
                ModelState.AddModelError("", "E-posta gönderilirken bir hata oluştu: " + ex.Message);
                return View();
            }

            return RedirectToAction("Login");
        }


        [HttpGet]
        public async Task<IActionResult> ChangePassword(ChangePasswordViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var result = await _userService.ChangePasswordAsync(User.Identity.Name, model.OldPassword, model.NewPassword);

            if (result)
            {
                TempData["Message"] = "Şifreniz başarıyla güncellendi.";
                return RedirectToAction("Index", "Users");
            }

            ModelState.AddModelError("", "Şifre değiştirme işlemi başarısız.");
            return View(model);
        }
        [AllowAnonymous]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync("Login");
            HttpContext.Session.Clear();
            // Çerezler manuel olarak temizlenir
            Response.Cookies.Delete("LoginCookie");
            Response.Cookies.Delete("ASP.NET_SessionId");
            Response.Cookies.Delete(".AspNetCore.Identity.Application");

            return RedirectToAction("Login", "Users");
        }


        public IActionResult AddUserRole(int id)
        {
            var user = _userService.GetById(id).FirstOrDefault();

            if (user == null)
            {
                ViewBag.ErrorMessage = "Kullanıcı bulunamadı.";
                return View();
            }

            var roles = _operationClaimService.GetAll();

            if (roles == null || !roles.Any())
            {
                TempData["ErrorMessage"] = "Rol bulunamadı.";
                return View();
            }

            // Kullanıcı ve rollerin görüntülenmesi için ViewModel hazırlanıyor
            var viewModel = new UserRoleViewModel
            {
                UserId = user.Id,
                UserName = user.FirstName,
                OperationClaim = roles.Select(role => new SelectListItem
                {
                    Value = role.Id.ToString(),
                    Text = role.Name
                }).ToList()
            };

            return View(viewModel);
        }





        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult AddUserRole(UserRoleViewModel viewModel)
        {
            if (viewModel == null || viewModel.UserId <= 0 || viewModel.RoleId <= 0)
            {
                viewModel.OperationClaim = _operationClaimService.GetAll().Select(role => new SelectListItem
                {
                    Value = role.Id.ToString(),
                    Text = role.Name,
                    Selected = role.Id == viewModel.SelectedRoleId
                }).ToList();

                TempData["ErrorMessage"] = "Kullanıcı yada rol seçilmedi.";
                return View(viewModel);
            }

            // Kullanıcının bu role zaten sahip olup olmadığını kontrol et
            var existingUserRole = _userOperationClaimService.GetAll()
                .FirstOrDefault(uoc => uoc.UserId == viewModel.UserId && uoc.OperationClaimId == viewModel.RoleId);

            if (existingUserRole != null)
            {
                TempData["ErrorMessage"] = "Bu kullanıcı bu role sahip.";
                return View(viewModel);
            }

            // Sadece seçilen rol ekleniyor
            var userOperationClaim = new UserOperationClaim
            {
                UserId = viewModel.UserId,
                OperationClaimId = viewModel.RoleId
            };

            try
            {
                _userOperationClaimService.Add(userOperationClaim);
                TempData["SuccessMessage"] = $"Rol başarıyla eklendi.";
            }
            catch (Exception ex)
            {
                ViewBag.ErrorMessage = "Error occurred while assigning the role: " + ex.Message;
                return View(viewModel);
            }

            return RedirectToAction("Index");
        }

        public IActionResult UpdateUser(int id)
        {
            // Kullanıcı bilgilerini getir
            var user = _userService.GetById(id).FirstOrDefault();
            if (user == null)
            {
                ViewBag.ErrorMessage = "Kullanıcı bulunamadı.";
                return RedirectToAction("Index");
            }

            // Tüm rollerin listesini getir
            var roles = _operationClaimService.GetAll();

            // Kullanıcıya atanmış mevcut rolü kontrol et
            var currentUserRole = _userOperationClaimService.GetAll()
                .FirstOrDefault(uoc => uoc.UserId == user.Id);

            // ViewModel oluştur
            var viewModel = new UserRoleViewModel
            {
                UserId = user.Id,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Email = user.Email,
                SelectedRoleId = currentUserRole?.OperationClaimId ?? 0, // Mevcut rol (Varsa)
                OperationClaim = roles.Select(role => new SelectListItem
                {
                    Value = role.Id.ToString(),
                    Text = role.Name,
                    Selected = role.Id == (currentUserRole?.OperationClaimId ?? 0) // Doğru rol işaretlenir
                }).ToList()
            };

            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult UpdateUser(UserRoleViewModel viewModel)
        {
            //if (!ModelState.IsValid)
            //{
            //    ModelState.AddModelError("SelectedRoleId", "Bir rol seçmeniz gerekmektedir.");
            //    viewModel.OperationClaim = _operationClaimService.GetAll().Select(role => new SelectListItem
            //    {
            //        Value = role.Id.ToString(),
            //        Text = role.Name,
            //        Selected = role.Id == viewModel.SelectedRoleId
            //    }).ToList();

            //    TempData["ErrorMessage"] = "Geçersiz bilgiler!";
            //    return View(viewModel);
            //}

            //Eğer SelectedRoleId 0 ise hata ekleyip tekrar göster
            if (viewModel.SelectedRoleId == 0)
            {
                ModelState.AddModelError("SelectedRoleId", "Bir rol seçmeniz gerekmektedir.");
                return View(viewModel);
            }

            try
            {
                // Kullanıcı bilgilerini güncelle
                var user = _userService.GetById(viewModel.UserId).FirstOrDefault();
                if (user == null)
                {
                    TempData["ErrorMessage"] = "Kullanıcı bulunamadı!";
                    return RedirectToAction("Index");
                }

                user.FirstName = viewModel.FirstName;
                user.LastName = viewModel.LastName;
                user.Email = viewModel.Email;

                _userService.Update(user);

                // Kullanıcı rolünü güncelle
                var existingRole = _userOperationClaimService.GetAll()
                    .FirstOrDefault(uoc => uoc.UserId == user.Id);

                if (existingRole != null)
                {
                    // Mevcut rol güncellenir
                    existingRole.OperationClaimId = viewModel.SelectedRoleId;
                    _userOperationClaimService.Update(existingRole);
                }
                TempData["SuccessMessage"] = "Kullanıcı başarıyla güncellendi.";
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                ViewBag.ErrorMessage = "Bir hata oluştu: " + ex.Message;
                return View(viewModel);
            }
        }

        public IActionResult DeleteUserRole(int userId, int roleId)
        {
            try
            {
                // Kullanıcı ve rol eşleşmesini kontrol et
                var userRole = _userOperationClaimService.GetAll()
                    .FirstOrDefault(uoc => uoc.UserId == userId && uoc.OperationClaimId == roleId);

                if (userRole == null)
                {
                    TempData["ErrorMessage"] = "Kullanıcıya atanmış böyle bir rol bulunamadı!";
                    return RedirectToAction("UpdateUser", new { userId = userId });
                }

                // Rolü sil
                _userOperationClaimService.Delete(userRole);

                TempData["SuccessMessage"] = "Rol başarıyla silindi.";
                return RedirectToAction("UpdateUser", new { userId = userId });
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "Bir hata oluştu: " + ex.Message;
                return RedirectToAction("UpdateUser", new { userId = userId });
            }
        }


    }
}
