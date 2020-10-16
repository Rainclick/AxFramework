using System;
using AutoMapper;
using Entities.Framework;
using WebFramework.Api;

namespace API.Models
{
    public class MessageDto : BaseDto<MessageDto, Message, int>
    {
        public string Body { get; set; }
        public DateTime? InsertDateTime { get; set; }
        public int SenderId { get; set; }
        public int ReceiverId { get; set; }
        public bool FromContact { get; set; }

    }
}
