using System;
using System.Collections.Generic;

namespace studentDetails_Api.Models;

public partial class studentDetail
{
    public int studentID { get; set; }

    public string firstName { get; set; } = null!;

    public string? lastName { get; set; }

    public DateOnly? dateOfBirth { get; set; }

    public int? gender { get; set; }

    public string email { get; set; } = null!;

    public string? mobileNumber { get; set; }

    public DateTime? createdOn { get; set; }

    public long? createBy { get; set; }

    public DateTime? modifiedOn { get; set; }

    public long? modifiedBy { get; set; }

    public string studentPassword { get; set; } = null!;

    public int? studentstatus { get; set; }
}
