using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UIElements;

namespace Assets.Scripts.UI
{
    public class ToolListController
    {
        List<ToolData> _tools;

        VisualTreeAsset _template;
        ListView _toolList;
        Label _toolNameLabel;
        VisualElement _toolImage;
        Button _toolSelectButton;

        public void InitializeToolList(VisualElement root, VisualTreeAsset listElementTemplate)
        {
            InitializeTools();

            _template = listElementTemplate;
            _toolList = root.Q<ListView>("ToolList");
            _toolList.onSelectionChange += OnToolSelected;
            _toolNameLabel = root.Q<Label>("ToolName");
            _toolImage = root.Q<VisualElement>("ToolImage");
            _toolSelectButton = root.Q<Button>("SelectToolButton");

            FillList();
        }

        private void OnToolSelected(IEnumerable<object> selected)
        {
            var selectedTool = _toolList.selectedItem as ToolData;
            if (selectedTool != null)
            {
                _toolNameLabel.text = selectedTool.Name;
                _toolImage.style.backgroundImage = new StyleBackground(selectedTool.ToolImage);
                _toolSelectButton.SetEnabled(true);
            }
            else
            {
                _toolNameLabel.text = "";
                _toolImage.style.backgroundImage = null;
                _toolSelectButton.SetEnabled(false);
            }
        }

        void InitializeTools()
        {
            _tools = new List<ToolData>();
            _tools.AddRange(Resources.LoadAll<ToolData>("Tools"));
        }

        void FillList()
        {
            _toolList.makeItem = () =>
            {
                var newListEntry = _template.Instantiate();
                var controller = new ToolEntryListController();
                controller.SetVisualElement(newListEntry);
                newListEntry.userData = controller;
                return newListEntry;
            };
            _toolList.bindItem = (item, index) =>
            {
                (item.userData as ToolEntryListController).SetToolData(_tools[index]);
            };
            _toolList.fixedItemHeight = 45;
            _toolList.itemsSource = _tools;
        }
    }
}