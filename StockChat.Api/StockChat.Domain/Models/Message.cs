using Microsoft.AspNetCore.Identity;
using System;

namespace StockChat.Domain.Models
{
    public class Message
    {
        public int Id { get; private set; }
        public string Text { get; private set; }
        public DateTime Date { get; private set; }
        public virtual IdentityUser User { get; private set; }

        public Message() { }

        public Message(string text, IdentityUser user)
        {
            Text = text;
            User = user;
            Date = DateTime.Now;
        }
    }
}
