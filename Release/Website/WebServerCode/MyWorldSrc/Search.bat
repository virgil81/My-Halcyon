@ECHO OFF
:* search.bat Parameters: %1 - text to search for.
:* %2 Optional files to search list: *.ASP;*.ASPX;*.ASX  Default is *.ASP
If EXIST SEARCH.TXT DEL SEARCH.TXT
If %1=="?" goto Help
If NOT %2=="" Goto UseList
ECHO Search for %1 in *.ASPX files.
ECHO Search for %1 in *.ASPX files. >>SEARCH.TXT
ECHO >>SEARCH.TXT
FOR /R %%I IN (*.ASPX) DO @FIND /N /I %1 "%%I" >>SEARCH.TXT
GOTO END
:UseList
ECHO Searching for %1 in %2 files.
ECHO Search for %1 in %2 files. >>SEARCH.TXT
ECHO >>SEARCH.TXT
FOR /R %%I IN (%2) DO @FIND /N /I %1 "%%I" >>SEARCH.TXT
GOTO END
:Help
ECHO * First parameter: What to search for with "" around the text. 
ECHO *     The quotes are required.
ECHO * Second parameter: Optional files to search list: *.ASP;*.ASPX;*.ASX  
ECHO *     Default is *.ASPX. May not have any spaces in the list. 
:End
ECHO Search has completed!
