using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using WebClient.ICS.Client.Model;
using WebClient.ICS.Client.ViewModel;

namespace WebClient.ICS.Client.Controls
{
	/// <summary>
	/// Контрол, размещающий в дочернем элементе контролы для валидации.
	/// Если не XML, то контрол загрузки XSD скрывается.
	/// </summary>
	public partial class ValidatorGrid
	{
		/// <summary>
		/// The <see cref="ValidatorsData" /> dependency property's name.
		/// </summary>
		private const string ValidatorsPropertyName = "ValidatorsData";

		/// <summary>
		/// Identifies the <see cref="ValidatorsData" /> dependency property.
		/// </summary>
		public static readonly DependencyProperty ValidatorsDataProperty = DependencyProperty.Register(
			ValidatorsPropertyName,
			typeof (IEnumerable<ValidatorViewModel>),
			typeof (ValidatorGrid),
			new PropertyMetadata(OnValidatorsCallback));

		public ValidatorGrid()
		{
			InitializeComponent();
		}

		/// <summary>
		/// Gets or sets the value of the <see cref="ValidatorsData" />
		/// property. This is a dependency property.
		/// </summary>
		public IEnumerable<ValidatorViewModel> ValidatorsData
		{
			get { return (IEnumerable<ValidatorViewModel>) GetValue(ValidatorsDataProperty); }
			set { SetValue(ValidatorsDataProperty, value); }
		}

		private static void OnValidatorsCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			var validatorGrid = d.Cast<ValidatorGrid>();

			ClearChildren(validatorGrid);

			if (e.NewValue == null)
				return;

			foreach (var validatorViewModel in e.NewValue.Cast<IEnumerable<ValidatorViewModel>>())
			{
				validatorGrid.AddValidator(validatorViewModel);
			}
		}

		/// <summary>
		/// Очистить дочерние контролы.
		/// </summary>
		/// <param name="validatorGrid"></param>
		private static void ClearChildren(ValidatorGrid validatorGrid)
		{
			foreach (var grid in validatorGrid.Container.Children)
			{
				var uiElements = grid.Cast<Grid>().Children.ToList();
				grid.Cast<Grid>().Children.Clear();

				var textBlock = uiElements[0].Cast<TextBlock>();
				textBlock.DataContext = null;

				var textBox = uiElements[1].TryCast<TextBox>();
				if (textBox != null)
				{
					textBox.TextChanged -= OnTextChanged;
					textBox.ResetBinding(TextBox.TextProperty);
					textBox.DataContext = null;
				}
				else
				{
					var fileControl = uiElements[1].Cast<FileControl>();
					fileControl.TextChanged -= OnTextChanged;
					fileControl.SetBinding(FileControl.TextProperty, new Binding());
					// fileControl.ResetBinding(FileControl.TextProperty);
					fileControl.DataContext = null;
				}
			}

			validatorGrid.Container.Children.Clear();
		}

		/// <summary>
		/// Добавить строку для валидатора.
		/// </summary>
		/// <param name="validator"></param>
		private void AddValidator(ValidatorViewModel validator)
		{
			switch (validator.EditType)
			{
				case EditType.XmlFileLoader:
					CreateXmlValidator(validator);
					break;
				case EditType.Text:
					CreateStringValidator(validator);
					break;
			}
		}

		private void CreateXmlValidator(ValidatorViewModel validator)
		{
			var grid = new Grid
			{
				Margin = new Thickness(0, 0, 0, 2)
			};
			grid.RowDefinitions.Add(new RowDefinition());
			grid.RowDefinitions.Add(new RowDefinition());

			var block = CreateTextBlock(validator);
			var fileLoader = CreateXsdLoaderControl(validator);

			Grid.SetRow(block, 0);
			Grid.SetRow(fileLoader, 1);

			grid.Children.Add(block);
			grid.Children.Add(fileLoader);

			Container.Children.Add(grid);
		}

		private void CreateStringValidator(ValidatorViewModel validator)
		{
			var grid = new Grid
			{
				Margin = new Thickness(0, 0, 0, 2)
			};
			grid.RowDefinitions.Add(new RowDefinition());
			grid.RowDefinitions.Add(new RowDefinition());

			var block = CreateTextBlock(validator);
			var text = CreateTextBox(validator);

			Grid.SetRow(block, 0);
			Grid.SetRow(text, 1);

			grid.Children.Add(block);
			grid.Children.Add(text);

			Container.Children.Add(grid);
		}

		/// <summary>
		/// Создать поле для имени валидатора.
		/// </summary>
		/// <param name="validator"></param>
		/// <returns></returns>
		private static TextBlock CreateTextBlock(ValidatorViewModel validator)
		{
			var block = new TextBlock
			{
				Margin = new Thickness(0, 0, 0, 2)
			};
			block.SetBinding(TextBlock.TextProperty, CreateNameBinding());
			block.DataContext = validator;
			return block;
		}

		/// <summary>
		/// Создать поле для значения валидатора.
		/// </summary>
		/// <param name="validator"></param>
		/// <returns></returns>
		private static TextBox CreateTextBox(ValidatorViewModel validator)
		{
			var text = new TextBox();

			text.SetBinding(TextBox.TextProperty, CreateValueBinding());
			text.TextWrapping = TextWrapping.Wrap;
			text.MaxHeight = 300;
			text.VerticalScrollBarVisibility = ScrollBarVisibility.Auto;
			text.DataContext = validator;
			text.TextChanged += OnTextChanged;
			return text;
		}

		private static FileControl CreateXsdLoaderControl(ValidatorViewModel validator)
		{
			var xsdLocaderControl = new FileControl
			{
				ButtonText = "Load XSD ...",
				XmlEditorState = Visibility.Collapsed,
				IsTextFile = true,
				FileFilter = "XSD Files (.xsd)|*.xsd|All Files (*.*)|*.*",
				DataContext = validator,
				EditStyle = Application.Current.Resources["TextBoxStyle"].Cast<Style>()
			};

			xsdLocaderControl.SetBinding(FileControl.TextProperty, CreateValueBinding());
			xsdLocaderControl.TextChanged += OnTextChanged;
			return xsdLocaderControl;
		}

		/// <summary>
		/// Привязка для имени в TextBlock.
		/// </summary>
		/// <returns></returns>
		private static Binding CreateNameBinding()
		{
			return new Binding("Name")
			{
				Mode = BindingMode.OneWay
			};
		}

		/// <summary>
		/// Привязка для текста в TextBox - значение валидатора.
		/// </summary>
		/// <returns></returns>
		private static Binding CreateValueBinding()
		{
			return new Binding("Value")
			{
				Mode = BindingMode.TwoWay,
				NotifyOnValidationError = true,
				ValidatesOnExceptions = true
			};
		}

		private static void OnTextChanged(object sender, TextChangedEventArgs e)
		{
			var txt = sender as TextBox;
			if (txt == null)
				return;

			var bindingExpression = txt.GetBindingExpression(TextBox.TextProperty);

			if (bindingExpression == null)
				return;
			bindingExpression.UpdateSource();
		}
	}
}