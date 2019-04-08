
Partial Class Administration_UserForm
 Inherits System.Web.UI.Page

 '*************************************************************************************************
 '* Open Source Project Notice:
 '* The "MyWorld" website is a community supported open source project intended for use with the 
 '* Halcyon Simulator project posted at https://github.com/HalcyonGrid and compatible derivatives of 
 '* that work. 
 '* Contributions to the MyWorld website project are to be original works contributed by the authors
 '* or other open source projects. Only the works that are directly contributed to this project are
 '* considered to be part of the project, included in it as community open source content. This does 
 '* not include separate projects or sources used and owned by the respective contributors that may 
 '* contain similar code used in their other works. Each contribution to the MyWorld project is to 
 '* include in a header like this what its sources and contributor are and any applicable exclusions 
 '* from this project. 
 '* The MyWorld website is released as public domain content is intended for Halcyon Simulator 
 '* virtual world support. It is provided as is and for use in customizing a website access and 
 '* support for the intended application and may not be suitable for any other use. Any derivatives 
 '* of this project may not reverse claim exclusive rights or profiting from this work. 
 '*************************************************************************************************
 '* UserForm page provides access to world accounts and settings that can manage the Economy limitations 
 '* when used for each account, banish, or restore accounts in the world.
 '* 

 '* Built from MyWorld Form template v. 1.0

 ' Define common Page class properties and objects here for the page
 Private MyDB As New MySQLLib                               ' Provides data access methods and error handling
 Private PageCtl As New GridLib
 Private SQLCmd As String

 Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
  ' Validate logon and session existance.
  If Session.Count() = 0 Or Len(Session("UUID")) = 0 Then
   Response.Redirect("/Default.aspx")                       ' Return to logon page
  End If
  If Session("Access") < 2 Then                             ' Webadmin or SysAdmin access
   Response.Redirect("/Default.aspx")
  End If

  Trace.IsEnabled = False
  If Trace.IsEnabled Then Trace.Warn("UserForm", "Start Page Load")

  If Not IsPostBack Then                                    ' First time page accessed setup
   ' Define process unique objects here

   ' Get identity key
   If Len(Request("KeyID")) > 0 Then                        ' Update or Add Mode
    KeyID.Value = Request("KeyID")
   Else                                                     ' Critical error, cancel operation
    Response.Redirect("UserSelect.aspx")
   End If

   ' Setup general page controls
   Admin1.Visible = (Session("Access") = 9)
   Admin2.Visible = (Session("ELevel") = 3)
   Admin3.Visible = (Session("ELevel") = 3)
   Admin4.Visible = (Session("ELevel") = 3)
   If Session("Access") = 9 Then                            ' SysAdmin only option
    ' Setup webmaster access assignment selections
    Access.Items.Add(New ListItem("User", "1"))
    Access.Items.Add(New ListItem("Webmaster", "2"))
   End If
   Display()
  End If

 End Sub

 Private Sub Display()
  If Trace.IsEnabled Then Trace.Warn("UserForm", "* Display() Called")

  ' Display data fields based on edit or add mode
  Dim drApp As MySql.Data.MySqlClient.MySqlDataReader
  SQLCmd = "Select username,lastname,email,passwordSalt," +
           " (Select regionName From regions where uuid=users.homeRegionID) as HomeRegion," +
           " (Select agentOnline From agents Where UUID=users.UUID) as Status," +
           " (Select logoutTime From agents Where UUID=users.UUID) as LastLogon," +
           " (Select agentIP From agents Where UUID=users.UUID) as IP," +
           " (Select Count(avatarID) as Count From inventoryitems where avatarID=users.UUID)+" +
           " (Select Count(AgentID) as Count From osgroupmembership Where AgentID=users.UUID) as HadErrors " +
           "From users " +
           "Where UUID=" + MyDB.SQLStr(KeyID.Value)
  If Trace.IsEnabled Then Trace.Warn("UserForm", "Get display Values SQLCmd: " + SQLCmd.ToString())
  drApp = MyDB.GetReader("MyData", SQLCmd)
  If Trace.IsEnabled And MyDB.Error() Then Trace.Warn("UserForm", "DB Error: " + MyDB.ErrMessage().ToString())
  If drApp.Read() Then
   If Session("Access") = 9 Then                                 ' SysAdmin only option
    Dim GetLevels As MySql.Data.MySqlClient.MySqlDataReader
    SQLCmd = "Select Nbr2 From control " +
             "Where Control='WebmastersList' and Name=" + MyDB.SQLStr(drApp("username").ToString().Trim() + " " + drApp("lastname").ToString().Trim())
    If Trace.IsEnabled Then Trace.Warn("UserForm", "Get Access Level SQLCmd: " + SQLCmd.ToString())
    GetLevels = MyDB.GetReader("MySite", SQLCmd)
    If Trace.IsEnabled And MyDB.Error() Then Trace.Warn("UserForm", "DB Error: " + MyDB.ErrMessage().ToString())
    If GetLevels.HasRows() Then
     GetLevels.Read()
     Access.SelectedValue = GetLevels("Nbr2")
    Else
     Access.SelectedValue = 1
    End If
    GetLevels.Close()
    If Session("ELevel") = 3 Then                                ' Economy limit access
     Dim GetLimits As MySql.Data.MySqlClient.MySqlDataReader
     SQLCmd = "Select MaxBuy,MaxSell,Hours " +
              "From usereconomy " +
              "Where UUID=" + MyDB.SQLStr(KeyID.Value)
     If Trace.IsEnabled Then Trace.Warn("UserForm", "Get usereconomy Limits SQLCmd: " + SQLCmd.ToString())
     GetLimits = MyDB.GetReader("MySite", SQLCmd)
     If Trace.IsEnabled And MyDB.Error() Then Trace.Warn("UserForm", "DB Error: " + MyDB.ErrMessage().ToString())
     If GetLimits.HasRows() Then
      GetLimits.Read()
      MaxBuy.Text = GetLimits("MaxBuy")
      MaxSell.Text = GetLimits("MaxSell")
      NumHours.Text = GetLimits("Hours")
     Else
      SQLCmd = "Select " +
               " (Select Nbr2 From control " +
               "  Where Control='ECONOMY' and Parm1='MaxBuy') as MaxBuy," +
               " (Select Nbr2 From control " +
               "  Where Control='ECONOMY' and Parm1='MaxSell') as MaxSell," +
               " (Select Nbr2 From control " +
               "  Where Control='ECONOMY' and Parm1='BuySellTime') as Hours"
      If Trace.IsEnabled Then Trace.Warn("UserForm", "Get Default Limits SQLCmd: " + SQLCmd.ToString())
      GetLimits = MyDB.GetReader("MySite", SQLCmd)
      If Trace.IsEnabled And MyDB.Error() Then Trace.Warn("UserForm", "DB Error: " + MyDB.ErrMessage().ToString())
      GetLimits.Read()
      MaxBuy.Text = GetLimits("MaxBuy")
      MaxSell.Text = GetLimits("MaxSell")
      NumHours.Text = GetLimits("Hours")
     End If
     GetLimits.Close()
    End If
   End If
   Name.InnerText = drApp("username").ToString().Trim() + " " + drApp("lastname").ToString().Trim()
   Email.InnerText = drApp("email").ToString().Trim()
   UUID.InnerText = KeyID.Value
   IP.InnerText = drApp("IP").ToString().Trim()
   Home.InnerText = "Homeless"
   If drApp("HomeRegion").ToString().Trim().Length > 0 Then
    Home.InnerText = drApp("HomeRegion").ToString().Trim()
   End If
   Status.InnerText = "Offline"
   If Not drApp("Status") Is DBNull.Value Then
    If drApp("Status") > 0 Then
     Status.InnerText = "Online"
    End If
   End If
   If drApp("email").ToString().Trim() = drApp("passwordSalt").ToString().Trim() Then ' Account is logon blocked (may also mean IP blocked)
    Button1.Visible = False
    Button2.Visible = True
   Else                                                     ' Account is not blocked
    Button1.Visible = True
    Button2.Visible = False
   End If
   Button4.Visible = ((drApp("HadErrors") = 0 Or drApp("LastLogon")=0) And Session("Access") = 9) ' HadErrors and SysAdmin only option
  End If
  drApp.Close()
  ' Setup Edit Mode page display controls
  'FieldName.Focus()                                        ' Set focus to the first field for entry

  Dim SBMenu As New TreeView
  ' Set up navigation options
  SBMenu.SetTrace = Trace.IsEnabled
  'SBMenu.AddItem("M", "3", "Report List")                   ' Sub Menu entry requires number of expected entries following to contain in it
  'SBMenu.AddItem("B", "", "Blank Entry")                    ' Blank Line as item separator
  'SBMenu.AddItem("T", "", "Other Options")                  ' Title entry
  'SBMenu.AddItem("L", "CallEdit(0,'UserForm.aspx');", "New Entry") ' Javascript activated entry
  'SBMenu.AddItem("P", "/Path/page.aspx", "Link Name")       ' Program URL link entry
  SBMenu.AddItem("P", "UserSelect.aspx", "Account Select")
  SBMenu.AddItem("P", "Admin.aspx", "Website Administration")
  SBMenu.AddItem("P", "/Account.aspx", "Account")
  SBMenu.AddItem("P", "/Logout.aspx", "Logout")
  If Trace.IsEnabled Then Trace.Warn("UserForm", "Show Menu")
  SidebarMenu.InnerHtml = SBMenu.BuildMenu("Menu Selections", 14) ' Build and display Menu options
  ' Close Sidebar Menu object
  SBMenu.Close()

 End Sub

 ' Changed Access Level - SysAdmin only action
 Private Sub Access_SelectedIndexChanged(sender As Object, e As EventArgs) Handles Access.SelectedIndexChanged
  If Trace.IsEnabled Then Trace.Warn("UserForm", "* Access Changed Called")
  Dim tMsg As String
  tMsg = ""

  '' Only to allow Access to the list in the Control List program. Otherwise not needed.
  'Dim ChkWebMstList As MySql.Data.MySqlClient.MySqlDataReader
  'SQLCmd = "Select Parm1 From control " +
  '         "Where Control='ADMINLISTS' and Parm1='WebmastersList'"
  'If Trace.IsEnabled Then Trace.Warn("UserForm", "Check for WebmastersList SQLCmd: " + SQLCmd.ToString())
  'ChkWebMstList = MyDB.GetReader("MySite", SQLCmd)
  'If Not ChkWebMstList.HasRows() Then
  ' SQLCmd = "Insert control Set " +
  '          "Control='ADMINLISTS',Name='Webmasters Access List',Parm1='WebmastersList'," +
  '          "Parm2='Members contain the list of additional accounts accessing website admin tools.',Nbr1=0,Nbr2=0"
  ' MyDB.DBCmd("MySite", SQLCmd)
  ' If MyDB.Error() Then
  '  tMsg = "DB Error: " + MyDB.ErrMessage() + "\r\n"
  ' End If
  'End If
  'ChkWebMstList.Close()

  ' If selection is for Webmaster, check if entry exists already
  If Access.SelectedValue = 1 Then                          ' Remove Webmaster entry
   SQLCmd = "Delete From control " +
            "Where Control='WebmastersList' and Name=" + MyDB.SQLStr(Name.InnerText)
  Else                                                      ' Set the webmaster entry
   SQLCmd = "Insert control Set " +
            "Name=" + MyDB.SQLStr(Name.InnerText) + "," +
            "Control='WebmastersList',Parm2='Webmaster Access',Parm1='',Nbr1=0,Nbr2=2"
  End If
  If Trace.IsEnabled Then Trace.Warn("UserForm", "Change Webmaster Access SQLCmd: " + SQLCmd.ToString())
  MyDB.DBCmd("MySite", SQLCmd)
  If MyDB.Error() Then
   tMsg = "DB Error: " + MyDB.ErrMessage() + "\r\n"
  End If
  If Not BodyTag.Attributes.Item("onload") Is Nothing Then  ' Remove onload error message display 
   BodyTag.Attributes.Remove("onload")
  End If
  If tMsg.ToString().Trim().Length > 0 Then
   tMsg = "Cannot update Account:\r\n" + tMsg
   BodyTag.Attributes.Add("onload", "ShowMsg();")           ' Activate onload option to show message
   PageCtl.AlertMessage(Me, tMsg, "ErrMsg")                 ' Display Alert Message
  Else
   Display()
  End If

 End Sub

 ' Block Account Button
 Private Sub Button1_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles Button1.Click
  If Trace.IsEnabled Then Trace.Warn("UserForm", "* BLock Account Button Clicked")
  Dim tMsg As String
  tMsg = ""

  SQLCmd = "Update users Set " +
           "passwordSalt=" + MyDB.SQLStr(Email.InnerText) + " " +
           "Where UUID=" + MyDB.SQLStr(KeyID.Value)
  If Trace.IsEnabled Then Trace.Warn("UserForm", "Update users SQLCmd: " + SQLCmd.ToString())
  MyDB.DBCmd("MyData", SQLCmd)
  If MyDB.Error() Then
   tMsg = "DB Error: " + MyDB.ErrMessage() + "\r\n"
  End If
  If Not BodyTag.Attributes.Item("onload") Is Nothing Then  ' Remove onload error message display 
   BodyTag.Attributes.Remove("onload")
  End If
  If tMsg.ToString().Trim().Length > 0 Then
   tMsg = "Cannot update Account:\r\n" + tMsg
   BodyTag.Attributes.Add("onload", "ShowMsg();")           ' Activate onload option to show message
   PageCtl.AlertMessage(Me, tMsg, "ErrMsg")                 ' Display Alert Message
  Else
   Display()
  End If

 End Sub

 ' Unblock Account Button
 Private Sub Button2_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles Button2.Click
  If Trace.IsEnabled Then Trace.Warn("UserForm", "* Unblock Button Clicked")
  Dim NewSalt, tMsg As String
  tMsg = ""
  NewSalt = ""                                              ' for any grid that uses a salt value, set or generate it here!

  SQLCmd = "Update users Set " +
           "passwordSalt=" + MyDB.SQLStr(NewSalt) + " " +
           "Where UUID=" + MyDB.SQLStr(KeyID.Value)
  If Trace.IsEnabled Then Trace.Warn("UserForm", "Update users SQLCmd: " + SQLCmd.ToString())
  MyDB.DBCmd("MyData", SQLCmd)
  If MyDB.Error() Then
   tMsg = "DB Error: " + MyDB.ErrMessage() + "\r\n"
  End If
  If Not BodyTag.Attributes.Item("onload") Is Nothing Then  ' Remove onload error message display 
   BodyTag.Attributes.Remove("onload")
  End If

  If tMsg.ToString().Trim().Length > 0 Then
   tMsg = "Cannot update Account:\r\n" + tMsg
   BodyTag.Attributes.Add("onload", "ShowMsg();")           ' Activate onload option to show message
   PageCtl.AlertMessage(Me, tMsg, "ErrMsg")                 ' Display Alert Message
  Else
   Display()
  End If
 End Sub

 ' Update Economy Limits on Account
 Private Sub Button3_Click(sender As Object, e As EventArgs) Handles Button3.Click
  If Trace.IsEnabled Then Trace.Warn("UserForm", "* Update Economy Button Clicked")
  Dim tMsg As String
  tMsg = ""

  Dim GetLimits As MySql.Data.MySqlClient.MySqlDataReader
  ' Record check to see if it exists. Possible to not have been setup.
  SQLCmd = "Select UUID " +
           "From usereconomy " +
           "Where UUID=" + MyDB.SQLStr(KeyID.Value)
  If Trace.IsEnabled Then Trace.Warn("UserForm", "Check usereconomy SQLCmd: " + SQLCmd.ToString())
  GetLimits = MyDB.GetReader("MySite", SQLCmd)
  If Trace.IsEnabled And MyDB.Error() Then Trace.Warn("UserForm", "DB Error: " + MyDB.ErrMessage().ToString())
  If GetLimits.HasRows() Then
   SQLCmd = "Update usereconomy Set " +
            "MaxBuy=" + MyDB.SQLNo(MaxBuy.Text) + "," + "MaxSell=" + MyDB.SQLNo(MaxSell.Text) + "," +
            "Hours=" + MyDB.SQLNo(NumHours.Text) + " " +
            "Where UUID=" + MyDB.SQLStr(KeyID.Value)
   If Trace.IsEnabled Then Trace.Warn("UserForm", "Update usereconomy SQLCmd: " + SQLCmd.ToString())
   MyDB.DBCmd("MySite", SQLCmd)
   If MyDB.Error() Then
    If Trace.IsEnabled Then Trace.Warn("UserForm", "DB Error: " + MyDB.ErrMessage().ToString())
    tMsg = "DB Error: " + MyDB.ErrMessage() + "\r\n"
   End If
  Else
   SQLCmd = "Insert into usereconomy (UUID,MaxBuy,MaxSell,Hours) " +
            "Values (" + MyDB.SQLStr(KeyID.Value) + "," + MyDB.SQLNo(MaxBuy.Text) + "," +
            MyDB.SQLNo(MaxSell.Text) + "," + MyDB.SQLNo(NumHours.Text) + ")"
   If Trace.IsEnabled Then Trace.Warn("UserForm", "Insert usereconomy SQLCmd: " + SQLCmd.ToString())
   MyDB.DBCmd("MySite", SQLCmd)
   If MyDB.Error() Then
    If Trace.IsEnabled Then Trace.Warn("UserForm", "DB Error: " + MyDB.ErrMessage().ToString())
    tMsg = "DB Error: " + MyDB.ErrMessage() + "\r\n"
   End If
  End If
  GetLimits.Close()
  If Not BodyTag.Attributes.Item("onload") Is Nothing Then  ' Remove onload error message display 
   BodyTag.Attributes.Remove("onload")
  End If

  If tMsg.ToString().Trim().Length > 0 Then
   tMsg = "Cannot update Economy Settings:\r\n" + tMsg
   BodyTag.Attributes.Add("onload", "ShowMsg();")           ' Activate onload option to show message
   PageCtl.AlertMessage(Me, tMsg, "ErrMsg")                 ' Display Alert Message
  Else
   Display()
  End If
 End Sub

 ' Delete Account
 Private Sub Button4_Click(sender As Object, e As EventArgs) Handles Button4.Click
  '  -- Delete user account Restricted use!
  SQLCmd = "Delete From inventoryfolders Where agentID=" + MyDB.SQLStr(KeyID.Value) + ";" +
           "Delete From inventoryitems Where avatarID=" + MyDB.SQLStr(KeyID.Value) + ";" +
           "Delete From osagent Where agentID=" + MyDB.SQLStr(KeyID.Value) + ";" +
           "Delete From osgrouprolemembership Where AgentID=" + MyDB.SQLStr(KeyID.Value) + ";" +
           "Delete From osgroupmembership Where AgentID=" + MyDB.SQLStr(KeyID.Value) + ";" +
           "Delete From userpreferences Where user_id=" + MyDB.SQLStr(KeyID.Value) + ";" +
           "Delete From agents Where UUID=" + MyDB.SQLStr(KeyID.Value) + ";" +
           "Delete From users Where UUID=" + MyDB.SQLStr(KeyID.Value)
  If Trace.IsEnabled Then Trace.Warn("UserForm", "Remove User account records SQLCmd: " + SQLCmd.ToString())
  MyDB.DBCmd("MyData", SQLCmd)
  If MyDB.Error() Then
   If Trace.IsEnabled Then Trace.Warn("UserForm", "DB Error: " + MyDB.ErrMessage().ToString())
  End If
  If Not Trace.IsEnabled Then
   Response.Redirect("UserSelect.aspx")                  ' Return to Selection page
  End If

 End Sub

 Private Sub Page_Unload(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.Unload
  ' Close open page objects
  MyDB.Close()
  MyDB = Nothing
  PageCtl = Nothing
 End Sub
End Class
