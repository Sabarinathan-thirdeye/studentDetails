using System;
using System.Collections.Generic;

namespace studentDetails_Api.Models;

public partial class userType
{
    public long userTypeID { get; set; }

    public string userTypeName { get; set; } = null!;

    public DateTime createdOn { get; set; }

    public long? createdBy { get; set; }

    public DateTime? modifiedOn { get; set; }

    public long? modifiedBy { get; set; }

    public int userTypeStatus { get; set; }

    public virtual ICollection<userMaster> userMasters { get; set; } = new List<userMaster>();
}
