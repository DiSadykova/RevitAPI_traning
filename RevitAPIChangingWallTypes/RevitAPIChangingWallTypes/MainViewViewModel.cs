using Autodesk.Revit.DB;
using Autodesk.Revit.DB.Plumbing;
using Autodesk.Revit.UI;
using Prism.Commands;
using RevitAPITrainingLibrary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RevitAPIChangingWallTypes
{
    public class MainViewViewModel
    {
        private ExternalCommandData _commandData;

        public DelegateCommand SaveCommand { get; }
        public List<Wall> PickedObjects { get; } = new List<Wall>();
        public List<WallType> WallTypes { get; } = new List<WallType>();

        public WallType SelectedWallType { get; set; }
        public MainViewViewModel(ExternalCommandData commandData)
        {
            PickedObjects = SelectionUtils.PickWalls(commandData);
            if (PickedObjects == null)
            {
                RaiseCloseRequest();
                return;
            }
            _commandData = commandData;
            SaveCommand = new DelegateCommand(OnSaveCommand);
            WallTypes = WallsUtils.GetWallTypes(commandData);


        }

        private void OnSaveCommand()
        {
            UIApplication uiapp = _commandData.Application;
            UIDocument uidoc = uiapp.ActiveUIDocument;
            Document doc = uidoc.Document;

            if (PickedObjects.Count == 0 || SelectedWallType == null)

                return;

            using (var ts = new Transaction(doc, "Set wall type"))
            {
                ts.Start();

                foreach (var pickedObject in PickedObjects)
                {


                    pickedObject.WallType = SelectedWallType;


                }

                ts.Commit();
            }
            if (PickedObjects != null)
                PickedObjects.Clear();
            RaiseCloseRequest();
        }

        public event EventHandler CloseRequest;
        private void RaiseCloseRequest()
        {
            CloseRequest?.Invoke(this, EventArgs.Empty);
        }
    }
}
