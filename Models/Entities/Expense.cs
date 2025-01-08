using System;
using System.Collections.Generic;
namespace Server.Models.Entities;

public partial class Expense
{
    public int DebitId { get; set; }

    public int UserId { get; set; }

    public decimal Amount { get; set; }

    public string? Comment { get; set; }

    public string Category { get; set; } = null!;

    public DateTime Timestamp { get; set; } 

     public ICollection<SplitParticipant> ? SplitParticipants { get; set; }
}
