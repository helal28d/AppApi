using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace AppApi.Models
{

    public class User
    {
        //relation between user and product is m-m

        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public int? Phone { get; set; }

        public string Password { get; set; }

        public List<Product> Products { get; set; }



    }
}