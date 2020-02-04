<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="Settings.ascx.cs" Inherits="GIBS.Modules.FlexMLS_Favorites.Settings" %>
<%@ Register TagPrefix="dnn" TagName="Label" Src="~/controls/LabelControl.ascx" %>
<div class="dnnForm" id="form-settings">

    <asp:Label ID="lblDebug" runat="server" Text="Label"></asp:Label>
    <fieldset>
	        <div class="dnnFormItem">
            <dnn:label id="lblFlexMLSModule" runat="server" suffix=":" controlname="ddlFlexMLSModule" />
            <asp:dropdownlist id="ddlFlexMLSModule" Runat="server" datavaluefield="ModuleID" datatextfield="TabName"></asp:dropdownlist>
        </div>
		
		<div class="dnnFormItem">
            <dnn:label id="lblFlexMLSModulePage" runat="server" suffix=":" controlname="ddlFlexMLSModulePage" />
            <asp:dropdownlist id="ddlFlexMLSModulePage" Runat="server" datavaluefield="TabID" datatextfield="TabName"></asp:dropdownlist>
        </div>

        <div class="dnnFormItem">
    <dnn:label id="lblMLSImagesUrl" runat="server" controlname="txtMLSImagesUrl" suffix=":" />
            <asp:textbox id="txtMLSImagesUrl" runat="server" Text="https://gibs.com/images/" />		
        </div>	

	</fieldset>
	
</div>	