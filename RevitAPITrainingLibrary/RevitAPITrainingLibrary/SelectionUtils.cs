using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using Autodesk.Revit.UI.Selection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RevitAPITrainingLibrary
{
    public class SelectionUtils
    {
        public static Element PickObject(ExternalCommandData commandData, string message = "Выберите объект")
        {
            UIApplication uiapp = commandData.Application;
            UIDocument uidoc = uiapp.ActiveUIDocument;
            Document doc = uidoc.Document;

            var selectedObject = uidoc.Selection.PickObject(ObjectType.Element, message);
            var oElement = doc.GetElement(selectedObject);
            return oElement;
        }

        public static List<Wall> PickWalls(ExternalCommandData commandData, string message = "Выберите стены")
        {
            UIApplication uiapp = commandData.Application;
            UIDocument uidoc = uiapp.ActiveUIDocument;
            Document doc = uidoc.Document;

            IList<Reference> selectedElementRefList = null;
            try
            {
                selectedElementRefList = uidoc.Selection.PickObjects(Autodesk.Revit.UI.Selection.ObjectType.Element, new WallFilter(), "Выберите стены по грани");
            }
            catch (Autodesk.Revit.Exceptions.OperationCanceledException)
            { }
            catch (System.NullReferenceException)
            { }
            if (selectedElementRefList == null)
                return null;
            else
            {
                var WallList = new List<Wall>();
                foreach (var selectedElement in selectedElementRefList)
                {
                    Wall oWall = doc.GetElement(selectedElement) as Wall;
                    WallList.Add(oWall);
                }
                return WallList;
            }
        }
        public static List<XYZ> GetPoints(ExternalCommandData commandData,
                string promtMessage, ObjectSnapTypes objectSnapTypes)
        {
            UIApplication uiapp = commandData.Application;
            UIDocument uidoc = uiapp.ActiveUIDocument;

            List<XYZ> points = new List<XYZ>();
            while (true)
            {
                XYZ pickedPoint = null;
                try
                {
                    pickedPoint = uidoc.Selection.PickPoint(objectSnapTypes, promtMessage);
                }
                catch (Autodesk.Revit.Exceptions.OperationCanceledException)
                {
                    break;
                }
                points.Add(pickedPoint);
            }

            return points;
        }
        public static List<XYZ> GetTwoPoints(ExternalCommandData commandData,
               string promtMessage, ObjectSnapTypes objectSnapTypes)
        {
            UIApplication uiapp = commandData.Application;
            UIDocument uidoc = uiapp.ActiveUIDocument;

            List<XYZ> points = new List<XYZ>();
            while (points.Count < 2)
            {
                XYZ pickedPoint = null;
                try
                {
                    pickedPoint = uidoc.Selection.PickPoint(objectSnapTypes, promtMessage);
                }
                catch (Autodesk.Revit.Exceptions.OperationCanceledException)
                {
                    break;
                }
                points.Add(pickedPoint);
            }

            return points;
        }

        public static XYZ GetPoint(ExternalCommandData commandData,
                string promtMessage, ObjectSnapTypes objectSnapTypes)
        {
            UIApplication uiapp = commandData.Application;
            UIDocument uidoc = uiapp.ActiveUIDocument;

            XYZ pickedPoint = new XYZ();
            
                pickedPoint = null;
                try
                {
                    pickedPoint = uidoc.Selection.PickPoint(objectSnapTypes, promtMessage);
                }
                catch (Autodesk.Revit.Exceptions.OperationCanceledException)
                {
                }
                
            

            return pickedPoint;
        }
        public static List<XYZ> GetDuctPoints(ExternalCommandData commandData,
                string promtMessage, ObjectSnapTypes objectSnapTypes)
        {
            UIApplication uiapp = commandData.Application;
            UIDocument uidoc = uiapp.ActiveUIDocument;

            List<XYZ> points = new List<XYZ>();
            while (points.Count < 2)
            {
                XYZ pickedPoint = null;
                try
                {
                    pickedPoint = uidoc.Selection.PickPoint(objectSnapTypes, promtMessage);
                }
                catch (Autodesk.Revit.Exceptions.OperationCanceledException)
                {
                    break;
                }
                points.Add(pickedPoint);
            }

            return points;
        }
    }
}