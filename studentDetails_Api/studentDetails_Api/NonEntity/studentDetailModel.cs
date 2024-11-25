using System;
using System.Collections.Generic;

namespace studentDetails_Api.Models;

public partial class studentDetailModel
{
    public long studentID { get; set; }

    public string firstName { get; set; } = null!;

    public string lastName { get; set; } = null!;

    public DateOnly dateOfBirth { get; set; }

    public string gender { get; set; } = null!;

    public string email { get; set; } = null!;

    public long mobileNumber { get; set; }

    public int studentstatus { get; set; }

    public DateTime? createdOn { get; set; }

}
