using System.Threading.Tasks;

namespace Infrastructure.DataBase
{
	public interface IDataBaseCommand<TResult> {
		Task<TResult> ExecuteAsync(params object[] parameters);
	}
}