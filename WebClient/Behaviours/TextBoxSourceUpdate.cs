using System.Windows;
using System.Windows.Controls;
using System.Windows.Interactivity;

namespace WebClient.ICS.Client.Behaviours
{
	/// <summary>
	/// 	Обновляет DataSource при редактировании.
	/// </summary>
	public class TextBoxSourceUpdate : Behavior<TextBox>
	{
		private bool _isEnabled;

		protected override void OnAttached()
		{
			base.OnAttached();
			AssociatedObject.TextChanged += OnTextChanged;
			AssociatedObject.LostFocus += OnLostFocus;
			AssociatedObject.GotFocus += OnGotFocus;
		}

		protected override void OnDetaching()
		{
			base.OnDetaching();
			AssociatedObject.TextChanged -= OnTextChanged;
			AssociatedObject.LostFocus -= OnLostFocus;
			AssociatedObject.GotFocus -= OnGotFocus;
		}

		private void OnGotFocus(object sender, RoutedEventArgs e)
		{
			_isEnabled = true;
		}

		private void OnLostFocus(object sender, RoutedEventArgs e)
		{
			_isEnabled = false;
		}

		private void OnTextChanged(object sender, TextChangedEventArgs e)
		{
			var txt = sender as TextBox;
			if (txt == null)
				return;

			var bindingExpression = txt.GetBindingExpression(TextBox.TextProperty);
			if (bindingExpression == null)
				return;

			if (!_isEnabled)
				return;

			bindingExpression.UpdateSource();
		}
	}
}