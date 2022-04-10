using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEditor.UIElements;


public class QuickTool : EditorWindow
{
    [MenuItem("QuickTool/Open _%#T")]
    public static void ShowWindow()
    {
        // Opens the window, otherwise focuses it if it's already open.
        var window = GetWindow<QuickTool>();

        // Adds a title to the window.
        window.titleContent = new GUIContent("QuickTool");

        // Sets a minimum size to the window.
        window.minSize = new Vector2(280, 50);
    }

    [MenuItem("Window/UI Toolkit/QuickTool")]
    public static void ShowExample()
    {
        QuickTool wnd = GetWindow<QuickTool>();
        wnd.titleContent = new GUIContent("QuickTool");
    }

    public void CreateGUI()
    {
        // Reference to the root of the window.
        /*     var root = rootVisualElement;

             // Creates our button and sets its Text property.
             var myButton = new Button() { text = "My Button" };

             // Give it some style.
             myButton.style.width = 160;
             myButton.style.height = 30;

             // Adds it to the root.
             root.Add(myButton);
        */
    }
}