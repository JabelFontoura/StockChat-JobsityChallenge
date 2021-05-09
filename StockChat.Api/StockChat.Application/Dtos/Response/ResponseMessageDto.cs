using System;

namespace StockChat.Application.Dtos.Response
{
    public class ResponseMessageDto
    {
        public int Id { get; set; }
        public string Text { get; set; }
        public DateTime Date { get; set;  } 

        public virtual ResponseMessageUserDto User { get; set; }
    }
}
