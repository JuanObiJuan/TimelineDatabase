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

public class AnimationManager : MonoBehaviour
{
    public static AnimationManager instance;

    [Header("Animation Parameters")]
    public float slidingAnimationfactor = 0.5f;
    public float maxSlidingSpeed = 40f;
    public float dragFactor = 0.1f;
    public AnimationCurve slidingAnimation;

    [Header("Config")]
    [SerializeField]
    TimelineConfiguration config;

    [Header("Status Listeners")]

    
    [SerializeField]
    private bool _MouseTouchingElement;

    private void Awake()
    {
        instance = this;
    }
    private void OnEnable()
    {
        EventManager.instance.mouseTouchingTrack.AddListener(OnMouseTouchingTrack);
        EventManager.instance.mouseTouchingElement.AddListener(OnMouseTouchingElement);
        EventManager.instance.mouseDraggingTrack.AddListener(OnMouseDraggingTrack);
    }

    private void OnDisable()
    {
        EventManager.instance.mouseTouchingTrack.RemoveListener(OnMouseTouchingTrack);
        EventManager.instance.mouseTouchingElement.RemoveListener(OnMouseTouchingElement);
        EventManager.instance.mouseDraggingTrack.RemoveListener(OnMouseDraggingTrack);
    }
    private void OnMouseTouchingTrack(bool status, ChapterController cc)
    {
       
    }

    private void OnMouseTouchingElement(bool status, ChapterController cc, ElementController ec)
    {
        
    }
    private void OnMouseDraggingTrack(bool status, ChapterController cc)
    {
        
    }

    public void DragTrack(ChapterController cc,float speed)
    {
        if (cc.isExpanded) return;
 
            Vector3 direction = (cc.pointA.position - cc.pointB.position).normalized;
            cc.elementsTransform.position += direction * speed * dragFactor;
            //Limit the minimum
            if (cc.elementsTransform.position.z > 0f) cc.elementsTransform.localPosition = Vector3.zero;
            cc.updateBottomUIScroll();
      
        //TODO limit the maximum
    }

    internal void ExpandTimelineElement(ChapterController cc)
    {
        cc.controllerSelected.isBeingAnimated = true;
        if (!cc.controllerSelected.isExpanded)
        {

        }
        //remember to put it to false
        cc.controllerSelected.isBeingAnimated = false;
        cc.controllerSelected.isExpanded = true;
    }
 


        public void AnimateTrack(ChapterController cc, int destionationSlide, float speed )
    {
            //Fix this (destination slide should be real)
            Vector3 direction = (cc.pointA.position - cc.pointB.position).normalized;
            if (speed > 0) destionationSlide += 1;
            if (speed < 0) destionationSlide -= 1;
            if (destionationSlide < 0) destionationSlide = 0;
        
        Vector3 destination = direction * config.depthSeparation * (destionationSlide);
        //Debug.Log("AnimateTrack destination "+ destination + " speed "+ speed);
        StartCoroutine(MoveAllElements(cc, destination, speed));
    }

    //Move track directly
    public void MoveTrack(ChapterController cc, float value) {

        //find the value that match with a slide
        float deltaProportion = 1.0f / (cc.elements.Count);
        int discreteValue = (int)(value / deltaProportion);
        Vector3 direction = (cc.pointA.position - cc.pointB.position).normalized;
        cc.elementsTransform.position = cc.pointA.position+direction * config.depthSeparation * discreteValue;

    }

    IEnumerator MoveAllElements(ChapterController cc, Vector3 localDestination, float speed)
    {
        //Debug.Log("MoveAllElements coroutine");
        float timer = 0f;
        float factor = Math.Abs(speed) * slidingAnimationfactor / maxSlidingSpeed;

        Vector3 localOrigin = cc.elementsTransform.localPosition;
        //While animation is running and the mouse is not again back on this track
        //TODO check per track (touch)
        while (timer < 1f && !cc._TrackIsBeingDragged)
        {
            
            timer += Time.deltaTime * factor;
            cc.elementsTransform.localPosition = Vector3.Lerp(localOrigin, localDestination, slidingAnimation.Evaluate(timer));
            cc.updateBottomUIScroll();
            yield return null;
        }
        //We need to know the reason whay the courutine has finished
        if (timer>1f)
        {
            cc.elementsTransform.localPosition = localDestination;
            
            float index = (Vector3.Distance(cc.elementsTransform.position, Vector3.zero)) / config.depthSeparation;
            int slide = Mathf.RoundToInt(index);
            cc.controllerSelected = cc.elements[slide];
        }
        else
        {
            //Debug.Log("Touching again ");
        }
        cc.updateBottomUIScroll();

    }
    


   
}
