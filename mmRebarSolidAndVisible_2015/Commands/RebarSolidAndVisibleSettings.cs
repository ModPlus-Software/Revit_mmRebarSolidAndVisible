// ReSharper disable once CheckNamespace
namespace mmRebarSolidAndVisible
{
    using Autodesk.Revit.Attributes;
    using Autodesk.Revit.DB;
    using Autodesk.Revit.UI;

    [Regeneration(RegenerationOption.Manual)]
    [Transaction(TransactionMode.Manual)]
    public class RebarSolidAndVisibleSettings : IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            SettingsWindow window = new SettingsWindow();
            window.ShowDialog();
            return Result.Succeeded;
        }
    }
}
