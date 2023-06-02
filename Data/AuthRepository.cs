using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace dotnet_rpg.Data
{
    public class AuthRepository : IAuthRepository
    {
        private readonly DataContext _context;

        public AuthRepository(DataContext context)
        {
            _context = context;
        }

        public Task<ServiceResponse<string>> Login(string username, string password)
        {
            throw new NotImplementedException();
        }

        public async Task<ServiceResponse<int>> Register(User user, string password)
        {
            var response = new ServiceResponse<int>();
            if (await UserExists(user.Username))
            {
                response.Success = false;
                response.Message = "User Already Exists";
                return response;
            }
            CreatPasswordHash(password, out byte[] passwordHass, out byte[] passwordSalt);

            user.PasswordHash = passwordHass;
            user.PasswordSalt = passwordSalt;

            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            response.Data = user.Id;
            return response;
        }

        public async Task<bool> UserExists(string username)
        {
            if (await _context.Users.AnyAsync(u => u.Username.ToLower() == username.ToLower()))
            {
                return true;
            }
            return false;
        }

        private void CreatPasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
        {
            using (var hmac = new System.Security.Cryptography.HMACSHA512())
            {
                passwordSalt = hmac.Key;
                passwordHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
            }
        }

        // public async Task<ServiceResponse<bool>> ChangePassword(string username, string oldPassword, string newPassword)
        // {
        //     var user = await _context.Users.FirstOrDefaultAsync(u => u.Username == username);
        //     if (user == null)
        //     {
        //         return new ServiceResponse<bool> { Success = false, Message = "Invalid username." };
        //     }

        //     // Kiểm tra mật khẩu cũ
        //     if (!VerifyPasswordHash(oldPassword, user.PasswordHash, user.PasswordSalt))
        //     {
        //         return new ServiceResponse<bool> { Success = false, Message = "Incorrect old password." };
        //     }

        //     // Tạo mới salt và hash cho mật khẩu mới
        //     CreatePasswordHash(newPassword, out byte[] newPasswordHash, out byte[] newPasswordSalt);

        //     // Cập nhật mật khẩu mới cho user
        //     user.PasswordHash = newPasswordHash;
        //     user.PasswordSalt = newPasswordSalt;

        //     // Lưu thay đổi vào cơ sở dữ liệu
        //     await _context.SaveChangesAsync();

        //     return new ServiceResponse<bool> { Success = true, Data = true, Message = "Password changed successfully." };
        // }

        // private bool VerifyPasswordHash(string password, byte[] storedHash, byte[] storedSalt)
        // {
        //     using (var hmac = new System.Security.Cryptography.HMACSHA512(storedSalt))
        //     {
        //         var computedHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
        //         for (int i = 0; i < computedHash.Length; i++)
        //         {
        //             if (computedHash[i] != storedHash[i])
        //             {
        //                 return false;
        //             }
        //         }
        //         return true;
        //     }
        // }

        // private void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
        // {
        //     using (var hmac = new System.Security.Cryptography.HMACSHA512())
        //     {
        //         passwordSalt = hmac.Key;
        //         passwordHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
        //     }
        // }

    }
}