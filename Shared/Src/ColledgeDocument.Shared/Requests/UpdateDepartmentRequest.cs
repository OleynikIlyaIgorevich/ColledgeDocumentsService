using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ColledgeDocument.Shared.Requests;

public class UpdateDepartmentRequest
{
    [JsonPropertyName("title")]
    public string Title { get; set; }
}
