using System;
using System.Diagnostics;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Threading;
namespace Core.FclExtensions {
	public static class ThreadEx {

		public static bool AssignCore(this Thread t, int coreId) {
			if (t.ThreadState == System.Threading.ThreadState.Unstarted || coreId < 0 || coreId >= Environment.ProcessorCount)
				throw new ArgumentException("coreId");

			var affinitySet = false;
			var id = GetNativeThreadId(ref t);

			foreach (ProcessThread pt in Process.GetCurrentProcess().Threads) {
				if (pt.Id != id)
					continue;

				pt.ProcessorAffinity = (IntPtr)(1 << coreId);
				affinitySet = true;
				break;
			}

			return affinitySet;
		}

		private static int GetNativeThreadId(ref Thread thread) {
			var f = thread.GetType().GetField("DONT_USE_InternalThread", BindingFlags.GetField | BindingFlags.NonPublic | BindingFlags.Instance);
			//548 and 348 were found by analyzing the memory;

			if (f == null)
				return 0;
			var ptr = (IntPtr)f.GetValue(thread);
			return Marshal.ReadInt32(ptr, (IntPtr.Size == 8) ? 548 : 348);
		}
	}
}
