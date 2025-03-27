using System;
using System.Collections.Generic;

namespace ColledgeDocument.Api.Entities;

public partial class DocumentOrder
{
    public int Id { get; set; }

    public int UserId { get; set; }

    public int DocumentTypeId { get; set; }

    public int OrderStatusId { get; set; }

    public int DepartamentId { get; set; }

    public int Quantity { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public virtual Departament Departament { get; set; } = null!;

    public virtual DocumentType DocumentType { get; set; } = null!;

    public virtual OrderStatus OrderStatus { get; set; } = null!;

    public virtual User User { get; set; } = null!;
}
