using System;
using System.Collections.Generic;
using System.Text;

namespace Week2Queue.Core.DTO
{
    public class MessageDTO
    {
        public int Id { get; set; }
        public string Type { get; set; }
        public string JsonContent { get; set; }
    }
}
