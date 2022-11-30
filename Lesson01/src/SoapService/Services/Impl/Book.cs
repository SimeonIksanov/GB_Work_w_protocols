using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SoapService.Services.Impl
{
    public class Book
    {
        public string Id { get; set; }
        public string Title { get; set; }
        public string Category { get; set; }
        public Author[] Authors { get; set; }
        public int PublicationDate { get; set; }
        public string Lang { get; set; }
        public int Pages { get; set; }
        public int AgeLimit { get; set; } 
    }
}
