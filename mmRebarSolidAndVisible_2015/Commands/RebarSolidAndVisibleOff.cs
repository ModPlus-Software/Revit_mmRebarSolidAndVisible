// ReSharper disable once CheckNamespace
namespace mmRebarSolidAndVisible
{
    using System;
    using Autodesk.Revit.Attributes;
    using Autodesk.Revit.DB;
    using Autodesk.Revit.DB.Structure;
    using Autodesk.Revit.UI;
    using ModPlusAPI;
    using ModPlusAPI.Windows;

    [Regeneration(RegenerationOption.Manual)]
    [Transaction(TransactionMode.Manual)]
    public class RebarSolidAndVisibleOff : IExternalCommand
    {
        private const string LangItem = "mmRebarSolidAndVisible";

        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            Statistic.SendCommandStarting(new ModPlusConnector());
            try
            {
                var doc = commandData.Application.ActiveUIDocument.Document;
                var selectionVariant = Enum.TryParse(UserConfigFile.GetValue(LangItem, "OffSelectionVariant"),
                    out SelectionVariant sv)
                    ? sv
                    : SelectionVariant.PickObject;

                using (Transaction transaction = new Transaction(doc))
                {
                    var trName = Language.GetItem(LangItem, "t2");
                    if (string.IsNullOrEmpty(trName))
                        trName = "Отключение видимости арматуры";
                    transaction.Start(trName);

                    var list = MainWork.GetElements(commandData.Application, selectionVariant);
                    if (list != null)
                    {
                        foreach (var element in list)
                        {
                            if (element == null)
                                continue;
                            if (element is Rebar rebar)
                                MainWork.DisableRebarVisibility(doc.ActiveView, rebar);
                            else if (element is AreaReinforcement areaReinforcement)
                                MainWork.DisableRebarVisibility(doc.ActiveView, areaReinforcement);
                            else if (element is PathReinforcement pathReinforcement)
                                MainWork.DisableRebarVisibility(doc.ActiveView, pathReinforcement);
                            else
                                MainWork.DisableRebarVisibilityForElement(doc.ActiveView, element);
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
