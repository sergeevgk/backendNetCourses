using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebSocketsDataService.Providers
{
	public interface IDataProviderFactory
	{
		public IDataProvider Create(Type type);
	}
}
