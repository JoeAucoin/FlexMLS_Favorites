using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using DotNetNuke.Common;
using DotNetNuke.Common.Utilities;
using DotNetNuke.Services;
using DotNetNuke.Entities.Modules;
using DotNetNuke.Services.Exceptions;
using DotNetNuke.Services.Localization;
using DotNetNuke.Services.Tokens;
using GIBS.Modules.FlexMLS_Favorites.Components;
using GIBS.Modules.FlexMLS.Components;
using System.Text;

using System.Collections;

namespace GIBS.Modules.FlexMLS_Favorites
{
    public partial class Listings : PortalModuleBase
    {

        int _CurrentPage = 1;
        public string _FlexMLSTabModuleID = "-1";
        static string _MLSImagesURL = "";
        public string _FlexMLSModulePage = "";


        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            string myGMapKey = "";
            if (Settings["FlexMLSModuleID"] != null)
            {

                //Settings["FlexMLSModuleID"].ToString()
                // GET TAB MODULE SETTINGS
                var tmc = new ModuleController();
                var tmi = tmc.GetTabModule(Int32.Parse(Settings["FlexMLSModuleID"].ToString()));
                var tmSettings = tmi.TabModuleSettings;
                var smValue = tmSettings["GoogleMapAPIKey"];
                myGMapKey = smValue.ToString();
                var imageURL = tmSettings["MLSImagesUrl"];
                _MLSImagesURL = imageURL.ToString();
            }

        //    GMap1.Key = myGMapKey.ToString();

            //JavaScript.RequestRegistration(CommonJs.jQuery);
            //JavaScript.RequestRegistration(CommonJs.jQueryUI);
            //JavaScript.RequestRegistration(CommonJs.DnnPlugins);
        }


        protected void Page_Load(object sender, EventArgs e)
        {

            if (!IsPostBack)
            {
                if (UserId == -1)
                {
                    Response.Redirect(Globals.NavigateURL(this.PortalSettings.LoginTabId));
                }


            }

            LoadSettings();
            LoadFavListings();
            SetLinks();
        }


        public void SetLinks()
        {
            try
            {
                string vPage1 = Globals.NavigateURL(PortalSettings.ActiveTab.TabID, "FavSearch", "mid=" + ModuleId.ToString());

                HyperLinkViewSavedSearches.Visible = true;
                HyperLinkViewSavedSearches.NavigateUrl = vPage1.ToString();
            }
            catch (Exception ex)
            {
                Exceptions.ProcessModuleLoadException(this, ex);
            }
        }

        public void LoadSettings()
        {

            try
            {

                if (Settings.Contains("FlexMLSModuleID"))
                {
                    _FlexMLSTabModuleID = Settings["FlexMLSModule"].ToString();
                }

                //if (Settings.Contains("MLSImagesURL"))
                //{
                //    _MLSImagesURL = Settings["MLSImagesURL"].ToString();
                //}

                if (Settings.Contains("FlexMLSModulePage"))
                {
                    _FlexMLSModulePage = Settings["FlexMLSModulePage"].ToString();
                }


                
            }
            catch (Exception ex)
            {
                Exceptions.ProcessModuleLoadException(this, ex);
            }

        }

