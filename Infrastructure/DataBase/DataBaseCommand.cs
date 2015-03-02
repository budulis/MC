using System.Threading.Tasks;
using Core;

namespace Infrastructure.DataBase {
	public abstract class DataBaseCommand<TResult> : IDataBaseCommand<TResult> {
		protected readonly ILogger Logger;

		protected DataBaseCommand(ILogger logger) {
			Logger = logger;
		}

		public virtual Task<TResult> ExecuteAsync(params object[] parameters)
		{
			Logger.Audit("Begin "+GetType());
			var result = ExecuteAsyncImpl(parameters);
			Logger.Audit("End " + GetType());
			return result;
		}

		protected abstract Task<TResult> ExecuteAsyncImpl(params object[] parameters);

		
	}
}