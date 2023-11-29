using Microsoft.AspNetCore.Http.HttpResults;

namespace vsports.Models
{
    public class BaseEntity
    {
        public DateTime Created { get; set; }
        public bool IsDelete { get; set; }
    }
}