        public void LoadFavListings()
        {
            try
            {
                List<FlexMLS_FavoritesInfo> items;
                FlexMLS_FavoritesController controller = new FlexMLS_FavoritesController();

              //  FlexMLS_FavoritesSettings settingsData = new FlexMLS_FavoritesSettings(this.TabModuleId);
                if (_FlexMLSModulePage != "-1")
                {
                    // NO LONGER USING MODULEID - Convert.ToInt32(settingsData.FlexMLSModule)
                    items = controller.FlexMLS_Favorites_Get_List(0, this.UserId, "Listing");

                    if (items.Count > 0)
                    {
                        string MlNumbers = "";
                        for (int i = 0; i < items.Count; i++)
                        {
                            MlNumbers += (string)items[i].Favorite.ToString() + ",";
                            //  list.Add(new ListItem((string)items[i].Village.ToString(), (string)items[i].Village.ToString()));
                        }

                   //     lblDebug.Text = MlNumbers.ToString();
                        SearchMLS(MlNumbers.ToString());
                    }
                    else
                    {
                        //GMap1.Visible = false;
                        lblErrorMessage.Text = Localization.GetString("DefaultContent", LocalResourceFile);
                        lstSearchResults.Visible = false;
                    }


         //           hyperlinkFavListings.Visible = true;
                    string vPage = Globals.NavigateURL(PortalSettings.ActiveTab.TabID, "FavListing", "mid=" + ModuleId.ToString());
          //          hyperlinkFavListings.NavigateUrl = vPage.ToString();
                    
                    
                    int intModTabID = -1;
                    ModuleInfo objModuleInfo = new ModuleInfo();
                    ModuleController objModuleContr = new ModuleController();
                    objModuleInfo = objModuleContr.GetModuleByDefinition(PortalId, "GIBS - FlexMLS");
                    intModTabID = objModuleInfo.TabID;

                     string strRedir = Globals.NavigateURL(intModTabID);

                 //   string vLink = Globals.NavigateURL(Int32.Parse(settingsData.FlexMLSModule.ToString()),false);
                    HyperLinkNewSearch.NavigateUrl = strRedir.ToString();

                }
            }
            catch (Exception ex)
            {
                Exceptions.ProcessModuleLoadException(this, ex);
            }
        }


        protected void dlListings_ItemDataBound(object sender, System.Web.UI.WebControls.DataListItemEventArgs e)
        {
            HyperLink FavoriteLink = (HyperLink)e.Item.FindControl("hlFavorite");

            string _mlsNumber = DataBinder.Eval(e.Item.DataItem, "Favorite").ToString();

            FavoriteLink.Text = _mlsNumber.ToString();

            string vLink = Globals.NavigateURL("View", "pg", "v", "MLS", _mlsNumber.ToString());
            vLink = vLink.ToString().Replace("ctl/View/", "");
            vLink = vLink.ToString().Replace("Default.aspx", _mlsNumber.ToString()+ ".aspx");
            FavoriteLink.NavigateUrl = vLink.ToString();


        }



        public void SearchMLS(string MlNumbers)
        {

            try
            {

                int PageSize = 20;
                //Display 20 items per page
                //Get the currentpage index from the url parameter
                if (Request.QueryString["currentpage"] != null)
                {
                    _CurrentPage = Convert.ToInt32(Request.QueryString["currentpage"].ToString());
                }
                else
                {
                    _CurrentPage = 1;
                }

                List<FlexMLSInfo> items;
                FlexMLSController controller = new FlexMLSController();

                items = controller.FlexMLS_Search_MLS_Numbers(MlNumbers.ToString());

            //    lblDebug.Text = MlNumbers.ToString();

                PagedDataSource objPagedDataSource = new PagedDataSource();
                objPagedDataSource.DataSource = items;

                if (items.Count < 1)
                {
                //    GMap1.Visible = false;
                    lblErrorMessage.Text = Localization.GetString("DefaultContent", LocalResourceFile);
                }

                if (objPagedDataSource.PageCount > 0)
                {
                    objPagedDataSource.PageSize = PageSize;
                    objPagedDataSource.CurrentPageIndex = _CurrentPage - 1;
                    objPagedDataSource.AllowPaging = true;
                }


                lstSearchResults.DataSource = objPagedDataSource;
                lstSearchResults.DataBind();

                lblSearchSummary.Text = "Total Listings Found: " + items.Count.ToString();

                if (PageSize == 0 || items.Count <= PageSize)
                {
                    PagingControl1.Visible = false;
                }
                else
                {
                    PagingControl1.Visible = true;
                    PagingControl1.TotalRecords = items.Count;
                    PagingControl1.PageSize = PageSize;
                    PagingControl1.CurrentPage = _CurrentPage;
                    PagingControl1.TabID = TabId;
                    PagingControl1.QuerystringParams = "pg=List&" + GenerateQueryStringParameters(this.Request, "Town", "Village", "Beds", "Baths", "WaterFront", "WaterView", "Type", "Low", "High", "LOID");

                }

            }
            catch (Exception ex)
            {
                Exceptions.ProcessModuleLoadException(this, ex);
            }

        }


