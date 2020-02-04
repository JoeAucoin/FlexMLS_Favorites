<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ViewFlexMLS_Favorites.ascx.cs" Inherits="GIBS.Modules.FlexMLS_Favorites.ViewFlexMLS_Favorites" %>



<asp:Label ID="lblDebug" runat="server" Text="" />

<p><asp:HyperLink ID="hyperlinkFavListings" runat="server" CssClass="dnnSecondaryAction" Width="160px">Saved Listings</asp:HyperLink></p>	

<p><asp:HyperLink ID="hyperlinkFavSearches" runat="server" CssClass="dnnSecondaryAction" Width="160px">Saved Searches</asp:HyperLink></p>
<asp:Label ID="LabelNotLoggedIn" runat="server" Text=""></asp:Label>