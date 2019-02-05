using System;
using System.Collections.Generic;
using ModPlusAPI.Interfaces;

namespace mmRebarSolidAndVisible
{
    public class ModPlusConnector : IModPlusFunctionInterface
    {
        public SupportedProduct SupportedProduct => SupportedProduct.Revit;
        
        public string Name => "mmRebarSolidAndVisible";
        
        public string AvailProductExternalVersion => "2019";
        
        public string FullClassName => "mmRebarSolidAndVisible.RebarSolidAndVisible";
        
        public string AppFullClassName => string.Empty;
        
        public Guid AddInId => Guid.Empty;
        
        public string LName => "Арматура как тело";
        
        public string Description => "Изменение состояния видимости арматуры (Показать неперекрытой и Показать как тело)";
        
        public string Author => "Маркевич Максим";
        
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
            "Настройки вариантов выбор элементов, содержащих арматуру, при работе функций"
        };
        
        public List<string> SubFullDescriptions => new List<string> { "", "" };
        
        public List<string> SubHelpImages => new List<string> { "", "" };
        
        public List<string> SubClassNames => new List<string>
        {
            "mmRebarSolidAndVisible.RebarSolidAndVisibleOff",
            "mmRebarSolidAndVisible.RebarSolidAndVisibleSettings"
        };
    }
}
