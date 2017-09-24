using System;
using System.Collections.Generic;
using ModPlusAPI.Interfaces;

namespace mmRebarSolidAndVisible
{
    class Interface : IModPlusFunctionInterface
    {
        public SupportedProduct SupportedProduct => SupportedProduct.Revit;
        public string Name => "mmRebarSolidAndVisible";
        public string AvailProductExternalVersion => "2015";
        public string FullClassName => "mmRebarSolidAndVisible.RebarSolidAndVisible";
        public string AppFullClassName => string.Empty;
        public Guid AddInId => Guid.Empty;
        public string LName => "Арматура как тело";
        public string Description => "Изменение состояния видимости арматуры (Показать неперекрытой и Показать как тело) для выбранного элемента на текущем виде";
        public string Author => "Маркевич Максим";
        public string Price => "0";
        public bool CanAddToRibbon => true;
        public string FullDescription => "Состояние видимости Показать неперекрытой устанавливается на любом виде - как 2D, так и 3D. Показать как тело - устанавливается только на 3D виде";
        public string ToolTipHelpImage => string.Empty;
        public List<string> SubFunctionsNames => new List<string>();
        public List<string> SubFunctionsLames => new List<string>();
        public List<string> SubDescriptions => new List<string>();
        public List<string> SubFullDescriptions => new List<string>();
        public List<string> SubHelpImages => new List<string>();
        public List<string> SubClassNames => new List<string>();
    }
}
