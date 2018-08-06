#!Release/Release/bin/sh

mono ../../Release/Release/bin/Prebuild.exe /target nant
mono ../../Release/Release/bin/Prebuild.exe /target monodev
mono ../../Release/Release/bin/Prebuild.exe /target vs2005
