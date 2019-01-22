using SolidWorks.Interop.sldworks;

namespace SWapp
{
    public class interference
    {
        public int[] interferenceDir(SldWorks swApp, ModelDoc2 swModel, Feature swMateFeat, MateEntity2[] swMateEnt)
        {
            // Initiate variables
            MathUtility swMathUtil = default(MathUtility);
            AssemblyDoc swAssembly = default(AssemblyDoc);
            ModelDocExtension swModelDocExt = default(ModelDocExtension);
            DragOperator swDragOp = default(DragOperator);
            MathTransform swXform = default(MathTransform);
            Feature swSubFeat = default(Feature);
            Component2 swComp0 = default(Component2);
            Component2 swComp1 = default(Component2);
            SelectionMgr swSelMgr = default(SelectionMgr);
            Component2[] EntityArray;
            bool supstat = false;
            bool bRet = false;
            int i = 0;
            int j = 0;
            int k = 0;
            double[] dir;
            double[] entityParameters = new double[8];
            int[] swAssyDir = new int[6];
            double[] trans = new double[16];
            // Get accesss to the model
            swModelDocExt = swModel.Extension;
            // Suppressing mates to be able to move them
            swSubFeat = (Feature)swMateFeat.GetFirstSubFeature();
            while ((swSubFeat != null))
            {
                supstat = swModelDocExt.SelectByID2(swSubFeat.Name, "MATE", 0, 0, 0, false, 0, null, 0);
                supstat = swModel.EditSuppress2();
                swSubFeat = (Feature)swSubFeat.GetNextSubFeature();
            }
            // Get parts
            swComp0 = swMateEnt[0].ReferenceComponent;
            swComp1 = swMateEnt[1].ReferenceComponent;
            // Run .stl meshing function
            save.saveAs(swApp, swComp0, swComp1, swModelDocExt);
            // Define variables
            swAssembly = (AssemblyDoc)swModel;
            swDragOp = (DragOperator)swAssembly.GetDragOperator();
            swMathUtil = (MathUtility)swApp.GetMathUtility();
            swSelMgr = swModel.SelectionManager;
            EntityArray = new Component2[] { swComp0, swComp1 };
            // Decide parameters for DragOperator interface
            bRet = swDragOp.AddComponent(swComp1, false);
            bRet = swDragOp.CollisionDetection(EntityArray, true, true);
            swDragOp.CollisionDetectionEnabled = true;
            swDragOp.DynamicClearanceEnabled = false;
            swDragOp.UseAbsoluteTransform = false;
            swDragOp.IgnoreComplexSurfaces = true;
            swDragOp.HearClashes = false;
            swDragOp.HighlightClashes = false;
            swDragOp.TransformType = 0;
            swDragOp.DragMode = 0;
            // Begin moving component
            bRet = swDragOp.BeginDrag();
            for (i = 0; i < 6; i++)
            {
                if (i == 0)
                    dir = new double[] { 0.005, 0, 0 };
                else if (i == 1)
                    dir = new double[] { -0.005, 0, 0 };
                else if (i == 2)
                    dir = new double[] { 0, 0.005, 0 };
                else if (i == 3)
                    dir = new double[] { 0, -0.005, 0 };
                else if (i == 4)
                    dir = new double[] { 0, 0, 0.005 };
                else
                    dir = new double[] { 0, 0, -0.005 };
                // Move up to five steps along vector (dir)
                for (j = 0; j < 5; j++)
                {
                    trans = new double[] { 1, 0, 0, 0, 1, 0, 0, 0, 1, dir[0], dir[1], dir[2], 1, 0, 0, 0 };
                    swXform = (MathTransform)swMathUtil.CreateTransform(trans);
                    bRet = swDragOp.Drag(swXform);
                    // Check return status of interference detector
                    k = j + 1;
                    if (bRet == true)
                    {
                        swAssyDir[i] = 1;
                    }
                    else
                    {
                        swAssyDir[i] = 0;
                        break;
                    }
                }
                // Move back the number of steps moved
                if (swAssyDir[i] == 1)
                {
                    trans[9] = trans[9] * (-k);
                    trans[10] = trans[10] * (-k);
                    trans[11] = trans[11] * (-k);
                    swXform = (MathTransform)swMathUtil.CreateTransform(trans);
                    bRet = swDragOp.Drag(swXform);
                }
            }
            bRet = swDragOp.EndDrag();
            // Unsuppress mates
            swSubFeat = (Feature)swMateFeat.GetFirstSubFeature();
            while ((swSubFeat != null))
            {
                supstat = swModelDocExt.SelectByID2(swSubFeat.Name, "MATE", 0, 0, 0, false, 0, null, 0);
                supstat = swModel.EditUnsuppress2();
                swSubFeat = (Feature)swSubFeat.GetNextSubFeature();
            }
            // Return interference vector
            return swAssyDir;
        }
    }
}