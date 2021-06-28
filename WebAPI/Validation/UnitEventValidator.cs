using DAL.Models.DTO;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebAPI.Validation
{
	public class UnitEventValidator : AbstractValidator<UnitEventDto>
	{
		public UnitEventValidator()
		{
			RuleFor(unitEvent => unitEvent.Name).NotEmpty().WithMessage("Имя не может быть пустым");
			RuleFor(unitEvent => unitEvent.Description).NotEmpty().WithMessage("Описание не может быть пустым");
		}
	}

}