        //public void BuildGoogleMap(double Latitude, double Longitude, string BubbleText)
        //{

        //    try
        //    {

        //        GMap1.setCenter(new GLatLng(Latitude, Longitude), 14);
        //        //GMap1.mapType = GMapType.GTypes.Hybrid;
        //        //GMap1.Add(GMapType.GTypes.Normal);      //.addMapType(GMapType.GTypes.Physical);
        //        //GMap1.Add(GMapType.GTypes.Physical);

        //        GControl control = new GControl(GControl.preBuilt.SmallMapControl);
        //        GControl control2 = new GControl(GControl.preBuilt.MenuMapTypeControl, new GControlPosition(GControlPosition.position.Top_Left));

        //        GMap1.Add(control);
        //        GMap1.Add(control2);

        //        GLatLng latlng = new GLatLng(Latitude, Longitude);

        //        string vBubbleText = BubbleText.ToString();    // lblListingAddress.Text.ToString() + "<br />" + lblSummary.Text.ToString();
        //        //XPinLetter xPinLetter = new XPinLetter(PinShapes.pin_star, "A", Color.Red, Color.White, Color.Black);
        //        XPinLetter xPinLetter = new XPinLetter(PinShapes.pin_star, "+", System.Drawing.Color.Red, System.Drawing.Color.White, System.Drawing.Color.Gold);


        //        GMarker marker = new GMarker(latlng, new GMarkerOptions(new GIcon(xPinLetter.ToString(), xPinLetter.Shadow())));
        //      //  GInfoWindowOptions windowOptions = new GInfoWindowOptions();
        //        GInfoWindow commonInfoWindow = new GInfoWindow(marker, vBubbleText.ToString(), false);
        //        GMap1.Add(commonInfoWindow);


        //    }
        //    catch (Exception ex)
        //    {
        //        Exceptions.ProcessModuleLoadException(this, ex);
        //    }

        //}

        public void BuildGoogleMap(double Latitude, double Longitude, string BubbleText)
        {

            try
            {

            // //   GMap1.setCenter(new GLatLng(Latitude, Longitude), 14);


            ////    GLatLng latlng = new GLatLng(Latitude, Longitude);

            //    string vBubbleText = BubbleText.ToString();    // lblListingAddress.Text.ToString() + "<br />" + lblSummary.Text.ToString();

            //    //  vBubbleText = "<p>JOE</p>";
            //    // https://chart.apis.google.com/chart?chst=d_map_xpin_letter&chld=pin_star|+|FF0000|FFFFFF|FFD700
            //    // GMarker marker = new GMarker(latlng, new GMarkerOptions(new GIcon(xPinLetter.ToString(), xPinLetter.Shadow())));

            //    GMarker marker = new GMarker(latlng, new GMarkerOptions(new GIcon("https://chart.apis.google.com/chart?chst=d_map_xpin_letter&chld=pin_star|+|FF0000|FFFFFF|FFD700")));
            //    GInfoWindowOptions windowOptions = new GInfoWindowOptions();
            //    GInfoWindow commonInfoWindow = new GInfoWindow(marker, vBubbleText.ToString(), false);
            //    GMap1.Add(commonInfoWindow);



            }
            catch (Exception ex)
            {
                Exceptions.ProcessModuleLoadException(this, ex);
            }

        }



