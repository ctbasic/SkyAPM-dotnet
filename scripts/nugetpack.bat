@echo off
set projectRootPath=F:\SkyAPM-dotnet



dotnet pack -c Release --force  -o %projectRootPath%/LocalDebugPackage %projectRootPath%/src/SkyApm.Agent.AspNetCore/SkyApm.Agent.AspNetCore.csproj



pause