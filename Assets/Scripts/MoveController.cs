using UnityEngine;
using UnityEngine.Experimental.UIElements;
using UnityEngine.Rendering.PostProcessing;
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable UnassignedField.Global

namespace Assets.Scripts
{
    public class MoveController : MonoBehaviour
    {

        public PostProcessProfile ppp;
        public Camera cam;
        public GameObject scrollView;

        private Vector2 _oldMousePos;
        private bool _isSelectingBlockMenuActive;


        private void Update()
        {
            CheckMiddleButtMove();
            CheckBlockSelektingMenu();
        }

        private void CheckMiddleButtMove()
        {
            if (Input.GetMouseButtonDown(2))
            {
                _oldMousePos = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
                return;
            }

            if ((!Input.GetMouseButton(2)) ||
                ((Input.mousePosition.x == _oldMousePos.x) && (Input.mousePosition.y == _oldMousePos.y))) return;
            cam.transform.position -= new Vector3(((Input.mousePosition.x - _oldMousePos.x)) * Time.fixedDeltaTime, ((Input.mousePosition.y - _oldMousePos.y)) * Time.fixedDeltaTime);
            _oldMousePos = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
        }


        private void CheckBlockSelektingMenu()
        {
            if (!Input.GetKeyDown(KeyCode.E)) return; //Это по хорошему, как и все остальные использования input'ов, нужно переделать
            var umi = GetComponent<UiManager>().curModeOfUi;
            if (umi == UiModeIndex.Editor)
            {
                GetComponent<UiManager>().ChangeUiVisible(UiModeIndex.BlockSelecting);
                ppp.settings[0].active = true;
                
                
            }
            else 
            {
                GetComponent<UiManager>().ChangeUiVisible(UiModeIndex.Editor);
                ppp.settings[0].active = false;
            }
        }
    }
}
