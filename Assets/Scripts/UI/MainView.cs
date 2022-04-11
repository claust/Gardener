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

        private void OnEnable()
        {
            var uiDocument = GetComponent<UIDocument>();
            var toolController = new ToolListController();
            toolController.InitializeToolList(uiDocument.rootVisualElement, template);
        }
    }
}
