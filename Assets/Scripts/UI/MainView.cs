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
            SetupTools();
            SetupDatabinding();
        }

        private void SetupDatabinding()
        {
            _coins = _uiDocument.rootVisualElement.Q<Label>("Coins");
            _inventory = _uiDocument.rootVisualElement.Q<VisualElement>("Inventory");
            _inventory.visible = false;
        }

        private void Bind(Inventory inventory)
        {
            for (int i = 0; i < inventory.MaxSlots; i++)
            {
                var item = i < inventory.List.Count ? inventory.List[i] : null;
                var element = _inventory.Q<VisualElement>($"InventoryItem{i}");
                element.Q<Label>("ItemName").text = item?.Name ?? "";
                element.Q<VisualElement>("ItemIcon").style.backgroundImage = item != null ? new StyleBackground(item.Icon) : null;
                element.Q<Label>("Quantity").text = item?.Quantity.ToString() ?? "";
            }
        }

        private void SetupTools()
        {
            var toolGrassRemover = _uiDocument.rootVisualElement.Q<Button>("GrassRemoverToolButton");
            toolGrassRemover.clickable.clicked += () =>
            {
                Debug.Log("GrassRemover selected");
                _gameManager.OnToolClicked(ToolType.GrassRemover);
            };
            var toolWateringCan = _uiDocument.rootVisualElement.Q<Button>("WateringCanToolButton");
            toolWateringCan.clickable.clicked += () =>
            {
                Debug.Log("WateringCan selected");
                _gameManager.OnToolClicked(ToolType.WateringCan);
            };
            var toolSeeder = _uiDocument.rootVisualElement.Q<Button>("SeederToolButton");
            toolSeeder.clickable.clicked += () =>
            {
                Debug.Log("Seeder selected");
                _gameManager.OnToolClicked(ToolType.Seeder);
            };
            var toolScissors = _uiDocument.rootVisualElement.Q<Button>("ScissorsToolButton");
            toolScissors.clickable.clicked += () =>
            {
                Debug.Log("Scissors selected");
                _gameManager.OnToolClicked(ToolType.Scissors);
            };
        }

        void Update()
        {
            if(Input.GetKeyUp(KeyCode.Tab))
            {
                Debug.Log($"Tab pressed: Inventory {_inventory.resolvedStyle.display}");
                _inventory.visible = !_inventory.visible;
                Bind(_gameManager.Inventory);
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
