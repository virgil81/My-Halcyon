#!Release/bin/sh

mono ../../Release/bin/Prebuild.exe /target nant
mono ../../Release/bin/Prebuild.exe /target monodev
mono ../../Release/bin/Prebuild.exe /target vs2005
