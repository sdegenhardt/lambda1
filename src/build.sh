dotnet build -c Release /p:DebugType=None /p:DebugSymbols=false
# dotnet publish -c Release -r linux-x64 -p:PublishSingleFile=true -p:PublishTrimmed=true -p:DebugType=None -p:DebugSymbols=false --self-contained false
# trimmed

<PropertyGroup>
  <OutputType>Exe</OutputType>
  <TargetFramework>net8.0</TargetFramework>
  <PublishSingleFile>true</PublishSingleFile>
  <SelfContained>true</SelfContained>
  <RuntimeIdentifier>linux-x64</RuntimeIdentifier>
</PropertyGroup>

# Single file apps are always OS and architecture specific. 
# You need to publish for each configuration, such as 
# Linux x64, Linux Arm64, Windows x64, and so forth.

#dotnet publish -c Release -r linux-x64 -p:PublishSingleFile=false -p:PublishTrimmed=false -p:DebugType=None -p:DebugSymbols=false --self-contained false -p:PublishReadyToRun=true
# 110005969
#  41333202