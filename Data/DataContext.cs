using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AppApi.Models;
using Microsoft.EntityFrameworkCore;

namespace AppApi.Data;

public class DataContext : DbContext
{


    public DataContext(DbContextOptions<DataContext> options) : base(options)
    {

    }

    public DbSet<User> Users { get; set; }
    public DbSet<Product> Products { get; set; }


}
