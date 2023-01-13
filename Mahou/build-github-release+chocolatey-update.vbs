set fso = CreateObject("Scripting.FileSystemObject")
set shell = WScript.CreateObject("WScript.Shell")
dim mahouWver
dim ver
' Create release
shell.Run "clean.cmd && build.cmd release x86_x64 GITHUB_RELEASE", 1, true
ver = fso.GetFileVersion("bin\Release_x86_x64\Mahou.exe")
mahouWver =  "Mahou-v" & ver
' Create zip of release with MD5(for chocolatey)
shell.Run "cmd.exe /c " & _
 "cd bin\Release_x86_x64 &&" & _ 
 "7z a " & mahouWver  & ".zip" & " -mx=9 Mahou.exe && " &  _
 "cd ..\..\..\JKL && " & _
 "make re zip && " & _
 "7z a ..\Mahou\bin\Release_x86_x64\" & mahouWver & ".zip -mx=9 .\bin\* && " & _
 "cd ..\Mahou\bin\Release_x86_x64 && " & _
 "md5sum " & mahouWver  & ".zip" & " > " & mahouWver & "_MD5.txt", 1, true
' Update chocolatey
dim RegEx
set RegEx = new RegExp
RegEx.Global = true
RegEx.Pattern="<version>.*</version>"
' File path's
nuspec_path = "..\Chocolatey\Mahou.nuspec"
chocoInst_path = "..\Chocolatey\tools\chocolateyInstall.ps1"
md5sum_path = "bin\Release_x86_x64\" & mahouWver & "_MD5.txt"
' Replace nuspec version
set nuf = fso.OpenTextFile(nuspec_path, 1)
nuspec = nuf.ReadAll
repl = RegEx.Replace(nuspec, "<version>" & ver & "</version>")
set nuw = fso.CreateTextFile(nuspec_path, true)
nuw.Write(repl)
nuw.close
' Get MD5 of Zip
set m5f = fso.OpenTextFile(md5sum_path, 1)
md5sum = m5f.ReadAll
RegEx.Pattern="^(\w+) \*"
set md5ms = RegEx.Execute(md5sum)
dim md5
if md5ms.count <> 0 then
	md5 = md5ms.Item(0).submatches.Item(0)
end if
' Replace chocolateyInstall version
set cif = fso.OpenTextFile(chocoInst_path, 1)
chocoInst = cif.ReadAll
RegEx.Pattern="v.+Mahou.+\.zip"
repl = RegEx.Replace(chocoInst, "v" & ver & "/" & mahouWver & ".zip")
' Replace chocolateyInstall md5
RegEx.Pattern="-Checksum .+"
repl = RegEx.Replace(repl, "-Checksum """ & md5 & """")
set ciw = fso.CreateTextFile(chocoInst_path, true)
ciw.Write(repl)
ciw.close
' Pack Mahou.nupkg
shell.Run "cmd.exe /c " & _
"cd ..\Chocolatey && " & _
"cpack", 1, true