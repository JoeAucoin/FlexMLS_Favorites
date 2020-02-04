<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="Searches.ascx.cs" Inherits="GIBS.Modules.FlexMLS_Favorites.Searches" %>
<%@ Register TagPrefix="dnn" TagName="Label" Src="~/controls/LabelControl.ascx" %>
<%@ Register TagPrefix="dnn" Namespace="DotNetNuke.Web.Client.ClientResourceManagement" Assembly="DotNetNuke.Web.Client" %>

<dnn:DnnCssInclude ID="DnnCssInclude2" runat="server" FilePath="~/DesktopModules/GIBS/FlexMLS/css/Style.css" />


<div><asp:Label ID="lblErrorMessage" runat="server" Text="" /></div>

<div style="position:relative;float:right;padding-right:30px;">
<asp:HyperLink ID="HyperLinkViewSavedListings" runat="server">View Saved Listings</asp:HyperLink> 
| <asp:HyperLink ID="HyperLinkNewSearch" runat="server">New Search</asp:HyperLink>   
</div>

<asp:GridView ID="GridView1" runat="server" AutoGenerateColumns="False" DataKeyNames="ItemID"
    OnRowDataBound="GridView1_RowDataBound" OnRowCommand="GridView1_RowCommand"  
    OnRowDeleting="GridView1_RowDeleting" OnRowEditing="GridView1_RowEditing" 
    GridLines="None" 
    CssClass="dnnGrid">
    <AlternatingRowStyle cssclass="dnnGridAltItem" />
    <FooterStyle cssclass="dnnGridFooter" />
    <HeaderStyle cssclass="dnnGridHeader" />
    <PagerStyle cssclass="dnnGridPager" />
    <RowStyle cssclass="dnnGridItem" />

<Columns>

        <asp:TemplateField HeaderText="Remove" ItemStyle-VerticalAlign="Top" ItemStyle-Width="80px" ItemStyle-HorizontalAlign="Center" >
         <ItemTemplate>
           <asp:LinkButton ID="LinkButtonDelete" OnClientClick="return confirm('Are you sure you want to delete this record?');"     
             CommandArgument='<%# Eval("ItemID") %>' 
             CommandName="Delete" runat="server"><asp:image ID="imgDelete" runat="server" imageurl="~/images/delete.gif" AlternateText="Delete Record" /></asp:LinkButton>
         </ItemTemplate>
       </asp:TemplateField>


       <asp:TemplateField HeaderText="* Daily E-Mails" ItemStyle-VerticalAlign="Top" ItemStyle-Width="130px" ItemStyle-HorizontalAlign="Center">
         <ItemTemplate>
             <asp:CheckBox ID="cbxEmailSearch" runat="server" AutoPostBack="True" 
                                                Checked='<%# Eval("EmailSearch") %>' 
                                                oncheckedchanged="cbxComplete_CheckedChanged" />
         </ItemTemplate>
       </asp:TemplateField>


       <asp:BoundField HeaderText="Saved Searches" DataField="Favorite" Visible="False" HtmlEncode="False" ItemStyle-VerticalAlign="Bottom" ></asp:BoundField>

<asp:TemplateField Headertext ="Saved Searches">
        <ItemTemplate>


       <asp:Hyperlink runat= "server" Text='<%# DataBinder.Eval(Container.DataItem,"Favorite")%>' 
                          NavigateUrl='<%# DataBinder.Eval(Container.DataItem,"Favorite") %>' ID="Hyperlink2"/>   
        </ItemTemplate>
        </asp:TemplateField> 
<asp:TemplateField Headertext ="View" ItemStyle-HorizontalAlign="Center">
	<ItemTemplate>
		<asp:Hyperlink runat= "server" NavigateUrl='<%# DataBinder.Eval(Container.DataItem,"Favorite") %>' ID="HyperlinkIcon">
            <asp:Image ID="ImageSearch" runat="server" ImageUrl="~/DesktopModules/GIBS/FlexMLS_Favorites/Images/viewIcon.png" /></asp:Hyperlink>
	</ItemTemplate>
</asp:TemplateField> 
        
    </Columns>

</asp:GridView>


 <div style="position:relative;padding-left:10px;"><asp:Label ID="lblDailyEmails" runat="server" Text=""></asp:Label></div>

 <div style="text-align:center; font-weight:bold;"><asp:Label ID="lblMessage" runat="server" Text=""></asp:Label></div>



