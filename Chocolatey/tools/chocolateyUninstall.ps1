$instpth = "$(Split-Path -parent $MyInvocation.MyCommand.Definition)"
Remove-Item $instpth\Mahou.exe
Remove-Item $instpth\jkl.dll -Force
Remove-Item $instpth\jklx86.dll -Force
Remove-Item $instpth\jkl.exe -Force
Remove-Item $instpth\jklx86.exe -Force
$confirmation = Read-Host "Delete Mahou settings, history and snippets?[y/n]"
if ($confirmation -eq 'y' -And $confirmation -eq 'Y') {
  Remove-Item $instpth\Mahou.ini -Force
  Remove-Item $instpth\snippets.txt -Force
  Remove-Item $instpth\history.txt -Force
}