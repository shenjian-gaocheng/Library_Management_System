using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace backend.DTOs
{
    public class MyBorrowRecordDto
    {   
        public required string ISBN  { get; set; }
        
        public string BookTitle { get; set; }

        public string BookAuthor { get; set; }

        public DateTime BorrowTime { get; set; }

        public DateTime? ReturnTime { get; set; }

        public decimal OverdueFine { get; set; }
    }
}