$acl = Get-Acl "..\Zoey.Quartz\bin\Debug\netcoreapp3.1"
$aclRuleArgs = "", "Read,Write,ReadAndExecute", "ContainerInherit,ObjectInherit", "None", "Allow"
$accessRule = New-Object System.Security.AccessControl.FileSystemAccessRule($aclRuleArgs)
$acl.SetAccessRule($accessRule)
$acl | Set-Acl "..\Zoey.Quartz\Zoey.Quartz\bin\Debug\netcoreapp3.1"

New-Service -Name "定时任务" -BinaryPathName "..\Zoey.Quartz.exe" -Credential "" -Description "描述" -DisplayName "定时任务" -StartupType Automatic