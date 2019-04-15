﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TheOrganizer.Model;
using TheOrganizer.Services;

namespace TheOrganizer.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class CalendarsController : Controller
    {
        private ICalendarService _eventService;

        public CalendarsController(ICalendarService eventService)
        {
            _eventService = eventService;
        }

        [HttpPost("add")]
        public IActionResult AddCalendar([FromBody] Calendar calendar)
        {
            int.TryParse(User.Identity.Name, out int userId);
            calendar.OwnerId = userId;
            if (_eventService.AddCalendar(calendar) != null)
                return Ok(calendar);
            return BadRequest("There is something wrong with calendar info");
        }

        [HttpPut("edit")]
        public IActionResult EditCalendar([FromBody] Calendar calendar)
        {
            int.TryParse(User.Identity.Name, out int userId);
            calendar.OwnerId = userId;
            if (_eventService.EditCalendar(calendar))
                return Ok();
            return BadRequest("There is something wrong with calendar info");
        }

        [HttpDelete("delete/{id}")]
        public IActionResult RemoveCalendar([FromRoute] int id)
        {
            int.TryParse(User.Identity.Name, out int userId);
            if (_eventService.RemoveCalendar(id, userId))
                return Ok();
            return StatusCode(404);
        }

        [HttpGet("get/{id}")]
        public IActionResult GetCalendar([FromRoute] int id)
        {
            int.TryParse(User.Identity.Name, out int userId);
            return Ok(_eventService.GetCalendar(id, userId));
        }

        [HttpGet("get")]
        public IActionResult GetCalendars()
        {
            int.TryParse(User.Identity.Name, out int userId);
            return Ok(_eventService.GetCalendars(userId));
        }
    }
}
