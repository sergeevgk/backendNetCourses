using AutoMapper;
using DAL.Models;
using DAL.Models.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebAPI.Profiles
{
	public class DefaultMapProfiler : Profile
	{
		public DefaultMapProfiler()
		{
			CreateMap<Unit, UnitDto>();
			CreateMap<Unit, UnitWithTanksDto>();
			CreateMap<UnitCreateUpdateDto, Unit>();

			CreateMap<Tank, TankDto>();
			CreateMap<TankCreateUpdateDto, Tank>();

			CreateMap<Factory, FactoryDto>();
			CreateMap<Factory, FactoryWithUnitsDto>();
			CreateMap<FactoryCreateUpdateDto, Factory>();

			CreateMap<User, UserDto>();
			CreateMap<UserCreateUpdateDto, User>();

			CreateMap<UnitEventDto, UnitEvent>();
			CreateMap<UnitEvent, UnitEventDto>();
		}
	}
}
