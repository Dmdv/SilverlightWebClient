using System;
using System.Windows;
using WebClient.ICS.Client.Controls;

namespace WebClient.ICS.Client.Commands
{
	/// <summary>
	/// 	ItemClickedCommandBehavior.
	/// </summary>
	public class ItemClickedCommandBehavior : CommandBehaviorBase<LazyTreeView>
	{
		public ItemClickedCommandBehavior(LazyTreeView targetObject)
			: base(targetObject)
		{
			targetObject.ItemClicked += OnItemClicked;
		}

		private void OnItemClicked(object sender, EventArgs e)
		{
			CommandParameter = ((FrameworkElement) sender).DataContext;
			base.ExecuteCommand();
		}
	}
}