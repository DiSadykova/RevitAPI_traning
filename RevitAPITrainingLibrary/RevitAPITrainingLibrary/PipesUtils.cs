using Autodesk.Revit.DB;
using Autodesk.Revit.DB.Plumbing;
using Autodesk.Revit.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RevitAPITrainingLibrary
{
    public class PipesUtils
    {
        public static List<PipingSystemType> GetPipingSystems(ExternalCommandData commandData)
        {
            UIApplication uiapp = commandData.Application;
            UIDocument uidoc = uiapp.ActiveUIDocument;
            Document doc = uidoc.Document;

            List<PipingSystemType> pipingSystemTypes = new FilteredElementCollector(doc)
                                        .OfClass(typeof(PipingSystemType))
                                        .Cast<PipingSystemType>()
                                        .ToList();
            return pipingSystemTypes;

        }
    }
}
