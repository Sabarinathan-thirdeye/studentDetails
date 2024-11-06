using System;
using System.Collections.Generic;

namespace studentDetails_Api.Models;

public partial class LogInRequest
{
    public string Email { get; set; } = null!;

    public string Password { get; set; } = null!;

    public int studentID { get; set; }

    public virtual studentDetail student { get; set; } = null!;
}
