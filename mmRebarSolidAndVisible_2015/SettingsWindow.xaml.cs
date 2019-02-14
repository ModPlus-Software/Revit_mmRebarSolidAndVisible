namespace mmRebarSolidAndVisible
{
    using System;
    using System.Windows;
    using ModPlusAPI;

    public partial class SettingsWindow 
    {
        private const string LangItem = "mmRebarSolidAndVisible";

        public SettingsWindow()
        {
            InitializeComponent();
            Title = ModPlusAPI.Language.GetFunctionLocalName(LangItem, "Настройки видимости элементов", 2);
            CbOn.SelectedIndex = Enum.TryParse(UserConfigFile.GetValue(LangItem, "OnSelectionVariant"),
                out SelectionVariant sv)
                ? (int)sv
                : (int)SelectionVariant.PickObject;
            CbOff.SelectedIndex = Enum.TryParse(UserConfigFile.GetValue(LangItem, "OffSelectionVariant"), out sv)
                ? (int)sv
                : (int)SelectionVariant.PickObject;
            ChkViewUnobscured.IsChecked =
                !bool.TryParse(UserConfigFile.GetValue(LangItem, "ViewUnobscured"), out var b) || b;
            ChkViewAsSolid.IsChecked =
                !bool.TryParse(UserConfigFile.GetValue(LangItem, "ViewAsSolid"), out b) || b;
        }

        private void BtClose_OnClick(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void SettingsWindow_OnClosed(object sender, EventArgs e)
        {
            UserConfigFile.SetValue(LangItem, "OnSelectionVariant", CbOn.SelectedIndex.ToString(), false);
            UserConfigFile.SetValue(LangItem, "OffSelectionVariant", CbOff.SelectedIndex.ToString(), false);
            UserConfigFile.SetValue(LangItem, "ViewUnobscured", ChkViewUnobscured.IsChecked.ToString(), false);
            UserConfigFile.SetValue(LangItem, "ViewAsSolid", ChkViewAsSolid.IsChecked.ToString(), false);
            UserConfigFile.SaveConfigFile();
        }

        private void ChkViewUnobscured_OnUnchecked(object sender, RoutedEventArgs e)
        {
            ChkViewAsSolid.IsChecked = true;
        }

        private void ChkViewAsSolid_OnUnchecked(object sender, RoutedEventArgs e)
        {
            ChkViewUnobscured.IsChecked = true;
        }
    }
}
