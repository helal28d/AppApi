using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace AppApi.Models;

public class Product
{


    public int Id { get; set; }
    public string Name { get; set; }
    public int Price { get; set; }
    [JsonIgnore]
    public List<User> Users { get; set; }

}
