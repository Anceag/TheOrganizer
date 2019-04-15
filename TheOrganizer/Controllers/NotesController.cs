﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TheOrganizer.Model;
using TheOrganizer.Services;

namespace TheOrganizer.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class NotesController : ControllerBase
    {
        private INoteService _noteService;
        public NotesController(INoteService noteService)
        {
            _noteService = noteService;
        }

        [HttpPost("add")]
        public IActionResult AddNote([FromBody] Note note)
        {
            int.TryParse(User.Identity.Name, out int userId);
            if (_noteService.AddNote(note, userId))
                return Ok();
            return BadRequest("There is something wrong with Note info");
        }

        [HttpPut("edit")]
        public IActionResult EditNote([FromBody] Note note)
        {
            int.TryParse(User.Identity.Name, out int userId);
            if (_noteService.EditNote(note, userId))
                return Ok();
            return BadRequest("There is something wrong with Note info");
        }

        [HttpDelete("delete/{id}")]
        public IActionResult DeleteNote([FromRoute] int id)
        {
            int.TryParse(User.Identity.Name, out int userId);
            if (_noteService.RemoveNote(id, userId))
                return Ok();
            return NotFound();
        }

        [HttpGet("getAll/{notebookId}")]
        public IActionResult GetNotes([FromRoute] int notebookId)
        {
            int.TryParse(User.Identity.Name, out int userId);
            var notes = _noteService.GetNotes(userId, notebookId);
            if (notes != null)
            {
                return Ok(notes);
            }
            return StatusCode(404);
        }

        [HttpGet("get/{id}")]
        public IActionResult GetNote([FromRoute] int id)
        {
            int.TryParse(User.Identity.Name, out int userId);
            var res = _noteService.GetNote(id, userId);
            if (res != null)
                return Ok(res);
            return NotFound();
        }
    }
}