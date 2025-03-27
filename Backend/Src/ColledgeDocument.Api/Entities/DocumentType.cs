using System;
using System.Collections.Generic;

namespace ColledgeDocument.Api.Entities;

public partial class DocumentType
{
    public int Id { get; set; }

    public string Title { get; set; } = null!;

    public DateTime CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public virtual ICollection<DocumentOrder> DocumentOrders { get; set; } = new List<DocumentOrder>();
}
