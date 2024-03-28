using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SMlibraryApp.Core.Models;

namespace SMLibrary.Core.Models
{
    public class Comment
    {
        public int Id { get; set; }
        public string Text { get; set; }
        public string Sender { get; set; }
        public int BookId { get; set; }
        public Book Book {get;set;}
    }
}