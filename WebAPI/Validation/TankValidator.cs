using DAL.Models.DTO;
using FluentValidation;

namespace WebAPI.Validation
{
	public class TankValidator : AbstractValidator<TankCreateUpdateDto>
	{
		public TankValidator()
		{
			RuleFor(tank => tank.Name).NotEmpty().WithMessage("Имя не может быть пустым");
			RuleFor(tank => tank.Volume).GreaterThanOrEqualTo(0)
				.LessThanOrEqualTo(tank => tank.MaxVolume)
				.WithMessage("Объем не может быть отрицательным или превышать максимальный");
			RuleFor(tank => tank.MaxVolume).GreaterThanOrEqualTo(0)
				.WithMessage("Максимальный объем не может быть отрицательным");
		}
	}
}
