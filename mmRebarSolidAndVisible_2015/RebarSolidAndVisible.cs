#region Namespaces
using System;
using System.Linq;
using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using Autodesk.Revit.UI.Selection;
using ModPlusAPI;
using ModPlusAPI.Windows;
using OperationCanceledException = Autodesk.Revit.Exceptions.OperationCanceledException;

#endregion

namespace mmRebarSolidAndVisible
{
    [Transaction(TransactionMode.Manual)]
    public sealed class RebarSolidAndVisible : IExternalCommand
    {
        Result IExternalCommand.Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            Statistic.SendCommandStarting(new Interface());

            Result result = Result.Failed;
            try
            {
                var doc = commandData.Application.ActiveUIDocument.Document;
                var activeView = doc.ActiveView;

                using (var transaction = new TransactionGroup(doc, "RebarSolidAndVisible"))
                {
                    if (TransactionStatus.Started == transaction.Start())
                    {
                        if (DoWork(commandData, ref message, elements, activeView))
                        {
                            if (TransactionStatus.Committed == transaction.Assimilate())
                            {
                                result = Result.Succeeded;
                            }
                        }
                        else
                        {
                            transaction.RollBack();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ExceptionBox.Show(ex);
                result = Result.Failed;
            }
            return result;
        }

        private bool DoWork(ExternalCommandData commandData, ref String message, ElementSet elements, View view)
        {
            if (null == commandData)
            {
                throw new ArgumentNullException(nameof(commandData));
            }

            if (null == message)
            {
                throw new ArgumentNullException(nameof(message));
            }

            if (null == elements)
            {
                throw new ArgumentNullException(nameof(elements));
            }
            UIApplication uiApp = commandData.Application;
            Document doc = uiApp.ActiveUIDocument.Document;

            using (var tr = new Transaction(doc, "RebarSolidAndVisibleWork"))
            {

                if (TransactionStatus.Started == tr.Start())
                {

                    // ====================================
                    // Define a Reference object to accept the pick result
                    Reference pickedRef = null;

                    // Pick a reinforcement familyInstance using a filter
                    Selection sel = uiApp.ActiveUIDocument.Selection;
                    ObjReinPickFilter selFilter = new ObjReinPickFilter();
                    try
                    {
                        pickedRef = sel.PickObject(ObjectType.Element, selFilter, "Выберите элемент с арматурой");
                    }
                    catch (OperationCanceledException)
                    {
                        return false;

                    }
                    Element elem = doc.GetElement(pickedRef);
                    
                    // Get all rebars types
                    var data = Autodesk.Revit.DB.Structure.RebarHostData.GetRebarHostData(elem);

                    // Get rebars
                    var allRebar = data.GetRebarsInHost();
                    if (allRebar != null && allRebar.Any())
                    {
                        foreach (var rebars in allRebar)
                        {
                            if (view.ViewType == ViewType.ThreeD)
                                rebars.SetSolidInView((View3D)view, true);
                            rebars.SetUnobscuredInView(view, true);
                        }
                    }

                    // Get all area reinforcement
                    var areaRebar = data.GetAreaReinforcementsInHost();
                    if (areaRebar != null && areaRebar.Any())
                    {
                        foreach (var rebars in areaRebar)
                        {
                            if (view.ViewType == ViewType.ThreeD)
                                rebars.SetSolidInView((View3D)view, true);
                            rebars.SetUnobscuredInView(view, true);
                        }
                    }

                    // Get all path reinforcement
                    var pathRebar = data.GetPathReinforcementsInHost();
                    if (areaRebar != null && areaRebar.Any())
                    {
                        foreach (var rebars in pathRebar)
                        {
                            if (view.ViewType == ViewType.ThreeD)
                                rebars.SetSolidInView((View3D)view, true);
                            rebars.SetUnobscuredInView(view, true);
                        }
                    }

                    return TransactionStatus.Committed == tr.Commit();
                }
            }
            return false;
        }
    }

    public class ObjReinPickFilter : ISelectionFilter
    {
        public bool AllowElement(Element e)
        {
            return (e.Category.Id.IntegerValue.Equals(
                        (int)BuiltInCategory.OST_StructuralColumns) ||
                    e.Category.Id.IntegerValue.Equals(
                        (int)BuiltInCategory.OST_StructuralFoundation) ||
                    e.Category.Id.IntegerValue.Equals(
                        (int)BuiltInCategory.OST_Floors) ||
                    e.Category.Id.IntegerValue.Equals(
                        (int)BuiltInCategory.OST_Walls) ||
                    e.Category.Id.IntegerValue.Equals(
                        (int)BuiltInCategory.OST_StructuralFraming));
        }
        public bool AllowReference(Reference r, XYZ p)
        {
            return false;
        }
    }
}