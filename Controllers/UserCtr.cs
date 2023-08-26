using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AppApi.Data;
using AppApi.DTOs;
using AppApi.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AppApi.Controllers
{

    [ApiController]
    [Route("api/[controller]")]
    public class UserCtr : ControllerBase
    {
        private readonly DataContext _context;
        public UserCtr(DataContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IEnumerable<User>> Get()
        {
            return await _context.Users.Include(c => c.Products).ToListAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<User>> Get(int id)
        {


            if (id < 1)
                return BadRequest();
            var user = await _context.Users.Include(c => c.Products)
            .FirstOrDefaultAsync(c => c.Id == id)
            ;
            if (user == null)
                return NotFound();
            return Ok(user);

        }

        [HttpPost]
        public async Task<ActionResult<List<User>>> Post(UserCreateDto user)
        {
            var newUser = new User
            {
                FirstName = user.FirstName,
                LastName = user.LastName,
                Email = user.Email,
                Phone = user.Phone,
                //Password = user.Password
            };
            var products = user.Products.Select(p => new Product { Name = p.Name, Price = p.Price, Users = new List<User> { newUser } }).ToList();
            newUser.Products = products;
            _context.Users.Add(newUser);
            await _context.SaveChangesAsync();
            return Ok(await _context.Users.Include(u => u.Products).ToListAsync());


        }

        [HttpPut]
        public async Task<IActionResult> Put(User userUpdate)
        {
            if (userUpdate == null || userUpdate.Id == 0)
                return BadRequest();

            var user = await _context.Users.FindAsync(userUpdate.Id);
            if (user == null)
                return NotFound();
            user.FirstName = userUpdate.FirstName;
            user.LastName = userUpdate.LastName;

            user.Email = userUpdate.Email;
            user.Phone = userUpdate.Phone;
            //user.Password = userUpdate.Password;
            await _context.SaveChangesAsync();
            return Ok();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            if (id < 1)
                return BadRequest();
            var user = await _context.Users.FindAsync(id);
            if (user == null)
                return NotFound();
            _context.Users.Remove(user);
            await _context.SaveChangesAsync();
            return Ok();

        }
    }
}