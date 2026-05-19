using BrainBoost.API.DTOs.Quizzes;
using FluentValidation;

namespace BrainBoost.API.Validators.Quizzes;

public class CreateQuizDtoValidator
    : AbstractValidator<CreateQuizDto>
{
    public CreateQuizDtoValidator()
    {
        RuleFor(x => x.Title)
            .NotEmpty()
            .MaximumLength(150);

        RuleFor(x => x.CategoryId)
            .GreaterThan(0);

        RuleFor(x => x.TimeLimitMinutes)
            .GreaterThan(0);

        RuleFor(x => x.PassingScore)
            .InclusiveBetween(0, 100);
    }
}