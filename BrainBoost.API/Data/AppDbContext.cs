using BrainBoost.API.Entities;
using Microsoft.EntityFrameworkCore;

namespace BrainBoost.API.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options)
    {
    }

    public DbSet<User> Users => Set<User>();
    public DbSet<Category> Categories => Set<Category>();
    public DbSet<Role> Roles => Set<Role>();
    public DbSet<Quiz> Quizzes => Set<Quiz>();

    public DbSet<Question> Questions => Set<Question>();

    public DbSet<AnswerOption> AnswerOptions => Set<AnswerOption>();
    public DbSet<QuizAttempt> QuizAttempts => Set<QuizAttempt>();

    public DbSet<UserAnswer> UserAnswers => Set<UserAnswer>();
    public DbSet<MediaFile> MediaFiles => Set<MediaFile>();
    public DbSet<Flashcard> Flashcards => Set<Flashcard>();

    public DbSet<FlashcardProgress> FlashcardProgresses => Set<FlashcardProgress>();
    public DbSet<FlashcardSet> FlashcardSets => Set<FlashcardSet>();
    public DbSet<Bookmark> Bookmarks => Set<Bookmark>();
    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);
        builder.Entity<Role>().HasData(
            new Role
            {
                Id = 1,
                Name = "Admin",
                Description = "System Admin"
            },
            new Role
            {
                Id = 2,
                Name = "Teacher",
                Description = "Teacher"
            },
            new Role
            {
                Id = 3,
                Name = "Student",
                Description = "Student"
            }
        );
        builder.Entity<Category>().HasIndex(x => x.Name).IsUnique();
        builder.Entity<Category>().HasIndex(x => x.Slug).IsUnique();

        builder.Entity<Category>().HasData(
            new Category
            {
                Id = 1,
                Name = "Programming",
                Slug = "programming",
                Description = "Programming and software development",
                IsActive = true,
                CreatedAt = new DateTime(2026, 1, 1)
            },
            new Category
            {
                Id = 2,
                Name = "Mathematics",
                Slug = "mathematics",
                Description = "Mathematics quizzes and flashcards",
                IsActive = true,
                CreatedAt = new DateTime(2026, 1, 1)
            },
            new Category
            {
                Id = 3,
                Name = "Cyber Security",
                Slug = "cyber-security",
                Description = "Cyber security learning materials",
                IsActive = true,
                CreatedAt = new DateTime(2026, 1, 1)
            }
        );
        builder.Entity<Quiz>()
    .HasOne(x => x.Category)
    .WithMany()
    .HasForeignKey(x => x.CategoryId)
    .OnDelete(DeleteBehavior.Restrict);

        builder.Entity<Question>()
            .HasOne(x => x.Quiz)
            .WithMany(x => x.Questions)
            .HasForeignKey(x => x.QuizId);

        builder.Entity<AnswerOption>()
            .HasOne(x => x.Question)
            .WithMany(x => x.Options)
            .HasForeignKey(x => x.QuestionId);
        builder.Entity<QuizAttempt>()
    .HasOne(x => x.User)
    .WithMany()
    .HasForeignKey(x => x.UserId)
    .OnDelete(DeleteBehavior.Restrict);

        builder.Entity<QuizAttempt>()
            .HasOne(x => x.Quiz)
            .WithMany()
            .HasForeignKey(x => x.QuizId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.Entity<UserAnswer>()
            .HasOne(x => x.Attempt)
            .WithMany(x => x.Answers)
            .HasForeignKey(x => x.AttemptId);

        builder.Entity<UserAnswer>()
            .HasOne(x => x.Question)
            .WithMany()
            .HasForeignKey(x => x.QuestionId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.Entity<UserAnswer>()
            .HasOne(x => x.SelectedOption)
            .WithMany()
            .HasForeignKey(x => x.SelectedOptionId)
            .OnDelete(DeleteBehavior.Restrict);
        builder.Entity<Flashcard>()
    .HasOne(x => x.Category)
    .WithMany()
    .HasForeignKey(x => x.CategoryId)
    .OnDelete(DeleteBehavior.Restrict);

        builder.Entity<Flashcard>()
            .HasOne(x => x.CreatedByUser)
            .WithMany()
            .HasForeignKey(x => x.CreatedByUserId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.Entity<FlashcardProgress>()
            .HasOne(x => x.User)
            .WithMany()
            .HasForeignKey(x => x.UserId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.Entity<FlashcardProgress>()
            .HasOne(x => x.Flashcard)
            .WithMany()
            .HasForeignKey(x => x.FlashcardId)
            .OnDelete(DeleteBehavior.Restrict);
        builder.Entity<MediaFile>()
    .HasOne(x => x.UploadedByUser)
    .WithMany()
    .HasForeignKey(x => x.UploadedByUserId)
    .OnDelete(DeleteBehavior.Restrict);
        builder.Entity<Bookmark>()
    .HasOne(x => x.User)
    .WithMany()
    .HasForeignKey(x => x.UserId)
    .OnDelete(DeleteBehavior.Restrict);

        builder.Entity<Bookmark>()
            .HasOne(x => x.Quiz)
            .WithMany()
            .HasForeignKey(x => x.QuizId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.Entity<Bookmark>()
            .HasOne(x => x.Flashcard)
            .WithMany()
            .HasForeignKey(x => x.FlashcardId)
            .OnDelete(DeleteBehavior.Restrict);
        builder.Entity<FlashcardSet>()
    .HasOne(x => x.Category)
    .WithMany()
    .HasForeignKey(x => x.CategoryId)
    .OnDelete(DeleteBehavior.Restrict);

        builder.Entity<FlashcardSet>()
            .HasOne(x => x.CreatedByUser)
            .WithMany()
            .HasForeignKey(x => x.CreatedByUserId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.Entity<Flashcard>()
            .HasOne(x => x.FlashcardSet)
            .WithMany(x => x.Flashcards)
            .HasForeignKey(x => x.FlashcardSetId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}