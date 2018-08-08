<%@ Page Language="VB" AutoEventWireup="false" CodeFile="GridManager.aspx.vb" Inherits="Administration_GridManager" %>
<%@ Register TagPrefix="uc1" TagName="Header" Src="~/Header.ascx" %>
<%@ Register TagPrefix="uc1" TagName="SysMenu" Src="~/SysMenu.ascx" %>
<%@ Register TagPrefix="uc1" TagName="Footer" Src="~/Footer.ascx" %>

<!DOCTYPE html>
<html xmlns="https://www.w3.org/1999/xhtml" >
 <head>
  <title>Administration - My World</title>
  <link href="/styles/Site.css" type="text/css" rel="stylesheet" />
  <link href="/Styles/TopMenu.css" type="text/css" rel="stylesheet" />
  <link href="/styles/TreeView.css" type="text/css" rel="stylesheet" />
  <script type="text/javascript" src="/scripts/Cookie.js"></script>
  <script type="text/javascript" src="/scripts/TopMenu.js"></script>
  <script type="text/javascript" src="/scripts/TreeView.js"></script>
  <script type="text/javascript">

   var aVar = setInterval(DoRefresh, 60000);  // trigger refresh once a minute

   function DoRestart(aUUID) {
    document.getElementById('ReStart').value = aUUID;
    // Disable calling icon
    document.getElementById(aUUID + "RES").src = "/Images/Icons/RestartIconDis.png";
    document.getElementById(aUUID + "RES").onclick = '';
    document.getElementById(aUUID + "RES").style = '';
    // disable stop button
    document.getElementById(aUUID + "ACT").src = "/Images/Icons/StopIconDis.png";
    document.getElementById(aUUID + "ACT").onclick = '';
    document.getElementById(aUUID + "ACT").style = '';
    document.getElementById(aUUID + "STA").src = "/Images/Icons/Closing.png";
    //setTimeout('__doPostBack(\'ReStart\',\'\')', 0);
    __doPostBack('ReStart', '');
   }

   function DoShutdown(aUUID) {
    document.getElementById('Quit').value = aUUID;
    // Disable calling icon
    document.getElementById(aUUID + "ACT").src = "/Images/Icons/StopIconDis.png";
    document.getElementById(aUUID + "ACT").onclick = '';
    document.getElementById(aUUID + "ACT").style = '';
    // Disable Restart button
    document.getElementById(aUUID + "RES").src = "/Images/Icons/RestartIconDis.png";
    document.getElementById(aUUID + "RES").onclick = '';
    document.getElementById(aUUID + "RES").style = '';
    document.getElementById(aUUID + "STA").src = "/Images/Icons/Closing.png";
    //setTimeout('__doPostBack(\'Quit\',\'\')', 0);
    __doPostBack('Quit', '')
   }

   function DoStart(aUUID) {
    alert("Start region on server!");
    //document.getElementById('Start').value = aUUID;
    //// Disable calling icon
    //document.getElementById(aUUID + "ACT").src = "/Images/Icons/StartIconDis.png";
    //document.getElementById(aUUID + "ACT").onclick = "";
    //document.getElementById(aUUID + "ACT").style = '';
    //// Disable Restart button
    //document.getElementById(aUUID + "RES").src = "/Images/Icons/RestartIconDis.png";
    //document.getElementById(aUUID + "RES").onclick = '';
    //document.getElementById(aUUID + "RES").style = '';
    //document.getElementById(aUUID + "STA").src = "/Images/Icons/Starting.png";
    //setTimeout('__doPostBack(\'Start\',\'\')', 0);
    //__doPostBack('Start', '')
   }

   function DoRefresh() {
    document.getElementById('Refresh').checked = true;
    //setTimeout('__doPostBack(\'Refresh\',\'\')', 0);
    __doPostBack('Refresh', '');
   }

   function SetSort(tOrder) { // Set Page display order
    document.getElementById("Order").value = tOrder;
    setTimeout('__doPostBack(\'Order\',\'\')', 0);
   }

   function CallEdit(tID,tAction) { // Add / Edit a record
    document.EditPage.KeyID.value=tID;
    document.EditPage.action=tAction;
    document.EditPage.submit();
   }

  </script>
 </head>
 <body>
  <!-- Built from MyWorld Select template v. 1.0 -->
  <div id="HeaderPos">
   <uc1:Header id="Header1" runat="server"></uc1:Header>
   <uc1:SysMenu id="SysMenu1" runat="server"></uc1:SysMenu>
  </div>
  <div id="LSideBar">
   <table class="SidebarCtl">
    <tr>
     <td class="ProgTitle">Grid Manager</td>
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
      <!-- Body Content here -->
      <form id="aspnetForm" method="post" runat="server">
      <table style="width:100%;">
       <tr>
        <td>
         <table style="width: 100%;">
          <tr>
           <td class="Title" onclick="SetSort('Name');" title="Order by Name" style="width: 30%; cursor: pointer;">Name</td>
           <td class="Title" onclick="SetSort('Map');" title="Order by Map" style="width: 16%;">Map : Port</td>
           <td class="Title" onclick="SetSort('Owner');" title="Order by Owner" style="width: 24%; cursor: pointer;">Owner</td>
           <td class="Title" onclick="SetSort('Status');" title="Order by Status" style="width: 14%; cursor: pointer;">Status</td>
           <td class="Title" style="width: 8%;">Restart</td>
           <td class="Title" style="width: 8%;">Cmd</td>
          </tr>
         </table>
         <asp:GridView ID="gvDisplay" ShowHeader="false" runat="server" AllowPaging="False" AutoGenerateColumns="False" GridLines="None" CellSpacing="1" Width="100%" HeaderStyle-Height="28px" PagerStyle-HorizontalAlign="Center" AlternatingRowStyle-CssClass="AltLine">
          <Columns>
           <asp:TemplateField HeaderText="Name" ItemStyle-Width="30%" ItemStyle-HorizontalAlign="Left" HeaderStyle-CssClass="Title">
            <ItemTemplate>
             <span onclick="CallEdit('<%#Container.DataItem("UUID").ToString().Trim()%>','RegionConfig.aspx');" title="<%#Container.DataItem("UUID").ToString().Trim()%>" class="NavLink" onmouseover="this.className='NOverLink';" onmouseout="this.className='NavLink';">
              <%#Container.DataItem("regionName").ToString().Trim()%>
             </span>
            </ItemTemplate>
           </asp:TemplateField>
           <asp:TemplateField HeaderText="Map : Port" ItemStyle-Width="16%" ItemStyle-HorizontalAlign="Left" HeaderStyle-CssClass="Title">
            <ItemTemplate>
             <%#Container.DataItem("Map").ToString().Trim()%> : <%#Container.DataItem("Port").ToString().Trim()%>
            </ItemTemplate>
           </asp:TemplateField>
           <asp:TemplateField HeaderText="Owner" ItemStyle-Width="24%" ItemStyle-HorizontalAlign="Left" HeaderStyle-CssClass="Title">
            <ItemTemplate>
             <%#Container.DataItem("OwnerName").ToString().Trim()%>
            </ItemTemplate>
           </asp:TemplateField>
           <asp:TemplateField HeaderText="Status" ItemStyle-Width="14%" ItemStyle-HorizontalAlign="Left" HeaderStyle-CssClass="Title">
            <ItemTemplate>
             <%#ShowStatus(Container.DataItem("UUID").ToString(), Container.DataItem("status")).ToString()%>
            </ItemTemplate>
           </asp:TemplateField>
           <asp:TemplateField HeaderText="Restart" ItemStyle-Width="8%" ItemStyle-HorizontalAlign="Center" HeaderStyle-CssClass="Title">
            <ItemTemplate>
             <%#SetRestart(Container.DataItem("UUID").ToString(), Container.DataItem("status")).ToString()%>
            </ItemTemplate>
           </asp:TemplateField>
           <asp:TemplateField HeaderText="Cmd" ItemStyle-Width="8%" ItemStyle-HorizontalAlign="Center" HeaderStyle-CssClass="Title">
            <ItemTemplate>
             <%#SetAction(Container.DataItem("UUID").ToString(), Container.DataItem("status")).ToString()%>
            </ItemTemplate>
           </asp:TemplateField>
          </Columns>
          <PagerSettings FirstPageImageUrl="/Images/paging/First.jpg" LastPageImageUrl="/Images/paging/Last.jpg" Mode="NextPreviousFirstLast" NextPageImageUrl="/Images/paging/Next.jpg" PreviousPageImageUrl="/Images/paging/Prior.jpg" />
         </asp:GridView>
        </td>
       </tr>
      </table>
       <asp:TextBox ID="ReStart" runat="server" Text="" AutoPostBack="true" CssClass="NoShow" />
       <asp:TextBox ID="Start" runat="server" Text="" AutoPostBack="true" CssClass="NoShow" />
       <asp:TextBox ID="Quit" runat="server" Text="" AutoPostBack="true" CssClass="NoShow" />
       <asp:CheckBox ID="Refresh" runat="server" Text="" AutoPostBack="true" CssClass="NoShow" />
       <asp:TextBox ID="Order" runat="server" AutoPostBack="true" CssClass="NoShow" />
      </form>
     </td>
    </tr>
   </table>
  </div>
  <div id="FooterPos">
   <uc1:Footer id="Footer" runat="server"></uc1:Footer>
  </div>
  <form name="EditPage" action="" method="post">
   <input type="hidden" name="KeyID" value="" />
  </form>
 </body>
</html>
