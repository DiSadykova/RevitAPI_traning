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

namespace RevitAPICreateAndWriteParameterForPipes
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
                CreateSharedParameter(uiapp.Application, doc, "Длина с запасом", categorySet, BuiltInParameterGroup.PG_DATA, true);
                ts.Commit();
            }

            IList<Reference> selectedElementRefList = null;
            try
            {
                selectedElementRefList = uidoc.Selection.PickObjects(Autodesk.Revit.UI.Selection.ObjectType.Element, new PipeFilter(), "Выберите элементы");
            }
            catch (Autodesk.Revit.Exceptions.OperationCanceledException)
            { }
            if (selectedElementRefList == null)
            {
                return Result.Cancelled;
            }
            foreach (var selectedElement in selectedElementRefList)
            {
                Pipe oPipe = doc.GetElement(selectedElement) as Pipe;

                using (Transaction ts = new Transaction(doc, "Set parameters"))
                {
                    ts.Start();
                    Parameter lengthParameter = oPipe.get_Parameter(BuiltInParameter.CURVE_ELEM_LENGTH);
                    Double pipeLength = 0;
                    if (lengthParameter.StorageType == StorageType.Double)
                    {
                        pipeLength = UnitUtils.ConvertFromInternalUnits(lengthParameter.AsDouble(), UnitTypeId.Meters);
                    }
                    Double k = 1.1;
                    Double marginLength = pipeLength * k;
                    Parameter marginLengthParameter = oPipe.LookupParameter("Длина с запасом");

                    marginLengthParameter.Set(UnitUtils.ConvertToInternalUnits(marginLength, UnitTypeId.Meters));
                    ts.Commit();
                }
            }


            return Result.Succeeded;

        }

        private void CreateSharedParameter(Application application, Document doc,
            string parameterName, CategorySet categorySet,
            BuiltInParameterGroup builtInParameterGroup, bool isInstance)
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
