using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using WebClient.ICS.Client.Contracts;
using WebClient.ICS.Client.Model;
using WebClient.ICS.Client.ServiceReference;
using WebClient.ICS.Client.ViewModel;

namespace WebClient.ICS.Client.ServiceModel
{
	/// <summary>
	/// ServiceProxy.
	/// </summary>
	public class ServiceProxy : IServiceClient
	{
		public ServiceProxy(IcsContractClient client)
		{
			_client = client;

			_client.GetChildElementsCompleted += (o, args) =>
			{
				var func = args.UserState.Cast<Action<IEnumerable<Element>, string>>();
				func(args.Result.Result, GetMessage(args));
			};

			_client.GetElementCompleted += (o, args) =>
			{
				var func = args.UserState.Cast<Action<Element, string>>();
				func(args.Result.Result, GetMessage(args));
			};

			_client.UpdateElementCompleted += (o, args) =>
			{
				var func = args.UserState.Cast<Action<Element, string>>();
				func(args.Result.Result, GetMessage(args));
			};

			_client.GetVersionCompleted += (o, args) =>
			{
				var func = args.UserState.Cast<Action<string>>();
				func(args.Result);
			};

			_client.CreateElementCompleted += (o, args) =>
			{
				dynamic parameter = args.UserState;
				parameter.Action(args.Result.Result, GetMessage(args), parameter.ParentViewModel);
			};

			_client.GetUserPermissionsCompleted += (o, args) =>
			{
				var func = args.UserState.Cast<Action<IEnumerable<Permission>, string>>();
				func(args.Result.Result, GetMessage(args));
			};

			_client.GetElementPermissionsCompleted += (o, args) =>
			{
				var func = args.UserState.Cast<Action<IEnumerable<Permission>, string>>();
				func(args.Result.Result, GetMessage(args));
			};

			_client.UpdatePermissionCompleted += (o, args) =>
			{
				dynamic parameter = args.UserState;

				Guard.NotNull(parameter, "parameter");
				Guard.NotNull(parameter.Action, "Action");
				Guard.NotNull(parameter.UserName, "UserName");
				Guard.NotNull(args.Result, "Result");

				var func = parameter.Action;
				var userName = parameter.UserName;
				var permission = args.Result.Result;

				func(userName, permission, GetMessage(args));
			};

			_client.AddPermissionCompleted += (o, args) =>
			{
				var func = args.UserState.Cast<Action<Permission, string>>();
				func(args.Result.Result, GetMessage(args));
			};

			_client.GetPermissionCompleted += (o, args) =>
			{
				var func = args.UserState.Cast<Action<Permission, string>>();
				func(args.Result.Result, GetMessage(args));
			};

			_client.DeletePermissionCompleted += (o, args) =>
			{
				dynamic param = args.UserState;
				param.Action(param.Permission, GetMessage(args));
			};

			_client.GetUsersCompleted += (o, args) =>
			{
				var func = args.UserState.Cast<Action<IEnumerable<User>, string>>();
				func(args.Result.Result, GetMessage(args));
			};

			_client.DeleteElementCompleted += (o, args) =>
			{
				dynamic param = args.UserState;
				param.Action(param.SettingNode, GetMessage(args));
			};

			_client.DeleteUserCompleted += (o, args) =>
			{
				var func = args.UserState.Cast<Action<string>>();
				func(GetMessage(args));
			};

			_client.GenerateScriptCompleted += (o, args) =>
			{
				dynamic param = args.UserState;
				param.Action(args.Result.Result, param.Stream, GetMessage(args));
			};

			_client.ApplyScriptCompleted += (o, args) =>
			{
				var func = args.UserState.Cast<Action<string>>();
				func(GetMessage(args));
			};

			_client.GetElementHistoryCompleted += (o, args) =>
			{
				var func = args.UserState.Cast<Action<ObservableCollection<HistoryInfo>, string>>();
				func(args.Result.Result, GetMessage(args));
			};

			_client.GetHistoryRecordsCompleted += (o, args) =>
			{
				var func = args.UserState.Cast<Action<ObservableCollection<HistoryRecord>, string>>();
				func(args.Result.Result, GetMessage(args));
			};

			_client.GetElementSubscriptionsCompleted += (o, args) =>
			{
				var func = args.UserState.Cast<Action<ObservableCollection<Subscription>, string>>();
				func(args.Result.Result, GetMessage(args));
			};

			_client.UpdateSubscriptionCompleted += (o, args) =>
			{
				var func = args.UserState.Cast<Action<Subscription, string>>();
				func(args.Result.Result, GetMessage(args));
			};

			_client.DeleteSubscriptionCompleted += (o, args) =>
			{
				var func = args.UserState.Cast<Action<string>>();
				func(GetMessage(args));
			};

			_client.CreateSubscriptionCompleted += (o, args) =>
			{
				var func = args.UserState.Cast<Action<Subscription, string>>();
				func(args.Result.Result, GetMessage(args));
			};

			_client.GetSubscriptionCompleted += (o, args) =>
			{
				var func = args.UserState.Cast<Action<Subscription, string>>();
				func(args.Result.Result, GetMessage(args));
			};

			_client.GetUserSubscriptionsCompleted += (o, args) =>
			{
				var func = args.UserState.Cast<Action<ObservableCollection<Subscription>, string>>();
				func(args.Result.Result, GetMessage(args));
			};
		}

