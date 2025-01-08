using System;
using System.Collections.Generic;

namespace Server.Models.Entities;

public partial class User
{
    public int UserId { get; set; }

    public string Email { get; set; } = null!;

    public string Password { get; set; } = null!;

    public string Username {get; set; } = null!;


    public virtual ICollection<Income> Incomes { get; set; } = new List<Income>();

    public virtual ICollection<Split> Splits { get; set; } = new List<Split>();

    //  public ICollection<Split> Splits { get; set; } 

    public virtual UserBalance? UserBalance { get; set; }
}
