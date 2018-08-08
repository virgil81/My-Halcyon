<%@ Page Language="VB" AutoEventWireup="false" CodeFile="Logon.aspx.vb" Inherits="Logon" %>
<%@ Register TagPrefix="uc1" TagName="Header" Src="~/Header.ascx" %>
<%@ Register TagPrefix="uc1" TagName="SysMenu" Src="~/SysMenu.ascx" %>
<%@ Register TagPrefix="uc1" TagName="Footer" Src="~/Footer.ascx" %>

<!DOCTYPE html>
<html xmlns="https://www.w3.org/1999/xhtml" >
 <head>
  <title>Logon - My World</title>
  <link href="/styles/Site.css" type="text/css" rel="stylesheet" />
  <link href="/Styles/TopMenu.css" type="text/css" rel="stylesheet" />
  <link href="/styles/TreeView.css" type="text/css" rel="stylesheet" />
  <script type="text/javascript" src="/scripts/Cookie.js"></script>
  <script type="text/javascript" src="/scripts/TopMenu.js"></script>
  <script type="text/javascript" src="/scripts/TreeView.js"></script>
  <script type="text/javascript">

  </script>
 </head>
 <body id="BodyTag" runat="server">
  <!-- Built from WebSys Add/Edit template v. 1.0 -->
  <div id="HeaderPos">
   <uc1:Header id="Header1" runat="server"></uc1:Header>
   <uc1:SysMenu id="SysMenu1" runat="server"></uc1:SysMenu>
  </div>
  <div id="LSideBar">
   <table class="SidebarCtl">
    <tr>
     <td class="ProgTitle">My World Logon</td>
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
     <td class="BodyBg">
      <table style=" width:100%;">
       <tr>
        <td id="ShowContent" runat="server" style="width: 50%; vertical-align: top;">
        </td>
        <td style="width: 50%; vertical-align: top;">
         <form id="aspnetForm" method="post" runat="server">
         <table style="width: 95%; border: solid 1px #000000; margin: 10px 5px 10px 5px;">
          <tr>
           <td>
            <table style="width: 100%;">
             <tr>
              <td class="FldTitleCol">
               First Name
              </td>
              <td class="FldEntryCol">
               <input type="text" id="FirstName" runat="server" size="20" maxlength="20" class="Form"/>
              </td>
             </tr>
             <tr>
              <td class="FldTitleCol">
               Last Name
              </td>
              <td class="FldEntryCol">
               <input type="text" id="LastName" runat="server" size="20" maxlength="20" class="Form"/>
              </td>
             </tr>
             <tr>
              <td class="FldTitleCol">
               Password
              </td>
              <td class="FldEntryCol">
               <input type="password" id="Password" runat="server" size="15" maxlength="15" class="Form"/>
              </td>
             </tr>
            </table>
           </td>
          </tr>
          <tr id="UpdDelBtn" runat="server"> 
           <td style="text-align: center;" class="SubTitle">
            <asp:Button ID="Button1" Text="Logon" runat="server"/>
           </td>
          </tr>
         </table>
         </form>
        </td>
       </tr>
      </table>
     </td>
    </tr>
   </table>
  </div>
  <div id="FooterPos">
   <uc1:Footer id="Footer" runat="server"></uc1:Footer>
  </div>
 </body>
</html>
