﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TheOrganizer.Model;
using TheOrganizer.Services;

namespace TheOrganizer.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class ContactsController : Controller
    {
        private IContactServise _contactServise;
        public ContactsController(IContactServise contactServise)
        {
            _contactServise = contactServise;
        }

        [HttpPost("add")]
        public IActionResult AddContact([FromBody] Contact contact)
        {
            int.TryParse(User.Identity.Name, out int userId);
            contact.OwnerId = userId;
            if (_contactServise.AddContact(contact))
                return Ok();
            return BadRequest("There is something wrong with contact info");
        }

        [HttpPut("edit")]
        public IActionResult EditEvent([FromBody] Contact contact)
        {
            int.TryParse(User.Identity.Name, out int userId);
            contact.OwnerId = userId;
            if (_contactServise.EditContact(contact))
                return Ok();
            return BadRequest("There is something wrong with contact info");
        }

        [HttpDelete("delete/{id}")]
        public IActionResult RemoveEvent([FromRoute] int id)
        {
            int.TryParse(User.Identity.Name, out int userId);
            if (_contactServise.RemoveContact(id, userId))
                return Ok();
            return StatusCode(404);
        }

        [HttpGet]
        public IActionResult GetEvents()
        {
            int.TryParse(User.Identity.Name, out int userId);
            return Ok(_contactServise.GetContacts(userId));
        }
    }
}
