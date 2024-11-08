using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace studentDetails_Api.Models;

public partial class LoginRequestModel
{
    [Required]
    public string email { get; set; } = null!;

    [Required]
    public string studentPassword { get; set; } = null!;
}
