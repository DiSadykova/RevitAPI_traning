using Autodesk.Revit.DB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RevitAPITrainingLibrary
{
    public class ViewsUtils
    {
        public static List<View> GetViews(Document doc)
        {

            var views = new List<View>();

            var viewPlans = new FilteredElementCollector(doc)
                .OfClass(typeof(ViewPlan))
                .Cast<View>()
                .Where(x=>x.IsTemplate==false)
                .ToList();

            views.AddRange(viewPlans);

            var viewSections = new FilteredElementCollector(doc)
                .OfClass(typeof(ViewSection))
                .Cast<View>()
                .Where(x => x.IsTemplate == false)
                .ToList();

            views.AddRange(viewSections);

            return views;
        }

        public static List<Viewport> GetViewports(Document doc)
        {
            var viewports = new FilteredElementCollector(doc)
                .OfClass(typeof(Viewport))
                .Cast<Viewport>()
                .ToList();

            return viewports;
        }
    }
}
