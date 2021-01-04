using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using Week2Queue.Core.Models;

namespace Week2Queue.Infrastructure.Context
{
    public class QueueContext : DbContext
    {
        public QueueContext(DbContextOptions<QueueContext> options) : base(options) { }
        public DbSet<Message> Message { get; set; }

    }
}
