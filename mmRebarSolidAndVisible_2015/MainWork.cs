namespace mmRebarSolidAndVisible
{
    using System.Collections.Generic;
    using System.Linq;
    using Autodesk.Revit.DB;
    using Autodesk.Revit.UI;
    using Autodesk.Revit.UI.Selection;
    using ModPlusAPI;

    public static class MainWork
    {
        private const string LangItem = "mmRebarSolidAndVisible";

        public static List<Element> GetElements(UIApplication uiApp, SelectionVariant selectionVariant)
        {
            var doc = uiApp.ActiveUIDocument.Document;
            switch (selectionVariant)
            {
                case SelectionVariant.AllOnView:
                    return new FilteredElementCollector(doc, doc.ActiveView.Id)
                        .WhereElementIsNotElementType()
                        .Where(e => e.IsValidObject && e.Category != null)
                        .Where(e =>
                            e.Category.Id.IntegerValue == (int)BuiltInCategory.OST_StructuralColumns ||
                            e.Category.Id.IntegerValue == (int)BuiltInCategory.OST_StructuralFoundation ||
                            e.Category.Id.IntegerValue == (int)BuiltInCategory.OST_Floors ||
                            e.Category.Id.IntegerValue == (int)BuiltInCategory.OST_Walls ||
                            e.Category.Id.IntegerValue == (int)BuiltInCategory.OST_StructuralFraming)
                        .ToList();
                case SelectionVariant.PickObjects:
                    {
                        var pickedRefs = uiApp.ActiveUIDocument.Selection.PickObjects(
                            ObjectType.Element, new ObjReinPickFilter(), Language.GetItem(LangItem, "msg2"));
                        List<Element> elements = new List<Element>();
                        foreach (var reference in pickedRefs)
                        {
                            elements.Add(doc.GetElement(reference));
                        }

                        return elements;
                    }
                default:
                    {
                        var pickedRef = uiApp.ActiveUIDocument.Selection.PickObject(
                            ObjectType.Element, new ObjReinPickFilter(), Language.GetItem(LangItem, "msg1"));
                        return new List<Element>
                    {
                        doc.GetElement(pickedRef)
                    };
                    }
            }
        }

        public static void EnableRebarVisibilityForElement(View view, Element elem, bool viewUnobscured, bool viewAsSolid)
        {
            // Get all rebars types
            var data = Autodesk.Revit.DB.Structure.RebarHostData.GetRebarHostData(elem);

            // Get rebars
            var allRebar = data.GetRebarsInHost();
            if (allRebar != null && allRebar.Any())
            {
                foreach (var rebars in allRebar)
                {
                    if (view.ViewType == ViewType.ThreeD && viewAsSolid)
                        rebars.SetSolidInView((View3D) view, true);

                    if (viewUnobscured)
                        rebars.SetUnobscuredInView(view, true);
                }
            }

            // Get all area reinforcement
            var areaRebar = data.GetAreaReinforcementsInHost();
            if (areaRebar != null && areaRebar.Any())
            {
                foreach (var rebars in areaRebar)
                {
                    if (view.ViewType == ViewType.ThreeD && viewAsSolid)
                        rebars.SetSolidInView((View3D) view, true);

                    if (viewUnobscured)
                        rebars.SetUnobscuredInView(view, true);
                }
            }

            // Get all path reinforcement
            var pathRebar = data.GetPathReinforcementsInHost();
            if (areaRebar != null && areaRebar.Any())
            {
                foreach (var rebars in pathRebar)
                {
                    if (view.ViewType == ViewType.ThreeD && viewAsSolid)
                        rebars.SetSolidInView((View3D) view, true);

                    if (viewUnobscured)
                        rebars.SetUnobscuredInView(view, true);
                }
            }
        }

        public static void DisableRebarVisibilityForElement(View view, Element elem)
        {
            // Get all rebars types
            var data = Autodesk.Revit.DB.Structure.RebarHostData.GetRebarHostData(elem);

            // Get rebars
            var allRebar = data.GetRebarsInHost();
            if (allRebar != null && allRebar.Any())
            {
                foreach (var rebars in allRebar)
                {
                    if (view.ViewType == ViewType.ThreeD)
                        rebars.SetSolidInView((View3D) view, false);
                    rebars.SetUnobscuredInView(view, false);
                }
            }

            // Get all area reinforcement
            var areaRebar = data.GetAreaReinforcementsInHost();
            if (areaRebar != null && areaRebar.Any())
            {
                foreach (var rebars in areaRebar)
                {
                    if (view.ViewType == ViewType.ThreeD)
                        rebars.SetSolidInView((View3D)view, false);
                    rebars.SetUnobscuredInView(view, false);
                }
            }

            // Get all path reinforcement
            var pathRebar = data.GetPathReinforcementsInHost();
            if (areaRebar != null && areaRebar.Any())
            {
                foreach (var rebars in pathRebar)
                {
                    if (view.ViewType == ViewType.ThreeD)
                        rebars.SetSolidInView((View3D)view, false);
                    rebars.SetUnobscuredInView(view, false);
                }
            }
        }
    }
}