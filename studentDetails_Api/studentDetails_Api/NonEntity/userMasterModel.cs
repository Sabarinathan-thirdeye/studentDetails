using System;
using System.Collections.Generic;

namespace studentDetails_Api.Models;

public partial class userMasterModel
{
    public long userID { get; set; }

    public string firstName { get; set; } = null!;

    public string lastName { get; set; } = null!;

    public DateOnly dateOfBirth { get; set; }

    public string gender { get; set; } = null!;

    public string email { get; set; } = null!;

    public long mobileNumber { get; set; }

    public long userTypeID { get; set; }

    public string userName { get; set; } = null!;

    public string userPassword { get; set; } = null!;

    public DateTime? createdOn { get; set; }

    public long? createdBy { get; set; }

    public DateTime? modifiedOn { get; set; }

    public long? modifiedBy { get; set; }

    public int userMasterStatus { get; set; }

}
