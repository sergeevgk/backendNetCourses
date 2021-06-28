using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebSocketsDataService.Models.DTO
{
	public class BaseHubDto
	{
		public Guid Id { get; set; }
		public string Type { get; set; }
		public string Name { get; set; }
	}
}
