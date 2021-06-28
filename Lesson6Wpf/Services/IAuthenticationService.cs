using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lesson6Wpf.Services
{
	public interface IAuthenticationService
	{
		public string AuthenticationToken { get; set; }
		public Task GetAuthenticationToken(string login, string password);
	}
}
