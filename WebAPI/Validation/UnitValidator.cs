using DAL.Models.DTO;
using FluentValidation;

namespace WebAPI.Validation
{
	public class UnitValidator : AbstractValidator<UnitCreateUpdateDto>
	{
		public UnitValidator()
		{
			RuleFor(unit => unit.Name).NotEmpty().WithMessage("Имя не может быть пустым");
		}
	}
}
