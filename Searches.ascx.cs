using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using DotNetNuke.Common;
using DotNetNuke.Common.Utilities;
using DotNetNuke.Entities.Modules;
using DotNetNuke.Services.Exceptions;
using DotNetNuke.Services.Localization;
using GIBS.Modules.FlexMLS_Favorites.Components;
using System.Text;
using System.Collections.Specialized;
namespace GIBS.Modules.FlexMLS_Favorites
{
    public partial class Searches : PortalModuleBase
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            
            if (!IsPostBack)
            {
                if (UserId == -1)
                {
                    Response.Redirect(Globals.NavigateURL(this.PortalSettings.LoginTabId));
                }
                GetNewSearchURL();
                LoadGrid();
                lblDailyEmails.Text = Localization.GetString("lblDailyEmails", this.LocalResourceFile);
                SetLinks();
            }
        }


        public void SetLinks()
        {
            try
            {
                string vPage1 = Globals.NavigateURL(PortalSettings.ActiveTab.TabID, "FavListing", "mid=" + ModuleId.ToString());

                HyperLinkViewSavedListings.Visible = true;
                HyperLinkViewSavedListings.NavigateUrl = vPage1.ToString();
            }
            catch (Exception ex)
            {
                Exceptions.ProcessModuleLoadException(this, ex);
            }
        }

        public void GetNewSearchURL()
        {
            try
            {
                int intModTabID = -1;
                ModuleInfo objModuleInfo = new ModuleInfo();
                ModuleController objModuleContr = new ModuleController();
                objModuleInfo = objModuleContr.GetModuleByDefinition(PortalId, "GIBS - FlexMLS");
                intModTabID = objModuleInfo.TabID;

                string strRedir = Globals.NavigateURL(intModTabID);

                //   string vLink = Globals.NavigateURL(Int32.Parse(settingsData.FlexMLSModule.ToString()),false);
                HyperLinkNewSearch.NavigateUrl = strRedir.ToString();
            }
            catch (Exception ex)
            {
                Exceptions.ProcessModuleLoadException(this, ex);
            }
        }

        public string GetPropertyTypeDesc(string PropType)
        {

            try
            {
                string myRetValue = "";
                switch (PropType)
                {
                    case "RESI":
                        myRetValue = "Single Family";
                        break;
                    case "COND":
                        myRetValue = "Condominium";
                        break;
                    case "HOTL":
                        myRetValue = "Hotel/Motel";
                        break;
                    case "LOTL":
                        myRetValue = "Vacant Land";
                        break;
                    case "MULT":
                        myRetValue = "Multi-Family";
                        break;
                    case "COMM":
                        myRetValue = "Commercial";
                        break;
                    default:
                        myRetValue = PropType;
                        break;
                }
                return myRetValue.ToString();


            }
            catch (Exception ex)
            {
                Exceptions.ProcessModuleLoadException(this, ex);
                return "Error";
            }

        }


        public void LoadGrid()
        {
            try
            {
                List<FlexMLS_FavoritesInfo> items;
                FlexMLS_FavoritesController controller = new FlexMLS_FavoritesController();


               // FlexMLS_FavoritesSettings settingsData = new FlexMLS_FavoritesSettings(this.TabModuleId);
                if (Settings["FlexMLSModule"] != null)
                {
                    items = controller.FlexMLS_Favorites_Get_List(Convert.ToInt32(Settings["FlexMLSModule"].ToString()), this.UserId, "Search");

                    //if (items.Count == 0)
                    //{
                    //    FlexMLS_FavoritesInfo item = new FlexMLS_FavoritesInfo();
                    //    item.ModuleId = this.ModuleId;
                    //    item.CreatedByUser = this.UserId;
                    //    item.Favorite = Localization.GetString("DefaultContent", LocalResourceFile);

                    //    items.Add(item);
                    //}
                    if (items.Count > 0)
                    {
                        //bind the data
                        GridView1.Visible = true;
                        GridView1.DataSource = items;
                        GridView1.DataBind();
                        lblDailyEmails.Visible = true;
                    }
                    else
                    {
                        lblErrorMessage.Text = Localization.GetString("DefaultContent", LocalResourceFile);
                        lblDailyEmails.Visible = false;
                        GridView1.Visible = false;
                    }


                }
                else
                {
                    lblMessage.Text = "Please configure the module settings.";
                }
            }
            catch (Exception ex)
            {
                Exceptions.ProcessModuleLoadException(this, ex);
            }
        }


        protected void GridView1_RowDataBound(object sender, GridViewRowEventArgs e)
        {

            if (e.Row.RowType == DataControlRowType.DataRow)
            {

                StringBuilder _SearchCriteria = new StringBuilder();
                _SearchCriteria.Capacity = 500;
                
             
                HyperLink hl = (HyperLink)e.Row.FindControl("Hyperlink2");
                
                // DEBUG
                //Label1.Text += hl.Text.ToString() + "<br />";
                
                NameValueCollection query = HttpUtility.ParseQueryString(hl.Text.ToString());


                if (query["type"] != null && query["type"] != "")
                {
                    _SearchCriteria.Append("Type: <b>" + GetPropertyTypeDesc(query["Type"].ToString()) + "</b> &nbsp;");
                    
                }
                if (query["Town"] != null && query["Town"] != "")
                {
                    _SearchCriteria.Append(" Town: <b>" + query["Town"].ToString() + "</b> &nbsp;");
                }
                if (query["Village"] != null && query["Village"] != "")
                {
                    _SearchCriteria.Append(" Village: <b>" + query["Village"].ToString() + "</b> &nbsp;");
                }

                if (query["Complex"] != null && query["Complex"] != "")
                {
                    _SearchCriteria.Append(" Complex: <b>" + query["Complex"].ToString().Replace("_"," ") + "</b> &nbsp;");
                }

                if (query["Beds"] != null && query["Beds"] != "")
                {
                    _SearchCriteria.Append(" Bedrooms: <b>" + query["Beds"].ToString() + "</b> &nbsp;");
                }
                if (query["Baths"] != null && query["Baths"] != "")
                {
                    _SearchCriteria.Append(" Bathrooms: <b>" + query["Baths"].ToString() + "</b> &nbsp;");
                }
                if (query["WaterFront"] != null && query["WaterFront"] != "")
                {
                    _SearchCriteria.Append(" Waterfront: <b>" + query["WaterFront"].ToString() + "</b> &nbsp;");
                }
                if (query["WaterView"] != null && query["WaterView"] != "")
                {
                    _SearchCriteria.Append(" Waterview: <b>" + query["WaterView"].ToString() + "</b> &nbsp;");
                }
                if (query["Low"] != null && query["Low"] != "")
                {
                    _SearchCriteria.Append(" Min. Price: <b>" + query["Low"].ToString() + "</b> &nbsp;");
                }
                if (query["High"] != null && query["High"] != "")
                {
                    _SearchCriteria.Append(" Max Price: <b>" + query["High"].ToString() + "</b> &nbsp;");
                }
                if (query["LOID"] != null && query["LOID"] != "")
                {
                    _SearchCriteria.Append(" Office: <b>" + query["LOID"].ToString() + "</b> &nbsp;");
                }

                if (query["DOM"] != null && query["DOM"] != "")
                {
                    _SearchCriteria.Append(" Days on Market: <b>" + query["DOM"].ToString() + "</b> &nbsp;");
                }

                hl.Text = _SearchCriteria.ToString().Remove(_SearchCriteria.Length - 7);


            }

        }

        protected void GridView1_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            // No requirement to implement code here
        }

        protected void GridView1_RowEditing(object sender, GridViewEditEventArgs e)
        {
            // Prevents the GridView from going into EDIT MODE (textboxes)
            e.Cancel = true;
        }

        protected void GridView1_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            try
            {

                if (e.CommandName == "Delete")
                {

                    int ID = Convert.ToInt32(e.CommandArgument);


                    FlexMLS_FavoritesController controller = new FlexMLS_FavoritesController();
                    controller.FlexMLS_Favorites_Delete(ID);
                   
                    LoadGrid();

                }

                if (e.CommandName == "Edit")
                {
                    int ID = Convert.ToInt32(e.CommandArgument);

                 //   FillIncomeExpenseEdit(ieID);


                }




            }
            catch (Exception ex)
            {
                Exceptions.ProcessModuleLoadException(this, ex);
            }

        }




        protected void cbxComplete_CheckedChanged(object sender, EventArgs e)
        {

            try
            {


                int selRowIndex = ((GridViewRow)(((CheckBox)sender).Parent.Parent)).RowIndex;
                CheckBox cb = (CheckBox)GridView1.Rows[selRowIndex].FindControl("cbxEmailSearch");

                //int itemid = Convert.ToInt32(GridView1.Rows[selRowIndex].Cells[0].Text.ToString());
                int itemid = Convert.ToInt32(GridView1.DataKeys[selRowIndex].Value.ToString());

                FlexMLS_FavoritesInfo item = new FlexMLS_FavoritesInfo();
                FlexMLS_FavoritesController controller = new FlexMLS_FavoritesController();

                if (cb.Checked)
                {
                    item.EmailSearch = true;
                }
                else
                {
                    item.EmailSearch = false;
                }

                item.ItemId = itemid;

                controller.FlexMLS_Favorites_Update_EmailSearch(item);

                //lblMessage.Text = selRowIndex.ToString() + " is the row selected and ItemID = " + itemid.ToString();
                lblMessage.Text = "Your selection has been updated!";
                LoadGrid();

            }
            catch (Exception ex)
            {
                Exceptions.ProcessModuleLoadException(this, ex);
            }


        }




    }
}