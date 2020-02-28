using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class TrackMouse : MonoBehaviour
{
    [SerializeField]
#pragma warning disable 0649
    private ChapterController cc;
#pragma warning restore 0649


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    void OnMouseDown()
    {

       
     
        //sphereScreenPoint = Input.mousePosition.y;
        cc.TrackOnMouseDown(Input.mousePosition.y);
        

    }

    void OnMouseDrag()
    {
        
        cc.TrackOnMouseDrag(Input.mousePosition.y);
          


    }
    void OnMouseUp()
    {
        
        cc.TrackOnMouseUp(Input.mousePosition.y);

    }
}
