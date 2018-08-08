
Partial Class Heartbeat
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
 '* This page is the core process to track grid status and tracking by keeping the MySite.RegionXML
 '* table updated with the status of active regions. It is to be run on a timer triggered cycle 
 '* about every 15 minutes.
 '* Status: 0=Offline, 1=Starting, 2=Online, 3=Closing

 '* Built from Website Basic Page template v. 1.0

 ' Define common properties and objects here for the page
 Private MyDB As New MySQLLib                              ' Provides data access methods and error handling
 Private SQLCmd As String

 Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load

  ' Define process unique objects here
  Dim tLogOut, tFilePath As String
  tLogOut = ""
  Dim startTime As DateTime
  startTime = Date.Now()

  Dim tLog As Boolean
  tLog = True
  ' Log file output setup
  Dim sw As System.IO.StreamWriter

  If tLog Then                                             ' Log file output
   tFilePath = Server.MapPath("").ToString()
   sw = System.IO.File.AppendText(tFilePath.ToString() + "\Heartbeat.log")
   sw.WriteLine("Heartbeat Start: " + FormatDateTime(startTime, DateFormat.GeneralDate))
   sw.Flush()
  End If

  ' Outline: 
  ' Get list of active regions from the MyData.region_settings table, and current status from the MyData.regions table
  ' and check with the running region for live validation, update the MySite.regionxml table with the results, 
  ' adding any missing regions found.

  ' Get list of active regions
  Dim SQLFields, SQLValues As String
  Dim PrimMax, RegCount As Integer
  PrimMax = 0
  RegCount = 0

  Dim ChkRegionXML As MySql.Data.MySqlClient.MySqlDataReader
  SQLFields = "UUID,regionName,status,locationX,locationY,internalIP,port,externalIP,ownerUUID,lastmapUUID," +
              "lastmapRefresh,primMax,physicalMax,productType"

  Dim GetList As MySql.Data.MySqlClient.MySqlDataReader
  SQLCmd = "Select A.RegionUUID as UUID,B.regionName,B.locx as locationX,B.locy as locationY,B.product as productType, " +
           " Case When B.regionName is null Then 0 Else 1 End as Status,B.serverIP as externalIP," +
           " B.serverPort as port,B.owner_uuid as ownerUUID,B.regionMapTexture as lastmapUUID " +
           "From regionsettings A Left Join regions B on A.RegionUUID=B.UUID " +
           "Order by externalIP,port"
  If False And tLog Then                                   ' Log file output
   sw.WriteLine("Get Estate Regions SQLCmd: " + SQLCmd.ToString())
   sw.Flush()
  End If
  GetList = MyDB.GetReader("MyData", SQLCmd)
  If MyDB.Error() And tLog Then                            ' Log file output
   sw.WriteLine("DB Error: " + MyDB.ErrMessage().ToString())
   sw.Flush()
  End If
  If GetList.HasRows() Then
   While GetList.Read()
    RegCount = RegCount + 1
    SQLCmd = "Select UUID,regionName,status,locationX,locationY,internalIP,port,externalIP,ownerUUID,lastmapUUID," +
             "lastmapRefresh,primMax,productType " +
             "From regionxml " +
             "Where UUID=" + MyDB.SQLStr(GetList("UUID"))
    If False And tLog Then                                 ' Log file output
     sw.WriteLine("Check RegionXML SQLCmd: " + SQLCmd.ToString())
     sw.Flush()
    End If
    ChkRegionXML = MyDB.GetReader("MySite", SQLCmd)
    If MyDB.Error() And tLog Then                          ' Log file output
     sw.WriteLine("DB Error: " + MyDB.ErrMessage().ToString())
     sw.Flush()
    End If
    If ChkRegionXML.HasRows() Then                         ' Record Exists, Update it
     ChkRegionXML.Read()
     Dim ReturnInfo, IP As String
     ReturnInfo = ""
     If GetList("Status") = 1 Then                         ' Verify If region is online
      ' Ping server region for status
      ' Can use internal network address: http://10.0.0.30:9518/simstatus/ for communication with regions.
      If ChkRegionXML("internalIP").ToString().Trim().Length > 0 Then ' Use the internalIP if it has one
       IP = ChkRegionXML("internalIP").ToString()
      Else
       IP = ChkRegionXML("externalIP").ToString()
      End If
      Dim response As System.Net.WebResponse = Nothing
      Dim reader As System.IO.StreamReader = Nothing
      Dim requestWeb As System.Net.WebRequest
      Dim responseStream As System.IO.Stream
      If tLog Then                                ' Log file output
       sw.WriteLine("Ask Message: " + "http://" + IP.ToString() + ":" + ChkRegionXML("port").ToString() + "/simstatus/")
       sw.Flush()
      End If
      requestWeb = System.Net.WebRequest.Create("http://" + IP.ToString() + ":" + ChkRegionXML("port").ToString() + "/simstatus/")
      requestWeb.Timeout = 500                             ' Half second response timeout
      Try
       response = requestWeb.GetResponse()
       responseStream = response.GetResponseStream()
       reader = New System.IO.StreamReader(responseStream)
       ReturnInfo = reader.ReadToEnd()                     ' Get response text
       response.Close()
       reader.Close()
      Catch ex As Exception
       ReturnInfo = ""
      End Try
      requestWeb = Nothing
     End If
     If tLog Then                                ' Log file output
      sw.WriteLine("--Response: " + ReturnInfo.ToString())
      sw.Flush()
     End If
     If ReturnInfo.ToString() = "OK" Then                  ' Region responded correctly, its online
      SQLCmd = "status=2"                                  ' Online
      ' Update the region settings only if the record had been unknown before with invalid information.
      If GetList("regionName") = "Unknown" And GetList("regionName") <> ChkRegionXML("regionName") Then
       SQLCmd = SQLCmd.ToString() + ",regionName=" + MyDB.SQLStr(GetList("regionName"))
      End If
      If GetList("LocationX") = -1 And GetList("LocationX") <> ChkRegionXML("LocationX") Then
       SQLCmd = SQLCmd.ToString() + ",LocationX=" + MyDB.SQLNo(GetList("LocationX"))
      End If
      If GetList("LocationY") = -1 And GetList("LocationY") <> ChkRegionXML("LocationY") Then
       SQLCmd = SQLCmd.ToString() + ",LocationY=" + MyDB.SQLStr(GetList("LocationY"))
      End If
      If GetList("port") = 0 And GetList("port") <> ChkRegionXML("port") Then
       SQLCmd = SQLCmd.ToString() + ",port=" + MyDB.SQLStr(GetList("port"))
      End If
      If GetList("externalIP") = "0.0.0.0" And GetList("externalIP") <> ChkRegionXML("externalIP") Then
       SQLCmd = SQLCmd.ToString() + ",externalIP=" + MyDB.SQLStr(GetList("externalIP"))
      End If
      If GetList("ownerUUID") = "00000000-0000-0000-0000-000000000000" And GetList("ownerUUID") <> ChkRegionXML("ownerUUID") Then
       SQLCmd = SQLCmd.ToString() + ",ownerUUID=" + MyDB.SQLStr(GetList("ownerUUID"))
      End If
      If GetList("productType") = 0 And GetList("productType") <> ChkRegionXML("productType") Then
       SQLCmd = SQLCmd.ToString() + ",productType=" + MyDB.SQLStr(GetList("productType"))
      End If
      If ChkRegionXML("primMax") = 0 And GetList("productType") > 0 Then
       If GetList("productType") = 1 Then                  ' Full Region
        PrimMax = 30000
       ElseIf GetList("productType") = 2 Then              ' Open space
        PrimMax = 7500
       ElseIf GetList("productType") = 3 Then              ' Homestead
        PrimMax = 15000
       End If
       SQLCmd = SQLCmd.ToString() + ",primMax=" + MyDB.SQLStr(PrimMax)
      End If
      If GetList("lastmapUUID") = "00000000-0000-0000-0000-000000000000" And GetList("lastmapUUID") <> ChkRegionXML("lastmapUUID") Then
       SQLCmd = SQLCmd.ToString() + ",lastmapUUID=" + MyDB.SQLStr(GetList("lastmapUUID"))
      End If
     Else                                                  ' No Response OR GetList("Status") = 0
      If ChkRegionXML("Status") = 2 Then                   ' It is offline.
       SQLCmd = SQLCmd.ToString() + "Status=0"
      Else                                                 ' Check Status
       If ChkRegionXML("Status") <> 0 Then                 ' Retain prior Status: it may be starting up or shutting down.
        SQLCmd = SQLCmd.ToString() + "Status=" + MyDB.SQLStr(ChkRegionXML("Status"))
       End If
      End If
     End If
     SQLCmd = "Update regionxml Set " + SQLCmd.ToString() + " Where UUID=" + MyDB.SQLStr(GetList("UUID"))
     If False And tLog Then                                ' Log file output
      sw.WriteLine("Update regionxml SQLCmd: " + SQLCmd.ToString())
      sw.Flush()
     End If
    Else                                                   ' RegXML record is missing, Add it
     If GetList("Status") = 1 Then                         ' Has a Region record
      If GetList("productType") = 1 Then                   ' Full Region
       PrimMax = 30000
      ElseIf GetList("productType") = 2 Then               ' Open space
       PrimMax = 7500
      ElseIf GetList("productType") = 3 Then               ' Homestead
       PrimMax = 15000
      End If
      'SQLFields = "UUID,regionName,status,locationX,locationY,internalIP,port,externalIP,ownerUUID,lastmapUUID," +
      '            "lastmapRefresh,primMax,physicalMax,productType"
      SQLValues = MyDB.SQLStr(GetList("UUID")) + "," + MyDB.SQLStr(GetList("regionName")) + "," +
      MyDB.SQLNo(GetList("status")) + "," + MyDB.SQLNo(GetList("LocationX")) + "," +
                  MyDB.SQLNo(GetList("LocationY")) + "," + MyDB.SQLStr("0.0.0.0") + "," +
                  MyDB.SQLNo(GetList("port")) + "," + MyDB.SQLStr(GetList("externalIP")) + "," +
                  MyDB.SQLStr(GetList("ownerUUID")) + "," + MyDB.SQLStr(GetList("lastmapUUID")) + ",0," +
                  MyDB.SQLNo(PrimMax) + ",0," + MyDB.SQLStr(GetList("productType"))
     Else                                                  ' No idea what the region is supposed to be, Only has a regionsettings record.
      SQLValues = MyDB.SQLStr(GetList("UUID")) + ",'Unknown',0,-1,-1,'0.0.0.0',0,'0.0.0.0'," +
                  "'00000000-0000-0000-0000-000000000000','00000000-0000-0000-0000-000000000000',0,0,0,0"
     End If
     SQLCmd = "Insert Into regionxml (" + SQLFields + ") Values (" + SQLValues + ")"
     If False And tLog Then                                ' Log file output
      sw.WriteLine("Insert regionxml SQLCmd: " + SQLCmd.ToString())
      sw.Flush()
     End If
    End If
    MyDB.DBCmd("MySite", SQLCmd)
    If MyDB.Error() And tLog Then                          ' Log file output
     sw.WriteLine("DB Error: " + MyDB.ErrMessage().ToString())
     sw.Flush()
    End If
    ChkRegionXML.Close()
   End While
  End If
  GetList.Close()
  If tLog Then                                             ' Log file output
   sw.WriteLine("Regions Processed: " + RegCount.ToString() + " in " + DateDiff(DateInterval.Second, Date.Now(), startTime).ToString() + " seconds.")
   sw.WriteLine("********* Completed Processing *********" + vbCrLf)
   sw.Flush()
   sw.Close()
  End If
  ' Problem: Google Chrome now thinks this is a security problem and disallows it. There is no alternative to closing a browser.
  Response.Write("<html><head></head><body onload=""window.close();""></body></html>") ' close window and its process

 End Sub

 Private Sub Page_Unload(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.Unload
  ' Close open page objects
  MyDB.Close()
  MyDB = Nothing
 End Sub
End Class
