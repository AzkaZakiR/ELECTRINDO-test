using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using WebApplication1.Data;
using WebApplication1.Models;
using WebApplication1.Features.Auth.DTOs;
namespace WebApplication1.Features.Auth;

public static class AuthEndpoints
{
    public static IEndpointRouteBuilder MapAuthEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/auth");
        
        group.MapPost("/register", Register);
        group.MapPost("/login", Login);

        return app;
    }

    private static async Task<IResult> Register(AppDbContext db, RegisterRequest req)
    {
        var exists = await db.Users.AnyAsync(x => x.Email == req.Email);

        if(exists)
        {
            return Results.BadRequest(new
            {
                message = "Email sudah ada"
            });
        }

        var user = new User
        {
            Id = Guid.NewGuid(),
            Name = req.Name,
            Email = req.Email,
            Role = "admin"
        };

         var hasher = new PasswordHasher<User>();

        user.PasswordHash = hasher.HashPassword(user, req.Password);

        db.Users.Add(user);
        await db.SaveChangesAsync();

        return Results.Ok(new
        {
            message = "User berhasil diregister"
        });
    }

    private static async Task<IResult> Login(AppDbContext db, LoginRequest req, JwtGenerator jwtGenerator)
    {
        var user = await db.Users.FirstOrDefaultAsync(x => x.Email == req.Email);

        if (user == null)
        {
            return Results.BadRequest(new
            {
                message = "Email atau password salah"
            });
        }

        var hasher = new PasswordHasher<User>();
        var isMatch = hasher.VerifyHashedPassword(user, user.PasswordHash, req.Password) == PasswordVerificationResult.Success;

        if (!isMatch)
        {
            return Results.BadRequest(new
            {
                message = "Email atau password salah"
            });
        }

        var token = jwtGenerator.Generate(user);

        return Results.Ok(new
        {
            message = "Login berhasil",
            token = token,
            userId = user.Id
        });
    }
}