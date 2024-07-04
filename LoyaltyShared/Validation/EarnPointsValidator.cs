using FluentValidation;
using LoyaltyShared.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LoyaltyShared.Validation
{
    public class EarnPointsValidator : AbstractValidator<EarnPointsDTO>
    {
        public EarnPointsValidator() {
            RuleFor(x => x.Points).GreaterThan(0).WithMessage("Points must be greater than zero.");
        }
    }
}
