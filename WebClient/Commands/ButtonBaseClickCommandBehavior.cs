using System.Windows;
using System.Windows.Controls.Primitives;
using System.Windows.Input;

namespace WebClient.ICS.Client.Commands
{
	/// <summary>
	/// Behavior that allows controls that derrive from <see cref="ButtonBase" /> 
	/// to hook up with <see cref="ICommand" /> objects.
	/// </summary>
	/// <remarks>
	/// 	This Behavior is required in Silverlight, because Silverlight does not have Commanding support.
	/// </remarks>
	public class ButtonBaseClickCommandBehavior : CommandBehaviorBase<ButtonBase>
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="ButtonBaseClickCommandBehavior" /> 
		/// class and hooks up the Click event of <paramref name="clickableObject" /> to the ExecuteCommand() method.
		/// </summary>
		/// <param name="clickableObject"> The clickable object. </param>
		public ButtonBaseClickCommandBehavior(ButtonBase clickableObject) : base(clickableObject)
		{
			clickableObject.Click += OnClick;
		}

		private void OnClick(object sender, RoutedEventArgs e)
		{
			ExecuteCommand();
		}
	}
}