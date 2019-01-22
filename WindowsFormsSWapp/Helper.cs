using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using SolidWorks.Interop.sldworks;
using SolidWorks.Interop.swconst;
using System.Threading;
using System.Globalization;
using System.Diagnostics;

namespace SWapp
{
    internal static class Helper
    {
        internal static bool processModel(SldWorks swApp, string file, string targetFile, string calcFile, CancellationToken cancellationToken)
        {
            // Initiate variables
            Thread.CurrentThread.CurrentCulture = CultureInfo.CreateSpecificCulture("en-GB");
            StreamWriter toFile = new StreamWriter(targetFile);
            toFile.AutoFlush = true;
            StreamWriter toCalc = new StreamWriter(calcFile);
            toCalc.AutoFlush = true;
            MathUtility swMathUtil = default(MathUtility);
            ModelDoc2 swModel = default(ModelDoc2);
            Feature swFeat = default(Feature);
            Feature swMateFeat = null;
            Feature swSubFeat = default(Feature);
            Mate2 swMate = default(Mate2);
            Component2 swComp = default(Component2);
            MateEntity2[] swMateEnt = new MateEntity2[3];
            MathTransform swTrans = default(MathTransform);
            MathPoint swOrig = default(MathPoint);
            AssemblyDoc swAssembly = default(AssemblyDoc);
            double[] corners = new double[6];
            int[] swAssyDir = new int[6];
            double[] nPt = new double[3];
            object vPt = null;
            double height = 0;
            double width = 0;
            double depth = 0;
            int Warning = 0;
            int Error = 0;
            int i = 0;
            double[] entityParameters = new double[8];
            // Start function
            try
            {
                if (cancellationToken.IsCancellationRequested)
                {
                    return false;
                }                
                string extention = Path.GetExtension(file);
                int type = 0;
                if (extention.ToLower().Contains("sldprt"))
                {
                    type = (int)swDocumentTypes_e.swDocPART;
                }
                else
                {
                    type = (int)swDocumentTypes_e.swDocASSEMBLY;
                }
                // Get assembly model
                swModel = swApp.OpenDoc6(file, type, (int)swOpenDocOptions_e.swOpenDocOptions_Silent, "", ref Error, ref Warning) as ModelDoc2;
                if (Error != 0)
                {
                    return false;
                }
                if (swModel == null)
                {
                    return false;
                }
                swModel.Visible = true;            
                // Get first assembly feature
                swFeat = (Feature)swModel.FirstFeature();
                // Iterate over features in FeatureManager design tree
                while ((swFeat != null))
                {
                    if ("MateGroup" == swFeat.GetTypeName())
                    {
                        swMateFeat = swFeat;
                        break;
                    }
                    swFeat = swFeat = swFeat.GetNextFeature();
                }
                toFile.WriteLine(" " + swMateFeat.Name);
                toFile.WriteLine("");
                // Get first mate, which is a subfeature
                swSubFeat = (Feature)swMateFeat.GetFirstSubFeature();                
                while ((swSubFeat != null))
                {
                    swMate = (Mate2)swSubFeat.GetSpecificFeature2();
                    if ((swMate != null))
                    {
                        for (i = 0; i <= 1; i++)
                        {                            
                            swMateEnt[i] = swMate.MateEntity(i);
                            swComp = swMateEnt[i].ReferenceComponent;
                            // Initate point
                            nPt[0] = 0.0;
                            nPt[1] = 0.0;
                            nPt[2] = 0.0;
                            vPt = nPt;
                            // Get component origin point
                            swTrans = swComp.Transform2;
                            swMathUtil = (MathUtility)swApp.GetMathUtility();
                            swOrig = (MathPoint)swMathUtil.CreatePoint(vPt);
                            swOrig = (MathPoint)swOrig.MultiplyTransform(swTrans);
                            // Write parameters to readable ASCII file
                            toFile.WriteLine("    " + swSubFeat.Name);
                            toFile.WriteLine("      Type              = " + swMate.Type);
                            toFile.WriteLine("      Alignment         = " + swMate.Alignment);
                            toFile.WriteLine("      Can be flipped    = " + swMate.CanBeFlipped);
                            toFile.WriteLine("");                       
                            toFile.WriteLine("      Component         = " + swComp.Name2); 
                            toFile.WriteLine("      Origin            = (" + ((double[])swOrig.ArrayData)[0] * 1000.0 + ", " + ((double[])swOrig.ArrayData)[1] * 1000.0 + ", " + ((double[])swOrig.ArrayData)[2] * 1000.0 + ")");
                            toFile.WriteLine("      Mate enity type   = " + swMateEnt[i].ReferenceType);
                            entityParameters = (double[])swMateEnt[i].EntityParams;
                            toFile.WriteLine("      (x,y,z)           = (" + entityParameters[0]*1000 + ", " + entityParameters[1]*1000 + ", " + entityParameters[2]*1000 + ")");
                            toFile.WriteLine("      (i,j,k)           = (" + entityParameters[3] + ", " + entityParameters[4] + ", " + entityParameters[5] + ")");
                            toFile.WriteLine("      Radius 1          = " + entityParameters[6]*1000);
                            toFile.WriteLine("      Radius 2          = " + entityParameters[7]*1000);
                            toFile.WriteLine("");
                            // Write parameters to a simplified ASCII file for computation
                            toCalc.Write(swSubFeat.Name);
                            toCalc.Write(" " + swMate.Type);
                            toCalc.Write(" " + swMate.Alignment);
                            toCalc.Write(" " + swMate.CanBeFlipped);
                            toCalc.Write(" " + swComp.Name2);
                            toCalc.Write(" " + ((double[])swOrig.ArrayData)[0] * 1000.0 + "," + ((double[])swOrig.ArrayData)[1] * 1000.0 + "," + ((double[])swOrig.ArrayData)[2] * 1000.0);
                            toCalc.Write(" " + swMateEnt[i].ReferenceType);
                            toCalc.Write(" " + entityParameters[0] * 1000.0 + "," + entityParameters[1] * 1000.0 + "," + entityParameters[2] * 1000.0);
                            toCalc.Write(" " + entityParameters[3] + "," + entityParameters[4] + "," + entityParameters[5]);
                            toCalc.Write(" " + entityParameters[6] * 1000.0);
                            toCalc.WriteLine(" " + entityParameters[7] * 1000.0);                            
                        }
                        toFile.WriteLine(" ");                        
                    }
                    // Get the next mate in MateGroup
                    swSubFeat = (Feature)swSubFeat.GetNextSubFeature();
                }
                // Get bounding box around assembly
                swAssembly = (AssemblyDoc)swModel;
                corners = swAssembly.GetBox(1);
                height = (corners[4] - corners[1])*1000.0;
                width = (corners[3] - corners[0])*1000.0;
                depth = (corners[5] - corners[2])*1000.0;
                // Write to file
                toFile.WriteLine("Aprx. assembly dimensions");
                toFile.WriteLine("(Height, Width, Depth) = (" + height + ", " + width + ", " + depth + ")");
                toFile.WriteLine(" ");
                toCalc.WriteLine("dims(hwd) " + height + " " + width + " " + depth);
                // Get Possible Assembly Directions with interferenceDir function
                interference inter = new interference();
                swAssyDir = inter.interferenceDir(swApp, swModel, swMateFeat, swMateEnt);
                // Write swAssyDir to file
                toFile.WriteLine("(x+, x-, y+, y-, z+, z-) = (" + swAssyDir[0] + ", " + swAssyDir[1] + ", " + swAssyDir[2] + ", " + swAssyDir[3] + ", " + swAssyDir[4] + ", " + swAssyDir[5] + ") ");
                toFile.WriteLine("1 is possible, 0 is not possible");
                toFile.WriteLine("");
                toFile.WriteLine("All dimensions are in mm");
                toCalc.Write("dir " + swAssyDir[0] + " " + swAssyDir[1] + " " + swAssyDir[2] + " " + swAssyDir[3] + " " + swAssyDir[4] + " " + swAssyDir[5]);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
        internal static string[] getCADFilesFromDirectory(string directory)
        {
            // check if directory exists
            if ((!Directory.Exists(directory)))
                return null;
            // get sldprts from directory
            //string[] parts = Directory.GetFiles(directory, "*.sldprt");
            string[] assemblies = Directory.GetFiles(directory, "*.sldasm");
            List<string> files = new List<string>();
            //if (parts != null)
            //    files.AddRange(parts);
            if (assemblies != null)
            {
                files.AddRange(assemblies);
            }
            return files.ToArray();
        }
    }
}
