if not exist Download mkdir Download
if not exist Download\package mkdir Download\package
if not exist Download\package\lib mkdir Download\package\lib
if not exist Download\package\lib\net4 mkdir Download\package\lib\net4

copy LICENSE.txt Download

copy MVC.Utilities\bin\Release\MVC.Utilities.dll Download\Package\lib\net4\

nuget.exe pack mvc-utilities.nuspec -b Download\Package -o Download