<%@ Page Language="VB" AutoEventWireup="false" CodeFile="Connect.aspx.vb" Inherits="Connect" %>
<%@ Register TagPrefix="uc1" TagName="Header" Src="~/Header.ascx" %>
<%@ Register TagPrefix="uc1" TagName="SysMenu" Src="~/SysMenu.ascx" %>
<%@ Register TagPrefix="uc1" TagName="Footer" Src="~/Footer.ascx" %>

<!DOCTYPE html>
<html xmlns="https://www.w3.org/1999/xhtml" >
 <head>
  <title>Connect - My World</title>
  <link href="/styles/Site.css" type="text/css" rel="stylesheet" />
  <link href="/Styles/TopMenu.css" type="text/css" rel="stylesheet" />
  <link href="/styles/TreeView.css" type="text/css" rel="stylesheet" />
  <script type="text/javascript" src="/scripts/Cookie.js"></script>
  <script type="text/javascript" src="/scripts/TreeView.js"></script>
  <script type="text/javascript" src="/scripts/TopMenu.js"></script>
  <script type="text/javascript">
   
   function ShowTOS() {
    window.open('/TOSPage.aspx','TOSPage','width=840,height=600,scrollbars=yes,resizable=yes');
   }
  
  </script>
 </head>
 <body id="BodyTag" runat="server" class="School">
  <!-- Built from Web Basic Page Template v. 1.0 -->
  <div id="HeaderPos">
   <uc1:Header id="Header1" runat="server"></uc1:Header>
   <uc1:SysMenu id="SysMenu1" runat="server"></uc1:SysMenu>
  </div>
  <div id="LSideBar">
   <table class="SidebarCtl">
    <tr>
     <td class="ProgTitle">
      My World Access
     </td>
    </tr>
    <tr>
     <td class="SidebarSpacer">&nbsp;</td>
    </tr>
    <!-- Sidebar Menu Control -->
    <tr>
     <td id="SidebarMenu" runat="server">
      <!-- Sidebar menu content is set in code -->
     </td>
    </tr>
   </table>
  </div>
  <div id="BodyArea">
   <table class="BodyTable">
    <tr>
     <td id="ShowContent" runat="server" class="BodyBg">
     </td>
    </tr>
    <tr>
     <td class="BodyBg" style="text-align: center;">
      <a href="/Default.aspx">Return to My World</a>
     </td>
    </tr>
   </table>
  </div>
  <div id="FooterPos">
   <uc1:Footer id="Footer" runat="server"></uc1:Footer>
  </div>
 </body>
</html>
