using System;
using System.Collections.Generic;

namespace studentDetails_Api.Models;

public partial class UsersDetail
{
    public int Id { get; set; }

    public string FullName { get; set; } = null!;

    public string Email { get; set; } = null!;

    public string Password { get; set; } = null!;
}
