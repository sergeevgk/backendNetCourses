using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebSocketsDataService.Providers
{
	public interface IDataProvider
	{
		public Task<object> GetData();
	}
}
