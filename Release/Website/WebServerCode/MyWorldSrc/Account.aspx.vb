
Partial Class Account
 Inherits System.Web.UI.Page

 '*************************************************************************************************
 '* Open Source Project Notice:
 '* The "MyWorld" website is a community supported open source project intended for use with the 
 '* Halcyon Simulator project posted at https://github.com/inworldz and compatible derivatives of 
 '* that work. 
 '* Contributions to the MyWorld website project are to be original works contributed by the authors
 '* or other open source projects. Only the works that are directly contributed to this project are
 '* considered to be part of the project, included in it as community open source content. This does 
 '* not include separate projects or sources used and owned by the respective contibutors that may 
 '* contain simliar code used in their other works. Each contribution to the MyWorld project is to 
 '* include in a header like this what its sources and contributor are and any applicable exclusions 
 '* from this project. 
 '* The MyWorld website is released as public domain content is intended for Halcyon Simulator 
 '* virtual world support. It is provided as is and for use in customizing a website access and 
 '* support for the intended application and may not be suitable for any other use. Any derivatives 
 '* of this project may not reverse claim exclusive rights or profiting from this work. 
 '*************************************************************************************************
 '* The Account page is where users arrive after logon to the website. It provides access to all options 
 '* any user may have in the website, and to update their account access and partner registry.
 '* 

 '* Built from MyWorld Form template v. 1.0

 ' Define common Page class properties and objects here for the page
 Private MyDB As New MySQLLib                               ' Provides data access methods and error handling
 Private PageCtl As New GridLib
 Private SQLCmd As String

 Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
  ' Validate logon and session existance.
  If Session.Count() = 0 Or Len(Session("UUID")) = 0 Then
   Response.Redirect("Logon.aspx")                          ' Return to logon page
  End If
  If Request.ServerVariables("HTTPS") = "off" And Session("SSLStatus") Then ' Security is not active and is required
   Response.Redirect("Logon.aspx")
  End If

  Trace.IsEnabled = False
  If Trace.IsEnabled Then Trace.Warn("Account", "Start Page Load")

  If Not IsPostBack Then                                    ' First time page accessed setup
   ' Define process unique objects here

   ' Setup general page controls

   ' Display data fields based on edit or add mode
   ' Get database display values
   Dim drApp As MySql.Data.MySqlClient.MySqlDataReader
   SQLCmd = "Select A.email," +
            " B.recv_ims_via_email,B.listed_in_directory " +
            "From users A inner join userpreferences B on A.UUID=B.user_id " +
            "Where UUID=" + MyDB.SQLStr(Session("UUID"))
   If Trace.IsEnabled Then Trace.Warn("Account", "Get display Values SQLCmd: " + SQLCmd.ToString())
   drApp = MyDB.GetReader("MyData", SQLCmd)
   If drApp.Read() Then
    CurrEmail.InnerText = drApp("email").ToString().Trim()
    ImsInEmail.Checked = drApp("recv_ims_via_email").ToString().Trim()
    ListInDirect.Checked = drApp("listed_in_directory").ToString().Trim()
   End If
   drApp.Close()
   ' Setup Edit Mode page display controls
   Email.Focus()                                            ' Set focus to the first field for entry

   ' Setup Partner Option
   PartFound.visible = False
   Button4.Visible = False
   Button5.Visible = False
   Dim AcctName As String
   AcctName = ""
   Dim GetUser As MySql.Data.MySqlClient.MySqlDataReader
   SQLCmd = "Select Concat(username,' ',lastname) as AcctName," +
            " Case When partner<>'00000000-0000-0000-0000-000000000000' " +
            " Then " +
            "  (Select Concat(username,' ',lastname) as Name From users B " +
            "   Where UUID=users.partner) " +
            " Else '' " +
            " End as PartnerName " +
            "From users " +
            "Where UUID=" + MyDB.SQLStr(Session("UUID"))
   If Trace.IsEnabled Then Trace.Warn("Account", "Get GetUser partner SQLCmd: " + SQLCmd.ToString())
   GetUser = MyDB.GetReader("MyData", SQLCmd)
   If GetUser.Read() Then
    AcctName = GetUser("AcctName").ToString().Trim()
    If GetUser("PartnerName").ToString().Trim().Length = 0 Then ' Allow Partner Selection
     PartnerSel.Visible = True
     ShowPartner.Visible = False
    Else
     PartnerSel.Visible = False
     ShowPartner.Visible = True
     ' Show Partner name
     CurrPartner.InnerText = GetUser("PartnerName").ToString().Trim()
     Button5.Visible = True
    End If
   End If
   GetUser.Close()

   Session("TLimited") = False                              ' Presume no time limit
   ' Check User account Transaction time limit
   Dim GetLimits As MySql.Data.MySqlClient.MySqlDataReader
   SQLCmd = "Select " +
            " Case When Hours>0 " +
            " Then IF(Hours> " +
            "  Case When " +
            "   (Select TIMESTAMPDIFF(HOUR,TransDate,Now()) as Hours From accountbal " +
            "    Where UUID=usereconomy.UUID Order by TransDate Desc Limit 0,1) is null " +
            "  Then 0 " +
            "  Else" +
            "   (Select TIMESTAMPDIFF(HOUR,TransDate,Now()) as Hours From accountbal " +
            "    Where UUID=usereconomy.UUID Order by TransDate Desc Limit 0,1) " +
            "  End,TRUE,FALSE) " +
            " Else FALSE " +
            " End as HourLimit " +
            "From usereconomy " +
            "Where UUID=" + MyDB.SQLStr(Session("UUID"))
   If Trace.IsEnabled Then Trace.Warn("Account", "Get usereconomy Limit SQLCmd: " + SQLCmd.ToString())
   GetLimits = MyDB.GetReader("MySite", SQLCmd)
   If Trace.IsEnabled And MyDB.Error() Then Trace.Warn("Account", "DB Error: " + MyDB.ErrMessage().ToString())
   If GetLimits.Read() Then
    Session("TLimited") = GetLimits("HourLimit")
   End If
   GetLimits.Close()

   ' Check for any estates owned
   Dim hasEstate As Boolean
   hasEstate = False
   Dim GetEstate As MySql.Data.MySqlClient.MySqlDataReader
   SQLCmd = "Select Count(EstateID) as Count From estate_settings Where EstateOwner=" + MyDB.SQLStr(Session("UUID"))
   If Trace.IsEnabled Then Trace.Warn("Account", "Get usereconomy Limit SQLCmd: " + SQLCmd.ToString())
   GetEstate = MyDB.GetReader("MyData", SQLCmd)
   If GetEstate.Read() Then
    hasEstate = (GetEstate("Count") > 0)
   End If
   GetEstate.Close()

   Dim SBMenu As New TreeView
   ' Set up navigation options
   SBMenu.SetTrace = Trace.IsEnabled
   'SBMenu.AddItem("M", "3", "Report List")                  ' Sub Menu entry requires number of expected entries following to contain in it
   'SBMenu.AddItem("B", "", "Blank Entry")                   ' Blank Line as item separator
   'SBMenu.AddItem("T", "", "Other Options")                 ' Title entry
   'SBMenu.AddItem("L", "CallEdit(0,'Account.aspx');", "New Entry") ' Javascript activated entry
   'SBMenu.AddItem("P", "/Path/page.aspx", "Link Name")      ' Program URL link entry
   SBMenu.AddItem("P", "Logout.aspx", "Logout")
   If Session("Access") > 1 Or (Session("ELevel") > 1) Or hasEstate Then   ' Page Options Show
    SBMenu.AddItem("B", "", "Blank Entry")
    SBMenu.AddItem("T", "", "Page Options")
   End If
   If Session("ELevel") > 1 Then                            ' Economy Levels 2&3
    SBMenu.AddItem("P", "TransHist.aspx", "$ Transaction History")
   End If
   If Session("ELevel") = 3 And Not Session("TLimited") Then ' Economy Level 3 only
    ' PayPal Email address and PayPal Merchant ID is required before going here.
    Dim GetPayPal As MySql.Data.MySqlClient.MySqlDataReader
    SQLCmd = "Select " +
             " (Select Parm2 From control Where Control='ECONOMY' and Parm1='PayPalEmail') as PPEmail," +
             " (Select Parm2 From control Where Control='ECONOMY' and Parm1='PPMerchantID') as PPID"
    If Trace.IsEnabled Then Trace.Warn("Account", "Get usereconomy Limit SQLCmd: " + SQLCmd.ToString())
    GetPayPal = MyDB.GetReader("MySite", SQLCmd)
    If Trace.IsEnabled And MyDB.Error() Then Trace.Warn("Account", "DB Error: " + MyDB.ErrMessage().ToString())
    GetPayPal.Read()
    If GetPayPal("PPEmail").ToString().Trim().Length > 0 And GetPayPal("PPID").ToString().Trim().Length > 0 Then
     SBMenu.AddItem("P", "Buy.aspx", "Buy M$")
     'SBMenu.AddItem("P", "Sell.aspx", "Sell M$")             ' Comment out if not used!
    End If
    GetPayPal.Close()
   End If
   If hasEstate Then                                         ' If user has lands, access Estate Management
    SBMenu.AddItem("P", "Estate.aspx", "Estate Management")
   Else
    DelTitle.InnerText = "Account: " + AcctName.ToString()
    SBMenu.AddItem("L", "ShowDelWin();", "Remove Account")
   End If
   If Session("Access") > 1 Then                            ' Access only for Web Admin or System Admin
    SBMenu.AddItem("B", "", "Blank Entry")
    SBMenu.AddItem("P", "/Administration/Admin.aspx", "Website Administration")
   End If
   If Trace.IsEnabled Then Trace.Warn("Account", "Show Menu")
   SidebarMenu.InnerHtml = SBMenu.BuildMenu("Menu Selections", 14) ' Build and display Menu options
   ' Close Sidebar Menu object
   SBMenu.Close()
  End If

 End Sub

 ' Process data validation checks
 Private Function ValAddEdit(ByVal tButton As Integer) As String
  ' Parameter tAdd allows selections of Add mode only testing
  Dim aMsg As String
  aMsg = ""
  ' Process error checking as required, place messages in tMsg.
  If tButton = 1 Then                                       ' Change Email field tests
   If Email.Text.ToString().Trim().Length = 0 Then
    aMsg = aMsg.ToString() + "Missing Email address!\r\n"
   Else
    If Not PageCtl.ValidEmail(Email.Text.ToString()) Then
     aMsg = aMsg.ToString() + "Invalid Email address!\r\n"
    End If
   End If
  End If
  If tButton = 2 Then                                       ' Change password tests
   If Current.Text.ToString().Trim().Length = 0 Then
    aMsg = aMsg.ToString() + "Missing Current Password!\r\n"
   End If
   If NewPass.Text.ToString().Trim().Length = 0 Then
    aMsg = aMsg.ToString() + "Missing New Password!\r\n"
   End If
   If ConPass.Text.ToString().Trim().Length = 0 Then
    aMsg = aMsg.ToString() + "Missing Confirm New Password!\r\n"
   End If
   If aMsg.ToString().Length = 0 Then                       ' No errors yet...
    If NewPass.Text.ToString().Trim() <> ConPass.Text.ToString().Trim() Then
     aMsg = aMsg.ToString() + "New Password does not match Confirm New Password!\r\n"
    Else
     If Current.Text.ToString().Trim() = NewPass.Text.ToString().Trim() Then
      aMsg = aMsg.ToString() + "New Password is the same as the current password!\r\n"
     End If
    End If
   End If
  End If
  'If tButton = 3 Then                                       ' Set Preferences testing
  'End If

  'If FieldName.Text.ToString().Trim().Length = 0 Then
  ' aMsg = aMsg.ToString() + "Missing Field Name!\r\n"
  'End If
  Return aMsg
 End Function

 ' Change Email
 Private Sub Button1_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles Button1.Click
  Dim tMsg As String
  tMsg = ValAddEdit(1)

  If tMsg.ToString().Trim().Length = 0 Then
   ' Send confirmation email to current email address with new email to change to. 
   ' Email will have data to send to confirmEmail page which will change the email address.

   ' Get User Account record
   Dim Users As MySql.Data.MySqlClient.MySqlDataReader
   SQLCmd = "Select username,email " +
            "From users " +
            "Where UUID=" + MyDB.SQLStr(Session("UUID"))
   If Trace.IsEnabled Then Trace.Warn("Account", "Get user data SQLCmd: " + SQLCmd.ToString())
   Users = MyDB.GetReader("MyData", SQLCmd)
   If MyDB.Error() Then
    tMsg = "DB Error: " + MyDB.ErrMessage() + "\r\n"
   Else
    Users.Read()
    ' Apply email address change
    SQLCmd = "Update users Set email=" + MyDB.SQLStr(Email.Text) + " " +
             "Where UUID=" + MyDB.SQLStr(Session("UUID"))
    If Trace.IsEnabled Then Trace.Warn("Account", "Update users SQLCmd: " + SQLCmd.ToString())
    MyDB.DBCmd("MyData", SQLCmd)
    If Users("Email").ToString().Trim().Length > 0 Then     ' Send notice of email address change to prior address
     Dim GetEmail As MySql.Data.MySqlClient.MySqlDataReader
     SQLCmd = "Select Parm2 as SMTPServer From control Where Parm1='SMTPServer'"
     If Trace.IsEnabled Then Trace.Warn("Account", "Get SMTPServer SQLCmd: " + SQLCmd.ToString())
     GetEmail = MyDB.GetReader("MySite", SQLCmd)
     If GetEmail.HasRows() Then
      GetEmail.Read()
      ' Get system email content for a confirmation email.
      Dim tBody As String
      tBody = "Greetings [%FirstName%],<br /><br />" +
              "This is a notice to let you know the email address was changed on your My World account.<br />" +
              "Your prior email [%OldEmail%] was changed to [%NewEmail%].<br /><br />" +
              "My World Administrator"
      tBody = tBody.ToString().Replace("[%FirstName%]", Users("username").ToString().Trim())
      tBody = tBody.ToString().Replace("[%OldEmail%]", Users("email").ToString().Trim())
      tBody = tBody.ToString().Replace("[%NewEmail%]", Email.Text.ToString().Trim())
      If Trace.IsEnabled Then Trace.Warn("Account", "Sending Email (My World Email Changed Notice) to " + Users("Email").ToString().Trim() + " From mailer@" + Request.ServerVariables("HTTP_HOST") + ".")
      Dim SendMail As New SendEmail
      SendMail.EmailServer = GetEmail("SMTPServer").ToString().Trim()
      SendMail.FromAddress = "mailer@" + Request.ServerVariables("HTTP_HOST")
      SendMail.ToAddress = Users("Email").ToString().Trim()
      SendMail.Subject = "My World Email Changed Notice"
      SendMail.Body = " " + vbCrLf + tBody.ToString()
      SendMail.IsHTML = True
      If Not SendMail.SendMail() Then
       If Trace.IsEnabled Then Trace.Warn("Account", "Email Failed: " + SendMail.ErrMessage.ToString())
       tMsg = SendMail.ErrMessage.ToString()
       SendMail.ToAddress = "support@" + Request.ServerVariables("HTTP_HOST")
       SendMail.Subject = "My World Email Changed Notice - Failed to Send!"
       SendMail.Body = " " + vbCrLf + "Failed to send change email to grid user " + Users("Email").ToString().Trim() + vbCrLf +
                       "Error Message: " + tMsg.ToString()
       SendMail.IsHTML = False
       SendMail.SendMail()
      End If
      SendMail.Close()
      SendMail = Nothing
     End If
     GetEmail.Close()
    End If
   End If
   Users.Close()

   If Not BodyTag.Attributes.Item("onload") Is Nothing Then ' Remove onload error message display 
    BodyTag.Attributes.Remove("onload")
   End If
  End If
  If tMsg.ToString().Trim().Length > 0 Then
   tMsg = "Cannot update Entry:\r\n" + tMsg
   BodyTag.Attributes.Add("onload", "ShowMsg();")           ' Activate onload option to show message
   PageCtl.AlertMessage(Me, tMsg, "ErrMsg")                 ' Display Alert Message
  Else
   Email.Text = ""
  End If

 End Sub

 ' Update Password
 Private Sub Button2_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles Button2.Click
  Dim tMsg As String
  tMsg = ValAddEdit(2)

  If tMsg.Length = 0 Then

   Dim tPass, tSalt As String
   tSalt = "" '+ UserUUID.Replace("-", "").ToString()
   Dim MyLib As New GridLib
   tPass = MyLib.CodePassword(NewPass.Text.ToString(), tSalt.ToString())
   MyLib.Close()

   SQLCmd = "Update users set passwordHash=" + MyDB.SQLStr(tPass) + " " +
            "Where UUID=" + MyDB.SQLStr(Session("UUID"))
   If Trace.IsEnabled Then Trace.Warn("Account", "Update users SQLCmd: " + SQLCmd.ToString())
   MyDB.DBCmd("MyData", SQLCmd)
   If MyDB.Error() Then
    tMsg = "DB Error: " + MyDB.ErrMessage() + "\r\n"
   End If
   If Not BodyTag.Attributes.Item("onload") Is Nothing Then ' Remove onload error message display 
    BodyTag.Attributes.Remove("onload")
   End If
  End If
  If tMsg.ToString().Trim().Length > 0 Then
   tMsg = "Cannot add Entry:\r\n" + tMsg
   BodyTag.Attributes.Add("onload", "ShowMsg();")           ' Activate onload option to show message
   PageCtl.AlertMessage(Me, tMsg, "ErrMsg")                 ' Display Alert Message
  Else                                                      ' Clear Password fields
   BodyTag.Attributes.Add("onload", "ShowMsg();")           ' Activate onload option to show message
   PageCtl.AlertMessage(Me, "Password was updated!", "ErrMsg")  ' Display Alert Message
   Current.Text = ""
   NewPass.Text = ""
   ConPass.Text = ""
  End If
 End Sub

 ' Set Preferences
 Protected Sub Button3_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles Button3.Click
  Dim tMsg As String
  tMsg = ValAddEdit(3)

  If tMsg.Length = 0 Then
   SQLCmd = "Update userpreferences Set recv_ims_via_email=" + MyDB.SQLNo(IIf(ImsInEmail.Checked, 1, 0)) + "," +
            " listed_in_directory=" + MyDB.SQLNo(IIf(ListInDirect.Checked, 1, 0)) + " " +
            "Where user_id=" + MyDB.SQLNo(Session("UUID"))
   If Trace.IsEnabled Then Trace.Warn("Account", "Update userpreferences SQLCmd: " + SQLCmd.ToString())
   MyDB.DBCmd("MyData", SQLCmd)
   If MyDB.Error() Then
    tMsg = "DB Error: " + MyDB.ErrMessage() + "\r\n"
   End If
   If Not BodyTag.Attributes.Item("onload") Is Nothing Then ' Remove onload error message display 
    BodyTag.Attributes.Remove("onload")
   End If
  End If
  If tMsg.ToString().Trim().Length > 0 Then
   tMsg = "Cannot add Entry:\r\n" + tMsg
   BodyTag.Attributes.Add("onload", "ShowMsg();")           ' Activate onload option to show message
   PageCtl.AlertMessage(Me, tMsg, "ErrMsg")                 ' Display Alert Message
  End If
 End Sub

 ' Partner Search
 Private Sub Search_TextChanged(sender As Object, e As EventArgs) Handles Search.TextChanged
  ' Find possible partners that match name
  Dim tMsg, Name() As String
  tMsg = ""
  If Search.Text.ToString().Trim().Contains(" ") Then
   Name = Search.Text.ToString().Trim().Split(" ")

   ' Find matching names who dont have any current partners
   Dim GetUser As MySql.Data.MySqlClient.MySqlDataReader
   SQLCmd = "Select UUID,Concat(username,' ',lastname) as Name " +
            "From users " +
            "Where partner='00000000-0000-0000-0000-000000000000' and " +
            " username like " + MyDB.SQLLike(Name(0)) + " and lastname Like " + MyDB.SQLLike(Name(1))
   If Trace.IsEnabled Then Trace.Warn("Account", "Get display Values SQLCmd: " + SQLCmd.ToString())
   GetUser = MyDB.GetReader("MyData", SQLCmd)
   If Trace.IsEnabled And MyDB.Error() Then Trace.Warn("Account", "DB Error: " + MyDB.ErrMessage().ToString())
   If GetUser.HasRows() Then
    PartFound.Visible = True
    While GetUser.Read()
     Partners.Items.Add(GetUser("Name").ToString().Trim())
    End While
    Button4.Visible = True
   Else
    PartFound.Visible = False
    Button4.Visible = False
    tMsg = "No match was found! Please check the name"
   End If
   GetUser.Close()
  Else
   tMsg = "A space is required between first and last names. "
  End If

  If tMsg.ToString().Trim().Length > 0 Then
   tMsg = "Cannot find a partner:\r\n" + tMsg
   BodyTag.Attributes.Add("onload", "ShowMsg();")           ' Activate onload option to show message
   PageCtl.AlertMessage(Me, tMsg, "ErrMsg")                 ' Display Alert Message
  Else
   Search.Text = ""                                         ' Allow search to be done again on change
  End If
 End Sub

 ' Send Partnership offer
 Private Sub Button4_Click(sender As Object, e As EventArgs) Handles Button4.Click
  Dim tMsg, Name() As String
  tMsg = ""
  Name = Partners.SelectedValue.ToString().Split(" ")

  ' Check for Requested partner User Account
  Dim Users As MySql.Data.MySqlClient.MySqlDataReader
  SQLCmd = "Select UUID,username,email," +
           " (Select Concat(username,' ',lastname) as Name From users " +
           "  Where UUID=" + MyDB.SQLStr(Session("UUID")) + ") as Name " +
           "From users " +
           "Where username=" + MyDB.SQLStr(Name(0)) + " and lastname=" + MyDB.SQLStr(Name(1))
  If Trace.IsEnabled Then Trace.Warn("Account", "Get User Record SQLCmd: " + SQLCmd.ToString())
  Users = MyDB.GetReader("MyData", SQLCmd)
  If MyDB.Error() Then
   tMsg = tMsg.ToString() + "Get users account MyDB. Error: " + MyDB.ErrMessage() + "\r\n" +
          "SQLCmd: " + SQLCmd.ToString() + "\r\n"
  Else
   If Users.HasRows() Then
    Users.Read()
    If Users("Email").ToString().Trim().Length > 0 Then
     ' Get system email content for a confirmation email.
     Dim tBody As String
     tBody = "[%Name2%]," +
             "<p>[%Name%] is asking you to partnership in the My World.<br><br>" +
             "If you accept this request then open <a href=""" + IIf(Session("SSLStatus"), "https://", "http://") + Request.ServerVariables("HTTP_HOST") + "/ConfirmPartner.aspx?Part1=[%UUID%]&Part2=[%UUID2%]"">My World Confirmation</a> " +
             "to send your acceptance.</p> " +
             "Thank You!"
     tBody = tBody.ToString().Replace("[%Name%]", Users("Name").ToString())           ' Requestor
     tBody = tBody.ToString().Replace("[%UUID%]", Session("UUID").ToString())
     tBody = tBody.ToString().Replace("[%Name2%]", Users("username").ToString())      ' Requestee
     tBody = tBody.ToString().Replace("[%UUID2%]", Users("UUID").ToString())
     If Trace.IsEnabled Then Trace.Warn("Account", "Sending Partner Confirmation Email to " + Users("email").ToString().Trim() + " From mailer@MyWorld.com")
     Dim GetEmail As MySql.Data.MySqlClient.MySqlDataReader
     SQLCmd = "Select Parm2 as SMTPServer From control Where Parm1='SMTPServer'"
     If Trace.IsEnabled Then Trace.Warn("Account", "Get SMTPServer SQLCmd: " + SQLCmd.ToString())
     GetEmail = MyDB.GetReader("MySite", SQLCmd)
     If GetEmail.HasRows() Then
      GetEmail.Read()
      ' Compose Confirmation Email to requestor
      Dim SendMail As New SendEmail
      SendMail.EmailServer = GetEmail("SMTPServer").ToString().Trim()
      SendMail.FromAddress = "mailer@" + Request.ServerVariables("HTTP_HOST")
      SendMail.ToAddress = Users("Email").ToString().Trim()
      SendMail.Subject = "My World Partnership Request Email"
      SendMail.Body = " " + vbCrLf + tBody.ToString()
      SendMail.IsHTML = True
      If Not SendMail.SendMail() Then
       If Trace.IsEnabled Then Trace.Warn("Account", "Email Failed: " + SendMail.ErrMessage.ToString())
       tMsg = SendMail.ErrMessage.ToString()
       SendMail.ToAddress = "support@" + Request.ServerVariables("HTTP_HOST")
       SendMail.Subject = "My World Partnership Request Email - Failed to Send!"
       SendMail.Body = " " + vbCrLf + "Failed to send Confirmation email to the designated Partner " + Users("Email").ToString().Trim() + vbCrLf +
                       "Error Message: " + tMsg.ToString()
       SendMail.IsHTML = False
       SendMail.SendMail()
       If Not SendMail.SendMail() And Trace.IsEnabled Then Trace.Warn("Account", "Support Email Failed: " + SendMail.ErrMessage.ToString())
      End If
      SendMail.Close()
      SendMail = Nothing
     End If
     GetEmail.Close()
    Else
     tMsg = "Selected Partner is missing an email address!"
    End If
   Else
    tMsg = "No account was found for the First and Last names entered."
   End If
  End If
  Users.Close()

  If tMsg.ToString().Trim().Length > 0 Then
   tMsg = "Cannot add Entry:\r\n" + tMsg
   BodyTag.Attributes.Add("onload", "ShowMsg();")           ' Activate onload option to show message
   PageCtl.AlertMessage(Me, tMsg, "ErrMsg")                 ' Display Alert Message
  End If

 End Sub

 ' Disolve Partnership
 Private Sub Button5_Click(sender As Object, e As EventArgs) Handles Button5.Click
  Dim tMsg As String
  tMsg = ""

  ' Get Current Partner
  Dim Divorcee As MySql.Data.MySqlClient.MySqlDataReader
  SQLCmd = " Select partner From users Where UUID =" + MyDB.SQLStr(Session("UUID"))
  Divorcee = MyDB.GetReader("MyData", SQLCmd)
  Divorcee.Read()

  ' Make DB Updates
  SQLCmd = "Update users Set Partner = '00000000-0000-0000-0000-000000000000' " +
           "Where UUID = " + MyDB.SQLStr(Session("UUID")) + ";" +
           "Update users Set Partner = '00000000-0000-0000-0000-000000000000' " +
           "Where UUID = " + MyDB.SQLStr(Divorcee("partner")).ToString()
  If Trace.IsEnabled Then Trace.Warn("Account", "Update users SQLCmd: " + SQLCmd.ToString())
  MyDB.DBCmd("MyData", SQLCmd)
  tMsg = "Your partnership dissolution to " + MyDB.SQLStr(Divorcee).ToString() + " has been processed successfully."
  Divorcee.Close()

  BodyTag.Attributes.Add("onload", "ShowMsg();")           ' Activate onload option to show message
  PageCtl.AlertMessage(Me, tMsg, "ErrMsg")                 ' Display Alert Message

  'Clean up display fields
  PartnerSel.Visible = True                                ' Reset to allow partner selection again
  ShowPartner.Visible = False
  PartFound.Visible = False
  Button4.Visible = False
  Button5.Visible = False

 End Sub

 ' Delete Account
 Private Sub DelAccount_CheckedChanged(sender As Object, e As EventArgs) Handles DelAccount.CheckedChanged
  '  -- Delete user account! Will orphan any items user had rezzed out on any region.
  SQLCmd = "Delete From inventoryfolders Where agentID=" + MyDB.SQLStr(Session("UUID")) + ";" +
           "Delete From inventoryitems Where avatarID=" + MyDB.SQLStr(Session("UUID")) + ";" +
           "Delete From osagent Where agentID=" + MyDB.SQLStr(Session("UUID")) + ";" +
           "Delete From osgrouprolemembership Where AgentID=" + MyDB.SQLStr(Session("UUID")) + ";" +
           "Delete From osgroupmembership Where AgentID=" + MyDB.SQLStr(Session("UUID")) + ";" +
           "Delete From userpreferences Where user_id=" + MyDB.SQLStr(Session("UUID")) + ";" +
           "Delete From agents Where UUID=" + MyDB.SQLStr(Session("UUID")) + ";" +
           "Delete From users Where UUID=" + MyDB.SQLStr(Session("UUID"))
  If Trace.IsEnabled Then Trace.Warn("Account", "Remove User account records SQLCmd: " + SQLCmd.ToString())
  MyDB.DBCmd("MyData", SQLCmd)
  If Trace.IsEnabled And MyDB.Error() Then Trace.Warn("Account", "DB Error: " + MyDB.ErrMessage().ToString())
  If Not Trace.IsEnabled Then
   Response.Redirect("Logout.aspx")                        ' Cancel logon
  End If
 End Sub

 Private Sub Page_Unload(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.Unload
  ' Close open page objects
  MyDB.Close()
  MyDB = Nothing
  PageCtl = Nothing
 End Sub

End Class
