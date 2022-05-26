using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UIElements;

namespace Assets.Scripts.UI
{
    public class MainView : MonoBehaviour
    {
        // [SerializeField]
        // private VisualTreeAsset template;
        private UIDocument _uiDocument;
        private GameManager _gameManager;
        private Label _coins;
        private VisualElement _inventory;
        private readonly List<VisualElement> tools = new();

        public void SetCoins(int coins)
        {
            _coins.text = coins.ToString();
        }

        private void OnEnable()
        {
            _uiDocument = GetComponent<UIDocument>();
            // var toolController = new ToolListController();
            // toolController.InitializeToolList(uiDocument.rootVisualElement, template);
            var gameManagerObject = GameObject.FindGameObjectWithTag("GameController");
            _gameManager = gameManagerObject.GetComponent<GameManager>();
            _gameManager.IsMouseOverHUD = IsMouseOverMe;
            SetupDatabinding();
        }

        private void SetupDatabinding()
        {
            _coins = _uiDocument.rootVisualElement.Q<Label>("Coins");
            _inventory = _uiDocument.rootVisualElement.Q<VisualElement>("Inventory");
            _inventory.visible = false;
        }

        public void Bind()
        {
            var inventory = _gameManager.Inventory;
            for (int i = 0; i < inventory.MaxSlots; i++)
            {
                var item = i < inventory.List.Count ? inventory.List[i] : null;
                var element = _inventory.Q<VisualElement>($"InventoryItem{i}");
                element.Q<Label>("ItemName").text = item?.Name ?? "";
                element.Q<VisualElement>("ItemIcon").style.backgroundImage = item != null ? new StyleBackground(item.Icon) : null;
                element.Q<Label>("Quantity").text = item?.Quantity.ToString() ?? "";
            }
            _coins.text = _gameManager.Coins.ToString();
            SetupTools(_gameManager.SelectedTool);
        }

        private void SetupTools(ToolType selectedTool)
        {
            tools.Clear();
            void configure(string name, ToolType type)
            {
                var tool = _uiDocument.rootVisualElement.Q<Button>(name);
                tools.Add(tool);
                if (selectedTool == type)
                {
                    Debug.Log($"Selected tool is {type}");
                    SelectTool(tool.style);
                }
                tool.clickable.clicked += () =>
                {
                    Debug.Log($"{type} selected");
                    _gameManager.OnToolClicked(type);
                    ClearToolSelection();
                    SelectTool(tool.style);
                };
            }
            configure("GrassRemoverToolButton", ToolType.GrassRemover);
            configure("WateringCanToolButton", ToolType.WateringCan);
            configure("SeederToolButton", ToolType.Seeder);
            configure("ScissorsToolButton", ToolType.Scissors);
        }

        private void SelectTool(IStyle style)
        {
            var color = new StyleColor(Color.yellow);
            var size = 3;
            style.borderTopWidth = size;
            style.borderTopColor = color;
            style.borderBottomWidth = size;
            style.borderBottomColor = color;
            style.borderLeftWidth = size;
            style.borderLeftColor = color;
            style.borderRightWidth = size;
            style.borderRightColor = color;
        }

        private void ClearToolSelection()
        {
            tools.ForEach(t =>
            {
                t.style.borderBottomWidth = 0;
                t.style.borderTopWidth = 0;
                t.style.borderLeftWidth = 0;
                t.style.borderRightWidth = 0;
            });
        }

        void Update()
        {
            if (Input.GetKeyUp(KeyCode.Tab))
            {
                Debug.Log($"Tab pressed: Inventory {_inventory.resolvedStyle.display}");
                _inventory.visible = !_inventory.visible;
                Bind();
            };
        }

        public bool IsMouseOverMe()
        {
            var position = Input.mousePosition;
            Vector2 pointerUiPos = new Vector2 { x = position.x, y = Screen.height - position.y };
            var picked = new List<VisualElement>();
            _uiDocument.rootVisualElement.panel.PickAll(pointerUiPos, picked);
            return picked.Any(ve => ve.enabledInHierarchy && ve.resolvedStyle.backgroundColor.a != 0);
        }
    }
}
