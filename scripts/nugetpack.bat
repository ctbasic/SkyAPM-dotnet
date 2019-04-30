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

dotnet pack -c Release --force  -o %projectRootPath%/LocalDebugPackage %projectRootPath%/src/SkyApm.Agent.AspNetCore/SkyApm.Agent.AspNetCore.csproj

dotnet pack -c Release --force  -o %projectRootPath%/LocalDebugPackage %projectRootPath%/src/SkyApm.Agent.NetCoreHost/SkyApm.Agent.NetCoreHost.csproj

dotnet pack -c Release --force  -o %projectRootPath%/LocalDebugPackage %projectRootPath%/src/SkyApm.Soap/SkyApm.Soap.csproj

dotnet pack -c Release --force  -o %projectRootPath%/LocalDebugPackage %projectRootPath%/src/SkyApm.Thrift/SkyApm.Thrift.csproj
pause