using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Week2Queue.Core.Interfaces;
using Week2Queue.Core.Models;
using Week2Queue.Infrastructure.Context;

namespace Week2Queue.Infrastructure.Repositories
{
        public class QueueRepository : IQueueRepository
        {
            private readonly QueueContext _context;
            public QueueRepository(QueueContext context)
            {
                _context = context;
            }
            public async Task<bool> AddMessage(Message message)
            {
                await _context.Message.AddAsync(message);
                return (await _context.SaveChangesAsync()) > 0;
            }

            public async Task<Message> GetUnhandledEmailMessage()
            {
                var message = await _context.Message.Where(x => !x.Handled && x.Type == "email").OrderBy(x => x.AddedAt).FirstOrDefaultAsync();
                return message;
            }

            public async Task<Message> GetUnhandledLoggingMessage()
            {
                var message = await _context.Message.Where(x => !x.Handled && x.Type == "log").OrderBy(x => x.AddedAt).FirstOrDefaultAsync();
                return message;
            }

            public async Task<bool> SetHandled(Guid messageId)
            {
                var message = _context.Message.Where(x => x.Id == messageId).FirstOrDefault();
                if (message == null) return false;
                message.Handled = true;
                message.HandledAt = DateTime.Now;
                return (await _context.SaveChangesAsync()) > 0;
            }
        }
    }
