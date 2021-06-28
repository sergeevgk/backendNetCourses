using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace Common.Extensions
{
	public static class EnumerableTaskExtension
	{
		public static async Task RunInParallel(this IEnumerable<Func<Task>> tasks, ILogger logger, int maxParallelTasksCount = 4)
		{
			var semaphoreSlim = new SemaphoreSlim(maxParallelTasksCount, maxParallelTasksCount);

			await Task.WhenAll(tasks.Select(task => WrapWithSemaphore(task)));

			async Task WrapWithSemaphore(Func<Task> task)
			{
				await semaphoreSlim.WaitAsync();
				try
				{
					await task().ConfigureAwait(false);
				}
				catch (Exception ex)
				{
					logger.LogError(ex, "Error on parallel task executing");
				}
				semaphoreSlim.Release();
			}
		}

	}
}
