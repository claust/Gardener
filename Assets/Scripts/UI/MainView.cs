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
        [SerializeField]
        private VisualTreeAsset template;
        private UIDocument _uiDocument;
        private GameManager _gameManager;

        private void OnEnable()
        {
            _uiDocument = GetComponent<UIDocument>();
            // var toolController = new ToolListController();
            // toolController.InitializeToolList(uiDocument.rootVisualElement, template);
            var gameManagerObject = GameObject.FindGameObjectWithTag("GameController");
            _gameManager = gameManagerObject.GetComponent<GameManager>();
            SetupTools();
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
    }
}
