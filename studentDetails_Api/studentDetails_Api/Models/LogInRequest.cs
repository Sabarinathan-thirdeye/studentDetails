﻿using System;
using System.Collections.Generic;

namespace studentDetails_Api.Models;

public partial class LoginRequest
{
    public string email { get; set; } = null!;

    public string studentPassword { get; set; } = null!;
}
