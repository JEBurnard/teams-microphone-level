# Teams Microphone Level

Microphone level indicator for Microsoft Teams.

## Installation

TODO

## Getting Started

* Start the Microsoft Teams desktop app in debug mode:
  * Use the system tray right click menu
  * Or `%localappdata%\Microsoft\Teams\current\Teams.exe --remote-debugging-port=8315`

## Technical Details

* Uses Chrome Developer Tools protocol to inspect the Microsoft Teams electron application
to find the state of the mute button (ctrl+shift+I on Windws).
* Uses MMDevice API to find the microphone device used by Teams.

## Sources

System tray based on FontAwesome volume-high icon, CC BY 4.0 license:
https://fontawesome.com/icons/volume-high?s=solid

swharden csharp-data-visualization example project, MIT license:
https://github.com/swharden/Csharp-Data-Visualization/tree/main/projects/maui-audio-monitor
