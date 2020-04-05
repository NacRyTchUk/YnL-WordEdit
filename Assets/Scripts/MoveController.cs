using System;
using Assets.Scripts.Ui;
using UnityEngine;
using UnityEngine.Experimental.UIElements;
using UnityEngine.Rendering.PostProcessing;
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable UnassignedField.Global

namespace Assets.Scripts
{
    public class MoveController : MonoBehaviour
    {

        [SerializeField] private PostProcessProfile _ppp;
        [SerializeField] private Camera _cam;
        [SerializeField] private GameObject _scrollView;
        [SerializeField] private GameObject _uiManager; 
        private Vector2 _oldMousePos;
        private bool _isSelectingBlockMenuActive;


        private void Update()
        {
            CameraMove();
            BlockSelektingMenu();
            BlockPlace();
        }

        private void BlockPlace()
        {
            if (!Input.GetMouseButton(Convert.ToInt32(MouseButton.LeftMouse))) return;
            var selectLayer = _uiManager.GetComponent<SelectLayer>();
            var layer = selectLayer.Layer;
            var globalPoint = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            var chunkPositionX = Mathf.FloorToInt(globalPoint.x / Chunk.CHUNK_WIGHT);
            var chunkPositionY = Mathf.FloorToInt(globalPoint.y / Chunk.CHUNK_HEIGHT);
            var blockPositionX = Mathf.FloorToInt(globalPoint.x - chunkPositionX * Chunk.CHUNK_WIGHT) + 1; 
            var blockPositionY = Mathf.FloorToInt(globalPoint.y - chunkPositionY * Chunk.CHUNK_HEIGHT) + 1;
            
            
            Debug.Log(string.Format("x:{0} y:{1} gx:{2} gy:{3}",chunkPositionX,chunkPositionY,blockPositionX,blockPositionY));
        }   
        
        private void CameraMove()
        {
            if (Input.GetMouseButtonDown(2))
            {
                _oldMousePos = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
                return;
            }

            if ((!Input.GetMouseButton(2)) ||
                ((Input.mousePosition.x == _oldMousePos.x) && (Input.mousePosition.y == _oldMousePos.y))) return;
            _cam.transform.position -= new Vector3(((Input.mousePosition.x - _oldMousePos.x)) * Time.fixedDeltaTime, ((Input.mousePosition.y - _oldMousePos.y)) * Time.fixedDeltaTime);
            _oldMousePos = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
        }


        private void BlockSelektingMenu()
        {
            if (!Input.GetKeyDown(KeyCode.E)) return; //Это по хорошему, как и все остальные использования input'ов, нужно переделать
            var umi = GetComponent<UiManager>().curModeOfUi;
            if (umi == UiModeIndex.Editor)
            {
                GetComponent<UiManager>().ChangeUiVisible(UiModeIndex.BlockSelecting);
                _ppp.settings[0].active = true;
                
                
            }
            else 
            {
                GetComponent<UiManager>().ChangeUiVisible(UiModeIndex.Editor);
                _ppp.settings[0].active = false;
            }
        }
    }
}
