﻿namespace ColledgeDocument.Shared.Requests;

public class UpdateUserRequest
{
    public string LastName { get; set; }
    public string FirstName { get; set; }
    public string? MiddleName { get; set; }
    public string Phone { get; set; }
    public string Username { get; set; }
    public bool IsChangePassword { get; set; }
    public string? Password { get; set; }
    public int RoleId { get; set; }
}
