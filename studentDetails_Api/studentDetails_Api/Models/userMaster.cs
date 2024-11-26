using System;
using System.Collections.Generic;

namespace studentDetails_Api.Models;

public partial class userMaster
{
    public long userID { get; set; }

    public string firstName { get; set; } = null!;

    public string? lastName { get; set; }

    public string email { get; set; } = null!;

    public long userTypeID { get; set; }

    public string userName { get; set; } = null!;

    public string userPassword { get; set; } = null!;

    public DateTime? createdOn { get; set; }

    public long? createdBy { get; set; }

    public DateTime? modifiedOn { get; set; }

    public long? modifiedBy { get; set; }

    public int userMasterStatus { get; set; }

    public virtual userType userType { get; set; } = null!;
}
