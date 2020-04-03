using System.IO;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts
{
    
    public class Event : MonoBehaviour
    {
        public GameObject eventObj;
        private InputField _ib;

        private void Start()
        {
            _ib = GetComponent<MapFileSystem>().inputBox;
        }
    
        public void LoadButt()
        {
       
            if (File.Exists(_ib.text + "/Map.info"))
            {
                GetComponent<MapFileSystem>().LoadTheMap();
                eventObj.GetComponent<UiManager>().ChangeUiVisible(UiModeIndex.Editor);
            }
        }

        public void CreateNewButt()
        {
            MapFileSystem.CreateNewMap();
            eventObj.GetComponent<UiManager>().ChangeUiVisible(UiModeIndex.Editor);
        }
    }
}
