@echo off

set sunburn=Indiefreaks.Game.AI Indiefreaks.Game.Framework Indiefreaks.Game.Instancing Indiefreaks.Game.Lidgren Indiefreaks.Game.Live Indiefreaks.Game.Logic Indiefreaks.Game.Particles Indiefreaks.Game.Physics Indiefreaks.Game.Sprites Indiefreaks.Game.UI 
set other=Indiefreaks.Game.IndieCity Indiefreaks.Game.PostProcess Indiefreaks.Game.Mercury
REM copy Windows Debug binaries
for %%i in (%sunburn%
) do echo Copying %%i... & xcopy source\%%i\bin\x86\Debug\Pro Binaries\Debug\Pro\Windows /E /I /Q /Y & xcopy source\%%i\bin\x86\Debug\Indie Binaries\Debug\Indie\Windows /E /I /Q /Y 

for %%i in (%other%
) do echo Copying %%i... & xcopy source\%%i\bin\x86\Debug Binaries\Debug\Pro\Windows /E /I /Q /Y & xcopy source\%%i\bin\x86\Debug Binaries\Debug\Indie\Windows /E /I /Q /Y

REM copy xbox Debug binaries
for %%i in (%sunburn%
) do echo Copying %%i... & xcopy "source\%%i\bin\XBOX 360\Debug\Pro" Binaries\Debug\Pro\XBOX /E /I /Q /Y & xcopy "source\%%i\bin\XBOX 360\Debug\Indie" Binaries\Debug\Indie\XBOX /E /I /Q /Y 

for %%i in (%other%
) do echo Copying %%i... & xcopy "source\%%i\bin\XBOX 360\Debug" Binaries\Debug\Pro\XBOX /E /I /Q /Y & xcopy "source\%%i\bin\XBOX 360\Debug" Binaries\Debug\Indie\XBOX /E /I /Q /Y


REM copy Windows Phone Debug binaries
for %%i in (%sunburn%
) do echo Copying %%i... & xcopy "source\%%i\bin\Windows Phone\Debug\Pro" Binaries\Debug\Pro\WP /E /I /Q /Y & xcopy "source\%%i\bin\Windows Phone\Debug\Indie" Binaries\Debug\Indie\WP /E /I /Q /Y 

for %%i in (%other%
) do echo Copying %%i... & xcopy "source\%%i\bin\Windows Phone\Debug" Binaries\Debug\Pro\WP /E /I /Q /Y & xcopy "source\%%i\bin\Windows Phone\Debug" Binaries\Debug\Indie\WP /E /I /Q /Y