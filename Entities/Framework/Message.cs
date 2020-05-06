using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entities.Framework
{
    public class Message : BaseEntity
    {
        public string Title { get; set; }
        public string Body { get; set; }
        public DateTime? SeenDateTime { get; set; }
        public bool IsSeen { get; set; }
        public int SenderId { get; set; }
        [ForeignKey("SenderId")]
        public User Sender { get; set; }
        public int ReceiverId { get; set; }
        public MessageType MessageType { get; set; }
        public int? ReplayId { get; set; }
        [ForeignKey("ReplayId")]
        public Message ReplayMessage { get; set; }
    }
    public enum MessageType
    {
        Text,
        Image,
        Voice,
        Video,
    }
}
