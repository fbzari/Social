using FluentValidation;
using Social.APi.Dtos;

namespace Social.APi.Validators
{
    public class FriendrequestValidators : AbstractValidator<FriendRequestDto>
    {
        public FriendrequestValidators()
        {
            RuleFor(x => x.ReceiverId)
                       .NotEmpty().WithMessage("Receiver email cannot be empty.")
                       .EmailAddress().WithMessage("Receiver email must be a valid email address.");
        }
    }
}
