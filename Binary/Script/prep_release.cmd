@echo off

if [%1] EQU [] goto usage

setlocal

set RELEASEFOLDER=c:\Release\%1

del .\C#\Build\C1.*.xml > nul
del .\C#\Build\Infragistics2.*.xml > nul
del .\C#\Build\Report.*.resx > nul

md "%RELEASEFOLDER%\output"
md "%RELEASEFOLDER%\output\external"
md "%RELEASEFOLDER%\output\framework"
md "%RELEASEFOLDER%\output\module"
md "%RELEASEFOLDER%\output\report"
md "%RELEASEFOLDER%\output\resource"
md "%RELEASEFOLDER%\output\resource\rdl"
md "%RELEASEFOLDER%\output\clr"

copy .\C#\Build\FlexiNet.exe "%RELEASEFOLDER%\output"
copy .\C#\Build\FlexiNet.exe.config "%RELEASEFOLDER%\output"

copy .\C#\Build\framework\FlexAdmin.Core.dll "%RELEASEFOLDER%\output\framework"
copy .\C#\Build\framework\FlexiNet.Common.dll "%RELEASEFOLDER%\output\framework"
copy .\C#\Build\framework\FlexiNet.Controls.dll "%RELEASEFOLDER%\output\framework"
copy .\C#\Build\framework\FlexiNet.Data.MSSQL.dll "%RELEASEFOLDER%\output\framework"
copy .\C#\Build\framework\FlexiNet.Data.MSSQL.Regulatory.dll "%RELEASEFOLDER%\output\framework"
copy .\C#\Build\framework\FlexiNet.Interface.dll "%RELEASEFOLDER%\output\framework"
copy .\C#\Build\framework\FlexiNet.Resources.dll "%RELEASEFOLDER%\output\framework"
copy .\C#\Build\framework\Hadz369.Controls.SelectDatabase.MSSQL.dll "%RELEASEFOLDER%\output\framework"

copy .\C#\Build\module\FlexiNet.Module.MachineManager.dll "%RELEASEFOLDER%\output\module"
copy .\C#\Build\module\FlexiNet.Module.UserRights.dll "%RELEASEFOLDER%\output\module"
rem copy .\C#\Build\module\FlexiNet.Module.MemberUtility.dll "%RELEASEFOLDER%\output\module"

copy .\C#\Build\report\FlexAdmin.Report.AuditTrail.dll "%RELEASEFOLDER%\output\report"
copy .\C#\Build\report\FlexAdmin.Report.CashierFloat.dll "%RELEASEFOLDER%\output\report"
copy .\C#\Build\report\FlexAdmin.Report.LinkReconcilliation.dll "%RELEASEFOLDER%\output\report"
copy .\C#\Build\report\FlexAdmin.Report.MoneyInVariance.dll "%RELEASEFOLDER%\output\report"
copy .\C#\Build\report\FlexAdmin.Report.MoneyOutVariance.dll "%RELEASEFOLDER%\output\report"
copy .\C#\Build\report\FlexAdmin.Report.PlayerActivityStatement.dll "%RELEASEFOLDER%\output\report"
copy .\C#\Build\report\FlexAdmin.Report.RedeemedTickets.dll "%RELEASEFOLDER%\output\report"
copy .\C#\Build\report\FlexAdmin.Report.TicketReconcilliation.dll "%RELEASEFOLDER%\output\report"
copy .\C#\Build\report\FlexAdmin.Report.UnclaimedTicketMgmt.dll "%RELEASEFOLDER%\output\report"
copy .\C#\Build\report\FlexAdmin.Report.UnclaimedTickets.dll "%RELEASEFOLDER%\output\report"

