using DAL.Models.DTO;
using FluentValidation;

namespace WebAPI.Validation
{
	public class FactoryValidator : AbstractValidator<FactoryCreateUpdateDto>
	{
		public FactoryValidator()
		{
			RuleFor(factory => factory.Name).NotEmpty().WithMessage("Имя не может быть пустым");
			RuleFor(factory => factory.Description).NotEmpty().WithMessage("Описание не может быть пустым");
		}
	}
}