        //protected virtual string ReplaceTokens(string strSourceText)
        //{
        //    if (strSourceText == null)
        //    { return string.Empty; }
        //    var Result = new StringBuilder(); 
        //    foreach (Match currentMatch in TokenizerRegex.Matches(strSourceText))
        //    { 
        //        string strObjectName = currentMatch.Result("${object}"); 
        //        if (!String.IsNullOrEmpty(strObjectName)) { if (strObjectName == "[") { strObjectName = ObjectLessToken; } string strPropertyName = currentMatch.Result("${property}"); 
        //            string strFormat = currentMatch.Result("${format}"); 
        //            string strIfEmptyReplacment = currentMatch.Result("${ifEmpty}"); 
        //            string strConversion = replacedTokenValue(strObjectName, strPropertyName, strFormat); 
        //            if (!String.IsNullOrEmpty(strIfEmptyReplacment) && String.IsNullOrEmpty(strConversion)) 
        //            { strConversion = strIfEmptyReplacment; } Result.Append(strConversion); } 
        //        else 
        //        { Result.Append(currentMatch.Result("${text}")); } 
        //    } 
        //    return Result.ToString();
        //}


        public string FixTokens(string _myOriginal, string _myToken, string _myReplacement)
        {

            try
            {
                string _ReturnValue = "";

                _ReturnValue = _myOriginal.ToString().Replace(_myToken, _myReplacement.ToString()).ToString();

                return _ReturnValue;
            }
            catch (Exception ex)
            {
                Exceptions.ProcessModuleLoadException(this, ex);
                return ex.ToString();
            }
        }