copy "C:\project\FlexiNet\FlexiNet.Reports\Audit Trail Report.rdl" "%RELEASEFOLDER%\output\resource\rdl"
copy "C:\project\FlexiNet\FlexiNet.Reports\Cashier Float.rdl" "%RELEASEFOLDER%\output\resource\rdl"
copy "C:\project\FlexiNet\FlexiNet.Reports\Link Reconcilliation Report.rdl" "%RELEASEFOLDER%\output\resource\rdl"
copy "C:\project\FlexiNet\FlexiNet.Reports\Money In Variance.rdl" "%RELEASEFOLDER%\output\resource\rdl"
copy "C:\project\FlexiNet\FlexiNet.Reports\Money Out Variance.rdl" "%RELEASEFOLDER%\output\resource\rdl"
copy "C:\project\FlexiNet\FlexiNet.Reports\Player Activity Statement 1.0.2.5.rdl" "%RELEASEFOLDER%\output\resource\rdl"
copy "C:\project\FlexiNet\FlexiNet.Reports\Redeemed Tickets.rdl" "%RELEASEFOLDER%\output\resource\rdl"
copy "C:\project\FlexiNet\FlexiNet.Reports\Ticket Reconcilliation Report.rdl" "%RELEASEFOLDER%\output\resource\rdl"
copy "C:\project\FlexiNet\FlexiNet.Reports\Unclaimed Tickets.rdl" "%RELEASEFOLDER%\output\resource\rdl"

copy c:\flexadmin\development\external\C1.C1Excel.2.dll "%RELEASEFOLDER%\output\external"
copy c:\flexadmin\development\external\C1.Win.C1Command.2.dll "%RELEASEFOLDER%\output\external"
copy c:\flexadmin\development\external\C1.Win.C1Editor.2.dll "%RELEASEFOLDER%\output\external"
copy c:\flexadmin\development\external\C1.Win.C1FlexGrid.2.dll "%RELEASEFOLDER%\output\external"
copy c:\flexadmin\development\external\C1.Win.C1Gauge.2.dll "%RELEASEFOLDER%\output\external"
copy c:\flexadmin\development\external\C1.Win.C1Input.2.dll "%RELEASEFOLDER%\output\external"
copy c:\flexadmin\development\external\C1.Win.C1InputPanel.2.dll "%RELEASEFOLDER%\output\external"
copy c:\flexadmin\development\external\C1.Win.C1List.2.dll "%RELEASEFOLDER%\output\external"
copy c:\flexadmin\development\external\C1.Win.C1Ribbon.2.dll "%RELEASEFOLDER%\output\external"
copy c:\flexadmin\development\external\Infragistics2.Shared.v10.3.dll "%RELEASEFOLDER%\output\external"
copy c:\flexadmin\development\external\Infragistics2.Win.Misc.v10.3.dll "%RELEASEFOLDER%\output\external"
copy c:\flexadmin\development\external\Infragistics2.Win.UltraWinGrid.v10.3.dll "%RELEASEFOLDER%\output\external"
copy c:\flexadmin\development\external\Infragistics2.Win.UltraWinEditors.v10.3.dll "%RELEASEFOLDER%\output\external"
copy c:\flexadmin\development\external\Infragistics2.Win.UltraWinListView.v10.3.dll "%RELEASEFOLDER%\output\external"
copy c:\flexadmin\development\external\Infragistics2.Win.v10.3.dll "%RELEASEFOLDER%\output\external"
copy c:\flexadmin\development\external\Infragistics3.Excel.v10.3.dll "%RELEASEFOLDER%\output\external"
copy c:\flexadmin\development\external\Microsoft.ReportViewer.Common.dll "%RELEASEFOLDER%\output\external"
copy c:\flexadmin\development\external\Microsoft.ReportViewer.ProcessingObjectModel.dll "%RELEASEFOLDER%\output\external"
copy c:\flexadmin\development\external\Microsoft.ReportViewer.WinForms.dll "%RELEASEFOLDER%\output\external"

copy c:\flexadmin\development\clr\FlexiNet.CLR.Regulatory.dll "%RELEASEFOLDER%\output\clr"

pause

rem xcopy .\C#\Build "%RELEASEFOLDER%\output" /Y /E /I /EXCLUDE:prep_exclude1.txt

