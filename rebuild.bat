SET start=%DATE% %TIME%

"C:\Windows\Microsoft.NET\Framework64\v4.0.30319\msbuild.exe" MC.sln /t:clean /p:Configuration=Debug
"C:\Windows\Microsoft.NET\Framework64\v4.0.30319\msbuild.exe" MC.sln /p:Configuration=Debug

SET end=%DATE% %TIME%
ECHO Start: %start%
ECHO   End: %end%