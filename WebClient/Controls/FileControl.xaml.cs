using System;
using System.Globalization;
using System.Windows;
using System.Windows.Browser;
using System.Windows.Controls;
using WebClient.ICS.Client.Model;

namespace WebClient.ICS.Client.Controls
{
	/// <summary>
	/// FileControl.
	/// </summary>
	[ScriptableType]
	public partial class FileControl
	{
		private const string DataPropertyName = "Data";
		private const string FilterPropertyName = "FileFilter";
		private const string IsTextFilePropertyName = "IsTextFile";
		private const string TextPropertyName = "Text";
		private const string ButtonTextPropertyName = "ButtonText";
		private const string XmlEditorStatePropertyName = "XmlEditorState";
		private const string EditHeightPropertyName = "EditHeight";
		private const string IsEditEnabledPropertyName = "IsEditEnabled";
		private const string EditStylePropertyName = "EditStyle";

		/// <summary>
		/// Identifies the <see cref="IsTextFile" /> dependency property.
		/// </summary>
		public static readonly DependencyProperty IsTextFileProperty = DependencyProperty.Register(
			IsTextFilePropertyName, typeof (bool), typeof (FileControl), null);

		/// <summary>
		/// Identifies the <see cref="Data" /> dependency property.
		/// </summary>
		public static readonly DependencyProperty DataProperty = DependencyProperty.Register(
			DataPropertyName, typeof (byte[]), typeof (FileControl), new PropertyMetadata(OnDataChangedCallback));

		/// <summary>
		/// Identifies the <see cref="FileFilter" /> dependency property.
		/// </summary>
		public static readonly DependencyProperty FilterProperty = DependencyProperty.Register(
			FilterPropertyName, typeof (string), typeof (FileControl), null);

		/// <summary>
		/// Identifies the <see cref="Text" /> dependency property.
		/// </summary>
		public static readonly DependencyProperty TextProperty = DependencyProperty.Register(
			TextPropertyName, typeof (string), typeof (FileControl), new PropertyMetadata(OnTextChangedCallback));

		/// <summary>
		/// Identifies the <see cref="XmlEditorState" /> dependency property.
		/// </summary>
		public static readonly DependencyProperty XmlEditorStateProperty = DependencyProperty.Register(
			XmlEditorStatePropertyName, typeof(Visibility), typeof(FileControl), null);

		/// <summary>
		/// Identifies the <see cref="ButtonText" /> dependency property.
		/// </summary>
		public static readonly DependencyProperty ButtonTextProperty = DependencyProperty.Register(
			ButtonTextPropertyName, typeof(string), typeof(FileControl), null);

		/// <summary>
		/// Identifies the <see cref="EditHeight" /> dependency property.
		/// </summary>
		public static readonly DependencyProperty EditHeightProperty = DependencyProperty.Register(
			EditHeightPropertyName, typeof (int), typeof (FileControl),
			new PropertyMetadata(OnEditHeightChangedCallback));

		/// <summary>
		/// Identifies the <see cref="IsEditEnabled" /> dependency property.
		/// </summary>
		public static readonly DependencyProperty IsEditEnabledProperty = DependencyProperty.Register(
			IsEditEnabledPropertyName, typeof (bool), typeof (FileControl),
			new PropertyMetadata(true, OnIsEnabledCallback));

		/// <summary>
		/// Identifies the <see cref="EditStyle" /> dependency property.
		/// </summary>
		public static readonly DependencyProperty EditStyleProperty = DependencyProperty.Register(
			EditStylePropertyName, typeof (Style), typeof (FileControl), new PropertyMetadata(OnStyleChangedCallback));

		public FileControl()
		{
			InitializeComponent();
			FileText.TextChanged += FileTextTextChanged;
		}

		/// <summary>
		/// Высота текстового поля.
		/// </summary>
		public int EditHeight
		{
			get { return (int) GetValue(EditHeightProperty); }
			set { SetValue(EditHeightProperty, value); }
		}

		/// <summary>
		/// Текст кнопки
		/// </summary>
		public string ButtonText
		{
			get { return (string) GetValue(ButtonTextProperty); }
			set { SetValue(ButtonTextProperty, value); }
		}

		/// <summary>
		/// Видимость кнопки для открытия xml редактора.
		/// </summary>
		public Visibility XmlEditorState
		{
			get { return (Visibility)GetValue(XmlEditorStateProperty); }
			set { SetValue(XmlEditorStateProperty, value); }
		}

