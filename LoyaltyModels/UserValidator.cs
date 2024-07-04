using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LoyaltyModels
{
    internal class UserValidator : AbstractValidator<User>
    {
        public UserValidator() { 
            RuleFor(x=>x.Id).NotEmpty().WithMessage("ID is required");
            RuleFor(x => x.Username).NotNull().NotEmpty().WithMessage("ID is required");

        }
    }
}
