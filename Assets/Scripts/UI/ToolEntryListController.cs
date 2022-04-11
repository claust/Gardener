using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class ToolEntryListController 
{
    private Label Name;

    public void SetVisualElement(VisualElement element)
    {
        Name = element.Q<Label>("ToolName");
    }

    public void SetToolData(ToolData toolData)
    {
        Name.text = toolData.name;
    }
}
