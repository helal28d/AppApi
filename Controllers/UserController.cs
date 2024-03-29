using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Threading.Tasks;
using AppApi.Data;
using AppApi.Models;
using AppApi.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AppApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UserController : ControllerBase
{
    private readonly DataContext _context;
    private readonly IEmailService _emailService;
    public UserController(DataContext context, IEmailService emailService)
    {
        _context = context;
        _emailService = emailService;
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register(UserRegisterRequest request)
    {
        if (_context.Users.Any(u => u.Email == request.Email))
        {
            return BadRequest("User already exists.");
        }

        CreatePasswordHash(request.Password,
             out byte[] passwordHash,
             out byte[] passwordSalt);

        var user = new User
        {
            Email = request.Email,
            PasswordHash = passwordHash,
            PasswordSalt = passwordSalt,
            VerificationToken = CreateRandomToken()
        };

        _emailService.SendCodeEmail(user.Email, "Verfiy code", user.VerificationToken);

        _context.Users.Add(user);
        await _context.SaveChangesAsync();

        return Ok("User successfully created!");


    }

    [HttpPost("login")]
    public async Task<IActionResult> Login(UserLoginRequest request)
    {
        var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == request.Email);
        if (user == null)
        {
            return BadRequest("User not found.");
        }

        if (!VerifyPasswordHash(request.Password, user.PasswordHash, user.PasswordSalt))
        {
            return BadRequest("Password is incorrect.");
        }

        if (user.VerifiedAt == null)
        {
            return BadRequest("Not verified!");
        }

        return Ok($"Welcome back, {user.Email}! :)");
    }

    [HttpPost("verify")]
    public async Task<IActionResult> Verify(string token)
    {
        var user = await _context.Users.FirstOrDefaultAsync(u => u.VerificationToken == token);
        if (user == null)
        {
            return BadRequest("Invalid token.");
        }

        user.VerifiedAt = DateTime.Now;
        await _context.SaveChangesAsync();

        return Ok("User verified! :)");
    }

    [HttpPost("forgot-password")]
    public async Task<IActionResult> ForgotPassword(string email)
    {
        var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == email);
        if (user == null)
        {
            return BadRequest("User not found.");
        }

        user.PasswordResetToken = CreateRandomToken();
        user.ResetTokenExpires = DateTime.Now.AddDays(1);
        await _context.SaveChangesAsync();

        return Ok("You may now reset your password.");
    }

    [HttpPost("reset-password")]
    public async Task<IActionResult> ResettPassword(ResetPasswordRequest request)
    {
        var user = await _context.Users.FirstOrDefaultAsync(u => u.PasswordResetToken == request.Token);
        if (user == null || user.ResetTokenExpires < DateTime.Now)
        {
            return BadRequest("Invalid Token.");
        }

        CreatePasswordHash(request.Password, out byte[] passwordHash, out byte[] passwordSalt);

        user.PasswordHash = passwordHash;
        user.PasswordSalt = passwordSalt;
        user.PasswordResetToken = null;
        user.ResetTokenExpires = null;

        await _context.SaveChangesAsync();

        return Ok("Password successfully reset.");
    }

    private void CreatePasswordHash(string pass, out byte[] passwordHash, out byte[] passwordSalt)
    {
        using (var hmac = new HMACSHA512())
        {
            passwordSalt = hmac.Key;
            passwordHash = hmac
                .ComputeHash(System.Text.Encoding.UTF8.GetBytes(pass));
        }
    }

    private bool VerifyPasswordHash(string password, byte[] passwordHash, byte[] passwordSalt)
    {
        using (var hmac = new HMACSHA512(passwordSalt))
        {
            var computedHash = hmac
                .ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
            return computedHash.SequenceEqual(passwordHash);
        }
    }

    private string CreateRandomToken()
    {

        return Convert.ToHexString(RandomNumberGenerator.GetBytes(64));

    }
}
