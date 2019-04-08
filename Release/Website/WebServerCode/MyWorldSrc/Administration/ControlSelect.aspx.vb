
Partial Class Administration_ControlSelect
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
 '* Control Value Selection page. 
 '* 
 '* 

 '* Built from MyWorld Select template v. 1.0

 ' Define common properties and objects here for the page
 Private MyDB As New MySQLLib                               ' Provides data access methods and error handling
 Private SQLCmd As String

 Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
  ' Validate logon and session existance.
  If Session.Count() = 0 Or Len(Session("UUID")) = 0 Then
   Response.Redirect("/Default.aspx")                       ' Return to logon page
  End If
  If Session("Access") <> 9 Then                            ' SysAdmin Only access
   Response.Redirect("Admin.aspx")
  End If

  Trace.IsEnabled = False
  If Trace.IsEnabled Then Trace.Warn("ControlSelect", "Start Page Load")

  If Not IsPostBack Then                                    ' First time page is called setup
   ' Define process unique objects here

   ' Define local objects here
   If Session("FirstTime") = "" Then
    Session("FirstTime") = "No"
    Session("SelPage") = 0                                  ' Value is updated in the Page index change event
   End If
   Session("DBConnection") = "MySite"                       ' Define Database connection for this program

   ' Setup general page controls

   ' Set up navigation options
   ' Sidebar Options control based on Clearance or Write Access
   Dim SBMenu As New TreeView
   SBMenu.SetTrace = Trace.IsEnabled
   'SBMenu.AddItem("M", "3", "Report List")                  ' Sub Menu entry requires number of expected entries following to contain in it
   'SBMenu.AddItem("B", "", "Blank Entry")                   ' Blank Line as item separator
   'SBMenu.AddItem("T", "", "Other Options")                 ' Title entry
   'SBMenu.AddItem("L", "CallEdit(0,'TempForm.aspx');", "New Entry") ' Javascript activated entry
   'SBMenu.AddItem("P", "/Path/page.aspx", "Link Name")      ' Program URL link entry
   SBMenu.AddItem("P", "Admin.aspx", "Website Administration")
   SBMenu.AddItem("P", "/Account.aspx", "Account")
   SBMenu.AddItem("P", "/Logout.aspx", "Logout")
   SBMenu.AddItem("B", "", "Blank Entry")
   SBMenu.AddItem("T", "", "Page Options")
   SBMenu.AddItem("L", "CallEdit('','ControlForm.aspx');", "Add a New Control")
   If Trace.IsEnabled Then Trace.Warn("ControlSelect", "Show Menu")
   SidebarMenu.InnerHtml = SBMenu.BuildMenu("Menu Selections", 14) ' Build and display Menu options
   SBMenu.Close()

   ' Gridview settings
   gvDisplay.PageIndex = Session("SelPage")
   gvDisplay.PageSize = 40
  End If

  ' Get Display list Items here
  Dim Display As MySql.Data.MySqlClient.MySqlDataReader
  SQLCmd = "Select Name " +
           "From control " +
           "Where Control='ADMINSYSTEM'" +
           "Order by Name"
  If Trace.IsEnabled Then Trace.Warn("ControlSelect", "Get SQLCmd: " + SQLCmd.ToString())
  ' gvDisplay is a gridview data object placed on the page.
  Display = MyDB.GetReader("MySite", SQLCmd)
  gvDisplay.DataSource = Display
  gvDisplay.DataBind()
  If Not IsPostBack Then
   If gvDisplay.PageCount > 1 Then
    ShowPages.InnerText = "Page " + (gvDisplay.PageIndex + 1).ToString() + " of " + gvDisplay.PageCount.ToString()
   Else
    ShowPages.InnerText = " "
   End If
  End If

 End Sub

 Protected Sub gvDisplay_PageIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles gvDisplay.PageIndexChanged
  ShowPages.InnerText = "Page " + (gvDisplay.PageIndex + 1).ToString() + " of " + gvDisplay.PageCount.ToString()
  Session("SelPage") = gvDisplay.PageIndex                  ' Remember current page displayed.
 End Sub

 Private Sub Page_Unload(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.Unload
  ' Close open page objects
  MyDB.Close()
  MyDB = Nothing
  SqlClient.SqlConnection.ClearAllPools()      ' Causes all pooled connection to be forced closed when finished, reducing pooled connection size.
 End Sub

End Class
