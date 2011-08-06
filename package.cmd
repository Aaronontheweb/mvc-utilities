if not exist Download mkdir Download
if not exist Download\package mkdir Download\package
if not exist Download\package\lib mkdir Download\package\lib
if not exist Download\package\lib\net4 mkdir Download\package\lib\net4

tools\ilmerge.exe /lib:MVC.Utilities\bin\Release /internalize /ndebug /v2 /out:Download\MVC.Utilities.dll MVC.Utilities.dll DevOne.Security.Cryptography.BCrypt.dll Microsoft.ApplicationServer.Caching.Client.dll Microsoft.ApplicationServer.Caching.Core.dll Microsoft.Web.DistributedCache.dll Microsoft.WindowsFabric.Common.dll Microsoft.WindowsFabric.Data.Common.dll

copy LICENSE.txt Download

copy MVC.Utilities\bin\Release\MVC.Utilities.dll Download\Package\lib\net4\

nuget.exe pack mvc-utilities.nuspec -b Download\Package -o Download