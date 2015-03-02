using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Owin.Hosting;

namespace UI.Web {
	class Program {
		static void Main(string[] args) {
			const string url = "http://+:8080";

			using (WebApp.Start<Startup>(url)) {
				Console.WriteLine("Running on {0}", url);
				Console.WriteLine("Press enter to exit");
				Console.ReadLine();
			}
		}
	}
}
