using System;
using System.Collections.Generic;

namespace Server.Models.Entities;

public partial class Income
{
    public int CreditId { get; set; }

    public int UserId { get; set; }

    public decimal Amount { get; set; }

    public string? Comment { get; set; }

    public DateTime? Timestamp { get; set; }

    public virtual User User { get; set; } = null!;
}