		/// <summary>
		/// Содержимое файла в текстовом виде, если установлено свойство <see cref="IsTextFile"/>
		/// </summary>
		public string Text
		{
			get { return (string) GetValue(TextProperty); }
			set { SetValue(TextProperty, value); }
		}

		/// <summary>
		/// Массив байтов - содержимое файла.
		/// </summary>
		public byte[] Data
		{
			get { return (byte[]) GetValue(DataProperty); }
			set { SetValue(DataProperty, value); }
		}

		/// <summary>
		/// File filter.
		/// </summary>
		public string FileFilter
		{
			get { return (string) GetValue(FilterProperty); }
			set { SetValue(FilterProperty, value); }
		}

		/// <summary>
		/// Свойство указывает, что результат загрузки необходимо записать в текстовом виде в поле <see cref="Text"/>
		/// </summary>
		public bool IsTextFile
		{
			get { return (bool) GetValue(IsTextFileProperty); }
			set { SetValue(IsTextFileProperty, value); }
		}

		/// <summary>
		/// Enabled текстовое поле.
		/// </summary>
		public bool IsEditEnabled
		{
			get { return (bool) GetValue(IsEditEnabledProperty); }
			set { SetValue(IsEditEnabledProperty, value); }
		}

		/// <summary>
		/// Стиль для редактирования значения поля.
		/// </summary>
		public Style EditStyle
		{
			get { return (Style) GetValue(EditStyleProperty); }
			set { SetValue(EditStyleProperty, value); }
		}

		/// <summary>
		/// Вызывается при изменении текстового поля.
		/// </summary>
		public event TextChangedEventHandler TextChanged;

		/// <summary>
		/// Функция, вызываемая из JS.
		/// </summary>
		/// <param name="text"></param>
		[ScriptableMember]
		public void ChangeXml(string text)
		{
			if (IsTextFile)
			{
				FileText.Text = text;
			}
		}

		private void ShowBrowser(string xmlContent)
		{
			var scriptObject = (ScriptObject) HtmlPage.Window.GetProperty("ShowEditor");
			scriptObject.InvokeSelf(xmlContent);
		}

		private void OpenXmlEditor(object sender, RoutedEventArgs e)
		{
			if (IsTextFile)
			{
				ShowBrowser(FileText.Text);
			}
		}

		private void OpenFileDialogClick(object sender, RoutedEventArgs e)
		{
			var openFileDialog = new OpenFileDialog
			{
				Filter = FileFilter,
				FilterIndex = 1,
				Multiselect = true
			};
			try
			{
				var userClickedOk = openFileDialog.ShowDialog();
				if (userClickedOk != true)
					return;

				if (IsTextFile)
				{
					using (var streamReader = openFileDialog.File.OpenText())
					{
						FileText.Text = streamReader.ReadToEnd();
					}
				}
				else
				{
					using (var fileStream = openFileDialog.File.OpenRead())
					{
						var length = fileStream.Length;
						var bytes = new byte[length];
						fileStream.Read(bytes, 0, (int) length);
						Data = bytes;
					}
				}
			}
			catch (Exception exception)
			{
				// Debug.WriteLine(exception);
			}
		}

		private void FileTextTextChanged(object sender, TextChangedEventArgs e)
		{
			if (!IsEditEnabled) return;
			if (string.CompareOrdinal(FileText.Text, Text) == 0) return;
			Text = FileText.Text;

			InvokeTextChanged(e);
		}

		private void InvokeTextChanged(TextChangedEventArgs e)
		{
			var handler = TextChanged;
			if (handler != null)
				handler(FileText, e);
		}

		private static void OnEditHeightChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			d.Cast<FileControl>().FileText.Height = e.NewValue.Cast<int>();
		}

		private static void OnIsEnabledCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			d.Cast<FileControl>().FileText.IsEnabled = e.NewValue.Cast<bool>();
		}

		private static void OnDataChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			var str = "null";
			var value = e.NewValue.TryCast<byte[]>();
			if (value != null)
				str = value.Length.ToString(CultureInfo.InvariantCulture);
			d.Cast<FileControl>().FileText.Text = string.Format("Length: {0} bytes", str);
		}

		private static void OnTextChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			var fileText = d.Cast<FileControl>().FileText;
			var text = e.NewValue == null ? string.Empty : e.NewValue.Cast<string>();
			if (fileText != null && string.CompareOrdinal(fileText.Text, text) != 0)
				fileText.Text = text;
		}

		private static void OnStyleChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			d.Cast<FileControl>().FileText.Style = e.NewValue.TryCast<Style>();
		}
	}
}