using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Helpers
{
    public class LikesParams:PaginationParams
    {
        public string? Predicate { get; set; }
        public int UserId { get; set; }
    }
}