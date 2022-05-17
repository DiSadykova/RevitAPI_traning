using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RevitAPITrainingLibrary
{
    public class WallsUtils
    {
        public static void VolumeSumInModel(List<Wall> walls)
        {
            double value = 0;
            double sum = 0;
            foreach (var wall in walls)
            {
                Parameter volumeParameter = wall.get_Parameter(BuiltInParameter.HOST_VOLUME_COMPUTED);
                if (volumeParameter.StorageType == StorageType.Double)
                {
                    value = UnitUtils.ConvertFromInternalUnits(volumeParameter.AsDouble(), UnitTypeId.CubicMeters);
                }
                sum += value;
            }
            TaskDialog.Show("Сумма объемов всех стен в модели", sum.ToString());
        }

        public static List<WallType> GetWallTypes(ExternalCommandData commandData)
        {
            UIApplication uiapp = commandData.Application;
            UIDocument uidoc = uiapp.ActiveUIDocument;
            Document doc = uidoc.Document;

            List<WallType> wallTypes = new FilteredElementCollector(doc)
                                        .OfClass(typeof(WallType))
                                        .Cast<WallType>()
                                        .ToList();
            return wallTypes;

        }
    }
}
