using BrainBoost.API.DTOs.Auth;
using FluentValidation;

namespace BrainBoost.API.Validators.Auth;

public class RegisterDtoValidator : AbstractValidator<RegisterDto>
{
    public RegisterDtoValidator()
    {
        RuleFor(x => x.FullName)
            .NotEmpty()
            .WithMessage("Ad boş ola bilməz.");

        RuleFor(x => x.Email)
            .NotEmpty()
            .EmailAddress()
            .WithMessage("Email formatı yanlışdır.");

        RuleFor(x => x.Password)
            .NotEmpty()
            .MinimumLength(8)
            .WithMessage("Şifrə minimum 8 simvol olmalıdır.");
    }
}