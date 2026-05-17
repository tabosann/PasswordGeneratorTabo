dotnet publish -p:PublishProfile=NoNativeAOT
pushd 'bin\x64\Release_Unpackaged\net10.0-windows10.0.26100.0\win-x64\publish\PasswordGeneratorTabo'
Compress-Archive -Path '.\*' -DestinationPath '..\..\..\..\net10.0-windows10.0.26100.0-win-x64.zip' -Force
popd