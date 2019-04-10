﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TheOrganizer.Model
{
    public class Todo
    {
        public int Id { get; set; }
        public int TodoListId { get; set; }
        public string Text { get; set; }
        public bool IsDone { get; set; }

        public TodoList TodoList { get; set; }
    }
}
