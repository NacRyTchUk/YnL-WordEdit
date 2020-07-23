using UnityEngine;
using UnityEngine.UI;

namespace Ui
{
    public class BlockPanel : MonoBehaviour {
        
        public void OnBlockSelected(Button butt)
        {
            Debug.Log(butt.name);
        }
    }
}
