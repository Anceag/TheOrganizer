﻿using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using TheOrganizer.Entities;
using TheOrganizer.Helpers;
using TheOrganizer.Model;

namespace TheOrganizer.Services
{
    public class UserService : IUserService
    {
        private readonly AppSettings _appSettings;
        private readonly TheOrganizerDBContext _db;

        public UserService(IOptions<AppSettings> appSettings, TheOrganizerDBContext db)
        {
            _appSettings = appSettings.Value;
            _db = db;
        }

        public UserEntity Authenticate(string email, string password)
        {
            var user = _db.Users.FirstOrDefault(x => x.Email == email && x.Password == password);

            // return null if user not found
            if (user == null)
                return null;

            var userEntity = new UserEntity
            {
                Id = user.Id,
                Name = user.Name,
                Email = user.Email,
                Password = user.Password,
            };

            // authentication successful so generate jwt token
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_appSettings.Secret);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.Name, userEntity.Id.ToString())
                }),
                Expires = DateTime.UtcNow.AddDays(7),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            userEntity.Token = tokenHandler.WriteToken(token);

            // remove password before returning
            userEntity.Password = null;

            return userEntity;
        }

        public IEnumerable<User> GetAll()
        {
            // return users without passwords
            return _db.Users
            .AsEnumerable()
            .Select(u => {
                u.Password = null;
                return u;
            });
        }

        public User GetByEmail(string email)
        {
            var user = _db.Users.FirstOrDefault(u => u.Email == email);

            // return user without password
            if (user != null)
                user.Password = null;

            return user;
        }
    }
}
