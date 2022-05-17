using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RevitAPITExportToNWC
{
    [Transaction(TransactionMode.Manual)]
    public class Main : IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            Document doc = commandData.Application.ActiveUIDocument.Document;
            /*using (var ts = new Transaction(doc, "export NWC"))
            {
            ts.Start();*/
                var options = new NavisworksExportOptions();
                doc.Export((Environment.GetFolderPath(Environment.SpecialFolder.Desktop)), "export.nwc", options);
                //ts.Commit();
            //}
            return Result.Succeeded;

        }
    }
}
