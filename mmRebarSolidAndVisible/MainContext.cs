namespace mmRebarSolidAndVisible
{
    using System.Windows.Input;
    using Autodesk.Revit.DB;
    using Autodesk.Revit.UI;
    using ModPlusAPI;
    using ModPlusAPI.Abstractions;
    using ModPlusAPI.Mvvm;

    /// <summary>
    /// Главный контекст плагина
    /// </summary>
    public class MainContext : VmBase
    {
        private readonly UIApplication _uiApplication;

        public MainContext(UIApplication uiApplication)
        {
            _uiApplication = uiApplication;
            Settings = new PluginSettings();
            Settings.PropertyChanged += (sender, args) =>
            {
                if (args.PropertyName == nameof(PluginSettings.ElementsProcessVariant))
                    OnPropertyChanged(nameof(ProcessButtonText));
            };

            CanChangeRebarAsSolidOnCurrentView = uiApplication.ActiveUIDocument.ActiveView is View3D;
            if (!CanChangeRebarAsSolidOnCurrentView)
                Settings.IsActiveChangeSolidInViewProperty = false;
        }

        /// <summary>
        /// Сохраняемые настройки плагина
        /// </summary>
        public PluginSettings Settings { get; }

        /// <summary>
        /// Можно ли менять свойство "Как тело" на текущем виде
        /// </summary>
        public bool CanChangeRebarAsSolidOnCurrentView { get; }

        /// <summary>
        /// Текст на кнопке в зависимости от варианта работы
        /// </summary>
        public string ProcessButtonText => 
            Language.GetItem(Settings.ElementsProcessVariant == ElementsProcessVariant.AllElementsOnView ? "apply" : "continue");

        /// <summary>
        /// Выполнить изменение свойств арматуры
        /// </summary>
        public ICommand ProcessCommand => new RelayCommand<IClosable>(win =>
        {
            win.Close();
            new MainWork(_uiApplication, Settings).Process();
        });
    }
}
