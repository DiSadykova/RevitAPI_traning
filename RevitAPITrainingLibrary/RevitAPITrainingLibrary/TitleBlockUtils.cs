using Autodesk.Revit.DB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RevitAPITrainingLibrary
{
    public class TitleBlockUtils
    {
        public static List<FamilySymbol> GetTitleBlockTypes(Document doc)
        {
            var titleBlockTypes = new FilteredElementCollector(doc)
                .OfCategory(BuiltInCategory.OST_TitleBlocks)
                .WhereElementIsElementType()
                .Cast<FamilySymbol>()
                .ToList();
            return titleBlockTypes;
        }
    }
}
