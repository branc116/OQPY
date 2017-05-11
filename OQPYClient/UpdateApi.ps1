Get-ChildItem  | Where-Object -Property Name -NotLike "*.csproj*" | Where-Object -Property Name -NotLike "Update*" | Remove-Item -Recurse -Force -WhatIf;
Get-ChildItem  | Where-Object -Property Name -NotLike "*.csproj*" | Where-Object -Property Name -NotLike "Update*" | Remove-Item -Recurse -Force;

Set-Location ./../OQPYManager/;
dotnet.exe build;
# Write-Output "Build skipped";

Start-Process -FilePath dotnet -ArgumentList "run";
Write-Output "process started, waiting 20 seconds for it to warm up";

Start-Sleep -Seconds 20;
Set-Location ./../OQPYClient;
Write-Output "Changed location to: ${Get-Location}";
$curNum = Get-ChildItem -File | Where-Object -Property Name -Like "api*.json" | ForEach-Object -Begin {$i = 0} -Process {$i = $i + 1} -End {$i};
Write-Output "Number of current file is ${curNum}";

$curName = "apiv03.json";
Write-Output "Name of the current json file is ${curName}";

Invoke-WebRequest -Uri http://localhost:5000/swagger/v1/swagger.json -OutFile "./../OQPYClient/${curName}";
Write-Output "Download finished!";

autorest -Input $curName -CodeGeneratioMode CSharp -Namespace "OQPYClient.APIv03" -ModelsName OQPYModels.Models.CoreModels -OutputDirectory "./APIv03";
Write-Output "Generation finished";
$modelsLocation = "./APIv03/OQPYModels.Models.CoreModels";
Remove-Item $modelsLocation -Force -Recurse;
Write-Output "Removed models ${modelsLocation}";

Get-Process | Where-Object -Property Name -Like "dotnet" | Stop-Process
Write-Output "Process stopped";

dotnet restore;
dotnet build;