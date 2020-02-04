using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using System.Web.UI;
using System.Web.UI.WebControls;
using DotNetNuke.Common.Utilities;
using DotNetNuke.Entities.Modules;
using DotNetNuke.Entities.Modules.Actions;
using DotNetNuke.Services.Exceptions;
using DotNetNuke.Services.Localization;

using GIBS.Modules.FlexMLS_Favorites.Components;
using DotNetNuke.Common;

namespace GIBS.Modules.FlexMLS_Favorites
{
    public partial class ViewFlexMLS_Favorites : PortalModuleBase, IActionable
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (!IsPostBack)
                {
                 //   lblDebug.Text = this.ModuleId.ToString();
                }

                LoadModule();
                CountRecords();
              //  LoadFavSearches();


            }
            catch (Exception ex)
            {
                Exceptions.ProcessModuleLoadException(this, ex);
            }
        }

        public void LoadModule()
        {
            try
            {

                if (Request.IsAuthenticated)
                {
                    //List<FlexMLS_FavoritesInfo> items;
                    //FlexMLS_FavoritesController controller = new FlexMLS_FavoritesController();

                    //if (Settings.Contains("FlexMLSModule"))
                    //{
                    //    ddlFlexMLSModule.SelectedValue = Settings["FlexMLSModule"].ToString();
                    //}
                    

              //      FlexMLS_FavoritesSettings settingsData = new FlexMLS_FavoritesSettings(this.TabModuleId);
                    if (Settings["FlexMLSModulePage"] != null)
                    {

                        //Int32.Parse(settingsData.FlexMLSModule.ToString())

                        // SEARCHES
                        hyperlinkFavSearches.Visible = true;
                        string vPage1 = Globals.NavigateURL(PortalSettings.ActiveTab.TabID, "FavSearch", "mid=" + ModuleId.ToString());
                        hyperlinkFavSearches.NavigateUrl = vPage1.ToString();


                        // LISTINGS
                        hyperlinkFavListings.Visible = true;
                        string vPage = Globals.NavigateURL(PortalSettings.ActiveTab.TabID, "FavListing", "mid=" + ModuleId.ToString());
                        hyperlinkFavListings.NavigateUrl = vPage.ToString();

                    }
                }
                else
                //NOT LOGGED IN
                {
                    hyperlinkFavSearches.Visible = false;
                    hyperlinkFavListings.Visible = false;
                    LabelNotLoggedIn.Visible = true;
                    LabelNotLoggedIn.Text = Localization.GetString("LabelNotLoggedIn", this.LocalResourceFile);
                    //(string)GetGlobalResourceObject("", "LabelNotLoggedIn.Text");
                    ModuleConfiguration.ModuleTitle = Localization.GetString("ModuleTitleNotLoggedIn", this.LocalResourceFile);
                }

            }
            catch (Exception ex)
            {
                Exceptions.ProcessModuleLoadException(this, ex);
            }
        }

        public void CountRecords()
        {
            try
            {
                List<FlexMLS_FavoritesInfo> items;
                List<FlexMLS_FavoritesInfo> items1;
                FlexMLS_FavoritesController controller = new FlexMLS_FavoritesController();

             //  FlexMLS_FavoritesSettings settingsData = new FlexMLS_FavoritesSettings(this.TabModuleId);
                if (Settings["FlexMLSModule"] != null)
                {
                    //SEARCHES
                    items = controller.FlexMLS_Favorites_Get_List(Convert.ToInt32(Settings["FlexMLSModule"].ToString()), this.UserId, "Search");
                    hyperlinkFavSearches.Text = "Saved Searches (" + items.Count.ToString() + ")";
                    
                    //LISTINGS
                    items1 = controller.FlexMLS_Favorites_Get_List(Convert.ToInt32(Settings["FlexMLSModule"].ToString()), this.UserId, "Listing");
                    hyperlinkFavListings.Text = "Saved Listings (" + items1.Count.ToString() + ")";

                }
            }
            catch (Exception ex)
            {
                Exceptions.ProcessModuleLoadException(this, ex);
            }
        }


        #region IActionable Members

        public DotNetNuke.Entities.Modules.Actions.ModuleActionCollection ModuleActions
        {
            get
            {
                //create a new action to add an item, this will be added to the controls
                //dropdown menu
                ModuleActionCollection actions = new ModuleActionCollection();
                //actions.Add(GetNextActionID(), Localization.GetString(ModuleActionType.AddContent, this.LocalResourceFile),
                //    ModuleActionType.AddContent, "", "", EditUrl(), false, DotNetNuke.Security.SecurityAccessLevel.Edit,
                //     true, false);

                return actions;
            }
        }

        #endregion


        /// <summary>
        /// Handles the items being bound to the datalist control. In this method we merge the data with the
        /// template defined for this control to produce the result to display to the user
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void dlListings_ItemDataBound(object sender, System.Web.UI.WebControls.DataListItemEventArgs e)
        {
            HyperLink FavoriteLink = (HyperLink)e.Item.FindControl("hlFavorite");


            FavoriteLink.Text = DataBinder.Eval(e.Item.DataItem, "Favorite").ToString();
            
            
            //string contentValue = string.Empty;

            //FlexMLS_FavoritesSettings settingsData = new FlexMLS_FavoritesSettings(this.TabModuleId);

            //if (settingsData.FlexMLSModule != null)
            //{
            //    //apply the content to the template
            //    ArrayList propInfos = CBO.GetPropertyInfo(typeof(FlexMLS_FavoritesInfo));
            //    contentValue = settingsData.FlexMLSModule;

            //    if (contentValue.Length != 0)
            //    {
            //        foreach (PropertyInfo propInfo in propInfos)
            //        {
            //            object propertyValue = DataBinder.Eval(e.Item.DataItem, propInfo.Name);
            //            if (propertyValue != null)
            //            {
            //                contentValue = contentValue.Replace("[" + propInfo.Name.ToUpper() + "]",
            //                        Server.HtmlDecode(propertyValue.ToString()));
            //            }
            //        }
            //    }
            //    else
            //        //blank template so just set the content to the value
            //        contentValue = Server.HtmlDecode(DataBinder.Eval(e.Item.DataItem, "Favorite").ToString());
            //}
            //else
            //{
            //    //no template so just set the content to the value
            //    contentValue = Server.HtmlDecode(DataBinder.Eval(e.Item.DataItem, "Favorite").ToString());
            //}

            //content.Text = contentValue;
        }

    }
}