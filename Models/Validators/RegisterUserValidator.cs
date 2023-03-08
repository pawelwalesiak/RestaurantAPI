using FluentValidation;
using RestaurantAPI.Entities;

namespace RestaurantAPI.Models.Validators
{
    public class RegisterUserValidator : AbstractValidator<RegisterUserDto>
    {
        //warynki walidacji w konstruktorze

        public RegisterUserValidator(RestaurantDbContext dbContext)
        {
            RuleFor(x=>x.Email)
                .NotEmpty() // nie moze byc puste 
                .EmailAddress(); //format adresu emial

            RuleFor(x => x.Password)
                .NotEmpty()
                .MinimumLength(6);

            RuleFor(x => x.ConfirmedPassword)
                .Equal(e => e.Password);

            RuleFor(x => x.Email)
                .Custom((value, context) =>
                {
                    // odniesienie sie do bazy danyc - wstrzykniecie dbcontext 
                    var emailInUse = dbContext.Users.Any(u => u.Email == value);
                    if (emailInUse)
                    {
                        context.AddFailure("Email", "That emial is allready taken");
                    }
                });
        }
    }
}
