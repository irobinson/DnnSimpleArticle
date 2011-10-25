<%@ Control Language="C#" AutoEventWireup="false" Inherits="DotNetNuke.Modules.dnnsimplearticle.Settings"
    CodeBehind="Settings.ascx.cs" %>
<%@ Register TagPrefix="dnn" TagName="label" Src="~/controls/labelControl.ascx" %>
<dl>
    <dt>
        <dnn:label ID="lblPageSize" runat="server" ControlName="txtPageSize" Text="Page Size">
        </dnn:label>
    </dt>
    <dd>
        <asp:TextBox ID="txtPageSize" runat="server" />
    </dd>
    <dt>
        <dnn:label ID="lblShowCategories" runat="server" ControlName="chkShowCategories"
            Text="Show Categories"></dnn:label>
    </dt>
    <dd>
        <asp:CheckBox ID="chkShowCategories" runat="server" /></dd>
        <!--
    <dt>
        <dnn:label ID="lblRichDescriptions" runat="server" ControlName="chkRichDescriptions"
            Text="Enable Rich Descriptions"></dnn:label>
    </dt>
    <dd>
        <asp:CheckBox ID="chkRichDescriptions" runat="server" /></dd>
        -->
</dl>
<br />
<asp:LinkButton ID="lnkDeleteAll" runat="server" resourcekey="lnkDeleteAll" OnClick="lnkDeleteAll_Click" />
