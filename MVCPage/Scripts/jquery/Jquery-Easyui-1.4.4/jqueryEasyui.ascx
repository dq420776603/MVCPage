<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="jqueryEasyui.ascx.cs" Inherits="WebSystem.Scripts.jquery.Jquery_Easyui.jqueryEasyui" %>
<asp:PlaceHolder ID="PlaceHolder1" runat="server" EnableViewState="false">    
    <%=string.Format(@"<link rel='stylesheet' type='text/css' href='{0}/Scripts/jquery/Jquery-Easyui-1.4.4/themes/default/easyui.css'>", GlobalObject.LinkRootUrl)%>
    <%=string.Format(@"<link rel='stylesheet' type='text/css' href='{0}/Scripts/jquery/Jquery-Easyui-1.4.4/themes/icon.css'>", GlobalObject.LinkRootUrl)%>
    <%=string.Format(@"<script type='text/javascript' src='{0}/Scripts/jquery/Jquery-Easyui-1.4.4/jquery.easyui.js'></script>", GlobalObject.LinkRootUrl)%>
    <%=string.Format(@"<script type='text/javascript' src='{0}/Scripts/jquery/Jquery-Easyui-1.4.4/locale/easyui-lang-zh_CN.js'></script>", GlobalObject.LinkRootUrl)%>
</asp:PlaceHolder>