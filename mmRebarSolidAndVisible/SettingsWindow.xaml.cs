namespace mmRebarSolidAndVisible
{
    using ModPlusAPI.Abstractions;

    public partial class SettingsWindow : IClosable
    {
        public SettingsWindow()
        {
            InitializeComponent();
            Title = ModPlusAPI.Language.GetFunctionLocalName(new ModPlusConnector());
        }
    }
}
