..\..\..\Release\Release\bin\flatc-win.exe --csharp Vector3.fbs
..\..\..\Release\Release\bin\flatc-win.exe --csharp Quaternion.fbs
..\..\..\Release\Release\bin\flatc-win.exe --csharp HalcyonPrimitiveBaseShape.fbs
..\..\..\Release\Release\bin\flatc-win.exe --csharp HalcyonPrimitive.fbs
..\..\..\Release\Release\bin\flatc-win.exe --csharp HalcyonGroup.fbs

move /Y .\Enhanced\Arbiter\Serialization\* ..\Serialization
rmdir /S/Q .\InWorldz
