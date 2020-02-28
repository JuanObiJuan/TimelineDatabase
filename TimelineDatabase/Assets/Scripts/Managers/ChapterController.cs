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
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;
using UnityEngine.Events;



public class ChapterController : MonoBehaviour
{
    [SerializeField]
    public int controllerId;
   
    public int arrayId;

    public Transform elementsTransform;
   
    //private TimeLineController tlc;

    [Header("Chapter Info")]

    public int chapterId;
    public string title_EN;
    public string title_DE;
    public string subtitle_EN;
    public string subtitle_DE;
    public int from;
    public int to;

    [Header("Timeline Elements")]
    public List<ElementController> elements = new List<ElementController>();

    [Header("Geometry")]
    public EventPointsUpdate eventPointsUpdate;
    public Transform pointA;
    public Transform pointB;
    public LineRenderer lr1;
    public LineRenderer lr2;
    public Color trackLineColorFrom;
    public Color trackLineColorTo;

    [Header("Selection")]
    public bool _TrackIsBeingTouched = false;
    public bool _TrackIsBeingDragged = false;
    public float firstTrackInputMouseY;
    public float firstElementInputMouseY;
    public float tempTrackMouseY;
    public float mouseDirection = 0;

    public ElementController controllerSelected;
    public bool isExpanded = false;

    [Header("Chapter Data")]
    [SerializeField]
    public TimelineMedias timelineMedias;
    [SerializeField]
    public TimelineElements timelineElements;

    [Header("UI Panel")]
    public UIChapterController uicc;
    List<GameObject> uiTopPanelImagesList = new List<GameObject>();


    #region listener subscribers / unsubscribers

    private void OnEnable()
    {
        eventPointsUpdate.AddListener(UpdatePoints);
        EventManager.instance.selectTimelineElement.AddListener(OnSelectElement);
        EventManager.instance.closeTopPanel.AddListener(closeTopPanel);
    }

   

    private void OnDisable()
    {
        eventPointsUpdate.RemoveListener(UpdatePoints);
        EventManager.instance.selectTimelineElement.RemoveListener(OnSelectElement);
        EventManager.instance.closeTopPanel.RemoveListener(closeTopPanel);
    }

    #endregion

    private void closeTopPanel(UIChapterController closedUIcc)
    {
        if (closedUIcc == uicc)
        {
            isExpanded = false;
        }
    }

    private void OnSelectElement(ChapterController cc, ElementController ec)
    {
        if (cc != this) return;
        
        isExpanded = true;
        cc.controllerSelected = ec;
        cc.isExpanded = true;
        ec.isExpanded = true;
        cc.uicc.TopPanelYear.text = ec.from.ToString();
        cc.uicc.TopPanelTitle.text = ec.elementType.ToString();
        cc.uicc.TextScroll.text = ec.text_DE;
        cc.uicc.rtTopPanel.gameObject.SetActive(true);
     
        for (int i = 0; i < uiTopPanelImagesList.Count; i++)
        {

            Destroy(uiTopPanelImagesList[i]);
        }
        uiTopPanelImagesList.Clear();
        List<TimelineMedia> images = timelineMedias.GetMedia(cc.chapterId, ec.from, ec.to, ec.elementType, MediaType.Image);
        
        
        //Load Images
        for (int i = 0; i < images.Count; i++)
        {
            GameObject imageObject = Instantiate(uicc.scrollImagePrefab, uicc.rtImageScroll.transform);
            uiTopPanelImagesList.Add(imageObject);
            RawImage ri = imageObject.GetComponent<RawImage>();
            ri.texture = BuilderManager.instance.getImageTexture(images[i]);
            RectTransform rt = uicc.rtImageScroll.GetComponent<RectTransform>();
            float fixedHeight =  rt.sizeDelta.y;
            //we keep the same height and we play with the width
            LayoutElement loe = imageObject.GetComponent<LayoutElement>();
            if (images[i].width>= images[i].height)
            {
                loe.preferredWidth = fixedHeight * images[i].width / images[i].height;
                //Debug.Log("prefered width " + fixedHeight * images[i].width / images[i].height);
            }
            if (images[i].height > images[i].width)
            {
                loe.preferredWidth = fixedHeight * images[i].width / images[i].height;
            }



        }
        //TODO
        //    AnimationManager.instance.ExpandTimelineElement(cc);

    }

    internal void updateBottomUIScroll()
    {
        float value = InverseLerp(pointB.position, pointA.position, elementsTransform.position)-1f;
        //Debug.Log("value "+ value);
        uicc._sliderValue = value;
        
        uicc.UpdateBotomSlider(value);

    }

    public static float InverseLerp(Vector3 a, Vector3 b, Vector3 value)
    {
        Vector3 AB = b - a;
        Vector3 AV = value - a;
        return Vector3.Dot(AV, AB) / Vector3.Dot(AB, AB);
    }

    private void Start()
    {
        lr1.startWidth = 0.005f;
        lr1.endWidth=0.005f; lr1.startWidth = 0.005f;
        lr1.startColor = trackLineColorFrom;
        lr1.endColor = trackLineColorTo;

        lr2.startWidth = 0.005f;
        lr2.endWidth = 0.005f;
        lr2.startColor = trackLineColorFrom;
        lr2.endColor = trackLineColorTo;

    }

    private void UpdatePoints(Transform pointA, Transform pointB)
    {
        EventManager.instance.eventTrackGeometryUpdate.Invoke(pointA, pointB, this);
        lr1.SetPosition(0, pointA.transform.position - Vector3.right * 0.5f);
        lr1.SetPosition(1, pointB.transform.position - Vector3.right * 0.5f);
        lr2.SetPosition(0, pointA.transform.position + Vector3.right * 0.5f);
        lr2.SetPosition(1, pointB.transform.position + Vector3.right * 0.5f);
 
    }

    #region mouse events
    
    public void TrackOnMouseDown(float mouseYpos)
    {
        MouseManager.instance.TrackOnMouseDown(mouseYpos, this);
    
    }
    public void TrackOnMouseDrag(float mouseYpos)
    {
        MouseManager.instance.TrackOnMouseDrag(mouseYpos, this);
    }
    
    public void TrackOnMouseUp(float mouseYpos)
    {
        MouseManager.instance.TrackOnMouseUp(mouseYpos, this);

    }

    internal void ElementOnMouseDown(float y, ElementController ec)
    {
        MouseManager.instance.ElementOnMouseDown(y,ec, this);

    }

    internal void ElementOnMouseDrag(float y, ElementController ec)
    {
        MouseManager.instance.ElementOnMouseDrag(y, ec, this);
        
    }

    internal void ElementOnMouseUp(float y, ElementController ec)
    {
        MouseManager.instance.ElementOnMouseUp(y, ec, this);
    }

    #endregion




















}
