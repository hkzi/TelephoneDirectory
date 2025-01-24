using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using TelephoneDirectory.Business.Abstract;
using TelephoneDirectory.Core.Entities.Concrete;
using TelephoneDirectory.Core.Utilities.Results;
using TelephoneDirectory.DataAccess.Abstract;
using TelephoneDirectory.DataAccess.Concrete;
using TelephoneDirectory.Entities.DTOs;

using Microsoft.AspNet.Identity;

namespace TelephoneDirectory.Business.Concrete
{
    public class UserManager : IUserService
    {
        IUserDal _userDal;
        IUserOperationClaimDal _userOperationClaimDal;
        private PhoneContext _context;


        public UserManager(IUserDal userDal, IUserOperationClaimDal userOperationClaimDal, PhoneContext context)
        {
            _userDal = userDal;
            _userOperationClaimDal = userOperationClaimDal;
            _context = context;
        }

        public List<OperationClaim> GetClaims(User user)
        {
            return _userDal.GetClaims(user);
        }

        public User Add(User user)
        {
            return _userDal.Add(user);
        }



        public User Update(User user)
        {
            return _userDal.Update(user);
        }

        public User Delete(User user)
        {
            return _userDal.Delete(user);
        }

        public User GetByMail(string email)
        {
            return _userDal.Get(u => u.Email == email);
        }
    
        public Task<bool> ChangePasswordAsync(string? identityName, string modelOldPassword, string modelNewPassword)
        {
            throw new NotImplementedException();
        }

        public async Task UpdateUserPasswordAsync(UserForLoginDto userDto)
        {
            // Kullanıcıyı e-posta adresi ile bul
            var users = await _userDal.GetAllAsync(u => u.Email == userDto.Email); // Asenkron olarak kullanıcıyı al
            var existingUser = users.FirstOrDefault(); // İlk kullanıcıyı seç

            if (existingUser == null)
            {
                throw new KeyNotFoundException("Kullanıcı bulunamadı.");
            }

            // Şifreyi hash'le ve salt oluştur
            var (passwordHash, passwordSalt) = HashPassword(userDto.Password);

            // Veritabanındaki kullanıcıyı güncelle
            existingUser.PasswordHash = passwordHash;
            existingUser.PasswordSalt = passwordSalt;

            // Güncellenmiş veriyi kaydet
            await _userDal.UpdateAsync(existingUser);


        }

        private (byte[] PasswordHash, byte[] PasswordSalt) HashPassword(string password)
        {
            using (var hmac = new HMACSHA512())
            {
                byte[] salt = hmac.Key; // Salt oluştur
                byte[] hash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password)); // Şifreyi hash'le
                return (hash, salt);
            }
        }


        public async Task<UserForLoginDto> GetUserByEmailAsync(string email)
        {
            return await _userDal.GetUserByEmailAsync(email);
        }

        public List<User> GetAll()
        {
            return _userDal.GetAll();
        }

        public List<User> GetById(int id)
        {
            return _userDal.GetAll(u => u.Id == id);

        }

        public User GetUserWithClaims(string email)
        {
            return _userDal.GetUserWithClaims(email);
        }



    }
}