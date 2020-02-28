using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ElementMouse : MonoBehaviour
{

    public ChapterController cc;

    public ElementController ec;

    private void Start()
    {
        //We take the chapter controller from the element controller
        cc = ec.cc;
    }

    void OnMouseDown()
    {


        
        cc.ElementOnMouseDown(Input.mousePosition.y, ec);


    }

    void OnMouseDrag()
    {
       
        cc.ElementOnMouseDrag(Input.mousePosition.y, ec);



    }
    void OnMouseUp()
    {
        
        cc.ElementOnMouseUp(Input.mousePosition.y, ec);

    }
}
