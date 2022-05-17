using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using Autodesk.Revit.UI.Selection;
using Prism.Commands;
using RevitAPITrainingLibrary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RevitAPIFurnitureArrangement
{
    public class MainViewViewModel
    {
        private ExternalCommandData _commandData;

        public List<FamilySymbol> FamilyTypes { get; } = new List<FamilySymbol>();
        public DelegateCommand SaveCommand { get; }
        public List<Level> Levels { get; } = new List<Level>();
        public Level SelectedLevel { get; set; }
        public XYZ Point { get; set; }
        public FamilySymbol SelectedFamilyType { get; set; }

        public MainViewViewModel(ExternalCommandData commandData)
        {
            _commandData = commandData;
            Levels = LevelsUtils.GetLevels(commandData);
            FamilyTypes = FamilySymbolUtils.GetFurneturesFamilySymbols(commandData);
            SaveCommand = new DelegateCommand(OnSaveCommand);
            Point = SelectionUtils.GetPoint(commandData, "Выберите точки", ObjectSnapTypes.Endpoints);
        }

        private void OnSaveCommand()
        {
            UIApplication uiapp = _commandData.Application;
            UIDocument uidoc = uiapp.ActiveUIDocument;
            Document doc = uidoc.Document;

            if (Point == null ||
                FamilyTypes == null ||
                SelectedLevel == null)
                return;

            FamilyInstanceUtils.CreateFamilyInstance(_commandData, SelectedFamilyType, Point, SelectedLevel);

            RaiseCloseRequest();

        }

        public event EventHandler CloseRequest;
        private void RaiseCloseRequest()
        {
            CloseRequest?.Invoke(this, EventArgs.Empty);
        }
    }
}
