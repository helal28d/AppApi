using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AppApi.DTOs
{
    public record struct UserCreateDto(
        string FirstName,
        string LastName,
        string Email,
        int Phone,
        string Password,
       List<ProductCreateDto> Products);
    //        public class UserCreateDto
    // {
    //     public string FirstName { get; set; }
    //     public string LastName { get; set; }
    //     public string Email { get; set; }
    //     public int? Phone { get; set; }

    //     public string Password { get; set; }

    //     public List<ProductCreateDto> Products;
    // }
}


