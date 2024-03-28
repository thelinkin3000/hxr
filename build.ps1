dotnet publish -c Release -r win-x64 --self-contained /p:PublishSingleFile=true
cp "bin/Release/net6.0/win-x64/publish/hxr.exe" ~/scoop/shims/
