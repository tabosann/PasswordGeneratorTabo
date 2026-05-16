pushd 'bin\x64\Release\net10.0-windows10.0.26100.0\win-x64\publish\win-x64'
Get-ChildItem -Directory -Exclude 'en-us','ja-JP' | ?{ Test-Path "$_\Microsoft.ui.xaml.dll.mui" } | rm -Recurse
Compress-Archive -Path '.\*' -DestinationPath '..\..\..\..\net10.0-windows10.0.26100.0-win-x64.zip'
popd
