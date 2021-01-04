using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Week2Queue.Core.Models;

namespace Week2Queue.Core.Interfaces
{
    public interface IQueueRepository
    {
        Task<bool> AddMessage(Message message);
        Task<bool> SetHandled(Guid messageId);
        Task<Message> GetUnhandledEmailMessage();
        Task<Message> GetUnhandledLoggingMessage();
    }
}
