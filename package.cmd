if not exist Download mkdir Download
if not exist Download\packages mkdir Download\packages
if not exist Download\packages\MVC.Utilities mkdir Download\packages\MVC.Utilities
if not exist Download\packages\MVC.Utilities\lib mkdir Download\packages\MVC.Utilities\lib
if not exist Download\packages\MVC.Utilities\lib\net4 mkdir Download\packages\MVC.Utilities\lib\net4
if not exist Download\packages\MVC.Utilities.Azure mkdir Download\packages\MVC.Utilities.Azure
if not exist Download\packages\MVC.Utilities.Azure\lib mkdir Download\packages\MVC.Utilities.Azure\lib
if not exist Download\packages\MVC.Utilities.Azure\lib\net4 mkdir Download\packages\MVC.Utilities.Azure\lib\net4
if not exist Download\packages\MVC.Utilities.BCrypt mkdir Download\packages\MVC.Utilities.BCrypt
if not exist Download\packages\MVC.Utilities.BCrypt\lib mkdir Download\packages\MVC.Utilities.BCrypt\lib
if not exist Download\packages\MVC.Utilities.BCrypt\lib\net4 mkdir Download\packages\MVC.Utilities.BCrypt\lib\net4

copy LICENSE.txt Download\packages\MVC.Utilities
copy LICENSE.txt Download\packages\MVC.Utilities.Azure
copy LICENSE.txt Download\packages\MVC.Utilities.BCrypt

copy MVC.Utilities\bin\Release\MVC.Utilities.dll Download\packages\MVC.Utilities\lib\net4
copy MVC.Utilities.Azure\bin\Release\MVC.Utilities.Azure.dll Download\packages\MVC.Utilities.Azure\lib\net4
copy MVC.Utilities.BCrypt\bin\Release\MVC.Utilities.BCrypt.dll Download\packages\MVC.Utilities.BCrypt\lib\net4

nuget.exe pack mvc-utilities.nuspec -BasePath Download\packages\MVC.Utilities -OutputDirectory Download
nuget.exe pack mvc-utilities-azure.nuspec -BasePath Download\packages\MVC.Utilities.Azure -OutputDirectory Download
nuget.exe pack mvc-utilities-bcrypt.nuspec -BasePath Download\packages\MVC.Utilities.BCrypt -OutputDirectory Download