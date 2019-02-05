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
        }

        private void BtClose_OnClick(object sender, RoutedEventArgs e)
        {
            UserConfigFile.SetValue(LangItem, "OnSelectionVariant", CbOn.SelectedIndex.ToString(), false);
            UserConfigFile.SetValue(LangItem, "OffSelectionVariant", CbOff.SelectedIndex.ToString(), false);
            UserConfigFile.SaveConfigFile();
            Close();
        }
    }
}
