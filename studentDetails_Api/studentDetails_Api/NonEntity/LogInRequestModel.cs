using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace studentDetails_Api.Models;

public partial class LoginRequestModel
{
    /// <summary>
    /// Email
    /// </summary>
    public string email { get; set; } = null!;
    /// <summary>
    /// Student Password
    /// </summary>
    public string studentPassword { get; set; } = null!;
}