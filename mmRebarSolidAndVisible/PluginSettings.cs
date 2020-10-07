namespace mmRebarSolidAndVisible
{
    using System;
    using ModPlusAPI;
    using ModPlusAPI.Mvvm;

    /// <summary>
    /// Пользовательские настройки плагина
    /// </summary>
    public class PluginSettings : VmBase
    {
        private readonly string _langItem;

        public PluginSettings()
        {
            _langItem = new ModPlusConnector().Name;
        }

        /// <summary>
        /// Активность опции "Арматура как тело"
        /// </summary>
        public bool IsActiveChangeSolidInViewProperty
        {
            get => !bool.TryParse(UserConfigFile.GetValue(_langItem, nameof(IsActiveChangeSolidInViewProperty)), out var b) || b;
            set
            {
                UserConfigFile.SetValue(_langItem, nameof(IsActiveChangeSolidInViewProperty), value.ToString(), true);
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Активность опции "Показать неперекрытой"
        /// </summary>
        public bool IsActiveChangeUnobscuredInViewProperty
        {
            get => !bool.TryParse(UserConfigFile.GetValue(_langItem, nameof(IsActiveChangeUnobscuredInViewProperty)), out var b) || b;
            set
            {
                UserConfigFile.SetValue(_langItem, nameof(IsActiveChangeUnobscuredInViewProperty), value.ToString(), true);
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Показать как тело на 3D виде
        /// </summary>
        public bool ShowAsSolidInThreeDView
        {
            get => !bool.TryParse(UserConfigFile.GetValue(_langItem, nameof(ShowAsSolidInThreeDView)), out var b) || b;
            set
            {
                UserConfigFile.SetValue(_langItem, nameof(ShowAsSolidInThreeDView), value.ToString(), true);
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Показать как линии на 3D виде
        /// </summary>
        public bool ShowAsLineInThreeDView
        {
            get => bool.TryParse(UserConfigFile.GetValue(_langItem, nameof(ShowAsLineInThreeDView)), out var b) && b;
            set
            {
                UserConfigFile.SetValue(_langItem, nameof(ShowAsLineInThreeDView), value.ToString(), true);
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Показать неперекрытой
        /// </summary>
        public bool ShowAsUnobscured
        {
            get => !bool.TryParse(UserConfigFile.GetValue(_langItem, nameof(ShowAsUnobscured)), out var b) || b;
            set
            {
                UserConfigFile.SetValue(_langItem, nameof(ShowAsUnobscured), value.ToString(), true);
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Показать скрытой (противоположность неперекрытой)
        /// </summary>
        public bool ShowAsHidden
        {
            get => bool.TryParse(UserConfigFile.GetValue(_langItem, nameof(ShowAsHidden)), out var b) && b;
            set
            {
                UserConfigFile.SetValue(_langItem, nameof(ShowAsHidden), value.ToString(), true);
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Вариант обработки элементов
        /// </summary>
        public ElementsProcessVariant ElementsProcessVariant
        {
            get => Enum.TryParse(
                UserConfigFile.GetValue(_langItem, nameof(ElementsProcessVariant)), out ElementsProcessVariant processVariant)
                ? processVariant
                : ElementsProcessVariant.AllElementsOnView;
            set
            {
                UserConfigFile.SetValue(_langItem, nameof(ElementsProcessVariant), value.ToString(), true);
                OnPropertyChanged();
            }
        }
    }
}
