using System;
using System.Collections.Generic;

namespace ColledgeDocument.Api.Entities;

public partial class RefreshSession
{
    public int Id { get; set; }

    public int UserId { get; set; }

    public string RefreshToken { get; set; } = null!;

    public DateTime ExpiresAt { get; set; }

    public DateTime? CreatedAt { get; set; }

    public virtual User User { get; set; } = null!;
}
