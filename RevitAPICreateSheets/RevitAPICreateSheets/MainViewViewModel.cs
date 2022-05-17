using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using Prism.Commands;
using RevitAPITrainingLibrary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RevitAPICreateSheets
{
    internal class MainViewViewModel
    {
        private ExternalCommandData _commandData;
        private Document _doc;

        public List<FamilySymbol> TitleBlockType { get; } = new List<FamilySymbol>();
        public FamilySymbol SelectedTitleBlockType { get; set; }
        public DelegateCommand CreateSheetCommand { get; }
        public List<View> Views { get; } = new List<View>();
        public View SelectedView { get; set; }
        public int Amount { get; set; } = 1;
        public string DesignedBy { get; set; }
        public MainViewViewModel(ExternalCommandData commandData)
        {
            _commandData = commandData;
            _doc = _commandData.Application.ActiveUIDocument.Document;
            TitleBlockType = TitleBlockUtils.GetTitleBlockTypes(_doc);
            Views = ViewsUtils.GetViews(_doc);
            CreateSheetCommand = new DelegateCommand(OnCreateSheetCommand);


        }

        private void OnCreateSheetCommand()
        {
            if (SelectedTitleBlockType == null ||
                Amount < 1 ||
                SelectedView == null ||
                DesignedBy == null || SelectedView.Id == null)
                return;
            List<Viewport> viewportList = ViewsUtils.GetViewports(_doc);
            int a = 0;
            foreach (Viewport viewport in viewportList)
            {
                if (viewport.ViewId == SelectedView.Id)
                    a++;
            }
            using (var ts = new Transaction(_doc, "Createv View Sheet"))
            {
                ts.Start();

                for (int i = 0; i < Amount; i++)
                {
                    ViewSheet viewSheet = ViewSheet.Create(_doc, SelectedTitleBlockType.Id);
                    if ((viewportList.Count == 0 || a == 0) && (i == 0))
                    {

                        Viewport viewport = Viewport.Create(_doc, viewSheet.Id, SelectedView.Id, new XYZ(1, 1, 0));
                    }
                    else
                    {
                        ElementId viewportId = (SelectedView as View).Duplicate(ViewDuplicateOption.WithDetailing);
                        Viewport viewport = Viewport.Create(_doc, viewSheet.Id, viewportId, new XYZ(1, 1, 0));
                    }
                    viewSheet.get_Parameter(BuiltInParameter.SHEET_DRAWN_BY).Set(DesignedBy);
                }
                ts.Commit();
            }
            RaiseCloseRequest();
        }

        public event EventHandler CloseRequest;
        private void RaiseCloseRequest()
        {
            CloseRequest?.Invoke(this, EventArgs.Empty);
        }
    }
}
