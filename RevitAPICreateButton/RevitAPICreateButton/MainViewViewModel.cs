using Autodesk.Revit.DB;
using Autodesk.Revit.DB.Plumbing;
using Autodesk.Revit.UI;
using Prism.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RevitAPITrainingLibrary;

namespace RevitAPICreateButton
{
    internal class MainViewViewModel
    {
        private ExternalCommandData _commandData;

        public DelegateCommand OutputForPipesCommand { get; }
        public DelegateCommand OutputForWallsCommand { get; }
        public DelegateCommand OutputForDoorsCommand { get; }

        public MainViewViewModel(ExternalCommandData commandData)
        {
            _commandData = commandData;
            OutputForPipesCommand = new DelegateCommand(OnOutputForPipesCommand);
            OutputForWallsCommand = new DelegateCommand(OnOutputForWallsCommand);
            OutputForDoorsCommand = new DelegateCommand(OnOutputForDoorsCommand);
        }

        public event EventHandler ShowRequest;
        private void RaiseShowRequest()
        {
            ShowRequest?.Invoke(this, EventArgs.Empty);
        }
        public event EventHandler HideRequest;
        private void RaiseHideRequest()
        {
            HideRequest?.Invoke(this, EventArgs.Empty);
        }

        private void OnOutputForPipesCommand()
        {
            RaiseHideRequest();

            List<Pipe> pipes = FilterUtils.FilterForPipes(_commandData);

            TaskDialog.Show("Количество труб в модели", pipes.Count.ToString());

            RaiseShowRequest();
        }

        private void OnOutputForWallsCommand()
        {
            RaiseHideRequest();

            List<Wall> walls = FilterUtils.FilterForWalls(_commandData);
            WallsUtils.VolumeSumInModel(walls);

            RaiseShowRequest();
        }

        private void OnOutputForDoorsCommand()
        {
            RaiseHideRequest();
            List<FamilyInstance> doorsFInstances = FilterUtils.FilterForDoorsFamilyInstance(_commandData);

            TaskDialog.Show("Количество дверей в модели", doorsFInstances.Count.ToString());

            RaiseShowRequest();
        }

        
    }
}
