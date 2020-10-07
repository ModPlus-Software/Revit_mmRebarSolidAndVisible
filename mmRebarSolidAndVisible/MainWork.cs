namespace mmRebarSolidAndVisible
{
    using System.Collections.Generic;
    using System.Linq;
    using Autodesk.Revit.DB;
    using Autodesk.Revit.DB.Structure;
    using Autodesk.Revit.UI;
    using Autodesk.Revit.UI.Selection;
    using ModPlusAPI;

    public class MainWork
    {
        private readonly UIApplication _uiApplication;
        private readonly Document _doc;
        private readonly View _view;
        private readonly PluginSettings _settings;
        private const string LangItem = "mmRebarSolidAndVisible";

        public MainWork(UIApplication uiApplication, PluginSettings settings)
        {
            _uiApplication = uiApplication;
            _doc = uiApplication.ActiveUIDocument.Document;
            _view = _doc.ActiveView;
            _settings = settings;
        }

        public void Process()
        {
            if (!_settings.IsActiveChangeSolidInViewProperty && !_settings.IsActiveChangeUnobscuredInViewProperty)
                return;

            bool? showUnobscured = null;
            if (_settings.IsActiveChangeUnobscuredInViewProperty)
                showUnobscured = _settings.ShowAsUnobscured;
            bool? showAsSolid = null;
            if (_settings.IsActiveChangeSolidInViewProperty)
                showAsSolid = _settings.ShowAsSolidInThreeDView;

            using (var transaction = new Transaction(_doc))
            {
                var trName = Language.GetItem(LangItem, "t1");
                if (string.IsNullOrEmpty(trName))
                    trName = "Изменение видимости арматуры";
                transaction.Start(trName);

                var list = GetElements();
                if (list != null)
                {
                    foreach (var element in list)
                    {
                        if (element == null)
                            continue;
                        if (element is Rebar rebar)
                            ChangeRebarVisibility(rebar, showUnobscured, showAsSolid);
                        else if (element is AreaReinforcement areaReinforcement)
                            ChangeRebarVisibility(areaReinforcement, showUnobscured, showAsSolid);
                        else if (element is PathReinforcement pathReinforcement)
                            ChangeRebarVisibility(pathReinforcement, showUnobscured, showAsSolid);
                        else if (element is RebarContainer rebarContainer)
                            ChangeRebarVisibility(rebarContainer, showUnobscured, showAsSolid);
                        else
                            ChangeRebarVisibilityForHostElement(element, showUnobscured, showAsSolid);
                    }
                }

                transaction.Commit();
            }
        }

        private List<Element> GetElements()
        {
            switch (_settings.ElementsProcessVariant)
            {
                case ElementsProcessVariant.AllElementsOnView:
                    {
                        return new FilteredElementCollector(_doc, _view.Id)
                              .WhereElementIsNotElementType()
                              .Where(e => e.IsValidObject && e.Category != null)
                              .Where(ReinforcementSelectionFilter.IsAllowableElement)
                              .ToList();
                    }

                case ElementsProcessVariant.SelectedElements:
                    {
                        var pickedRefs = _uiApplication.ActiveUIDocument.Selection.PickObjects(
                            ObjectType.Element, new ReinforcementSelectionFilter(), Language.GetItem(LangItem, "msg2"));

                        return pickedRefs.Select(reference => _doc.GetElement(reference)).ToList();
                    }

                default:
                    {
                        var pickedRef = _uiApplication.ActiveUIDocument.Selection.PickObject(
                            ObjectType.Element, new ReinforcementSelectionFilter(), Language.GetItem(LangItem, "msg1"));
                        return new List<Element>
                        {
                            _doc.GetElement(pickedRef)
                        };
                    }
            }
        }

        private void ChangeRebarVisibilityForHostElement(Element elem, bool? showUnobscured, bool? showAsSolid)
        {
            var data = RebarHostData.GetRebarHostData(elem);
            if (data != null)
            {
                var rebarsInHost = data.GetRebarsInHost();
                if (rebarsInHost != null && rebarsInHost.Any())
                {
                    foreach (var rebar in rebarsInHost)
                    {
                        ChangeRebarVisibility(rebar, showUnobscured, showAsSolid);
                    }
                }

                var areaRebar = data.GetAreaReinforcementsInHost();
                if (areaRebar != null && areaRebar.Any())
                {
                    foreach (var rebar in areaRebar)
                    {
                        ChangeRebarVisibility(rebar, showUnobscured, showAsSolid);
                    }
                }

                var pathRebar = data.GetPathReinforcementsInHost();
                if (areaRebar != null && areaRebar.Any())
                {
                    foreach (var rebar in pathRebar)
                    {
                        ChangeRebarVisibility(rebar, showUnobscured, showAsSolid);
                    }
                }

                var rebarContainers = data.GetRebarContainersInHost();
                if (rebarContainers != null && rebarContainers.Any())
                {
                    foreach (var rebar in rebarContainers)
                    {
                        ChangeRebarVisibility(rebar, showUnobscured, showAsSolid);
                    }
                }
            }
        }

        private void ChangeRebarVisibility(Rebar rebar, bool? showUnobscured, bool? showAsSolid)
        {
            if (showUnobscured.HasValue)
                rebar.SetUnobscuredInView(_view, showUnobscured.Value);

            if (showAsSolid.HasValue && (_view is View3D view3D))
                rebar.SetSolidInView(view3D, showAsSolid.Value);
        }

        private void ChangeRebarVisibility(PathReinforcement rebar, bool? showUnobscured, bool? showAsSolid)
        {
            if (showUnobscured.HasValue)
                rebar.SetUnobscuredInView(_view, showUnobscured.Value);

            if (showAsSolid.HasValue && (_view is View3D view3D))
                rebar.SetSolidInView(view3D, showAsSolid.Value);
        }

        private void ChangeRebarVisibility(AreaReinforcement rebar, bool? showUnobscured, bool? showAsSolid)
        {
            if (showUnobscured.HasValue)
                rebar.SetUnobscuredInView(_view, showUnobscured.Value);

            if (showAsSolid.HasValue && (_view is View3D view3D))
                rebar.SetSolidInView(view3D, showAsSolid.Value);
        }

        private void ChangeRebarVisibility(RebarContainer rebar, bool? showUnobscured, bool? showAsSolid)
        {
            if (showUnobscured.HasValue)
                rebar.SetUnobscuredInView(_view, showUnobscured.Value);

            if (showAsSolid.HasValue && (_view is View3D view3D))
                rebar.SetSolidInView(view3D, showAsSolid.Value);
        }
    }
}