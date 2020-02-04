using System;
using System.Collections;
using System.Collections.Generic;
using System.Web.UI.WebControls;
using DotNetNuke.Entities.Modules;
using DotNetNuke.Services.Exceptions;
using DotNetNuke.Entities.Tabs;
using DotNetNuke.Security;
using DotNetNuke.Services.Localization;
using DotNetNuke.Common.Utilities;
using GIBS.Modules.FlexMLS_Favorites.Components;

//using GIBS.FlexMLS_Favorites.Components;

namespace GIBS.Modules.FlexMLS_Favorites
{
    public partial class Settings : FlexMLS_FavoritesSettings 
    {

        /// <summary>
        /// handles the loading of the module setting for this   FlexMLS_FavoritesSettings
        /// control
        /// </summary>
        public override void LoadSettings()
        {
            try
            {
                if (!IsPostBack)
                {
                    BindModules();

                    //if (FlexMLSModule != null)
                    //{
                    //    ddlFlexMLSModule.SelectedValue = Settings["FlexMLSModule"].ToString();
                    //    lblDebug.Text = "FlexMLSModule: " + Settings["FlexMLSModule"].ToString() + " - FlexMLSModulePage: " + Settings["FlexMLSModulePage"].ToString()
                    //        + " - ModuleId:" + this.ModuleId.ToString();
                    //}
                    //if (FlexMLSModulePage != null)
                    //{
                    //    ddlFlexMLSModulePage.SelectedValue = FlexMLSModulePage.ToString();
                    //}
                    //if (MLSImagesURL != null)
                    //{
                    //    txtMLSImagesUrl.Text = MLSImagesURL.ToString();
                    //}



                    if (Settings.Contains("FlexMLSModuleID"))
                    {
                        ddlFlexMLSModule.SelectedValue = Settings["FlexMLSModuleID"].ToString();
                        lblDebug.Text = "FlexMLSModuleID: " + Settings["FlexMLSModuleID"].ToString() + " - FlexMLSModulePage: " + Settings["FlexMLSModulePage"].ToString()
                            + " - ModuleId:" + this.ModuleId.ToString()
                             + " - TabModuleId:" + this.TabModuleId.ToString();
                    }
                    if (Settings.Contains("FlexMLSModulePage"))
                    {
                        ddlFlexMLSModulePage.SelectedValue = Settings["FlexMLSModulePage"].ToString();
                    }
                    if (Settings.Contains("MLSImagesURL"))
                    {
                        txtMLSImagesUrl.Text = Settings["MLSImagesURL"].ToString();
                    }



                }





            }
            catch (Exception ex)
            {
                Exceptions.ProcessModuleLoadException(this, ex);
            }
        }


        // GET THE DROPDOWN FOR GIBS - FlexMLS MODULES
        private void BindModules()
        {

            DotNetNuke.Entities.Modules.ModuleController mc = new ModuleController();
            ArrayList existMods = mc.GetModulesByDefinition(this.PortalId, "GIBS - FlexMLS");

            foreach (DotNetNuke.Entities.Modules.ModuleInfo mi in existMods)
            {
                if (!mi.IsDeleted)
                {
                    DotNetNuke.Entities.Tabs.TabController tabController = new DotNetNuke.Entities.Tabs.TabController();
                    DotNetNuke.Entities.Tabs.TabInfo tabInfo = tabController.GetTab(mi.TabID, this.PortalId);

                    string strPath = tabInfo.TabName.ToString();

                    ListItem objListItem = new ListItem();

                    objListItem.Value = mi.TabModuleID.ToString();         //mi.ModuleID.ToString();
                    objListItem.Text = strPath + " -> " + mi.ModuleTitle.ToString();

                    ddlFlexMLSModule.Items.Add(objListItem);

                    ListItem objListItemPage = new ListItem();

                    objListItemPage.Value = mi.TabID.ToString();    // mi.ModuleID.ToString();
                    objListItemPage.Text = mi.ModuleTitle.ToString();
                   
                    ddlFlexMLSModulePage.Items.Add(objListItemPage);
                    
                }
            }
           

            ddlFlexMLSModule.Items.Insert(0, new ListItem(Localization.GetString("SelectModule", this.LocalResourceFile), "-1"));
            ddlFlexMLSModulePage.Items.Insert(0, new ListItem("Select Module Page", "-1"));
        }


        /// <summary>
        /// handles updating the module settings for this control
        /// </summary>
        public override void UpdateSettings()
        {
            try
            {
             
                var modules = new ModuleController();

                modules.UpdateModuleSetting(ModuleId, "FlexMLSModuleID", ddlFlexMLSModule.SelectedValue.ToString());
                modules.UpdateModuleSetting(ModuleId, "FlexMLSModulePage", ddlFlexMLSModulePage.SelectedValue.ToString());
                modules.UpdateModuleSetting(ModuleId, "MLSImagesURL", txtMLSImagesUrl.Text.ToString());


                //the following are two sample Module Settings, using the text boxes that are commented out in the ASCX file.
                //module settings
                //modules.UpdateModuleSetting(ModuleId, "Setting1", txtSetting1.Text);
                //modules.UpdateModuleSetting(ModuleId, "Setting2", txtSetting2.Text);

                //tab module settings
                //modules.UpdateTabModuleSetting(TabModuleId, "Setting1",  txtSetting1.Text);
                //modules.UpdateTabModuleSetting(TabModuleId, "Setting2",  txtSetting2.Text);

            }
            catch (Exception ex)
            {
                Exceptions.ProcessModuleLoadException(this, ex);
            }
        }
    }
}