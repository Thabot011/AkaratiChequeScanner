using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AkaratiCheckScanner.Models
{
    public class LoginRequest
    {
        public string Username { get; set; }
        public string Password { get; set; }
        public string Identifier { get; set; } = "KS6YL+e8wNw3VFhRXx7ssQ==";
    }

    public class LookupItem
    {
        public long Id { get; set; } // Assuming ID is long based on usage
        public string Name { get; set; }

        // Override ToString for easy display in ComboBox
        public override string ToString()
        {
            return Name;
        }
    }

    public class ImageDto
    {
        public byte[] ImageData { get; set; }
        public string Extension { get; set; } // e.g., "jpeg", "png"
    }

    public class ChequeDto
    {
        public string Number { get; set; }
        public decimal Amount { get; set; }
        public DateTime DueDate { get; set; }
        public ImageDto Image { get; set; }
    }

    public class CreateChequesRequestDto
    {
        public long CustomerParticipantId { get; set; }
        public long BankId { get; set; }
        public string NameOnCheque { get; set; }
        public List<ChequeDto> Cheques { get; set; } = new List<ChequeDto>();
    }
}
