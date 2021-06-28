using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace DaDataConsoleApplication
{
	public interface IApiService
	{
		public Task<string> GetCompanyNameById(string id);
	}
}
