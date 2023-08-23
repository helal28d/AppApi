using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AppApi.DTOs
{

    public record struct ProductCreateDto(string Name, int Price);
}

// {
//     public string Name { get; set; }
//     public int Price { get; set; }
// }
