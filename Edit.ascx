<%@ Control Language="C#" Inherits="DotNetNuke.Modules.DnnSimpleArticle.Edit" AutoEventWireup="True" CodeBehind="Edit.ascx.cs" %>
<%@ Register TagPrefix="dnn" TagName="label" Src="~/controls/LabelControl.ascx" %>
<%@ Register TagPrefix="dnn" TagName="TextEditor" Src="~/controls/TextEditor.ascx" %>
<%@ Register TagPrefix="dnn" Assembly="DotNetNuke.Web" Namespace="DotNetNuke.Web.UI.WebControls" %>
<dl>
    <dt>
        <dnn:label ID="lblTitle" ControlName="txtTitle" runat="server" />
    <dd>
        <asp:TextBox ID="txtTitle" runat="server" Columns="50" /><asp:RequiredFieldValidator
            ID="rfvTitle" runat="server" ControlToValidate="txtTitle" CssClass="NormalRed" />
    </dd>
    <dt>
        <dnn:label ID="lblDescription" ControlName="txtDescription" runat="server" />
    <dd>
        <dnn:TextEditor ID="txtDescription" runat="server" Width="600px" Height="200px" />
        <asp:RequiredFieldValidator ID="rfvDescription" runat="server" ControlToValidate="txtDescription" CssClass="NormalRed" />
    </dd>
    <dt>
        <dnn:label ID="lblBody" ControlName="txtBody" runat="server" />
    <dd>
        <dnn:TextEditor ID="txtBody" runat="server" Width="600px" Height="400px" />
    </dd>
    <dt>
        <dnn:label ID="lblTerms" runat="server" ControlName="tsTerms" />
    <dd>
        <dnn:TermsSelector ID="tsTerms" runat="server" Height="250" Width="600" AllowCustomText="true" />
    </dd>
    <dt>
        <br />
    </dt>
    <dd>
        <asp:LinkButton ID="lbSave" runat="server" resourcekey="lbSave" OnClick="LbSaveClick" />
        <asp:LinkButton ID="lbCancel" runat="server" resourcekey="lbCancel" OnClick="LbCancelClick" CausesValidation="false" />
    </dd>
</dl>
