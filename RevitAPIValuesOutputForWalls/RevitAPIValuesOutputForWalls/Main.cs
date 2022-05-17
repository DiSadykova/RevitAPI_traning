using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace RevitAPIValuesOutputForWalls
{
    [Transaction(TransactionMode.Manual)]
    public class Main : IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            UIApplication uiapp = commandData.Application;
            UIDocument uidoc = uiapp.ActiveUIDocument;
            Document doc = uidoc.Document;

            string wallInfo = string.Empty;

            var walls = new FilteredElementCollector(doc)
                .OfClass(typeof(Wall))
                .Cast<Wall>()
                .ToList();

            foreach (Wall wall in walls)
            {


                string wallName = wall.Name;
                Parameter volumeWall = wall.get_Parameter(BuiltInParameter.HOST_VOLUME_COMPUTED);
                double volume = 0;
                if (volumeWall.StorageType == StorageType.Double)
                {
                    volume = UnitUtils.ConvertFromInternalUnits(volumeWall.AsDouble(), UnitTypeId.CubicMeters);
                }

                wallInfo += $"{wallName}\t{volume.ToString()}{Environment.NewLine}";


            }

            var saveDialog = new SaveFileDialog
            {
                OverwritePrompt = true,
                InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Desktop),
                Filter = "All files (*.*)|(*.*)",
                FileName = "wallInfo.csv",
                DefaultExt = ".csv"
            };

            string selectedFilePath = string.Empty;
            if (saveDialog.ShowDialog() == DialogResult.OK)
            {
                selectedFilePath = saveDialog.FileName;
            }

            if (string.IsNullOrEmpty(selectedFilePath))
                return Result.Cancelled;

            File.WriteAllText(selectedFilePath, wallInfo, Encoding.GetEncoding(1251));


            return Result.Succeeded;

        }
    }
}
