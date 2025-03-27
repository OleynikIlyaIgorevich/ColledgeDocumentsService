using System;
using System.Collections.Generic;

namespace ColledgeDocument.Api.Entities;

public partial class DocumentRequest
{
    public int Id { get; set; }

    public int UserId { get; set; }

    public int DocumentTypeId { get; set; }

    public int RequestStatusId { get; set; }

    public int DepartamentId { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public virtual Departament Departament { get; set; } = null!;

    public virtual DocumentType DocumentType { get; set; } = null!;

    public virtual RequestStatus RequestStatus { get; set; } = null!;

    public virtual User User { get; set; } = null!;
}
