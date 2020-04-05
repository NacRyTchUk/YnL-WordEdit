using System.IO;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts
{
    
    public class Event : MonoBehaviour
    {
        [SerializeField] private GameObject eventObj;
        private InputField _ib;

        private void Start()
        {
            _ib = GetComponent<MapFileSystem>().InputBox;
        }
    
        public void LoadButt()
        {
            if (!File.Exists(_ib.text + "/Map.info")) return;
            GetComponent<MapFileSystem>().LoadTheMap();
            eventObj.GetComponent<UiManager>().ChangeUiVisible(UiModeIndex.Editor);
        }

        public void CreateNewButt()
        {
            MapFileSystem.CreateNewMap();
            eventObj.GetComponent<UiManager>().ChangeUiVisible(UiModeIndex.Editor);
        }
    }
}