		/// <summary>
		/// Enumerate child elements of tree settings.
		/// </summary>
		public void GetChildElements(Node parent, Action<IEnumerable<Element>, string> func)
		{
			_client.GetChildElementsAsync(parent, func);
		}

		/// <summary>
		/// Create settings element.
		/// </summary>
		public void CreateElement(
			Node parent,
			Element newElement,
			SettingNodeViewModel parentViewModel,
			Action<Element, string, SettingNodeViewModel> func)
		{
			Guard.NotNull(parent);
			Guard.NotNull(newElement);

			var parameter = new
			{
				Action = func,
				ParentViewModel = parentViewModel
			};
			_client.CreateElementAsync(parent, newElement, parameter);
		}

		/// <summary>
		/// UpdateElement.
		/// </summary>
		public void UpdateElement(Element element, Action<Element, string> func)
		{
			Guard.NotNull(element);
			_client.UpdateElementAsync(element, func);
		}

		/// <summary>
		/// DeleteElement.
		/// </summary>
		public void DeleteElement(
			Element element,
			SettingNodeViewModel settingNode,
			Action<SettingNodeViewModel, string> func)
		{
			Guard.NotNull(element);
			var param = new
			{
				Action = func,
				SettingNode = settingNode
			};
			_client.DeleteElementAsync(element, param);
		}

		/// <summary>
		/// Updates element.
		/// </summary>
		public void GetElement(Element element, Action<Element, string> func)
		{
			Guard.NotNull(element);
			_client.GetElementAsync(element, func);
		}

		/// <summary>
		/// GetVersion.
		/// </summary>
		public void GetVersion(Action<string> func)
		{
			_client.GetVersionAsync(func);
		}

		/// <summary>
		/// GetUserPermissions
		/// </summary>
		public void GetUserPermissions(User user, Action<IEnumerable<Permission>, string> func)
		{
			Guard.NotNull(user);
			_client.GetUserPermissionsAsync(user, func);
		}

		/// <summary>
		/// GetElementPermissions
		/// </summary>
		public void GetElementPermissions(Element element, Action<IEnumerable<Permission>, string> func)
		{
			Guard.NotNull(element);
			_client.GetElementPermissionsAsync(element, func);
		}

		/// <summary>
		/// UpdatePermission.
		/// </summary>
		/// <param name="permission"></param>
		/// <param name="func">1: OldPermission, 2: NewPermission, 3: Message</param>
		public void UpdatePermission(Permission permission, Action<string, Permission, string> func)
		{
			Guard.NotNull(permission);
			var commandParameter = new
			{
				Action = func,
				UserName = permission.User.Name
			};
			_client.UpdatePermissionAsync(permission, commandParameter);
		}

		/// <summary>
		/// AddPermission
		/// </summary>
		public void AddPermission(Element element, Permission permission, Action<Permission, string> func)
		{
			Guard.NotNull(element);
			_client.AddPermissionAsync(element, permission, func);
		}

