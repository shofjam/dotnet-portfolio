using WebApi.Entities;
using WebApi.Helpers;
using WebApi.Models.Users;

using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using bCrypt = BCrypt.Net.BCrypt;
using System.Net;

namespace WebApi.Services
{
    public interface IUserService
    {
        IEnumerable<User> GetAll(string? keyword);
        User GetById(int id);
        void Create(CreateModel payload);
        void Update(User oldData, UpdateModel newData);
        IActionResult ChangePassword(User data, ChangePasswordModel payload);
        void Delete(int id);
    }

    public class UserService : IUserService
    {
        private DataContext _context;
        private readonly IMapper _mapper;

        public UserService(
            DataContext context,
            IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public IEnumerable<User> GetAll(string? keyword)
        {
            if (!string.IsNullOrWhiteSpace(keyword))
            {
                return _context.Users.Where(x => x.FirstName.ToLower().Contains(keyword.ToLower().Trim()) || x.LastName.ToLower().Contains(keyword.ToLower().Trim())).ToList();
            }
            else
            {
                return _context.Users;
            }
        }

        public User GetById(int id)
        {
            return getUser(id);
        }

        public void Create(CreateModel model)
        {
            // validate
            if (_context.Users.Any(x => x.Email == model.Email))
                throw new AppException("User with the email '" + model.Email + "' already exists", true);

            // map model to new user object
            var user = _mapper.Map<User>(model);

            // hash password
            user.PasswordHash = bCrypt.HashPassword(model.Password);

            // save user
            _context.Users.Add(user);
            _context.SaveChanges();
        }

        public void Update(User oldData, UpdateModel newData)
        {
            // validate
            if (newData.Email != oldData.Email && _context.Users.Any(x => x.Email == newData.Email))
                throw new AppException("User with the email '" + newData.Email + "' already exists");

            // copy model to user and save
            _mapper.Map(newData, oldData);
            _context.Users.Update(oldData);
            _context.SaveChanges();
        }

        public IActionResult ChangePassword(User data, ChangePasswordModel payload)
        {
            try
            {
                data.PasswordHash = bCrypt.ValidateAndReplacePassword(payload.OldPassword, data.PasswordHash, payload.NewPassword);
            }
            catch
            {
                return new BadRequestObjectResult(new { message = "Invalid Password" });
            }

            _context.Users.Update(data);
            _context.SaveChanges();

            return new OkObjectResult(new { message = "Password Changed" });
        }

        public void Delete(int id)
        {
            var user = getUser(id);
            _context.Users.Remove(user);
            _context.SaveChanges();
        }

        // helper methods

        private User getUser(int id)
        {
            var user = _context.Users.Find(id);
            if (user == null) throw new KeyNotFoundException("User not found");
            return user;
        }
    }
}
