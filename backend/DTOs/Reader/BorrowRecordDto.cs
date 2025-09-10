using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace backend.DTOs
{
    public class BorrowRecordDto
    {   
        public required string ReaderId { get; set; }

        public required string BookId { get; set; }

        public required string ReaderName { get; set; }

        public required string BookName { get; set; }

        public DateTime BorrowTime { get; set; }

        public DateTime? ReturnTime { get; set; }

        public decimal OverdueFine { get; set; }
    }
}