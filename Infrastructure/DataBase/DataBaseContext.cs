using System.Collections.Concurrent;
using System.Collections.Generic;
using Core;
using Core.ReadModel;
using Infrastructure.Services.Product;

namespace Infrastructure.DataBase {
	public class DataBaseContext {
		private readonly ILogger _logger;

		public DataBaseContext(ILogger logger) {
			_logger = logger;
		}

		public virtual IDataBaseCommand<int> InsertOrder {
			get { return new InsertOrder(_logger); }
		}
		public virtual IDataBaseCommand<int> InsertReciept {
			get { return new InsertReciept(_logger); }
		}
		public virtual IDataBaseCommand<OrderReadModel> SelectOrderById {
			get { return new SelectOrderById(_logger); }
		}
		public virtual IDataBaseCommand<ReceiptReadModel> SelectReceiptById {
			get { return new SelectReceiptById(_logger); }
		}
		public virtual IDataBaseCommand<IEnumerable<OrderReadModel>> SelectAllOrders {
			get { return new SelectAllOrders(_logger); }
		}
		public virtual IDataBaseCommand<IEnumerable<ReceiptReadModel>> SelectAllReceipts {
			get { return new SelectAllReciepts(_logger); }
		}
		public virtual IDataBaseCommand<IEnumerable<ProductInfo>> SelectAllProducts {
			get { return new SelectAllProducts(_logger); }
		}
		public virtual IDataBaseCommand<int> InsertEvents {
			get { return new InsertEvents(_logger); }
		}
		public virtual IDataBaseCommand<IEnumerable<string>> SelectAllEventsByKey {
			get { return new SelectAllEventsByKey(_logger); }
		}
		public virtual IDataBaseCommand<int> SelectEventCountByKey
		{
			get{return new SelectEventCountByKey(_logger);}
		} 
	}
}
