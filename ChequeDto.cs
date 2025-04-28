using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AkaratiCheckScanner
{
    public class ChequeDto
    {
        public string Number { get; set; }
        public DateTime DueDate { get; set; }
        public decimal Amount { get; set; }
        public ImageDto Image { get; set; }
    }
}
