using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

public class MoveController : MonoBehaviour
{

    public PostProcessProfile ppp;
    public Camera cam;
    public GameObject scrollView;

    private Vector2 oldMousePos;
    private bool isSelectingBlockMenuActive;


    void Update()
    {
        CheckMiddleButtMove();
        CheckBlockSelektingMenu();
    }

    void CheckMiddleButtMove()
    {
        if (Input.GetMouseButtonDown(2))
        {
            oldMousePos = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
            return;
        }
        if ((Input.GetMouseButton(2)) && (((Input.mousePosition.x - oldMousePos.x) != 0) || ((Input.mousePosition.y - oldMousePos.y) != 0)))
        {
            cam.transform.position -= new Vector3(((Input.mousePosition.x - oldMousePos.x)) * Time.fixedDeltaTime, ((Input.mousePosition.y - oldMousePos.y)) * Time.fixedDeltaTime);
            oldMousePos = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
        }
    }


    void CheckBlockSelektingMenu()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {

            if (isSelectingBlockMenuActive)
            {
                scrollView.gameObject.SetActive(false);
                isSelectingBlockMenuActive = false;
                ppp.settings[0].active = false;
                
                
            }
            else
            {
                scrollView.gameObject.SetActive(true);
                isSelectingBlockMenuActive = true;
                ppp.settings[0].active = true;
            }
        }
    }
}
