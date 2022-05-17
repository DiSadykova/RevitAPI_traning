using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace RevitAPICreateButtonRibbon
{
    [Transaction(TransactionMode.Manual)]
    public class Main : IExternalApplication
    {
        public Result OnShutdown(UIControlledApplication application)
        {
            return Result.Succeeded;
        }

        public Result OnStartup(UIControlledApplication application)
        {
            string tabName = "Revit API training";
            application.CreateRibbonTab(tabName);
            string utilsFolderPath = @"C:\Program Files\RevitAPITraining\";

            var panel = application.CreateRibbonPanel(tabName, "Информация по элементам");

            var button1 = new PushButtonData("Информация", "Информация по элементам",
                Path.Combine(utilsFolderPath, "RevitAPICreateButton.dll"),
                "RevitAPICreateButton.Main");

            Uri uriImage1 = new Uri(@"C:\Program Files\RevitAPITraining\Images\RevitAPITrainingUI_32.png", UriKind.Absolute);
            BitmapImage largeImage1 = new BitmapImage(uriImage1);
            button1.LargeImage = largeImage1;

            panel.AddItem(button1);


            var button2 = new PushButtonData("Стены", "Изменение типа стен",
                Path.Combine(utilsFolderPath, "RevitAPIChangingWallTypes.dll"),
                "RevitAPIChangingWallTypes.Main");

            Uri uriImage2 = new Uri(@"C:\Program Files\RevitAPITraining\Images\RevitAPITrainingUI_32.png", UriKind.Absolute);
            BitmapImage largeImage2 = new BitmapImage(uriImage2);
            button2.LargeImage = largeImage2;
            panel.AddItem(button2);

            return Result.Succeeded;
        }
    }
}
