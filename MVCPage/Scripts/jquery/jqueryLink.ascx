<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="jqueryLink.ascx.cs" Inherits="WebSystem.Scripts.jquery.jqueryLink" %>
<asp:PlaceHolder ID="PlaceHolder1" runat="server" EnableViewState="false">    
    <%=string.Format(@"<script src='{0}/Scripts/jquery/jquery-3.2.1.js'></script>", GlobalObject.LinkRootUrl)%>
    <%--    <%=string.Format(@"<script src='{0}/Scripts/AdminEx/js/jquery-ui-1.9.2.custom.min.js'></script>", GlobalObject.LinkRootUrl)%>--%>
    <%--    <%=string.Format(@"<script src='{0}/Scripts/AdminEx/js/jquery-migrate-1.2.1.min.js'></script>", GlobalObject.LinkRootUrl)%>--%>
</asp:PlaceHolder>
