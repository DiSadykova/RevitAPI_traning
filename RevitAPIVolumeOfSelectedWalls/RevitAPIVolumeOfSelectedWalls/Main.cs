using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RevitAPIVolumeOfSelectedWalls
{
    [Transaction(TransactionMode.Manual)]
    public class Main : IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            UIApplication uiapp = commandData.Application;
            UIDocument uidoc = uiapp.ActiveUIDocument;
            Document doc = uidoc.Document;

            IList<Reference> selectedElementRefList = null;
            try
            {
                selectedElementRefList = uidoc.Selection.PickObjects(Autodesk.Revit.UI.Selection.ObjectType.Element, new WallFilter(), "Выберите стены по грани");
            }
            catch (Autodesk.Revit.Exceptions.OperationCanceledException)
            {}
            if (selectedElementRefList == null)
            {
                return Result.Cancelled;
            }
            
            var WallList = new List<Wall>();
            double Value = 0;
            double Sum = 0;
            foreach (var selectedElement in selectedElementRefList)
            {
                Wall oWall = doc.GetElement(selectedElement) as Wall;

                Parameter volumeParameter = oWall.get_Parameter(BuiltInParameter.HOST_VOLUME_COMPUTED);
                if (volumeParameter.StorageType == StorageType.Double)
                {
                    Value = UnitUtils.ConvertFromInternalUnits(volumeParameter.AsDouble(), UnitTypeId.CubicMeters);
                }
                Sum += Value;
            }
            TaskDialog.Show("Сумма объемов стен ", Sum.ToString());
            return Result.Succeeded;

        }
    }
}
