using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using TelephoneDirectory.Core.DataAccess;
using TelephoneDirectory.Core.Entities.Concrete;
using TelephoneDirectory.DataAccess.Abstract;
using TelephoneDirectory.Entities.DTOs;


namespace TelephoneDirectory.DataAccess.Concrete
{
    public class UserDal : EntityRepositoryBase<User, PhoneContext>, IUserDal
    {
        private readonly PhoneContext _phoneContext;

        public UserDal(PhoneContext phoneContext)
        {
            _phoneContext = phoneContext;
        }

        public List<OperationClaim> GetClaims(User user)
        {

            using (var context = new PhoneContext())
            {
                var result = from operationClaim in context.OperationClaims
                             join userOperationClaim in context.UserOperationClaims
                                 on operationClaim.Id equals userOperationClaim.OperationClaimId
                             where userOperationClaim.UserId == user.Id
                             select new OperationClaim { Id = operationClaim.Id, Name = operationClaim.Name };

                return result.ToList();
            }
        }


        public async Task UpdateUserPasswordAsync(UserForLoginDto userDto)
        {
            // Kullanıcıyı email ile bul
            var existingUser = await _phoneContext.Users
                .FirstOrDefaultAsync(u => u.Email == userDto.Email);

            if (existingUser == null)
            {
                throw new KeyNotFoundException("Kullanıcı bulunamadı.");
            }

            // Şifreyi hash'leyip salt oluştur
            var (passwordHash, passwordSalt) = HashPassword(userDto.Password);

            // Veritabanındaki kullanıcıyı güncelle
            existingUser.PasswordHash = passwordHash;
            existingUser.PasswordSalt = passwordSalt;

            await _phoneContext.SaveChangesAsync();
        }

        private (byte[] PasswordHash, byte[] PasswordSalt) HashPassword(string password)
        {
            // Salt oluşturuluyor


            byte[] saltBytes = new byte[16]; // 16 byte'lık salt
            RandomNumberGenerator.Fill(saltBytes); // Rastgele salt oluştur
            byte[] salt = saltBytes;

            // Şifreyi hash'liyoruz
            using (var sha256 = SHA256.Create())
            {
                byte[] passwordBytes = Encoding.UTF8.GetBytes(password);
                byte[] saltedPassword = Encoding.UTF8.GetBytes(salt + password); // Salt + Password
                byte[] hashBytes = sha256.ComputeHash(saltedPassword); // Hash'liyoruz

                return (hashBytes, saltBytes); // Hash ve Salt'ı byte[] olarak geri döndürüyoruz
            }

        }

        public async Task<UserForLoginDto> GetUserByEmailAsync(string email)
        {
            // Kullanıcıyı veritabanından e-posta adresine göre getirir.
            var user = await _phoneContext.Users
                .Where(u => u.Email == email)
                .Select(u => new UserForLoginDto
                {
                    Email = u.Email
                    // Gerekirse diğer özellikleri de ekleyebilirsiniz
                })
                .FirstOrDefaultAsync();

            if (user == null)
            {
                throw new KeyNotFoundException("Bu e-posta adresine sahip bir kullanıcı bulunamadı.");
            }

            // User nesnesini DTO'ya map ediyoruz.
            return new UserForLoginDto
            {
                Email = user.Email,
                Password = user.Password
                // Diğer alanları da map edebilirsiniz.
            };
        }
        public async Task<List<User>> GetAllAsync(Expression<Func<User, bool>> filter)
        {
            return await _phoneContext.Set<User>().Where(filter).ToListAsync();
        }

        public async Task UpdateAsync(User user)
        {
            _phoneContext.Set<User>().Update(user);
            await _phoneContext.SaveChangesAsync();
        }

        public Task UpdateUserAsync(User user)
        {
            throw new NotImplementedException();
        }
        public User GetUserWithClaims(string email)
        {
            return _phoneContext.Users
                .Include(u => u.UserOperationClaims)
                .ThenInclude(uoc => uoc.OperationClaim)
                .FirstOrDefault(u => u.Email == email);
        }

    }
}

