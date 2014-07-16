using System;
using System.Diagnostics.CodeAnalysis;
using System.Windows;
using System.Windows.Threading;

namespace WebClient.ICS.Client.Commands
{
	/// <summary>
	/// 	Hides the dispatcher mis-match between Silverlight and .Net, largely so code reads a bit easier
	/// </summary>
	internal class DispatcherProxy
	{
		private readonly Dispatcher _innerDispatcher;

		private DispatcherProxy(Dispatcher dispatcher)
		{
			_innerDispatcher = dispatcher;
		}

		/// <summary>
		/// CreateDispatcher.
		/// </summary>
		public static DispatcherProxy CreateDispatcher()
		{
			return new DispatcherProxy(Deployment.Current.Dispatcher);
		}

		/// <summary>
		/// CheckAccess.
		/// </summary>
		public bool CheckAccess()
		{
			return _innerDispatcher.CheckAccess();
		}

		// TODO: Port of VS 2010 (AdeM)
		// Suppress error. Investigate if this is the right fix. May be moot if we target 4.0 by default or were OK
		// with this in Prism 2.0
		//
		// Error	39	CA1903 : Microsoft.Portability : 
		// Member 'WeakEventHandlerManager.DispatcherProxy.BeginInvoke(Delegate, params object[])' uses member 
		// 'Dispatcher.BeginInvoke(Delegate, DispatcherPriority, params object[])'. 
		// Because this member was introduced 
		// in .NET Framework 3.0 Service Pack 2, which was not included in the project's target framework, .NET 
		// Framework 3.5, your application may fail to run on systems without this service pack installed.	

		/// <summary>
		/// BeginInvoke.
		/// </summary>
		[SuppressMessage("Microsoft.Portability", "CA1903:UseOnlyApiFromTargetedFramework",
			MessageId = "System.Windows.Threading.Dispatcher.#BeginInvoke" +
				"(System.Delegate,System.Windows.Threading.DispatcherPriority,System.Object[])")]
		public DispatcherOperation BeginInvoke(Delegate method, params object[] args)
		{
			return _innerDispatcher.BeginInvoke(method, args);
		}
	}
}