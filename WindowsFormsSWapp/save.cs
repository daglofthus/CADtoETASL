using System.IO;
using SolidWorks.Interop.sldworks;


namespace SWapp
{
    public static class save
    {
        public static bool saveAs(SldWorks swApp, Component2 swComp0, Component2 swComp1, ModelDocExtension swModelDocExt)
        {
            // Initiate variables
            string stComp0;
            int Error = 0;
            int Warnings = 0;
            bool bRet = false;
            // Get component name
            stComp0 = swComp0.GetPathName();
            stComp0 = stComp0.Replace(Path.GetExtension(stComp0), ".stl");
            // Set file preferences
            swApp.SetUserPreferenceDoubleValue(2, 0.00002);
            swApp.SetUserPreferenceDoubleValue(3, 0.174532925199433);
            swApp.SetUserPreferenceIntegerValue(78, 433);
            swApp.SetUserPreferenceToggle(69, false);
            swApp.SetUserPreferenceToggle(70, false);
            swApp.SetUserPreferenceToggle(191, false);
            // Save as .stl
            bRet = swModelDocExt.SaveAs(stComp0, 0, 1, null, ref Error, ref Warnings);
            // Check for errors
            if (Error != 0)
            {
                return false;
            }
            if (Warnings != 0)
            {
                return false;
            }
            // Return status bool
            return bRet;
        }
    }
}
