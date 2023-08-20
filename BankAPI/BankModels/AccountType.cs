using System;
using System.Collections.Generic;

namespace BankAPI.BankModels;

public partial class AccountType
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public DateTime RegDate { get; set; }

    public virtual ICollection<Account> Accounts { get; set; } = new List<Account>();
}
