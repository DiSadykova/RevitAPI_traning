using Autodesk.Revit.DB;
using Autodesk.Revit.DB.Plumbing;
using Autodesk.Revit.UI.Selection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RevitAPILengthOfSelectedPipes
{
    public class PipeFilter : ISelectionFilter
    {
        public bool AllowElement(Element elem)
        {

            return elem is Pipe;

        }

        public bool AllowReference(Reference reference, XYZ position)
        {
            return false;
        }
    }
}
