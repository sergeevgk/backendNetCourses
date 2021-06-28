using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Dadata;

namespace DaDataConsoleApplication
{
	public class ApiService : IApiService
	{
		private readonly string apiToken;
		public ApiService(string apiToken)
		{
			this.apiToken = apiToken;
		}

		public async Task<string> GetCompanyNameById(string id)
		{
			try
			{
				var apiClent = new SuggestClientAsync(apiToken);
				var response = await apiClent.FindParty(id);
				if (response.suggestions.Count > 0)
				{
					var party = response.suggestions[0].data;
					return party.name.full;
				}
			}
			catch (InvalidOperationException ex)
			{
				Console.WriteLine("Search operation failed due to wrong uri. " + ex.Message);
			}
			catch (HttpRequestException ex)
			{
				Console.WriteLine("Search operation failed due to network issues. " + ex.Message);
			}
			catch (TaskCanceledException ex)
			{
				Console.WriteLine("Search operation was cancelled due to timeout. " + ex.Message);
			}
			return null;
		}
	}
}
