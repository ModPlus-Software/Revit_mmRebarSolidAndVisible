// ReSharper disable once CheckNamespace
namespace mmRebarSolidAndVisible
{
    using System;
    using Autodesk.Revit.Attributes;
    using Autodesk.Revit.DB;
    using Autodesk.Revit.UI;
    using ModPlusAPI;
    using ModPlusAPI.Windows;

    [Regeneration(RegenerationOption.Manual)]
    [Transaction(TransactionMode.Manual)]
    public class RebarSolidAndVisible : IExternalCommand
    {
        private const string LangItem = "mmRebarSolidAndVisible";

        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            try
            {
                var doc = commandData.Application.ActiveUIDocument.Document;
                var selectionVariant = Enum.TryParse(UserConfigFile.GetValue(LangItem, "OnSelectionVariant"),
                    out SelectionVariant sv)
                    ? sv
                    : SelectionVariant.PickObject;

                using (Transaction transaction = new Transaction(doc))
                {
                    var trName = Language.GetItem(LangItem, "t1");
                    if (string.IsNullOrEmpty(trName))
                        trName = "Включение видимости арматуры";
                    transaction.Start(trName);

                    var list = MainWork.GetElements(commandData.Application, selectionVariant);
                    if (list != null)
                    {
                        foreach (var element in list)
                        {
                            if (element == null)
                                continue;
                            MainWork.SetRebarVisibilityForElement(doc.ActiveView, element, true);
                        }
                    }

                    transaction.Commit();
                }

                return Result.Succeeded;
            }
            catch (Autodesk.Revit.Exceptions.OperationCanceledException)
            {
                return Result.Cancelled;
            }
            catch (Exception exception)
            {
                ExceptionBox.Show(exception);
                return Result.Failed;
            }
        }
    }
}
