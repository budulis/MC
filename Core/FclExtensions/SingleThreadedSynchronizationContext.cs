using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;

namespace Core.FclExtensions {
	public class SingleThreadedSynchronizationContext : SynchronizationContext, IDisposable{

		public static Task Execute(Func<Task> taskFactory) {
			var tcs = new TaskCompletionSource<bool>();

			if (taskFactory == null) {
				tcs.SetException(new ArgumentNullException("taskFactory"));
				return tcs.Task;
			}

			var current = Current;
			var ctx = new SingleThreadedSynchronizationContext();
			SetSynchronizationContext(ctx);

			var task = taskFactory();

			if (task == null) {
				tcs.SetException(new NullReferenceException("taskFactory provided task is null"));
				return tcs.Task;
			}

			task.ContinueWith(x => ctx.Complete(), TaskScheduler.Current);
			ctx.ProcessContinuations();

			SetSynchronizationContext(current);

			return task;
		}

		public static Task<TResult> Execute<TResult>(Func<Task<TResult>> taskFactory)
		{
			return (Task<TResult>)Execute((Func<Task>)taskFactory);
		}

		private BlockingCollection<KeyValuePair<SendOrPostCallback, object>> _queue;

		public SingleThreadedSynchronizationContext() {
			_queue = new BlockingCollection<KeyValuePair<SendOrPostCallback, object>>();
		}

		public override void Post(SendOrPostCallback d, object state) {
			_queue.Add(new KeyValuePair<SendOrPostCallback, object>(d, state));
		}

		public void ProcessContinuations() {
			foreach (var workItem in _queue.GetConsumingEnumerable()) {
				workItem.Key(workItem.Value);
			}
		}

		public void Complete() {
			_queue.CompleteAdding();
		}

		public void Dispose()
		{
			Dispose(true);
			GC.SuppressFinalize(this);
		}

		~SingleThreadedSynchronizationContext()
		{
			Dispose(false);
		}

		protected virtual void Dispose(bool disposing)
		{
			if (disposing)
			{
				_queue.Dispose();
				_queue = null;
			}
		}

	}
}