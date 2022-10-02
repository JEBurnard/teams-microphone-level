# Teams Microphone Level

Microphone level indicator for Microsoft Teams.

## Installation

Run the MSI from the releases page on GitHub:
https://github.com/JEBurnard/teams-microphone-level/releases

## Getting Started

* Start the application using the "Teams Microphone Level" start menu shortcut.
* Start the Microsoft Teams desktop app in debug mode:
  * Use the system tray right click menu
  * Or run `%localappdata%\Microsoft\Teams\current\Teams.exe --remote-debugging-port=8315`

## Technical Details

* Uses Chrome Developer Tools protocol to inspect the Microsoft Teams electron application
to find the state of the mute button (ctrl+shift+I on Windows).
* Uses MMDevice API to find the microphone device used by Teams.
* Uses Microsoft Visual Studio Installer Projects extension for the installer.

## Sources

Icon based on FontAwesome volume-high, CC BY 4.0 license:
https://fontawesome.com/icons/volume-high?s=solid

swharden csharp-data-visualization example project, MIT license:
https://github.com/swharden/Csharp-Data-Visualization/tree/main/projects/maui-audio-monitor
