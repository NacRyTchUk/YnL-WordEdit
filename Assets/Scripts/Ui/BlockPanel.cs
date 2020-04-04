using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts
{
    public class BlockPanel : MonoBehaviour {
        
        public void OnBlockSelected(Button butt)
        {
            Debug.Log(butt.name);
        }
    }
}
