using AutoMapper;
using WebSocketsDataService.Hubs;
using WebSocketsDataService.Models;
using WebSocketsDataService.Models.DTO;
using WebSocketsDataService.Models.HubData;
using WebSocketsDataService.Models.HubData.HubDataDTO;

namespace WebSocketsDataService.Profiles
{
	public class DefaultMapProfiler : Profile
	{
		public DefaultMapProfiler()
		{
			CreateMap<UserCreateDto, User>();
			CreateMap<BaseHub, BaseHubDto>();
			CreateMap<BaseHubDto, BaseHub>();

			#region simplechartprofiles
			CreateMap<SimpleChart, SimpleChartAdminDto>();
			CreateMap<SimpleChart, SimpleChartManagerDto>();
			CreateMap<SimpleChart, SimpleChartUserDto>();
			#endregion
			#region simplechartprofiles
			CreateMap<GraphChart, GraphChartDto>();
			#endregion


		}
	}
}
