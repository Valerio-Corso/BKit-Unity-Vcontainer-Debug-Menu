# bkit-unity-debug-menu
A lightweight simple unity debug menu, it uses an assembly definition to resolve your debug classes and invoke actions.
You can find a version not using VContainer here: https://github.com/Valerio-Corso/BKit-Unity-Debug-Menu/tree/main

## Requirements
- TextMeshPro 3.0.0
- VContainer

## Installation
- Import the Debug.Unity folder or the package in release.

## Usage
1. Create a new object in your scene that holds the following:
    a. You also need to set a 'Canvas references' object, you can find one already existing withing the Asset folder of the package, optionally you can define your elements and customize the UI looks (Buttons, Tabs, Header, Panel) 

![image](https://github.com/user-attachments/assets/6956c4e2-9ca7-4f1a-b834-8d83e9d93183)


2. Make sure to define an assembly, if you don't have one create it for the folder holding your debug scripts.
3. Press F1 while in-game

Start using it like this:

```
    /// Define your tab, seen at the top of the screen
    /// Inherit from IDebugProvider, any scripts inheriting from it will automatically be Registered
    [DebugTab("Test Debug One")]
    public class TestDebugOne : IDebugProvider
    {
        /// Define your actions, it can take an optional string as seen here to define an header object that will be spawned
        /// Debug actions are grouped by Tab->Header, so you can specificy multiple equal string and they will be merged together
        [DebugAction("Test Action")]
        public void TestAction()
        {
        }
    }
```

![image](https://github.com/user-attachments/assets/175321ef-2787-4efc-87c1-d0d5fe83b066)
