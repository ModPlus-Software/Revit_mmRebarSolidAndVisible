namespace mmRebarSolidAndVisible
{
    using Autodesk.Revit.DB;
    using Autodesk.Revit.UI.Selection;

    public class ObjReinPickFilter : ISelectionFilter
    {
        public bool AllowElement(Element e)
        {
            return IsAllowableElement(e);
        }

        public bool AllowReference(Reference r, XYZ p)
        {
            return false;
        }

        public static bool IsAllowableElement(Element e)
        {
            return e.Category.Id.IntegerValue == (int) BuiltInCategory.OST_StructuralColumns ||
                   e.Category.Id.IntegerValue == (int) BuiltInCategory.OST_StructuralFoundation ||
                   e.Category.Id.IntegerValue == (int) BuiltInCategory.OST_Floors ||
                   e.Category.Id.IntegerValue == (int) BuiltInCategory.OST_Walls ||
                   e.Category.Id.IntegerValue == (int) BuiltInCategory.OST_StructuralFraming ||
                   e.Category.Id.IntegerValue == (int) BuiltInCategory.OST_Rebar ||
                   e.Category.Id.IntegerValue == (int) BuiltInCategory.OST_EdgeSlab;
        }
    }
}