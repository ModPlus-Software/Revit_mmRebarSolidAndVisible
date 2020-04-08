#pragma warning disable SA1600 // Elements should be documented
namespace mmRebarSolidAndVisible
{
    using System;
    using System.Collections.Generic;
    using ModPlusAPI.Interfaces;

    public class ModPlusConnector : IModPlusFunctionInterface
    {
        public SupportedProduct SupportedProduct => SupportedProduct.Revit;

        public string Name => "mmRebarSolidAndVisible";

#if R2015
        public string AvailProductExternalVersion => "2015";
#elif R2016
        public string AvailProductExternalVersion => "2016";
#elif R2017
        public string AvailProductExternalVersion => "2017";
#elif R2018
        public string AvailProductExternalVersion => "2018";
#elif R2019
        public string AvailProductExternalVersion => "2019";
#elif R2020
        public string AvailProductExternalVersion => "2020";
#elif R2021
        public string AvailProductExternalVersion => "2021";
#endif
        
        public string FullClassName => "mmRebarSolidAndVisible.RebarSolidAndVisible";

        public string AppFullClassName => string.Empty;

        public Guid AddInId => Guid.Empty;

        public string LName => "Арматура как тело";

        public string Description => "Изменение состояния видимости арматуры (Показать неперекрытой и Показать как тело)";

        public string Author => "Пекшев Александр aka Modis";

        public string Price => "0";

        public bool CanAddToRibbon => true;

        public string FullDescription => "Состояние видимости Показать неперекрытой устанавливается на любом виде - как 2D, так и 3D. Показать как тело - устанавливается только на 3D виде";

        public string ToolTipHelpImage => string.Empty;

        public List<string> SubFunctionsNames => new List<string>
        {
            "mmRebarSolidAndVisibleOff",
            "mmRebarSolidAndVisibleSettings"
        };

        public List<string> SubFunctionsLames => new List<string>
        {
            "Отменить видимость",
            "Настройки выбора элементов"
        };

        public List<string> SubDescriptions => new List<string>
        {
            "Отмена состояния видимости арматуры (Показать неперекрытой и Показать как тело)",
            "Настройки вариантов выбора элементов, содержащих арматуру, при работе функций"
        };

        public List<string> SubFullDescriptions => new List<string> { string.Empty, string.Empty };

        public List<string> SubHelpImages => new List<string> { string.Empty, string.Empty };

        public List<string> SubClassNames => new List<string>
        {
            "mmRebarSolidAndVisible.RebarSolidAndVisibleOff",
            "mmRebarSolidAndVisible.RebarSolidAndVisibleSettings"
        };
    }
}
#pragma warning restore SA1600 // Elements should be documented