# Teams Microphone Level

Microphone level indicator for Microsoft Teams.

## Installation

TODO

## Getting Started

* Start the Microsoft Teams desktop app in debug mode:
  * `%localappdata%\Microsoft\Teams\current\Teams.exe --remote-debugging-port=8315`
  * TODO: a menu option to re-launch teams
* Note the system tray icon and microphone level UI (default top-left of screen)

## Technical Details

* Uses Chrome Developer Tools protocol to inspect the Microsoft Teams electron application
to find the state of the mute button.
* Uses MMDevice API to find the microphone device used by Teams.
* Starts a WaveIn session and caclates live microphone level.

## Sources

System tray based on FontAwesome volume-high icon, CC BY 4.0 license:
https://fontawesome.com/icons/volume-high?s=solid

swharden csharp-data-visualization example project, MIT license:
https://github.com/swharden/Csharp-Data-Visualization/tree/main/projects/maui-audio-monitor

