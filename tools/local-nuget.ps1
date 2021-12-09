dotnet clean
dotnet build

Remove-Item .\packages\* -Recurse -Force

dotnet pack .\src\AspNetCore\src -o .\packages

Get-ChildItem \NugetServer\BizStream.Kentico.Xperience.AspNetCore.StaticWebAssetsStorage -Recurse | Remove-Item -Recurse -Force -Confirm:$false

nuget init .\packages \NugetServer
