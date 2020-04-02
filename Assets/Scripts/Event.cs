using System.IO;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts
{
    public class Event : MonoBehaviour {

        private InputField IB;

        private void Start()
        {
            IB = GetComponent<MapLoader>().inputBox;
        }
    
        public void LoadButt()
        {
       
            if (File.Exists(IB.text + "/Map.info"))
            {
                GetComponent<MapLoader>().LoadTheMap();
            }
        }

        public void CreateNewButt()
        {
            GetComponent<MapLoader>().CreateNewMap();
        }
    }
}
