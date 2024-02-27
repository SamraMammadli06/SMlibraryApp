using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SMLibrary.Presentation.Dtos
{
    public class CardDto
    {
        public string CardNumber {get;set;}
        public string CVV {get;set;}
        public string Date {get;set;}
        public double Amount {get;set;}
    }
}