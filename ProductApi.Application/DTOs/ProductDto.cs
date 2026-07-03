using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductApi.Application.DTOs
{
    public record ProductDto(int Id, string ProductName, string CreatedBy, DateTime CreatedOn);
    public record CreateProductDto(string ProductName);
    public record UpdateProductDto(string ProductName);
}
