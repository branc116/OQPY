Set-Location ./../OQPYManager/;
# dotnet.exe build;
Write-Output "Build skipped";

Start-Process -FilePath dotnet -ArgumentList "run";
Write-Output "process started, waiting 15 seconds for it to warm up";

Start-Sleep -Seconds 15;
Set-Location ./../OQPYClient;
Write-Output "Changed location to: ${Get-Location}";
$curNum = Get-ChildItem -File | Where-Object -Property Name -Like "api*.json" | ForEach-Object -Begin {$i = 0} -Process {$i = $i + 1} -End {$i};
Write-Output "Number of current file is ${curNum}";

$curName = "apiv0${curNum}.json";
Write-Output "Name of the current json file is ${curName}";

Invoke-WebRequest -Uri http://localhost:5000/swagger/v1/swagger.json -OutFile "./../OQPYClient/${curName}";
Write-Output "Download finished!";

autorest -Input $curName -CodeGeneratioMode CSharp -Namespace "OQPYClient.APIv03" -ModelsName OQPYModels.Models.CoreModels -OutputDirectory "./APIv0${curNum}";
Write-Output "Generation finished";
$modelsLocation = "./APIv0${curNum}/OQPYModels.Models.CoreModels";
Remove-Item $modelsLocation -Force -Recurse;
Write-Output "Removed models ${modelsLocation}";

Get-Process | Where-Object -Property Name -Like "dotnet" | Stop-Process
Write-Output "Process stopped";

Get-ChildItem -Directory | Where-Object -Property Name -NE "APIv0${curNum}" | Remove-Item -Recurse -Force -WhatIf;
Get-ChildItem -Directory | Where-Object -Property Name -NE "APIv0${curNum}" | Remove-Item -Recurse -Force;

dotnet restore;
dotnet build;