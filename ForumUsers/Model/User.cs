using System;
using System.Collections.Generic;

namespace ForumUsers.Model;

public partial class User
{
    public int UserId { get; set; }

    public string Username { get; set; } = null!;

    public string EmailAddress { get; set; } = null!;

    public string PasswordSalt { get; set; } = null!;

    public string PasswordHash { get; set; } = null!;

    public byte[]? Photo { get; set; }

    public byte[]? Banner { get; set; }

    public DateTime? RegistrationDate { get; set; }

    public bool ConfirmedEmail { get; set; }

    public DateTime? ConfirmedEmailDate { get; set; }

    public bool _2fa { get; set; }

    public DateTime? _2fadate { get; set; }

    public string? Role { get; set; }
}
