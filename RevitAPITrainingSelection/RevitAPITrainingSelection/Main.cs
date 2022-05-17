using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.DB.Mechanical;
using Autodesk.Revit.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RevitAPITrainingSelection
{
    [Transaction(TransactionMode.Manual)]
    public class Main : IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            UIApplication uiapp = commandData.Application;
            UIDocument uidoc = uiapp.ActiveUIDocument;
            Document doc = uidoc.Document;
           
            List<Duct> fDucts = new FilteredElementCollector(doc)
                 .OfCategory(BuiltInCategory.OST_DuctCurves)
                 .WhereElementIsNotElementType()
                 .Cast<Duct>()
                 .ToList();

            TaskDialog.Show("Duct count", fDucts.Count.ToString());

            return Result.Succeeded;

        }
    }
}
