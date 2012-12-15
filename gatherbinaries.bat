@echo off
REM copy Windows Debug binaries
for %%i in (
Indiefreaks.Game.AI 
Indiefreaks.Game.Framework
Indiefreaks.Game.Instancing
Indiefreaks.Game.Lidgren
Indiefreaks.Game.Live
Indiefreaks.Game.Logic
Indiefreaks.Game.Particles
Indiefreaks.Game.Physics
Indiefreaks.Game.Sprites
Indiefreaks.Game.UI
) do echo Copying %%i... & xcopy source\%%i\bin\x86\Debug\Pro Binaries\Debug\Pro\Windows /E /I /Q /Y

for %%i in (
Indiefreaks.Game.IndieCity
Indiefreaks.Game.PostProcess
Indiefreaks.Game.Mercury
) do echo Copying %%i... & xcopy source\%%i\bin\x86\Debug Binaries\Debug\Pro\Windows /E /I /Q /Y