		/// <summary>
		/// GetPermission
		/// </summary>
		public void GetPermission(Permission permission, Action<Permission, string> func)
		{
			Guard.NotNull(permission);
			_client.GetPermissionAsync(permission, func);
		}

		/// <summary>
		/// DeletePermission.
		/// </summary>
		public void DeletePermission(Permission permission, Action<Permission, string> func)
		{
			Guard.NotNull(permission);
			var param = new
			{
				Action = func,
				Permission = permission
			};
			_client.DeletePermissionAsync(permission, param);
		}

		/// <summary>
		/// GetUsers.
		/// </summary>
		/// <param name="func"></param>
		public void GetUsers(Action<IEnumerable<User>, string> func)
		{
			_client.GetUsersAsync(func);
		}

		/// <summary>
		/// DeleteUser.
		/// </summary>
		public void DeleteUser(User user, Action<string> func)
		{
			Guard.NotNull(user);
			_client.DeleteUserAsync(user, func);
		}

		/// <summary>
		/// GenerateScript.
		/// </summary>
		public void GenerateScript(Node element, TextWriter stream, Action<string, TextWriter, string> func)
		{
			Guard.NotNull(element);
			var param = new
			{
				Stream = stream,
				Action = func
			};
			_client.GenerateScriptAsync(element, param);
		}

		/// <summary>
		/// ApplyScript.
		/// </summary>
		public void ApplyScript(Node element, string script, Action<string> func)
		{
			Guard.NotNull(element);
			_client.ApplyScriptAsync(element, script, func);
		}

		/// <summary>
		/// GetElementHistory.
		/// </summary>
		public void GetElementHistory(
			Element element,
			string successfull,
			DateTime from,
			DateTime to,
			string userFilter,
			string operationFilter,
			int max,
			Action<ObservableCollection<HistoryInfo>, string> func)
		{
			Guard.NotNull(element);
			_client.GetElementHistoryAsync(element, successfull, from, to, userFilter, operationFilter, max, func);
		}

		/// <summary>
		/// GetHistoryRecords.
		/// </summary>
		public void GetHistoryRecords(HistoryInfo historyInfo, Action<ObservableCollection<HistoryRecord>, string> func)
		{
			Guard.NotNull(historyInfo);
			_client.GetHistoryRecordsAsync(historyInfo, func);
		}

		/// <summary>
		/// GetElementSubscriptions.
		/// </summary>
		public void GetElementSubscriptions(Element element, Action<ObservableCollection<Subscription>, string> func)
		{
			Guard.NotNull(element);
			_client.GetElementSubscriptionsAsync(element, func);
		}

		/// <summary>
		/// UpdateSubscription.
		/// </summary>
		public void UpdateSubscription(Subscription subscription, Action<Subscription, string> func)
		{
			Guard.NotNull(subscription);
			_client.UpdateSubscriptionAsync(subscription, func);
		}

		/// <summary>
		/// DeleteSubscription.
		/// </summary>
		public void DeleteSubscription(Subscription subscription, Action<string> func)
		{
			Guard.NotNull(subscription);
			_client.DeleteSubscriptionAsync(subscription, func);
		}

		/// <summary>
		/// CreateSubscription.
		/// </summary>
		public void CreateSubscription(Element element, Subscription subscription, Action<Subscription, string> func)
		{
			Guard.NotNull(element);
			_client.CreateSubscriptionAsync(element, subscription, func);
		}

		/// <summary>
		/// GetSubscription.
		/// </summary>
		public void GetSubscription(Subscription subscription, Action<Subscription, string> func)
		{
			Guard.NotNull(subscription);
			_client.GetSubscriptionAsync(subscription, func);
		}

		/// <summary>
		/// GetUserSubscriptions.
		/// </summary>
		public void GetUserSubscriptions(User user, Action<ObservableCollection<Subscription>, string> func)
		{
			Guard.NotNull(user);
			_client.GetUserSubscriptionsAsync(user, func);
		}

		private static string GetMessage(dynamic args)
		{
			return args.Result.Code != 0 ? args.Result.Message : string.Empty;
		}

		private readonly IcsContractClient _client;
	}
}