using System;
using System.Collections.Generic;

namespace ColledgeDocument.Api.Entities;

public partial class User
{
    public int Id { get; set; }

    public int RoleId { get; set; }

    public string Lastname { get; set; } = null!;

    public string Firstname { get; set; } = null!;

    public string? Middlename { get; set; }

    public string Phone { get; set; } = null!;

    public string Username { get; set; } = null!;

    public string PasswordHash { get; set; } = null!;

    public DateTime CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public virtual ICollection<DocumentOrder> DocumentOrders { get; set; } = new List<DocumentOrder>();

    public virtual ICollection<RefreshSession> RefreshSessions { get; set; } = new List<RefreshSession>();

    public virtual Role Role { get; set; } = null!;
}
