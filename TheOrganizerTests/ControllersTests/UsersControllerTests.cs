﻿using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text;
using TheOrganizer.Controllers;
using TheOrganizer.Entities;
using TheOrganizer.Model;
using TheOrganizer.Services;
using TheOrganizerTests.TestServices;
using Xunit;

namespace TheOrganizerTests.ControllersTests
{
    public class UsersControllerTests
    {
        private readonly IUserService _userService;
        private readonly UsersController _controller;

        public UsersControllerTests()
        {
            _userService = new TestUserServices();
            _controller = new UsersController(_userService);
        }

        [Fact]
        public void Authenticate()
        {
            var user = new User
            {
                Email = "aaa@aaa.aaa",
                Password = "password",
            };

            var result = _controller.Authenticate(user) as ObjectResult;

            Assert.True(result != null, "result is null");
            Assert.True(result.StatusCode == 200, "Status code is not OK");
            Assert.True((result.Value as UserEntity).Token == "token", "Token is not valid");
        }

        [Fact]
        public void GetAll()
        {
            var result = _controller.GetAll() as ObjectResult;

            Assert.True(result != null, "result is null");
            Assert.True(result.StatusCode == 200, "Status code is not OK");
            Assert.True((result.Value as List<User>).Count == 3, "List count is not 3");
            Assert.True((result.Value as List<User>)[0].Name == "Name1", "Incorrect name of user at index 0");
        }

        [Fact]
        public void GetUser()
        {
            var result = _controller.GetUser("Email2") as ObjectResult;

            Assert.True(result != null, "result is null");
            Assert.True(result.StatusCode == 200, "Status code is not OK");
            Assert.True((result.Value as User).Name == "Name2", "Incorrect name of user");
            Assert.True((result.Value as User).Password == "Password2", "Incorrect password of user");
        }

        [Fact]
        public void AddUser()
        {
            var user = new User
            {
                Name = "Name",
                Email = "Email",
                Password = "Password",
            };

            var result = _controller.AddUser(user) as ObjectResult;

            Assert.True(result != null, "result is null");
            Assert.True(result.StatusCode == 200, "Status code is not OK");
            Assert.True((result.Value as UserEntity).Token == "token", "Incorrect token of user");
        }


    }
}