        protected void lstSearchResults_ItemDataBound(object sender, System.Web.UI.WebControls.DataListItemEventArgs e)
        {

            try
            {

            //    GMap1.mapType = GMapType.GTypes.Hybrid;
            //    GMap1.Add(GMapType.GTypes.Normal);      //.addMapType(GMapType.GTypes.Physical);
            //    GMap1.Add(GMapType.GTypes.Physical);
                string _ListingNumber = "";
                string _PropertyType = "";


                //  _myOriginalLetter = FixTokens(_myOriginalLetter, "[DriveName]", item.DriveName.ToString());

                if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
                {
                    _ListingNumber = DataBinder.Eval(e.Item.DataItem, "ListingNumber").ToString();
                    if (_ListingNumber.ToString().Length < 1)
                    {
                        _ListingNumber = DataBinder.Eval(e.Item.DataItem, "ItemID").ToString();
                    }
                    _PropertyType = DataBinder.Eval(e.Item.DataItem, "PropertyType").ToString();

                    // Retrieve the Hyperlink control in the current DataListItem.
                    HyperLink eLink = (HyperLink)e.Item.FindControl("hyperlinkListingDetail");
                    string _pageName = DataBinder.Eval(e.Item.DataItem, "Address").ToString().Replace(" ", "_").ToString().Replace("&", "").ToString() + "_" + DataBinder.Eval(e.Item.DataItem, "Village").ToString().Replace(" ", "_").ToString().Replace("&", "").ToString() + ".aspx";

                    string vLink = Globals.NavigateURL(Int32.Parse(_FlexMLSModulePage.ToString()));
                 //   var _MLSModulePage = Globals.NavigateURL(Int32.Parse(_FlexMLSModulePage.ToString()));
                    //lblDebug.Visible = true;
                    //lblDebug.Text += "FlexMLSPage = " + _FlexMLSPage.ToString() + "<br />";
              //      var result = vLink.Substring(vLink.LastIndexOf('/') + 1);
                    // DISABLE ADDING OF NEW RECORD IF COMING FROM THIS MODULE BY QUERYSTRING ADDITION OF . . . /t/f
                    vLink = vLink.ToString() +  "/pg/v/t/f/MLS/" + _ListingNumber.ToString() + "/" + _pageName.ToString();

                    eLink.NavigateUrl = vLink.ToString();

                    Label MLS = (Label)e.Item.FindControl("lblListingNumber");
                    //           MLS.Text = "MLS " + _ListingNumber.ToString();

                    // lblLotSquareFootage
                    Label LotSquareFootage = (Label)e.Item.FindControl("lblLotSquareFootage");
                    double sqft = Convert.ToDouble(DataBinder.Eval(e.Item.DataItem, "Acres"));
                    if (sqft > 0)
                    {
                        LotSquareFootage.Text = Math.Round(sqft, 2).ToString() + " Acres";
                    }

                    if (_PropertyType.ToString().ToUpper() == "COND" || _PropertyType.ToString().ToUpper() == "COMM")
                    {
                        LotSquareFootage.Text = DataBinder.Eval(e.Item.DataItem, "Complex").ToString();    //CONDO COMPLEX NAME
                    }

                    Label ListingStatus = (Label)e.Item.FindControl("lblListingStatus");
                    string _listingstatus = DataBinder.Eval(e.Item.DataItem, "StatusCode").ToString();
                    ListingStatus.Text = _listingstatus.ToString();

                    Label MLNumber = (Label)e.Item.FindControl("lblMLNumber");
                    MLNumber.Text = "MLS # " + _ListingNumber.ToString();


                    // CHECK TO SEE IF THE LISTING HAS BEEN REMOVED
                    if (DataBinder.Eval(e.Item.DataItem, "StatusCode").ToString() != "")
                    {





                        //lblBedsBaths
                        string _S_baths = "";
                        string _S_beds = "";
                        string _S_halfbaths = "";
                        if (Convert.ToInt32(DataBinder.Eval(e.Item.DataItem, "Bedrooms").ToString()) > 1)
                        {
                            _S_beds = "s";
                        }
                        if (Convert.ToInt32(DataBinder.Eval(e.Item.DataItem, "TotalBaths").ToString()) > 1)
                        {
                            _S_baths = "s";
                        }

                        Label BedsBaths = (Label)e.Item.FindControl("lblBedsBaths");




                        if (Convert.ToInt32(DataBinder.Eval(e.Item.DataItem, "Bedrooms").ToString()) > 0)
                        {
                            BedsBaths.Text = DataBinder.Eval(e.Item.DataItem, "Bedrooms").ToString() + " Bedroom" + _S_beds.ToString();
                        }
                        if (Convert.ToInt32(DataBinder.Eval(e.Item.DataItem, "FullBaths").ToString()) > 0)
                        {
                            BedsBaths.Text = BedsBaths.Text.ToString() + " - " + DataBinder.Eval(e.Item.DataItem, "FullBaths").ToString() + " Full Bath" + _S_baths.ToString();
                        }

                        if (Convert.ToInt32(DataBinder.Eval(e.Item.DataItem, "HalfBaths").ToString()) > 0)
                        {
                            BedsBaths.Text = BedsBaths.Text.ToString() + ", " + DataBinder.Eval(e.Item.DataItem, "HalfBaths").ToString() + " Half Bath" + _S_halfbaths.ToString();
                        }
                        if (_PropertyType.ToString() == "COMM" || _PropertyType.ToString() == "MULT")
                        {
                            BedsBaths.Text = DataBinder.Eval(e.Item.DataItem, "Style").ToString();
                        }


                        // lblLivingSpace  
                        double tempDouble = 00.000;
                        Label SquareFootage = (Label)e.Item.FindControl("lblLivingSpace");
                        if (double.TryParse(DataBinder.Eval(e.Item.DataItem, "LivingSpace").ToString(), out tempDouble))
                        {

                            double livingspace = double.Parse(DataBinder.Eval(e.Item.DataItem, "LivingSpace").ToString());
                            SquareFootage.Text = livingspace.ToString("##,###") + " Sqft.";
                        }


                        // lblAddress
                        string _UnitNumber = "";
                        if (DataBinder.Eval(e.Item.DataItem, "UnitNumber").ToString().Length >= 1 && DataBinder.Eval(e.Item.DataItem, "UnitNumber").ToString() != "0")
                        {
                            _UnitNumber = " #" + DataBinder.Eval(e.Item.DataItem, "UnitNumber").ToString();
                        }
                        HyperLink Address = (HyperLink)e.Item.FindControl("hyperlinkListingAddress");

                        Address.Text = DataBinder.Eval(e.Item.DataItem, "Address").ToString() + _UnitNumber.ToString() + ", " + DataBinder.Eval(e.Item.DataItem, "Village").ToString();
                        string BubbleAddress = Address.Text.ToString();
                        Address.NavigateUrl = vLink.ToString();

                        string vListingPrice = "";
                        Label ListingPrice = (Label)e.Item.FindControl("lblListingPrice");
                        if (double.TryParse(DataBinder.Eval(e.Item.DataItem, "ListingPrice").ToString(), out tempDouble))
                        {
                            // lblListingPrice

                            vListingPrice = DataBinder.Eval(e.Item.DataItem, "ListingPrice").ToString();
                            ListingPrice.Text = String.Format("{0:C0}", double.Parse(vListingPrice.ToString()));
                        }

                        //lblPropertyType
                        Label PropertyType = (Label)e.Item.FindControl("lblPropertyType");
                        PropertyType.Text = DataBinder.Eval(e.Item.DataItem, "PropertySubType1").ToString();
                        //lblYearBuilt
                        Label YearBuilt = (Label)e.Item.FindControl("lblYearBuilt");
                        YearBuilt.Text = "Built In " + DataBinder.Eval(e.Item.DataItem, "YearBuilt").ToString();

                        // CHECK FOR LAND LISTING
                        if (_PropertyType.ToString() == "LOTL" || _PropertyType.ToString() == "MULT")
                        {
                            YearBuilt.Text = "TBD";        // DataBinder.Eval(e.Item.DataItem, "PropertySubType1").ToString();
                        }


                        // IMAGE
                        // IMAGE
                        Image ListingImage = (Image)e.Item.FindControl("imgListingImage");

                        string checkImage = _MLSImagesURL.ToString() + _ListingNumber.ToString() + ".jpg";

                        if (UrlExists(checkImage.ToString()) == true)
                        {
                            // ListingImage.ImageUrl = checkImage.ToString();
                            ListingImage.ImageUrl = _MLSImagesURL.ToString() + _ListingNumber.ToString() + ".jpg";

                        }
                        else if (UrlExists(_MLSImagesURL.ToString() + _ListingNumber.ToString() + "_1.jpg") == true)
                        {
                            //
                            ListingImage.ImageUrl = _MLSImagesURL.ToString() + _ListingNumber.ToString() + "_1.jpg";

                        }
                        else
                        {

                            ListingImage.ImageUrl = _MLSImagesURL.ToString() + "NoImage.jpg";

                            ImageNeeded(_ListingNumber.ToString());
                        }

                        ListingImage.ToolTip = "MLS Listing " + _ListingNumber.ToString();
                        ListingImage.AlternateText = "MLS Listing " + _ListingNumber.ToString();



                        // CHECK IF AUTHENTICATED
                        if (HttpContext.Current.User.Identity.IsAuthenticated)
                        {
                            // lblMarketingRemarks
                            Label MarketingRemarks = (Label)e.Item.FindControl("lblMarketingRemarks");
                            MarketingRemarks.Text = DataBinder.Eval(e.Item.DataItem, "PublicRemarks").ToString();
                        }
                        // vLink = vLink.ToString().Replace(result.ToString(), "tabid/" + _FlexMLSPage.ToString() + "/pg/v/t/f/MLS/" + _ListingNumber.ToString() + "/" + _pageName.ToString());

                        //HyperLinkInquiry - CONTACT FORM
                        HyperLink InquiryHyperLink = (HyperLink)e.Item.FindControl("HyperLinkInquiry");
                        // string InquiryLink = Globals.NavigateURL("View", "pg", "Contact", "MLS", _ListingNumber.ToString());
                        string InquiryLink = vLink.ToString().Replace("v/t/f", "Contact").ToString();
                        InquiryHyperLink.NavigateUrl = InquiryLink.ToString();

                        //HyperLinkShowing - SCHEDULE A SHOWING
                        HyperLink ShowingHyperLink = (HyperLink)e.Item.FindControl("HyperLinkShowing");
                        string ShowingLink = vLink.ToString().Replace("v/t/f", "Contact/Schedule/Showing").ToString();
                        //Globals.NavigateURL("View", "pg", "Contact", "MLS", _ListingNumber.ToString(), "Schedule", "Showing");
                        //ShowingLink = ShowingLink.ToString().Replace("ctl/View/", "");
                        ShowingHyperLink.NavigateUrl = ShowingLink.ToString();


                        // ADD TO MAP
                        double _lat = Convert.ToDouble(DataBinder.Eval(e.Item.DataItem, "Latitude").ToString());
                        double _log = Convert.ToDouble(DataBinder.Eval(e.Item.DataItem, "Longitude").ToString());
                        //string _bubbleText = "<img src='" + checkImage.ToString() + "' id='" + _ListingNumber.ToString() + "' width='90' height='60' align='right' alt='" + BubbleAddress.ToString() + "'>"
                        //    + Address.Text.ToString() + "<br />" + ListingPrice.Text.ToString()
                        //    + "<br /><a href='" + vLink.ToString() + "'>MLS " + _ListingNumber.ToString() + "</a>";
                        string _bubbleText = "<div style='width:270px;height:120px;'><img src='" + "/DesktopModules/GIBS/FlexMLS/ImageHandler.ashx?MlsNumber="
                            + _ListingNumber.ToString() + "&MaxSize=120' id='" + _ListingNumber.ToString()
                            + "' align='right' alt='" + BubbleAddress.ToString() + "' style='border: 1px solid #000000;'>"
                            + Address.Text.ToString() + "<br />" + ListingPrice.Text.ToString()
                            + "<br /><a href='" + vLink.ToString() + "'>MLS " + _ListingNumber.ToString() + "<br />View Listing</a></div>";

                        if (_lat > 0)
                        {
                            BuildGoogleMap(_lat, _log, _bubbleText.ToString());
                        }

                    }
                    

                    // DEAL WITH REMOVED LISTINGS!
                    if (DataBinder.Eval(e.Item.DataItem, "StatusCode").ToString() == "")
                    {

                        HyperLink Address = (HyperLink)e.Item.FindControl("hyperlinkListingAddress");
                        Address.Text = "MLS# " + DataBinder.Eval(e.Item.DataItem, "ItemID").ToString() + " had been removed. Call agent for details!";

                        Image ListingImage = (Image)e.Item.FindControl("imgListingImage");

                        string checkImage = _MLSImagesURL.ToString() + DataBinder.Eval(e.Item.DataItem, "ItemID").ToString() + ".jpg";

                        if (UrlExists(checkImage.ToString()) == true)
                        {
                            // ListingImage.ImageUrl = checkImage.ToString();
                            ListingImage.ImageUrl = _MLSImagesURL.ToString() + DataBinder.Eval(e.Item.DataItem, "ItemID").ToString() + ".jpg";

                        }
                        else if (UrlExists(_MLSImagesURL.ToString() + DataBinder.Eval(e.Item.DataItem, "ItemID").ToString() + "_1.jpg") == true)
                        {
                            //
                            ListingImage.ImageUrl = _MLSImagesURL.ToString() + DataBinder.Eval(e.Item.DataItem, "ItemID").ToString() + "_1.jpg";

                        }
                        else
                        {

                            ListingImage.ImageUrl = _MLSImagesURL.ToString() + "NoImage.jpg";

                        }

                        ListingImage.ToolTip = "MLS Listing " + _ListingNumber.ToString();
                        ListingImage.AlternateText = "MLS Listing " + _ListingNumber.ToString();

                    }


                }

            }
            catch (Exception ex)
            {
                Exceptions.ProcessModuleLoadException(this, ex);
            }

        }


