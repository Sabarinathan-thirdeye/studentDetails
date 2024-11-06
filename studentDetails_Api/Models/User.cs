using System;
using System.Collections.Generic;
using System.Security.Claims;

namespace UsersDetailsApi.Models;

public partial class User
{
    public long UserID { get; set; }

    public string UserName { get; set; } = null!;

    public long UserTypeID { get; set; }

    public string UserEmail { get; set; } = null!;

    public string FirstName { get; set; } = null!;

    public string LastName { get; set; } = null!;

    public int Gender { get; set; }

    public long MobileNo { get; set; }

    public string Password { get; set; } = null!;

    public DateTime CreatedOn { get; set; }

    public long CreateBy { get; set; }

    public DateTime? ModifiedOn { get; set; }

    public long? ModifiedBy { get; set; }

    public long? UserTypeStatus { get; set; }
    public ClaimsIdentity? Email { get; internal set; }
}
