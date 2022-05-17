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

namespace RevitAPICreateElementsAtEqualDistances
{
    public class MainViewViewModel
    {
        private ExternalCommandData _commandData;
        public List<FamilySymbol> FamilyTypes { get; } = new List<FamilySymbol>();
        //public List<Level> Levels { get; } = new List<Level>();
        public DelegateCommand SaveCommand { get; }
        public List<XYZ> Points { get; set; }
        public FamilySymbol SelectedFamilyType { get; set; }
        //public Level SelectedLevel { get; set; }
        public int AmountElement { get; set; } = 2;

        public MainViewViewModel(ExternalCommandData commandData)
        {
            _commandData = commandData;
            FamilyTypes = FamilySymbolUtils.GetFamilySymbols(commandData);
            //Levels = LevelsUtils.GetLevels(commandData);
            SaveCommand = new DelegateCommand(OnSaveCommand);
            Points = SelectionUtils.GetTwoPoints(commandData, "Выберите точки", ObjectSnapTypes.Endpoints);
        }

        private void OnSaveCommand()
        {
            UIApplication uiapp = _commandData.Application;
            UIDocument uidoc = uiapp.ActiveUIDocument;
            Document doc = uidoc.Document;

            if (Points == null || Points.Count<2 ||
                FamilyTypes == null || AmountElement < 2)
                return;

            XYZ startPoint = Points[0];
            XYZ endPoint = Points[1];
            List<XYZ> midPoints = new List<XYZ>();
            XYZ midPoint = startPoint;

            for (int i = 0; i < AmountElement; i++)
            {
                FamilyInstanceUtils.CreateFamilyInstanceAtEqualDistances(_commandData, SelectedFamilyType, midPoint);

                midPoint += ((endPoint - startPoint) / (AmountElement - 1));
                midPoints.Add(midPoint);
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
