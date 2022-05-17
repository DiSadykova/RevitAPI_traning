using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.DB.Mechanical;
using Autodesk.Revit.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RevitAPIDuctsCountPerFloors
{
    [Transaction(TransactionMode.Manual)]
    public class Main : IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            UIApplication uiapp = commandData.Application;
            UIDocument uidoc = uiapp.ActiveUIDocument;
            Document doc = uidoc.Document;
            string info = string.Empty;
           var levels = new FilteredElementCollector(doc)
                .OfClass(typeof(Level))
                .OfType<Level>()
                .ToList();
            foreach (Level level in levels)
            {
                var ducts = new FilteredElementCollector(doc)
                    .OfClass(typeof(Duct))
                    .OfType<Duct>()
                    .Where(x => x.get_Parameter(BuiltInParameter.RBS_START_LEVEL_PARAM).AsValueString() ==level.Name)
                    .Count();
                info += $"{level.Name}: количество воздуховодов - {ducts} {Environment.NewLine}";
            }    

            TaskDialog.Show("Ducts count per floors", info);
             

            return Result.Succeeded;

        }
    }
}
