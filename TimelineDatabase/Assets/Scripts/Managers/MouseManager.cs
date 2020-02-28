/**MIT License

Copyright (c) 2020 JuanObiJuan

Permission is hereby granted, free of charge, to any person obtaining a copy
of this software and associated documentation files (the "Software"), to deal
in the Software without restriction, including without limitation the rights
to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
copies of the Software, and to permit persons to whom the Software is
furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in all
copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
SOFTWARE.
*/
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class MouseManager : MonoBehaviour
{
    public static MouseManager instance;

    [Header("Config")]
    [SerializeField]
    TimelineConfiguration config;

    [Header("Drag Movements")]
    public float movemenDragFactor;



    [Header("Mouse Status")]
    
    private bool elementIsBeingTouched = false;
    ElementController elementControllerBeingTouched;
    

    private void Awake()
    {
        instance = this;
    }

    internal void TrackOnMouseDown(float mouseYpos, ChapterController cc)
    {
        Debug.Log("TrackOnMouseDown " + cc.chapterId);
        if (cc.isExpanded) return;
        
        cc.firstTrackInputMouseY = mouseYpos;
        cc.tempTrackMouseY = mouseYpos;
        UpdateTrackTouchStatus(true,cc);
    }

    internal void TrackOnMouseDrag(float mouseYpos, ChapterController cc)
    {
        if (cc.isExpanded) return;

      
        UpdateTrackDragStatus(true, cc);
        float diff = cc.tempTrackMouseY - mouseYpos;
        AnimationManager.instance.DragTrack(cc,Math.Abs(diff)* cc.mouseDirection);
        UpdateMouseDirection(diff,cc);
        cc.tempTrackMouseY = mouseYpos;
       
       
    }

    public void UpdateMouseDirection(float updateValue, ChapterController cc)
    {
        //touchess in the same position dont give info
        if (updateValue != 0f) cc.mouseDirection = updateValue;
        //Debug.Log("updateValue = " + updateValue + " mouseDirection" + mouseDirection);
    }

    internal void TrackOnMouseUp(float mouseYpos, ChapterController cc)
    {
        if (cc.isExpanded) return;

        UpdateTrackTouchStatus(false, cc);
        UpdateTrackDragStatus(false, cc);
        if (Mathf.Abs(mouseYpos - cc.firstTrackInputMouseY) < 3f)
        {
            //Click On Track??
            //Debug.Log("Click on track?");
        }
        else
        {
            //Is a swipe
           
            float index = (Vector3.Distance(cc.elementsTransform.position, Vector3.zero)) / config.depthSeparation;
            //Debug.Log("Swipe on track --> AnimateTrack " + Mathf.RoundToInt(index) + " mouseDirection "+mouseDirection);
            //TODO
            AnimationManager.instance.AnimateTrack( cc, Mathf.RoundToInt(index), cc.mouseDirection);
            
        }

     
    }

    internal void ElementOnMouseDown(float y, ElementController ec, ChapterController cc)
    {
        if (cc.isExpanded) return;
        if (cc._TrackIsBeingDragged)
        {
            //This event start dragging a track
            //And it shud continue
            TrackOnMouseDrag(y, cc);
        }
        else
        {
            //This event start on element
            //Maybe a click maybe a drag
            cc.firstElementInputMouseY = y;
            cc.firstTrackInputMouseY = y;
            elementControllerBeingTouched = ec;
            elementIsBeingTouched = true;
            
        }
        
    }

    internal void ElementOnMouseDrag(float y, ElementController ec, ChapterController cc)
    {
        if (cc.isExpanded) return;

        if (cc._TrackIsBeingDragged)
        {
            //Is a drag that start with a track
            TrackOnMouseDrag(y, cc);
        }
        else
        {
            //start with a track drag and continue with element drag?
            //TrackOnMouseDrag(y, cc);
            //Debug.Log("Maybe a click on element maybe a drag");
            if (Mathf.Abs(y - cc.firstElementInputMouseY) < 3f)
            {
                Debug.Log("The finger is still in the same position ... maybe a click");
            }
            else
            {
                cc._TrackIsBeingDragged = true;
            }
        }
    }

    internal void ElementOnMouseUp(float y, ElementController ec, ChapterController cc)
        {
            if (cc.isExpanded) return;

            if (cc._TrackIsBeingDragged)
            {
                Debug.Log("It was dragging before");
                //It was dragging before
                TrackOnMouseUp(y, cc);
            }
            else
            {
                Debug.Log("Click?");
                //Click?
                if (Mathf.Abs(y - cc.firstElementInputMouseY) < 3f)
                {

                    //Click On Element
                    if (ec == cc.controllerSelected)
                    {
                        //Debug.Log("Is already the first... so do something");
                        EventManager.instance.selectTimelineElement.Invoke(cc, ec);
                    }
                    else
                    {
                        //Debug.Log("Move to this element " + cc.elements.IndexOf(ec));
                        AnimationManager.instance.AnimateTrack(cc, cc.elements.IndexOf(ec) - 1, 1f);
                    }

                }
                else
                {
                    //Swipe on element
                    //TODO sholud do the same as track
                    Debug.Log("Swipe on element");
                }
            }

        

        

    }

    private void UpdateElementTouchStatus(bool status, ChapterController cc, ElementController ec)
    {
        
    }

    private void UpdateTrackTouchStatus(bool status, ChapterController cc)
    {
        cc._TrackIsBeingDragged = status;
        EventManager.instance.mouseTouchingTrack.Invoke(status, cc);
    }

    private void UpdateTrackDragStatus(bool status, ChapterController cc)
    {
        cc._TrackIsBeingDragged = status;
        EventManager.instance.mouseDraggingTrack.Invoke(status, cc);
    }

}
