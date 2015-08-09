using System.Collections.Generic;
using System.Linq;

namespace UI.Web.Hubs {
	public class ConnectionMapping<T> {

		public static ConnectionMapping<T> Instance { get; private set; }

		private readonly IDictionary<T, HashSet<string>> _connections;
		private ConnectionMapping() {
			_connections = new Dictionary<T, HashSet<string>>();
		}

		static ConnectionMapping() {
			Instance = new ConnectionMapping<T>();
		}

		public int ConnectionCount {
			get {
				return _connections.Count;
			}
		}

		public void Add(T key, string connectionId) {
			lock (_connections) {
				HashSet<string> connections;
				if (!_connections.TryGetValue(key, out connections)) {
					connections = new HashSet<string>();
					_connections.Add(key, connections);
				}

				lock (connections) {
					connections.Add(connectionId);
				}
			}
		}

		public IEnumerable<string> GetConnections(T key) {
			lock (_connections) {
				HashSet<string> connections;
				if (_connections.TryGetValue(key, out connections)) {
					return connections;
				}
			}

			return Enumerable.Empty<string>();
		}

		public void Remove(T key, string connectionId) {
			lock (_connections) {
				HashSet<string> connections;
				if (!_connections.TryGetValue(key, out connections)) {
					return;
				}

				lock (connections) {
					connections.Remove(connectionId);

					if (connections.Count == 0) {
						_connections.Remove(key);
					}
				}
			}
		}
	}
}