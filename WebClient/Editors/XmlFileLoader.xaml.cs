using System;
using System.Windows.Browser;
using WebClient.ICS.Client.Commands;

namespace WebClient.ICS.Client.Editors
{
	/// <summary>
	/// Контрол содержит окно с xml данными.
	/// </summary>
	public partial class XmlFileLoader : IActiveAware
	{
		private bool _isActive;

		public XmlFileLoader()
		{
			InitializeComponent();
			HtmlPage.RegisterScriptableObject("Page", XmlFileLoaderControl);
		}

		public bool IsActive
		{
			get { return _isActive; }
			set
			{
				_isActive = value;
				InvokeIsActiveChanged(null);
			}
		}

		public event EventHandler IsActiveChanged;

		private void InvokeIsActiveChanged(EventArgs e)
		{
			var handler = IsActiveChanged;
			if (handler != null)
				handler(this, e);
		}
	}
}