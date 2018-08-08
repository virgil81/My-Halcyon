Partial Class SysMenu
 Inherits System.Web.UI.UserControl

 Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load

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
  '* Top menu bar content.

  'Put user code to initialize the page here
  Dim TopMenu As New TopMenu

  ' Menu Structure Parms:
  '  MenuID - Unique reference id number for each entry
  '  ParentID - If a menu entry is to be a submenu, this number must match the parent MenuID
  '  MenuCmd - M=Menu title entry, P=Page link, L=JavaScript function call, B=Blank separator for submenu use only
  '  MenuLink uses based on MenuCmd:
  '   M = blank or comment, field is not used in menu titles
  '   P = Page to open like /path/filename.ext
  '   L = JavaScript function call with parms
  '   B = Blank or comment, field is not used in separator entries
  '  MenuTitle - Display name for the entry in the menu. Blank for MenuCmd = B, required for all other commands.
  ' Example structure: TopMenu.AddItem(MenuID, ParentID, MenuCmd, MenuLink, MenuTitle)

  'Trace.IsEnabled = False                                 ' Page controls this setting. Use only as override for menu
  TopMenu.SetTrace = Trace.IsEnabled
  TopMenu.AddItem(1, 0, "P", "/Default.aspx", "Home")
  TopMenu.AddItem(2, 0, "P", "/FAQ.aspx", "FAQ")
  TopMenu.AddItem(3, 0, "M", "", "About US")
  TopMenu.AddItem(4, 3, "P", "/AboutUs.aspx", "About")
  TopMenu.AddItem(5, 3, "P", "/Team.aspx", "Meet The Team")
  TopMenu.AddItem(6, 0, "M", "", "World")
  TopMenu.AddItem(7, 6, "P", "/Places.aspx", "SG Places")
  TopMenu.AddItem(8, 0, "M", "", "Policy Center")
  TopMenu.AddItem(9, 8, "P", "/BankingPage.aspx", "Banking Policy")
  TopMenu.AddItem(10, 8, "P", "/CommunityStandardsPage.aspx", "Community Standards")
  TopMenu.AddItem(11, 8, "P", "/DMCAPage.aspx", "DMCA and EUCD Policies")
  TopMenu.AddItem(12, 8, "P", "/GamblingPage.aspx", "Gambling Policy")
  TopMenu.AddItem(13, 8, "P", "/PrivacyPage.aspx", "Privacy Policy")
  TopMenu.AddItem(14, 8, "P", "/TOSPage.aspx", "Terms of Service")
  TopMenu.AddItem(15, 8, "P", "/TPVPage.aspx", "Third Party Viewer Policy")
  TopMenu.AddItem(16, 0, "M", "", "Support")
  TopMenu.AddItem(17, 16, "P", "/ContactUs.aspx", "Contact Us")
  TopMenu.AddItem(18, 16, "P", "/ViewerDownloads.aspx", "Viewer Downloads")

  ' Place Menu structure into page
  TopMenuBar.InnerHTML = TopMenu.BuildMenu(8)
  TopMenu.Close()
  TopMenu = Nothing

 End Sub

End Class
