# Clipboard History

KGVSClipboardHistory is a <a href="http://www.microsoft.com/visualstudio/eng" target="_blank">Visual Studio</a>
extension that enables clipboard history.

It keeps track of all changes to the clipboard in a configurable list using a tool window.
You can then navigate the list to retrieve the desired clipboard data.

I developed this extension for my own needs, feel free to contribute if you can improve it.

## Supported Platforms

* Visual Studio 2012 (For sure!)
* Visual Studio 2013 (Untested)

## Getting Started

* Download VSIX file from <a href="http://www.microsoft.com/visualstudio/eng" target="_blank">Visual Studio Extensions Gallery</a>.
* Install the extension using the vsix package and then restart Visual Studio.
* Click **View** | **Other Windows** | **Clipboard History**.

| Image | Description |
|-------|-------------|
| ![ClipboardHistory Tool Window](/ClipboardHistory/AppResources/Images/ScreenShot_ToolWindow.png) | This tool window is the core of the Clipboard History extension.<br>It shows a list of past clipboard items ordered from the newest to the oldest.<br><br>To retrieve an item from the History List, simply Select this item and Press Ctrl + C. The item will be copied back into the Clipboard so you can Paste it (Ctrl + V) back anywhere you'd like!<br><br>You can also change the extension's settings from that window. (See **Settings** section) |

| Image | Description |
|-------|-------------|
| ![ClipboardHistory Tool Window](/ClipboardHistory/AppResources/Images/ScreenShot_ToolTip.png) | ToolTip Windows |

## Settings

You can view and change the Settings of this extension by clicking on the "Settings" expander at the top of the Tool Window.
All Settings take effect immediately. Invalid values will be ignored.

| Image | Description |
|-------|-------------|
| ![ClipboardHistory Settings Window](/ClipboardHistory/AppResources/Images/ScreenShot_Settings.png) | **History Max. Capacity**<br>Maximum number of items that the History List can contain.  <br><br>**Lines Displayed per Item**<br>Maximum number of Lines displayed for each item in the list.  <br><br>**ToolTip Hover Delay (in ms)**<br>Delay in milliseconds before the ToolTip appears when you leave the mouse over an item in the list.  <br><br>**Visual Studio Clipboard Only**<br>Limit History List entries to those copied from the current Visual Studio instance.  <br><br>**Prevent Duplicate Items**<br>Ignores successive copy operations if the item data is the same as the last one. |

## Bug Tracker

Have a bug or a feature request? [Please open a new issue](https://github.com/kavengagne/KGVSClipboardHistory/issues).

## License

KGVSClipboardHistory is released under the [Apache License, Version 2.0](/LICENSE.txt).
