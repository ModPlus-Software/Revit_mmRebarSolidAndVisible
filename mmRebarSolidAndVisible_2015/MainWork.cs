namespace mmRebarSolidAndVisible
{
    using System.Collections.Generic;
    using System.Linq;
    using Autodesk.Revit.DB;
    using Autodesk.Revit.DB.Structure;
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
                            e.Category.Id.IntegerValue == (int)BuiltInCategory.OST_StructuralFraming ||
                            e.Category.Id.IntegerValue == (int)BuiltInCategory.OST_Rebar)
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
            var data = RebarHostData.GetRebarHostData(elem);

            // Get rebars
            var allRebar = data.GetRebarsInHost();
            if (allRebar != null && allRebar.Any())
            {
                foreach (Rebar rebar in allRebar)
                {
                    EnableRebarVisibility(view, viewUnobscured, viewAsSolid, rebar);
                }
            }

            // Get all area reinforcement
            var areaRebar = data.GetAreaReinforcementsInHost();
            if (areaRebar != null && areaRebar.Any())
            {
                foreach (var rebar in areaRebar)
                {
                    EnableRebarVisibility(view, viewUnobscured, viewAsSolid, rebar);
                }
            }

            // Get all path reinforcement
            var pathRebar = data.GetPathReinforcementsInHost();
            if (areaRebar != null && areaRebar.Any())
            {
                foreach (var rebar in pathRebar)
                {
                    EnableRebarVisibility(view, viewUnobscured, viewAsSolid, rebar);
                }
            }
        }

        public static void EnableRebarVisibility(View view, bool viewUnobscured, bool viewAsSolid, Rebar rebar)
        {
            if (view.ViewType == ViewType.ThreeD && viewAsSolid)
                rebar.SetSolidInView((View3D)view, true);

            if (viewUnobscured)
                rebar.SetUnobscuredInView(view, true);
        }

        public static void EnableRebarVisibility(View view, bool viewUnobscured, bool viewAsSolid, PathReinforcement rebar)
        {
            if (view.ViewType == ViewType.ThreeD && viewAsSolid)
                rebar.SetSolidInView((View3D)view, true);

            if (viewUnobscured)
                rebar.SetUnobscuredInView(view, true);
        }

        public static void EnableRebarVisibility(View view, bool viewUnobscured, bool viewAsSolid, AreaReinforcement rebar)
        {
            if (view.ViewType == ViewType.ThreeD && viewAsSolid)
                rebar.SetSolidInView((View3D)view, true);

            if (viewUnobscured)
                rebar.SetUnobscuredInView(view, true);
        }

        public static void DisableRebarVisibilityForElement(View view, Element elem)
        {
            // Get all rebars types
            var data = RebarHostData.GetRebarHostData(elem);

            // Get rebars
            var allRebar = data.GetRebarsInHost();
            if (allRebar != null && allRebar.Any())
            {
                foreach (var rebar in allRebar)
                {
                    DisableRebarVisibility(view, rebar);
                }
            }

            // Get all area reinforcement
            var areaRebar = data.GetAreaReinforcementsInHost();
            if (areaRebar != null && areaRebar.Any())
            {
                foreach (AreaReinforcement rebar in areaRebar)
                {
                    DisableRebarVisibility(view, rebar);
                }
            }

            // Get all path reinforcement
            var pathRebar = data.GetPathReinforcementsInHost();
            if (areaRebar != null && areaRebar.Any())
            {
                foreach (PathReinforcement rebar in pathRebar)
                {
                    DisableRebarVisibility(view, rebar);
                }
            }
        }

        public static void DisableRebarVisibility(View view, Rebar rebar)
        {
            if (view.ViewType == ViewType.ThreeD)
                rebar.SetSolidInView((View3D)view, false);
            rebar.SetUnobscuredInView(view, false);
        }

        public static void DisableRebarVisibility(View view, PathReinforcement rebar)
        {
            if (view.ViewType == ViewType.ThreeD)
                rebar.SetSolidInView((View3D)view, false);
            rebar.SetUnobscuredInView(view, false);
        }

        public static void DisableRebarVisibility(View view, AreaReinforcement rebar)
        {
            if (view.ViewType == ViewType.ThreeD)
                rebar.SetSolidInView((View3D)view, false);
            rebar.SetUnobscuredInView(view, false);
        }
    }
}