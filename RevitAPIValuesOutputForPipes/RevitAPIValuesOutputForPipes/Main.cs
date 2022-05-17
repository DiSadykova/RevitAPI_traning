using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.DB.Plumbing;
using Autodesk.Revit.UI;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RevitAPIValuesOutputForPipes
{
    [Transaction(TransactionMode.Manual)]
    public class Main : IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            UIApplication uiapp = commandData.Application;
            UIDocument uidoc = uiapp.ActiveUIDocument;
            Document doc = uidoc.Document;

            string pipeInfo = string.Empty;

            var pipes = new FilteredElementCollector(doc)
                .OfClass(typeof(Pipe))
                .Cast<Pipe>()
                .ToList();

            string excelPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop), "pipes.xlsx");

            int rowIndex = 0;
            double outerDiameter = 0;
            double innerDiameter = 0;
            double pipeLength = 0;

            using (FileStream stream = new FileStream(excelPath, FileMode.Create, FileAccess.Write))
            {
                IWorkbook workbook = new XSSFWorkbook();
                ISheet sheet = workbook.CreateSheet("Лист1");

                
                foreach (Pipe pipe in pipes)
                {
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
                    Parameter lengthParameter = pipe.get_Parameter(BuiltInParameter.CURVE_ELEM_LENGTH);
                    if (lengthParameter.StorageType == StorageType.Double)
                    {
                        pipeLength = UnitUtils.ConvertFromInternalUnits(lengthParameter.AsDouble(), UnitTypeId.Meters);
                    }
                    sheet.SetCellValue(rowIndex, columnIndex: 0, pipe.Name);
                    sheet.SetCellValue(rowIndex, columnIndex: 1, outerDiameter);
                    sheet.SetCellValue(rowIndex, columnIndex: 2, innerDiameter);
                    sheet.SetCellValue(rowIndex, columnIndex: 3, pipeLength);
                   rowIndex++;
                }
                workbook.Write(stream);
                workbook.Close();
            }
            System.Diagnostics.Process.Start(excelPath);

            return Result.Succeeded;

        }
    }
}
