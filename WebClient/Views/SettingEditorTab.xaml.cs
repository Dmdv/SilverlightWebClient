using WebClient.ICS.Client.Converters;

namespace WebClient.ICS.Client.Views
{
    /// <summary>
    /// SettingEditorTab
    /// </summary>
    public partial class SettingEditorTab
    {
		public SettingEditorTab()
		{
			InitializeComponent();
		    DataTypesSelector.ItemsSource = EnumExtension.DataTypeEnum;
		}
	}
}