using FluentValidation;
using Social.APi.Dtos;

namespace Social.APi.Validators
{
    public class UserLoginValidator : AbstractValidator<UserLoginDTO>
    {
        public UserLoginValidator() {

            RuleFor(x => x.Email).NotEmpty().EmailAddress();
            RuleFor(x => x.Password).NotEmpty();

        }
    }
}
