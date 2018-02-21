<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="jqueryValidation.ascx.cs" Inherits="WebSystem.Scripts.jquery.jquery_validation.jqueryValidation" %>

<asp:PlaceHolder ID="PlaceHolder1" runat="server" EnableViewState="false">
    <%=string.Format(@"<link href='{0}/Scripts/jquery/jquery-validation-1.11.1/custom/jqueryValidation.css' rel='stylesheet' />", GlobalObject.LinkRootUrl)%>
    <%=string.Format(@"<script src='{0}/Scripts/jquery/jquery-validation-1.11.1/dist/jquery.validate.min.js'></script>", GlobalObject.LinkRootUrl)%>
</asp:PlaceHolder>
