@echo off

if "%1" NEQ "" (
  if not exist "..\Release.Bin\%1\flexadmin" md "..\Release.Bin\%1\flexadmin"

  echo Copying rdl files
  del /Q "..\Release.Bin\%1\flexadmin\resource\rdl\*.*"
  xcopy "..\..\FlexiNetV2\FlexiNet\FlexiAdmin.Reports\*.rdl" "..\Release.Bin\%1\flexadmin\resource\rdl" /Y /EXCLUDE:copy_excluderdl.txt
  
  echo Copying update files
  xcopy "..\schema\upgrades\Released" "..\Release.Bin\%1\flexadmin\resource\schema\upgrades" /S /Y

  echo Copying schema file
  copy "..\schema\xml\*_%1.xml" "..\Release.Bin\%1\flexadmin\resource\schema" /Y

  echo Copying Development files to the Release\%1 folder
  xcopy  ..\Develop.Bin "..\Release.Bin\%1\flexadmin" /y /e /exclude:copy_excludesys.txt

  pushd ".\Release\%1\flexadmin"
  call ..\Develop.Bin\fncrc.exe ..\Release.Bin\%1
  popd
) else (
  echo No target folder specified
)