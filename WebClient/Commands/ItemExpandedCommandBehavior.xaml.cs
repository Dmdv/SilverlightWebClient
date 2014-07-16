using System;
using System.Windows;
using WebClient.ICS.Client.Controls;

namespace WebClient.ICS.Client.Commands
{
	/// <summary>
	/// 	Tree view поведение.
	/// </summary>
	public class ItemExpandedCommandBehavior : CommandBehaviorBase<LazyTreeView>
	{
		public ItemExpandedCommandBehavior(LazyTreeView targetObject)
			: base(targetObject)
		{
			targetObject.ItemExpanded += TargetObjectItemExpanded;
		}

		private void TargetObjectItemExpanded(object sender, EventArgs e)
		{
			CommandParameter = ((FrameworkElement) sender).DataContext;
			base.ExecuteCommand();
		}
	}
}