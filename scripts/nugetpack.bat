@echo off
set projectRootPath=E:/github.com/king311247/SkyAPM-dotnet

dotnet pack -c Release --force  -o %projectRootPath%/LocalDebugPackage %projectRootPath%/src/SkyApm.Abstractions/SkyApm.Abstractions.csproj

dotnet pack -c Release --force  -o %projectRootPath%/LocalDebugPackage %projectRootPath%/src/SkyApm.Core/SkyApm.Core.csproj

dotnet pack -c Release --force  -o %projectRootPath%/LocalDebugPackage %projectRootPath%/src/SkyApm.Transport.Grpc/SkyApm.Transport.Grpc.csproj

dotnet pack -c Release --force  -o %projectRootPath%/LocalDebugPackage %projectRootPath%/src/SkyApm.Transport.Grpc.Protocol/SkyApm.Transport.Grpc.Protocol.csproj

dotnet pack -c Release --force  -o %projectRootPath%/LocalDebugPackage %projectRootPath%/src/SkyApm.Utilities.Configuration/SkyApm.Utilities.Configuration.csproj

dotnet pack -c Release --force  -o %projectRootPath%/LocalDebugPackage %projectRootPath%/src/SkyApm.Utilities.DependencyInjection/SkyApm.Utilities.DependencyInjection.csproj

dotnet pack -c Release --force  -o %projectRootPath%/LocalDebugPackage %projectRootPath%/src/SkyApm.Utilities.Logging/SkyApm.Utilities.Logging.csproj

dotnet pack -c Release --force  -o %projectRootPath%/LocalDebugPackage %projectRootPath%/src/SkyApm.Agent.AspNet/SkyApm.Agent.AspNet.csproj
pause