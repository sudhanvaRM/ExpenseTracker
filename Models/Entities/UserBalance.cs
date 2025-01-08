using System;
using System.Collections.Generic;

namespace Server.Models.Entities;

public partial class UserBalance
{
    public int UserId { get; set; }

    public decimal Balance { get; set; }

    public virtual User User { get; set; } = null!;
}
