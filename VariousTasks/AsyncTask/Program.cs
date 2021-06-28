using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace AsyncTask
{
	public static class IEnumerableExtension
	{
		public static async Task<IEnumerable<T>> RunInParallel<T>(this IEnumerable<Task<T>> tasks, int maxParallelTasks = 4)
		{
			SemaphoreSlim semaphore = new SemaphoreSlim(maxParallelTasks, maxParallelTasks);
			var taskResults = await Task.WhenAll(tasks.Select(t => WrapTaskWithSemaphore(t, semaphore)));
			return taskResults.AsEnumerable();
		}

		static async Task<T> WrapTaskWithSemaphore<T>(Task<T> task, SemaphoreSlim semaphore)
		{
			await semaphore.WaitAsync();

			try
			{
				var taskResult = await task.ConfigureAwait(false);
				return taskResult;
			}
			catch (Exception ex)
			{
				Console.WriteLine("Exception during task has occured: " + ex.Message);
			}
			finally
			{
				semaphore.Release();
			}
			return default;
		}

	}

	class Program
	{
		static async Task Main(string[] args)
		{
			using (HttpClient client = new HttpClient())
			{
				try
				{
					List<Task<string>> tasks = new List<Task<string>>();
					for (int i = 1; i <= 100; i++)
					{
						string uri = $"https://jsonplaceholder.typicode.com/posts/{i}";
						tasks.Add(client.GetStringAsync(uri));
					}
					var results = await tasks.AsEnumerable().RunInParallel<string>(5);
					foreach (var r in results)
						Console.WriteLine(r);
				}
				catch (HttpRequestException e)
				{
					Console.WriteLine("\nException Caught!");
					Console.WriteLine("Message :{0} ", e.Message);
				}
			}
		}

	}
}
