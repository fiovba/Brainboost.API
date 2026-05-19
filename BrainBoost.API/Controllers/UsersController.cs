using BrainBoost.API.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BrainBoost.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize(Roles = "Teacher,Admin")]
public class UsersController : ControllerBase
{
    private readonly AppDbContext _context;

    public UsersController(AppDbContext context)
    {
        _context = context;
    }

    [HttpGet("students")]

    public async Task<IActionResult> GetStudents()
    {
        var users = await _context.Users
            .Include(x => x.Role)
            .ToListAsync();

        var students = users
            .Where(x => x.Role != null &&
                        x.Role.Name.ToLower() == "student")
            .Select(x => new
            {
                id = x.Id,
                fullName = x.FullName,
                email = x.Email,
                bio = x.Bio,
                profileImageUrl = x.ProfileImageUrl,
                xp = x.XP,
                level = x.Level,
                learningStreak = x.LearningStreak,
                createdAt = x.CreatedAt,

                completedQuizzes = _context.QuizAttempts.Count(a =>
                    a.UserId == x.Id &&
                    a.Status == "Completed"
                )
            })
            .ToList();

        return Ok(students);
    }
}