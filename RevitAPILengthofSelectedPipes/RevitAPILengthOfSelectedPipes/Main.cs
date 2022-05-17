using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.DB.Plumbing;
using Autodesk.Revit.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RevitAPILengthOfSelectedPipes
{
    [Transaction(TransactionMode.Manual)]
    public class Main : IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            UIApplication uiapp = commandData.Application;
            UIDocument uidoc = uiapp.ActiveUIDocument;
            Document doc = uidoc.Document;

            var pipes = new FilteredElementCollector(doc)
                .OfClass(typeof(Pipe))
                .Cast<Pipe>()
                .ToList();


            IList<Reference> selectedElementRefList = null;
            try
            {
                selectedElementRefList = uidoc.Selection.PickObjects(Autodesk.Revit.UI.Selection.ObjectType.Element, new PipeFilter(), "Выберите элементы");
            }
            catch(Autodesk.Revit.Exceptions.OperationCanceledException)
            { }
            if (selectedElementRefList == null)
            {
                return Result.Cancelled;
            }

            var elementList = new List<Element>();
            double pipeLength = 0;
            double Sum = 0;
            foreach (var selectedElement in selectedElementRefList)
            {
                Pipe oPipe = doc.GetElement(selectedElement) as Pipe;
                Parameter lengthParameter = oPipe.get_Parameter(BuiltInParameter.CURVE_ELEM_LENGTH);
                if (lengthParameter.StorageType == StorageType.Double)
                {
                    pipeLength = UnitUtils.ConvertFromInternalUnits(lengthParameter.AsDouble(), UnitTypeId.Meters);
                }
                Sum += pipeLength;

            }
            TaskDialog.Show("Сумма длин выбранных труб", Sum.ToString());

            return Result.Succeeded;

        }
    }
}
