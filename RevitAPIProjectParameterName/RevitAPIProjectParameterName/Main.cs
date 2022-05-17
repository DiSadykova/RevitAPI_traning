using Autodesk.Revit.ApplicationServices;
using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.DB.Plumbing;
using Autodesk.Revit.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RevitAPIProjectParameterName
{
    [Transaction(TransactionMode.Manual)]
    public class Main : IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            UIApplication uiapp = commandData.Application;
            UIDocument uidoc = uiapp.ActiveUIDocument;
            Document doc = uidoc.Document;

            var categorySet = new CategorySet();
            categorySet.Insert(Category.GetCategory(doc, BuiltInCategory.OST_PipeCurves));

            using (Transaction ts = new Transaction(doc, "Add parameter"))
            {
                ts.Start();
                CreateSharedParameter(uiapp.Application, doc, "Наименование", categorySet, BuiltInParameterGroup.PG_DATA, true);
                ts.Commit();
            }


            var pipes = new FilteredElementCollector(doc)
                .OfClass(typeof(Pipe))
                .Cast<Pipe>()
                .ToList();
            double outerDiameter = 0;
            double innerDiameter = 0;
            foreach (var pipe in pipes)
            {
                using (Transaction ts = new Transaction(doc, "Set parameters"))
                {
                    ts.Start();
                    Parameter outerDiameterParam = pipe.get_Parameter(BuiltInParameter.RBS_PIPE_OUTER_DIAMETER);
                    if (outerDiameterParam.StorageType == StorageType.Double)
                    {
                        outerDiameter = UnitUtils.ConvertFromInternalUnits(outerDiameterParam.AsDouble(), UnitTypeId.Millimeters);
                    }
                    Parameter innerDiameterParam = pipe.get_Parameter(BuiltInParameter.RBS_PIPE_INNER_DIAM_PARAM);
                    if (innerDiameterParam.StorageType == StorageType.Double)
                    {
                        innerDiameter = UnitUtils.ConvertFromInternalUnits(innerDiameterParam.AsDouble(), UnitTypeId.Millimeters);
                    }
                    Parameter namePipeParam = pipe.LookupParameter("Наименование");
                    namePipeParam.Set($"Труба {outerDiameter} / {innerDiameter}");
                    ts.Commit();
                }
            }
            return Result.Succeeded;
        }

        private void CreateSharedParameter(Application application, Document doc,
            string parameterName, CategorySet categorySet, BuiltInParameterGroup builtInParameterGroup, bool isInstance)
        {
            DefinitionFile definitionFile = application.OpenSharedParameterFile();
            if (definitionFile == null)
            {
                TaskDialog.Show("Ошибка", "Не найден файл общих параметров");
                return;
            }

            Definition definition = definitionFile.Groups
                .SelectMany(group => group.Definitions)
                .FirstOrDefault(def => def.Name.Equals(parameterName));
            if (definition == null)
            {
                TaskDialog.Show("Ошибка", "Не найден указанный параметр");
                return;
            }

            Binding binding = application.Create.NewTypeBinding(categorySet);
            if (isInstance)
                binding = application.Create.NewInstanceBinding(categorySet);

            BindingMap map = doc.ParameterBindings;
            map.Insert(definition, binding, builtInParameterGroup);
        }
    }
}
