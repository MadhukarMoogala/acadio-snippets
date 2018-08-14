using Autodesk.AutoCAD.ApplicationServices.Core;
using Autodesk.AutoCAD.DatabaseServices;
using Autodesk.AutoCAD.EditorInput;
using Autodesk.AutoCAD.Runtime;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FPD.Compare
{
    public static class EditorExtensions
    {

        static public ObjectId SelectLastEnt(this Editor ed)
        {
            PromptSelectionResult LastEnt = ed.SelectLast();
            if (LastEnt.Value != null && LastEnt.Value.Count == 1)
            {
                return LastEnt.Value[0].ObjectId;
            }
            return ObjectId.Null;
        }
    }
    //C:\Program Files\Autodesk\AutoCAD 2019\accoreconsole.exe
    //i D:\Forge\SFO\Drawings\Kitchens.dwg /s "D:\Forge\SFO\FPD.Compare\load.scr"
    public class MyCommands
    {
        [CommandMethod("FDACOMMANDS", "ISDWGSIMILAR1", CommandFlags.Session)]
        public static async void Regress2()
        {
            
        }
        
        [CommandMethod("FDACOMMANDS", "ISDWGSIMILAR", CommandFlags.Session)]
        public static async void Regress()
        {
            var doc = Application.DocumentManager.MdiActiveDocument;
            var db = doc.Database;
            var ed = doc.Editor;
            var promptResult = ed.GetString("Select Drawing To Compare With");
            if (promptResult.Status != PromptStatus.OK) return;
            var drawingToCompareWith = promptResult.StringResult;
            await Application.DocumentManager.ExecuteInCommandContextAsync(
            async (o) =>
                {
                    ed =  Application.DocumentManager.MdiActiveDocument.Editor;

                    await ed.CommandAsync("-COMPARE");
                    await ed.CommandAsync("\n");
                    await ed.CommandAsync(drawingToCompareWith);
                    await ed.CommandAsync();

                }, null);
            doc.SendStringToExecute("COMPAREWINDOW\n", true, false, true);

        }
        [CommandMethod("FDACOMMANDS","COMPAREWINDOW", CommandFlags.Transparent)]
        public static void CompareDrawing()
        {
            var doc = Application.DocumentManager.MdiActiveDocument;
            var db = doc.Database;
            var ed = doc.Editor;
            ed = Application.DocumentManager.MdiActiveDocument.Editor;
            using (OpenCloseTransaction o = new OpenCloseTransaction())
            {
                BlockReference b = o.GetObject(ed.SelectLastEnt(), OpenMode.ForRead) as BlockReference;

                using (var writer = File.CreateText("output.txt"))
                {
                    if (b != null)
                    {
                        writer.WriteLine("TRUE Drawings are same");
                    }
                    else writer.WriteLine("FALSE Drawings aren't same");
                }
            }
        }
    }
}
