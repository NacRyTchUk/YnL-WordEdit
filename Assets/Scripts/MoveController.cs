using System;
using Map;
using Ui;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Rendering.PostProcessing;
using UnityEngine.UIElements;

// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable UnassignedField.Global

public struct BlockPosition
{
    public Vector2 chunkPos;
    public Vector2 blockPos;
    public int layer;
}
    
public class MoveController : MonoBehaviour
{
    [SerializeField] private PostProcessProfile ppp;
    [SerializeField] private Camera cam;
    [SerializeField] private GameObject scrollView;
    private Vector2 _oldMousePos;
    private UiManager _uiManager;
    private SelectLayer _selectLayer;
    private BlockManager _blockManager;
    private Camera _camera;
    private bool _isCameraNull;


    private void Start()
    {
        _isCameraNull = _camera == null;
        _camera = Camera.main;
        _blockManager = gameObject.AddComponent<BlockManager>();
        _selectLayer = GetComponent<SelectLayer>();
        _uiManager = GetComponent<UiManager>();
    }

    private void Update()
    {
        CameraMove();
        BlockSelektingMenu(); //Нужны события
        BlockPlace();
    }

    private void BlockPlace()
    {
        if (!Input.GetMouseButtonDown(Convert.ToInt32(MouseButton.LeftMouse))) return;
        if (EventSystem.current.IsPointerOverGameObject()) return;

        if (_isCameraNull) Debug.Log(new Exception("Camera is null"));
        
        int layer = _selectLayer.Layer;
        Vector3 globalPoint = _camera.ScreenToWorldPoint(Input.mousePosition);
        int chunkPositionX = Mathf.FloorToInt(globalPoint.x / Chunk.ChunkWight);
        int chunkPositionY = Mathf.FloorToInt(globalPoint.y / Chunk.ChunkHeight);
        int blockPositionX = Mathf.FloorToInt(globalPoint.x - chunkPositionX * Chunk.ChunkWight) + 1; 
        int blockPositionY = Mathf.FloorToInt(globalPoint.y - chunkPositionY * Chunk.ChunkHeight) + 1;
        BlockPosition bp;
        bp.layer = layer;
        bp.blockPos = new Vector2(blockPositionX,blockPositionY);
        bp.chunkPos = new Vector2(chunkPositionX,chunkPositionY);
        _blockManager.BlockPlace(bp);

        //Debug.Log(string.Format("x:{0} y:{1} gx:{2} gy:{3}",chunkPositionX,chunkPositionY,blockPositionX,blockPositionY));
    }   
        
    private void CameraMove()
    {
        if (Input.GetMouseButtonDown(2))
        {
            _oldMousePos = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
            return;
        }

        const float tolerance = 0.1f;
        if ((!Input.GetMouseButton(2)) ||
            ((Math.Abs(Input.mousePosition.x - _oldMousePos.x) < tolerance) && (Math.Abs(Input.mousePosition.y - _oldMousePos.y) < tolerance))) return;
        cam.transform.position -= new Vector3(((Input.mousePosition.x - _oldMousePos.x)) * Time.fixedDeltaTime, ((Input.mousePosition.y - _oldMousePos.y)) * Time.fixedDeltaTime);
        _oldMousePos = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
    }


    private void BlockSelektingMenu()
    {
        if (!Input.GetKeyDown(KeyCode.E)) return; //Это по хорошему, как и все остальные использования input'ов, нужно переделать
        var umi = _uiManager.curModeOfUi;
        if (umi == UiModeIndex.Editor)
        {
            _uiManager.ChangeUiVisible(UiModeIndex.BlockSelecting);
            ppp.settings[0].active = true;
                
                
        }
        else 
        {
            _uiManager.ChangeUiVisible(UiModeIndex.Editor);
            ppp.settings[0].active = false;
        }
    }
}