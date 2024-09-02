using FluentValidation;
using Social.APi.Dtos;
using Social.APi.Helpers;

namespace Social.APi.Validators
{
    public class FriendResponseValidators : AbstractValidator<FriendRespoonseDTO>
    {
        public FriendResponseValidators()
        {
            RuleFor(x => x.SenderId).NotEmpty().EmailAddress();
            RuleFor(x => x.Status).IsInEnum().WithMessage("You should Only enter 1 or 2, 1 for Accept 2 for Reject");
        }
    }
}
