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
| ![ClipboardHistory Tool Window](/ClipboardHistory/AppResources/Images/ScreenShot_ToolWindow.png) | This tool window is the core of the Clipboard History extension.<br>It shows a list of past clipboard items ordered from the newest to the oldest.<br><br>To retrieve an item from the History List, simply Select this item and Press Ctrl + C. The item will be copied back into the Windows Clipboard so you can Paste it (Ctrl + V) back anywhere you'd like! |

## Settings

![ClipboardHistory Settings Window](/ClipboardHistory/AppResources/Images/ScreenShot_Settings.png)

`Explain each configuration options and the behavior associated to it.`

History Max. Capacity:<br>
aaaaaaaaaa

Lines Displayed per Item:<br>
bbbbbbb

ToolTip Hover Delay (in ms):<br>
ccccccccccc

Visual Studio Clipboard Only:<br>
ddddddddd

Prevent Duplicate Items:<br>
eeeeee

## Bug Tracker

Have a bug or a feature request? [Please open a new issue](https://github.com/kavengagne/KGVSClipboardHistory/issues).

## License

KGVSClipboardHistory is released under the [Apache License, Version 2.0](/LICENSE.txt).
