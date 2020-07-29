using Microsoft.EntityFrameworkCore;
using StrongAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StrongAPI.Context
{
    public class TodoContext:DbContext
    {
        public TodoContext(DbContextOptions<TodoContext> options) :base(options) 
        {


        }


        public DbSet<TodoItem> TodoItems { get; set; }
    }
}
