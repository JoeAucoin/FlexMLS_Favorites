<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="Listings.ascx.cs" Inherits="GIBS.Modules.FlexMLS_Favorites.Listings" %>
<%@ Register TagPrefix="dnn" Namespace="DotNetNuke.UI.WebControls" Assembly="DotNetNuke" %>
<%@ Register TagPrefix="dnn" Namespace="DotNetNuke.Web.Client.ClientResourceManagement" Assembly="DotNetNuke.Web.Client" %>

<dnn:DnnCssInclude ID="DnnCssInclude1" runat="server" FilePath="~/DesktopModules/GIBS/FlexMLS/css/Style.css?1=2" />




<!-- fotorama.css & fotorama.js. -->
<link  href="https://cdnjs.cloudflare.com/ajax/libs/fotorama/4.6.4/fotorama.css" rel="stylesheet" /> <!-- 3 KB -->
<script src="https://cdnjs.cloudflare.com/ajax/libs/fotorama/4.6.4/fotorama.js" type="text/javascript"></script> <!-- 16 KB -->

<!-- 2. Add images to <div class="fotorama"></div>. -->



<asp:Label ID="lblDebug" runat="server" Visible="true"></asp:Label>



 <div style="position:relative;float:right;padding-right:30px;">
      
     <asp:HyperLink ID="HyperLinkViewSavedSearches" runat="server">View Saved Searches</asp:HyperLink> | <asp:HyperLink ID="HyperLinkNewSearch" runat="server">New Search</asp:HyperLink>   
          
         
         </div>






<div class="searchcriteria"><asp:Label ID="lblSearchSummary" runat="server" Text="0 Records Found"></asp:Label></div>


   


<script type="text/javascript" >




</script>

<div><asp:Label ID="lblErrorMessage" runat="server" Text="" /></div>





    <asp:DataList ID="lstSearchResults" DataKeyField="ItemID" runat="server" Width="100%" OnItemDataBound="lstSearchResults_ItemDataBound">
        <ItemTemplate>
<div class="row">
         <div class="col-sm-8"><div class="row">
                <div class="col-sm-8"><asp:HyperLink ID="hyperlinkListingAddress" Text="Listing Address" CssClass="ListingAddress" runat="server" /></div>
             
                <div class="col-sm-4"><asp:Label ID="lblListingPrice" runat="server" CssClass="ListingPrice" /></div>
            </div><div class="row">
                <div class="col-sm-8"><asp:Label ID="lblBedsBaths" runat="server" Text=""></asp:Label></div>
                
                <div class="col-sm-4"><asp:Label ID="lblMLNumber" runat="server" CssClass="ListingDetails" /></div>
            </div>
             <div class="row">
                <div class="col-sm-4"><asp:Label ID="lblPropertyType" runat="server" CssClass="ListingDetails" /></div>
				<div class="col-sm-4"><asp:Label ID="lblYearBuilt" runat="server" CssClass="ListingDetails" /></div>
				<div class="col-sm-4"><asp:Label ID="lblListingStatus" runat="server" Text="" CssClass="ListingDetails" /></div>
            </div>	
             <div class="row">
                <div class="col-sm-4"><asp:Label ID="lblLotSquareFootage" runat="server" CssClass="ListingDetails"></asp:Label></div>
                 
                 <div class="col-sm-4"><asp:Label ID="lblLivingSpace" runat="server" CssClass="ListingDetails" /></div>
                 <div class="col-sm-4"></div>
             </div>
             
             
             <div class="row">

                 <div class="col-sm-12">
                     <p style="text-align: center;">
                        
                     </p>
                     <div class="MarketingRemarks">
                         <asp:Label ID="lblMarketingRemarks" runat="server"></asp:Label></div>

                     <p class="ListingLinks">
                         <asp:HyperLink ID="hyperlinkListingDetail" Text="Listing Details" runat="server" CssClass="btn btn-sm btn-default" />
                          <asp:HyperLink ID="HyperLinkShowing" runat="server" CssClass="btn btn-sm btn-default" Text="Schedule Showing" />
                          <asp:HyperLink ID="HyperLinkInquiry" runat="server" CssClass="btn btn-sm btn-default" Text="Inquiry" />

                          <asp:LinkButton ID="linkButtonEmailAFriend" runat="server" CommandArgument='<%# Eval("ItemID") %>' Visible="false" 
                    OnClick="linkButtonEmailAFriend_Click" CssClass="btn btn-sm btn-default" Text="E-Mail a Friend" />
                          <asp:LinkButton ID="linkButtonRemoveListing" runat="server" CommandArgument='<%# Eval("ItemID") %>'
                    OnClick="linkButtonRemoveListing_Click" CssClass="btn btn-sm btn-default" Text="Remove Listing" />
                     </p>





                 </div>
             </div>
             
             </div>
        <div class="col-sm-4"><div class="piccontainer">
            <div class="fotorama" data-fit="cover" data-width="100%" data-ratio="800/600">
                            <asp:Image ID="imgListingImage" runat="server" AlternateText='MLS <%# Eval("ItemID") %>' />
                        </div></div>

        </div>


</div>
<div class="row">
    <div class="col-sm-12"><hr class="hrfancy" /></div>
    </div>

        </ItemTemplate>
    </asp:DataList>



<dnn:PagingControl id="PagingControl1" runat="server" Visible="False" BackColor="#FFFFFF" BorderColor="#000000" ></dnn:PagingControl>



<p class="disclaimer"><asp:Image ID="Image1" runat="server" ImageUrl="~/DesktopModules/GIBS/FlexMLS/Images/BrokerReciprocity.gif" AlternateText="Broker Reciprocity (BR) of the Cape Cod & Islands MLS" ImageAlign="Left" Width="107px" Height="25px" />

The data relating to real estate for sale on this site comes from the Broker Reciprocity (BR) of the Cape Cod &amp; Islands 
Multiple Listing Service, Inc. Summary or thumbnail real estate listings held by brokerage firms other than <b><%#   this.PortalSettings.PortalName.ToString() %></b> 
are marked with the BR Logo and detailed information about them includes the name of the listing broker.  Neither the 
listing broker nor <b><%#   this.PortalSettings.PortalName.ToString() %></b> shall be responsible for any typographical errors, misinformation, or misprints and 
shall be held totally harmless.
</p>   