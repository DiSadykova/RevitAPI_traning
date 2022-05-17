using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RevitAPIExportToJPG
{
    [Transaction(TransactionMode.Manual)]
    public class Main : IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            Document doc = commandData.Application.ActiveUIDocument.Document;
            using (var ts = new Transaction(doc, "export IFC"))
            {
                ts.Start();
                ViewPlan viewPlan = new FilteredElementCollector(doc)
                                        .OfClass(typeof(ViewPlan))
                                        .Cast<ViewPlan>()
                                        .FirstOrDefault(v => v.ViewType == ViewType.FloorPlan &&
                                                            v.Name.Equals("Level 1"));
                IList<ElementId> imageExportList = new List<ElementId>();

                imageExportList.Add(viewPlan.Id);

                ImageExportOptions options = new ImageExportOptions
                {
                    ZoomType = ZoomFitType.FitToPage,
                    PixelSize = 2024,
                    FilePath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + @"\export.jpg",
                    FitDirection = FitDirectionType.Horizontal,
                    HLRandWFViewsFileType = ImageFileType.JPEGLossless,
                    ImageResolution = ImageResolution.DPI_600,
                    ExportRange = ExportRange.SetOfViews,
                };
                options.SetViewsAndSheets(imageExportList);

                doc.ExportImage(options);
                ts.Commit();
            }
            return Result.Succeeded;

        }
    }
}