        public void ImageNeeded(string listingNumber)
        {
            try
            {

                FlexMLSController controller = new FlexMLSController();
                FlexMLSInfo item = new FlexMLSInfo();

                item.ListingNumber = listingNumber.ToString();

                controller.FlexMLS_ImagesNeeded_Insert(item);

            }
            catch (Exception ex)
            {
                Exceptions.ProcessModuleLoadException(this, ex);
            }

        }

        public string GetStatusDesc(string Status)
        {

            try
            {
                string myRetValue = "";
                switch (Status)
                {
                    case "A":
                        myRetValue = "Active";
                        break;
                    case "C":
                        myRetValue = "Pending with Contingencies";
                        break;
                    case "R":
                        myRetValue = "Listing Removed. Call agent for details!";
                        break;

                    default:
                        myRetValue = "";
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


        private static bool UrlExists(string url)
        {
            try
            {
                new System.Net.WebClient().DownloadData(url);
                return true;
            }
            catch (System.Net.WebException e)
            {
                if (((System.Net.HttpWebResponse)e.Response).StatusCode == System.Net.HttpStatusCode.NotFound)
                    return false;
                else
                    throw;
            }
        }

        protected static string GenerateQueryStringParameters(HttpRequest request, params string[] queryStringKeys)
        {
            StringBuilder queryString = new StringBuilder(64);
            foreach (string key in queryStringKeys)
            {
                if (request.QueryString[key] != null)
                {
                    if (queryString.Length > 0)
                    {
                        queryString.Append("&");
                    }

                    queryString.Append(key).Append("=").Append(request.QueryString[key]);
                }
            }

            return queryString.ToString();
        }

        private string GetAdditionalQueryStringParams()
        {
            throw new NotImplementedException();
        }


        protected void linkButtonFavoritesRemoveListing_Click(object sender, EventArgs e)
        {
            try
            {
                int MLSnumber = 0;
                LinkButton myButton = sender as LinkButton;

                if (myButton != null)
                {
                    MLSnumber = Convert.ToInt32(myButton.CommandArgument);
                }

                FlexMLSController controller = new FlexMLSController();
                FlexMLSInfo item = new FlexMLSInfo();

                item.Favorite = MLSnumber.ToString();
                item.FavoriteType = "Listing";
                item.ModuleId = this.ModuleId;
                item.UserID = this.UserId;

                controller.FlexMLS_Favorites_Add(item);

                myButton.Text = "SAVED! - " + item.Favorite.ToString();


            }
            catch (Exception ex)
            {
                Exceptions.ProcessModuleLoadException(this, ex);
            }
        }


        protected void linkButtonEmailAFriend_Click(object sender, EventArgs e)
        {
            try
            {
                int MLSnumber = 0;
                LinkButton myButton = sender as LinkButton;

                if (myButton != null)
                {
                    MLSnumber = Convert.ToInt32(myButton.CommandArgument);
                }



            }
            catch (Exception ex)
            {
                Exceptions.ProcessModuleLoadException(this, ex);
            }
        }

        protected void linkButtonRemoveListing_Click(object sender, EventArgs e)
        {
            try
            {
                int MLSnumber = 0;
                LinkButton myButton = sender as LinkButton;

                if (myButton != null)
                {
                    MLSnumber = Convert.ToInt32(myButton.CommandArgument);
                    //myButton.CommandName.ToString()
                }

                FlexMLS_FavoritesController controller = new FlexMLS_FavoritesController();

              
               
                // ModuleID is no longer used by this procedure
                controller.FlexMLS_Favorites_Delete_By_MlsNumber(0, this.UserId, MLSnumber.ToString());

                LoadFavListings();

            }
            catch (Exception ex)
            {
                Exceptions.ProcessModuleLoadException(this, ex);
            }
        }	

    }
}