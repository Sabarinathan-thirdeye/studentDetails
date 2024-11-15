using System;
using System.Collections.Generic;

namespace studentDetails_Api.Models;

public partial class studentDetail
{
    public long studentID { get; set; }

    public string studentName { get; set; } = null!;

    public string userName { get; set; } = null!;

    public DateOnly dateOfBirth { get; set; }

    public string gender { get; set; } = null!;

    public string email { get; set; } = null!;

    public long mobileNumber { get; set; }

    public DateTime? createdOn { get; set; }

    public long? createdBy { get; set; }

    public DateTime? modifiedOn { get; set; }

    public long? modifiedBy { get; set; }

    public string studentPassword { get; set; } = null!;

    public int studentstatus { get; set; }
}
