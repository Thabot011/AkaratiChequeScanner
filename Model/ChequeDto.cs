using System;

namespace AkaratiCheckScanner.Model
{
    public class ChequeDto
    {
        public string Number { get; set; }
        public DateTime DueDate { get; set; }
        public decimal Amount { get; set; }
        public ImageDto Image { get; set; }
    }
}
