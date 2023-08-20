using System;
using System.Collections.Generic;

namespace BankAPI.BankModels;

public partial class Client
{
    public int Id { get; set; }

    public string? Name { get; set; }

    public string PhoneNumber { get; set; } = null!;

    public string? Email { get; set; }

    public DateTime RegDate { get; set; }

    public string? Pwd { get; set; }

    public virtual ICollection<Account> Accounts { get; set; } = new List<Account>();
}