xcopy .\C#\report\FlexiNet.Report.AuditTrail "%RELEASEFOLDER%\source\report\FlexiNet.Report.AuditTrail"" /Y /E /I /EXCLUDE:prep_exclude2.txt
xcopy .\C#\report\FlexAdmin.Report.CashierFloat "%RELEASEFOLDER%\source\report\FlexAdmin.Report.CashierFloat" /Y /E /I /EXCLUDE:prep_exclude2.txt
xcopy .\C#\report\FlexAdmin.Report.LinkReconcilliation "%RELEASEFOLDER%\source\report\FlexAdmin.Report.LinkReconcilliation" /Y /E /I /EXCLUDE:prep_exclude2.txt
xcopy .\C#\report\FlexAdmin.Report.MoneyInVariance "%RELEASEFOLDER%\source\report\FlexAdmin.Report.MoneyInVariance" /Y /E /I /EXCLUDE:prep_exclude2.txt
xcopy .\C#\report\FlexAdmin.Report.MoneyOutVariance "%RELEASEFOLDER%\source\report\FlexAdmin.Report.MoneyOutVariance" /Y /E /I /EXCLUDE:prep_exclude2.txt
xcopy .\C#\report\FlexAdmin.Report.PlayerActivityStatement "%RELEASEFOLDER%\source\report\FlexAdmin.Report.PlayerActivityStatement" /Y /E /I /EXCLUDE:prep_exclude2.txt
xcopy .\C#\report\FlexAdmin.Report.RedeemedTickets "%RELEASEFOLDER%\source\report\FlexAdmin.Report.RedeemedTickets" /Y /E /I /EXCLUDE:prep_exclude2.txt
xcopy .\C#\report\FlexAdmin.Report.TicketReconcilliation "%RELEASEFOLDER%\source\report\FlexAdmin.Report.TicketReconcilliation" /Y /E /I /EXCLUDE:prep_exclude2.txt
xcopy .\C#\report\FlexAdmin.Report.UnclaimedTicketMgmt "%RELEASEFOLDER%\source\report\FlexAdmin.Report.UnclaimedTicketMgmt" /Y /E /I /EXCLUDE:prep_exclude2.txt
xcopy .\C#\report\FlexAdmin.Report.UnclaimedTickets "%RELEASEFOLDER%\source\report\FlexAdmin.Report.UnclaimedTickets" /Y /E /I /EXCLUDE:prep_exclude2.txt

xcopy .\C#\application\FlexiNet "%RELEASEFOLDER%\source\application\FlexiNet" /Y /E /I /EXCLUDE:prep_exclude2.txt

xcopy .\C#\clr\FlexiNet.CLR.Regulatory "%RELEASEFOLDER%\source\clr\FlexiNet.CLR.Regulatory" /Y /E /I /EXCLUDE:prep_exclude2.txt
pause

xcopy .\C#\control\FlexiNet.Controls.Common "%RELEASEFOLDER%\source\control\FlexiNet.Controls.Common" /Y /E /I /EXCLUDE:prep_exclude2.txt

xcopy .\C#\class\FlexiNet.Common "%RELEASEFOLDER%\source\class\FlexiNet.Common" /Y /E /I /EXCLUDE:prep_exclude2.txt
xcopy .\C#\class\FlexiNet.Data.MSSQL "%RELEASEFOLDER%\source\class\FlexiNet.Data.MSSQL" /Y /E /I /EXCLUDE:prep_exclude2.txt
xcopy .\C#\class\FlexiNet.Data.MSSQL.Regulatory "%RELEASEFOLDER%\source\class\FlexiNet.Data.MSSQL.Regulatory" /Y /E /I /EXCLUDE:prep_exclude2.txt
xcopy .\C#\class\FlexiNet.Resources "%RELEASEFOLDER%\source\class\FlexiNet.Resources" /Y /E /I /EXCLUDE:prep_exclude2.txt

xcopy .\C#\interface "%RELEASEFOLDER%\source\interface" /Y /E /I /EXCLUDE:prep_exclude2.txt

xcopy .\C#\module\FlexiNet.Module.MachineManager "%RELEASEFOLDER%\source\module\FlexiNet.Module.MachineManager" /Y /E /I /EXCLUDE:prep_exclude2.txt
xcopy .\C#\module\FlexiNet.Module.UserRights     "%RELEASEFOLDER%\source\module\FlexiNet.Module.UserRights" /Y /E /I /EXCLUDE:prep_exclude2.txt

goto end

:usage
echo Error: Missing version parameter
echo.
echo Usage: preprelease [version]
echo    eg: preprelease 1.0.2.5
echo.
pause > nul

:end