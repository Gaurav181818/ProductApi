using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductApi.Application.DTOs
{
    public record PagedResultDto<T>(
        IEnumerable<T> Items,
        int TotalCount,
        int Page,
        int PageSize,
        int TotalPages
    );


    public record PaginationParams(int Page = 1, int PageSize = 10);
}
