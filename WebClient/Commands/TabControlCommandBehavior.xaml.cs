using System.Windows;
using System.Windows.Controls;
using WebClient.ICS.Client.Model;

namespace WebClient.ICS.Client.Commands
{
	/// <summary>
	/// 	TabControlSelectionChangedCommandBehavior.
	/// </summary>
	public class TabSelectionCommandBehavior : CommandBehaviorBase<TabControl>
	{
		public TabSelectionCommandBehavior(TabControl targetObject)
			: base(targetObject)
		{
			targetObject.SelectionChanged += TargetObjectSelectionChanged;
		}

		private void TargetObjectSelectionChanged(object sender, SelectionChangedEventArgs e)
		{
			CommandParameter = e.AddedItems[0].Cast<TabItem>().Content.Cast<FrameworkElement>().DataContext;
			base.ExecuteCommand();
		}
	}
}