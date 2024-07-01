using System;
using UnityEngine;
using UnityEngine.UI;

namespace TexturePaintTool.UI
{
    public class SaveLoadPanel : MonoBehaviour
    {
        public event Action OnSaveClicked;
        public event Action OnLoadClicked;

        [SerializeField] private Button saveButton;
        [SerializeField] private Button loadButton;

        private void Start()
        {
            saveButton.onClick.AddListener(() => OnSaveClicked?.Invoke());
            loadButton.onClick.AddListener(() => OnLoadClicked?.Invoke());
        }
    }
}
