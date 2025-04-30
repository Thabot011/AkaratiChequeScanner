using System.Collections.Generic;

namespace AkaratiCheckScanner.Model
{
    public class CreateChequesRequestDto
    {
        public long CustomerParticipantId { get; set; }
        public string NameOnCheque { get; set; }
        public string CustomerAccountNumber { get; set; }
        public long BankId { get; set; }
        public List<ChequeDto> Cheques { get; set; }
    }
}
