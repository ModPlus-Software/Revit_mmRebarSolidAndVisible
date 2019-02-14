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
            Statistic.SendCommandStarting(new ModPlusConnector());
            try
            {
                var doc = commandData.Application.ActiveUIDocument.Document;
                var selectionVariant = Enum.TryParse(UserConfigFile.GetValue(LangItem, "OnSelectionVariant"),
                    out SelectionVariant sv)
                    ? sv
                    : SelectionVariant.PickObject;
                var viewUnobscured =
                    !bool.TryParse(UserConfigFile.GetValue(LangItem, "ViewUnobscured"), out var b) || b;
                var viewAsSolid =
                    !bool.TryParse(UserConfigFile.GetValue(LangItem, "ViewAsSolid"), out b) || b;

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
                            MainWork.EnableRebarVisibilityForElement(doc.ActiveView, element, viewUnobscured, viewAsSolid);
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
