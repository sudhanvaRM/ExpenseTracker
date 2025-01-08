using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.SignalR;

namespace Server.Models
{
    public class AddSplitDTO
    {
        public int TransactionID { get; set;} 
        
        public required List<SplitParticipantDTO> Participants { get; set; }

        public class SplitParticipantDTO
        {
            public int ParticipantId { get; set; }    // ID of the participant
            public decimal Amount { get; set; }       // Amount assigned to the participant
        }
    }
}