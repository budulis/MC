using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using Core.Domain;
using ProtoBuf.Meta;

namespace Infrastructure.Initialization {
	public class Bootstrapper : IDisposable {
		private List<Delegate> _tasks;

		public Bootstrapper() {
			_tasks = new List<Delegate>();
		}

		public Bootstrapper RegisterStartupTask(Action task) {
			_tasks.Add(task);
			return this;
		}

		public virtual void RunInitializationTasks() {
			_tasks.ForEach(task => task.DynamicInvoke());
		}

		protected virtual void Dispose(bool disposing) {
			if (disposing) {
				_tasks = null;
			}
		}

		~Bootstrapper()
		{
			Dispose(false);
		}

		public void Dispose() {
			Dispose(true);
			GC.SuppressFinalize(this);
		}

		public static Bootstrapper Default {
			get {
				return new Bootstrapper().RegisterStartupTask(InitProtobufRuntimeTypeModel);
			}
		}

		private static void InitProtobufRuntimeTypeModel() {

			var serializables = typeof(Core.ISerializable).Assembly.GetTypes()
				.Where(x => typeof(Core.ISerializable).IsAssignableFrom(x)
					&& x.IsClass
					&& !RuntimeTypeModel.Default.IsDefined(x));

			foreach (var s in serializables) {
				var properties = s.GetProperties(BindingFlags.Instance | BindingFlags.Public);
				var metaRuntime = RuntimeTypeModel.Default.Add(s, false);
				for (var i = 0; i < properties.Count(); i++)
					metaRuntime.Add(i + 1, properties[i].Name);
			}
		}


	}
}
