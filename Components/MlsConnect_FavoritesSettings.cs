using DotNetNuke.Entities.Modules;
using System;

namespace GIBS.Modules.FlexMLS_Favorites.Components
{
    /// <summary>
    /// Provides strong typed access to settings used by module
    /// </summary>
    public class FlexMLS_FavoritesSettings : ModuleSettingsBase
    {


        #region public properties

        /// <summary>
        /// get/set template used to render the module content
        /// to the user
        /// </summary>
        /// 


        public string FlexMLSModuleID
        {
            get
            {
                if (Settings.Contains("FlexMLSModuleID"))
                    return Settings["FlexMLSModuleID"].ToString();
                return "";
            }
            set
            {
                var mc = new ModuleController();
                mc.UpdateModuleSetting(ModuleId, "FlexMLSModuleID", value.ToString());
            }
        }


        public string MLSImagesURL
        {
            get
            {
                if (Settings.Contains("MLSImagesURL"))
                    return Settings["MLSImagesURL"].ToString();
                return "";
            }
            set
            {
                var mc = new ModuleController();
                mc.UpdateModuleSetting(ModuleId, "MLSImagesURL", value.ToString());
            }
        }


        public string FlexMLSModulePage
        {
            get
            {
                if (Settings.Contains("FlexMLSModulePage"))
                    return Settings["FlexMLSModulePage"].ToString();
                return "";
            }
            set
            {
                var mc = new ModuleController();
                mc.UpdateModuleSetting(ModuleId, "FlexMLSModulePage", value.ToString());
            }
        }

        //_MLSImagesURL   FlexMLSModulePage

        #endregion
    }
}
