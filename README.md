![logo](FaineSwitch.ico)

# FaineSwitcher - The magic Layout Switcher.

> This is fork from [Mahou project](https://github.com/BladeMight/Mahou). We disable all network activity that that Mahou has and add Ukraine dictionary for switching from EN to UA and UA to EN keyboard.

### How it works
FaineSwitcher works like **You** want, configure it as you wish, by default it switches *not by next layout*, but by **specified in settings** layouts.\
Even selected text switches just between **selected** layouts, though if you liked cycling through, starting from `v1.0.2.9` there is **Cycle Mode**, in `v2.0.0.0` and above to activate it you just need to disable function `Switch between layouts`. By default FaineSwitch stores configurations in folder where FaineSwitch.exe is, but there is a function that makes FaineSwitch store them in %AppData%.

### FaineSwitcher requires [.NET Framework 4.8 or greater](https://www.microsoft.com/en-US/download/details.aspx?id=17718) to work properly. Beginning from v1.4.3.9 error when running on .NET 4.0 were fixed.

### Features

###### How to enable autos witch:
1. Open Snippets tab.
2. Check 'Enable snippets'.
3. Open Auto switch tab.
4. Chack 'Enable auto-switch'.
5. Press 'Update auto-switch dictionary' (If version with requests).
6. Press 'Apply' button.

###### How to use:
1. To convert selection hit <kbd>Scroll</kbd> when select text.
2. To convert input hit <kbd>Pause</kbd> when typing.
3. To convert line hit <kbd>Shift</kbd>+<kbd>Pause</kbd>.
4. To change layout by one key press <kbd>CapsLock</kbd>.
5. Starting from v1.0.4.4 in Convert selection unrecognized text by all selected layout in settings (example: ♥) just rewrites.
6. Read the [wiki]() or [ask me](#license).

### Hotkeys
- <kbd>Pause</kbd> - Convert last input.
- <kbd>Scroll</kbd> - Convert selection.
- <kbd>Shift</kbd>+<kbd>Pause</kbd> - Convert last inputted line.
- <kbd>Scroll</kbd> - Convert selected text.
- <kbd>Shift</kbd>+<kbd>F11</kbd> - Convert multiple last words, to select quantity press 1-9(0 = 10) after hotkey on keyboard(not NumPad)..
- <kbd>Shift</kbd>+<kbd>F9</kbd> - Toggle language panel visibility.
- <kbd>Ctrl</kbd>+<kbd>Shift</kbd>+<kbd>Alt</kbd>+<kbd>Win</kbd>+<kbd>Insert</kbd> - To toggle configs windows visibility.
- <kbd>Shift</kbd>+<kbd>Alt</kbd><kbd>PageUp</kbd> - Restart FaineSwitch.
- <kbd>Ctrl</kbd>+<kbd>Shift</kbd>+<kbd>Alt</kbd>+<kbd>Win</kbd>+<kbd>F12</kbd> - To exit FaineSwitch.
- Other hotkeys disabled by default or have description in FaineSwitch.

### License
FaineSwitch is under [GPL v2+]().

# Contribute

## Getting Started

1. Install Visual Studio 2022
2. Install [Microsoft Visual Studio Installer Projects](https://marketplace.visualstudio.com/items?itemName=VisualStudioClient.MicrosoftVisualStudio2022InstallerProjects)

# Setup Project

This guide will walk you through the process of building a .NET project and a WIX installer project using Visual Studio and Winget. 

## Prerequisites 

Before getting started, you will need the following: 

- Windows 10 with the Winget package manager installed 
- Visual Studio installed on your computer
- The .NET project and WIX installer project files

## Installing Visual Studio and WIX with Winget 

1. Open the Command Prompt as an Administrator. 
2. Enter the following command to install Visual Studio: 

`winget install --id=Microsoft.VisualStudio2019Community --version=16.5.5`

3. Enter the following command to install WIX: 

`winget install --id=WixToolset.WixToolset --version=3.11.2.3012`

4. Wait for the installations to complete. 

## Building the .NET Project 

1. Open the .NET project in Visual Studio. 
2. In the menu bar, go to **Build**, then select **Build Solution**. 
3. If you are prompted to build the dependencies first, select **Yes**. 
4. After the solution builds successfully, you should now have an executable file in the project's output directory. 

## Building the WIX Installer Project 

1. Open the WIX installer project in Visual Studio. 
2. In the menu bar, go to **Build**, then select **Build Solution**. 
3. If you are prompted to build the dependencies first, select **Yes**. 
4. After the solution builds successfully, you should now have an installer file in the project's output directory. 

## Finishing Up 

Congratulations, you have now successfully setup FaineSwitcher project!