using Autodesk.Revit.DB;
using Autodesk.Revit.DB.Mechanical;
using Autodesk.Revit.UI;
using Autodesk.Revit.UI.Selection;
using Prism.Commands;
using RevitAPITrainingLibrary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RevitAPICreateDucts
{
    public class MainViewViewModel
    {
        private ExternalCommandData _commandData;
        public List<DuctType> DuctTypes { get; } = new List<DuctType>();
        public List<Level> Levels { get; } = new List<Level>();
        public DelegateCommand SaveCommand { get; }
        public double DuctHeight { get; set; }
        public List<XYZ> Points { get; } = new List<XYZ>();
        public DuctType SelectedDuctType { get; set; }
        public Level SelectedLevel { get; set; }

        public MainViewViewModel(ExternalCommandData commandData)
        {
            _commandData = commandData;
            Points = SelectionUtils.GetDuctPoints(_commandData, "Выберите точки", ObjectSnapTypes.Endpoints);
            DuctHeight = 100;
            DuctTypes = DuctsUtils.GetDuctTypes(commandData);
            Levels = LevelsUtils.GetLevels(commandData);
            SaveCommand = new DelegateCommand(OnSaveCommand);


        }

        private void OnSaveCommand()
        {
            UIApplication uiapp = _commandData.Application;
            UIDocument uidoc = uiapp.ActiveUIDocument;
            Document doc = uidoc.Document;

            if (Points.Count < 2 ||
                SelectedDuctType == null ||
                SelectedLevel == null)
                return;

            MEPSystemType systemType = new FilteredElementCollector(doc)
                .OfClass(typeof(MEPSystemType))
                .Cast<MEPSystemType>()
                .FirstOrDefault(m => m.SystemClassification == MEPSystemClassification.SupplyAir);
            
            
            using (var ts = new Transaction(doc, "Create duct"))
            {
                ts.Start();
                for (int i = 0; i < Points.Count; i++)
                {
                    if (i == 0)
                        continue;

                    XYZ prevPoint = Points[i - 1];
                    XYZ currentPoint = Points[i];
                
                    Duct duct = Duct.Create(doc, systemType.Id, SelectedDuctType.Id, SelectedLevel.Id, prevPoint, currentPoint);
                    Parameter ductHeight = duct.get_Parameter(BuiltInParameter.RBS_OFFSET_PARAM);
                    ductHeight.Set(UnitUtils.ConvertToInternalUnits(DuctHeight, UnitTypeId.Millimeters));
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
