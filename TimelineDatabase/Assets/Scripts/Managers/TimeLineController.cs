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
using TMPro;
using UnityEngine;



public class TimeLineController : MonoBehaviour
{

    [Header("Config")]
    [SerializeField]
    TimelineConfiguration config;
    [Header("Helpers")]
    [SerializeField]
    BuilderController builderController;
    [Header("Data")]
    [SerializeField]
    TimelineChapters timelineChapters;
    [SerializeField]
    TimelineElements timelineAll;
    [SerializeField]
    TimelineElements timelineElements01;
    [SerializeField]
    TimelineElements timelineElements02;
    [SerializeField]
    TimelineElements timelineElements03;
    [SerializeField]
    TimelineMedias timelineMedias;

    [Header("UI Hook")]
    [SerializeField]
    private Transform UIParentPanel;

    [Header("Chapters Controllers")]
    public ChapterController[] chapterControllers;
    public UIChapterController[] uiChapterPanels;

    [Header("Timeline Years")]
    public GameObject[] tlYearsObjects;
    public TMPro.TextMeshPro[] tlYearsText;
    public float yearTextWidth = 0.6f;
    public LineRenderer[] lineRenderers;

    public static TimeLineController instance;

    [Header("Camera position")]
    int pixelWidth, pixelHeight;
    public Vector3 middleLeft, middleRight;

    private void Awake()
    {
        instance = this;

        pixelWidth = Camera.main.pixelWidth;
        pixelHeight = Camera.main.pixelHeight;

        Vector3 screenPointMiddleLeft = new Vector3(0, pixelHeight * 0.5f, 0);
        Vector3 screenPointMiddleRigh = new Vector3(pixelWidth, pixelHeight * 0.5f, 0);
        middleLeft = Camera.main.ScreenToWorldPoint(screenPointMiddleLeft);
        middleRight = Camera.main.ScreenToWorldPoint(screenPointMiddleRigh);
    }

    private void OnEnable()
    {
       
        EventManager.instance.eventTrackGeometryUpdate.AddListener(TrackGeometryUpdate);
        
    }

    private void OnDisable()
    {
        
        EventManager.instance.eventTrackGeometryUpdate.RemoveListener(TrackGeometryUpdate);
    }

    private void Start()
    {
       
    }

    private void TrackGeometryUpdate(Transform pointA, Transform pointB, ChapterController chapterController)
    {
        RedistributeElements(pointA, pointB, chapterController);
    }

    private void RedistributeElements(Transform pointA, Transform pointB, ChapterController chapterController)
    {
       
        
        float delta = 1.0f / chapterController.elements.Count;
        
        for (int i = 0; i < chapterController.elements.Count; i++)
        {
            chapterController.elements[i].transform.position = Vector3.Lerp(pointA.transform.position,pointB.transform.position,i * delta);
        }
    }

    public void OnDatabaseReady()
    {
        LoadChapterControllers();
        LoadUIPanels();
        LoadSubtitles();
        LoadChaptersData();
        //LoadTimelineElements(timelineAll.elements);
        //LoadTimelineMedia(timelineMedias.elements);
        //LoadCollages();
        //SortTimelineElements();
        RedistributeChaptersHorizontally();
        ResizeTrackLengthBasedOnContent();
        
        ApplyInitialChapterOffset();
        
    }

    private void SortTimelineElements()
    {
        for (int i = 0; i < chapterControllers.Length; i++)
        {
           
            List<ElementController> elements01 = GetElementsPerType(chapterControllers[i], TimelineType.Type1, ElementControllerType.Collage);

           
            List<ElementController> elements02 = GetElementsPerType(chapterControllers[i], TimelineType.Type2, ElementControllerType.Collage);

            
            List<ElementController> elements03 = GetElementsPerType(chapterControllers[i], TimelineType.Type3, ElementControllerType.Collage);

            elements01.Reverse();
            elements02.Reverse();
            elements03.Reverse();

            if (elements01.Count==elements02.Count && elements01.Count == elements03.Count)
            {
                //All list should have the same length
                for (int j = 0; j < elements01.Count; j++)
                {
                    

                    chapterControllers[i].elements.Remove(elements03[j]);
                    chapterControllers[i].elements.Insert(1, elements03[j]);

                    chapterControllers[i].elements.Remove(elements02[j]);
                    chapterControllers[i].elements.Insert(1, elements02[j]);

                    chapterControllers[i].elements.Remove(elements01[j]);
                    chapterControllers[i].elements.Insert(1, elements01[j]);
                }
            }
            else
            {
                Debug.LogWarning("Update data base!! Parsing error ");
            }


        }
    }

    private List<ElementController> GetElementsPerType(ChapterController cc, TimelineType tlt, ElementControllerType ect) {
        List<ElementController> result = new List<ElementController>();
        for (int i = 0; i < cc.elements.Count; i++)
        {
            if (cc.elements[i].elementType == tlt && cc.elements[i].elemenetControllerType==ect)
            {
                result.Add(cc.elements[i]);
            }
        }
        return result;
    }

    private void ApplyInitialChapterOffset()
    {
        BuilderManager.instance.ApplyInitialChapterOffset(chapterControllers, config.depthSeparation);
    
    }

    private void LoadChapterControllers()
    {
        int chaptersIncluded;
        //Load all chapters?
        if (config.loadAllChapters)
        {
            chaptersIncluded = timelineChapters.elements.Count;
        }
        else
        {
            chaptersIncluded = config.loadChapterTo - config.loadChapterFrom + 1;
        }
        //make memory allocation
        chapterControllers = new ChapterController[chaptersIncluded];
        //Instantiate chapterControllers
        for (int i = 0; i < chaptersIncluded; i++)
        {
            int chapterdataIndex = i + config.loadChapterFrom - 1;
            chapterControllers[i] = builderController.BuildChapter(chapterdataIndex);
            chapterControllers[i].controllerId = i;
        }

    }

    private void LoadUIPanels()
    {
        //Memory allocation
        uiChapterPanels = new UIChapterController[chapterControllers.Length];
        //Instantiate one UI panel per chapter
        for (int i = 0; i < chapterControllers.Length; i++)
        {
            uiChapterPanels[i] = builderController.BuildUIChapterPanel(chapterControllers[i],UIParentPanel);
        }
      
    }

    private void SetMinimumTitleFontSize()
    {
        //Check minimum font
        float minimumTitleSize = 100000f;
        float minimumButtonSize = 10000f;
        for (int i = 0; i < uiChapterPanels.Length; i++)
        {
            //for title
            if (uiChapterPanels[i].BottomPanelTitle.fontSize < minimumTitleSize) minimumTitleSize = uiChapterPanels[i].BottomPanelTitle.fontSize;
            //and button pannels
            //TODO Fix this overload
            for (int j = 0; j < uiChapterPanels[i].buttonstext.Length; j++)
            {
                if (uiChapterPanels[i].buttonstext[j].fontSize < minimumButtonSize) minimumButtonSize = uiChapterPanels[i].buttonstext[j].fontSize;
            }
        }
        //Apply
        for (int i = 0; i < uiChapterPanels.Length; i++)
        {
            uiChapterPanels[i].BottomPanelTitle.enableAutoSizing = false;
            uiChapterPanels[i].BottomPanelTitle.fontSize = minimumTitleSize;
            for (int j = 0; j < uiChapterPanels[i].buttonstext.Length; j++)
            {
                uiChapterPanels[i].buttonstext[j].enableAutoSizing = false;
                uiChapterPanels[i].buttonstext[j].fontSize = minimumButtonSize;
            }
        }
    }

    private void LoadTimelineMedia(List<TimelineMedia> tlm)
    {
        //for every chapter
        for (int i = 0; i < chapterControllers.Length; i++)
        {
            //for every media
            for (int j = 0; j < tlm.Count; j++)
            {
                if (tlm[j].chapterId == chapterControllers[i].chapterId)
                {
                    ElementController element = BuilderManager.instance.BuildTimelineMedia(tlm[j], chapterControllers[i]);
                    chapterControllers[i].elements.Add(element);
                    element.transform.parent = chapterControllers[i].elementsTransform;
                }
            }
            
        }
        

     }

    private void LoadChaptersData() {
        for (int i = 0; i < chapterControllers.Length; i++)
        {
            chapterControllers[i].timelineElements = ScriptableObject.CreateInstance<TimelineElements>();
            chapterControllers[i].timelineElements.elements.AddRange(timelineElements03.GetElements(chapterControllers[i].chapterId, TimelineType.Type1));
            chapterControllers[i].timelineElements.elements.AddRange(timelineElements01.GetElements(chapterControllers[i].chapterId, TimelineType.Type2));
            chapterControllers[i].timelineElements.elements.AddRange(timelineElements02.GetElements(chapterControllers[i].chapterId, TimelineType.Type3));
            chapterControllers[i].timelineMedias = ScriptableObject.CreateInstance<TimelineMedias>();
            chapterControllers[i].timelineMedias.elements.AddRange(timelineMedias.GetMedia(chapterControllers[i].chapterId, MediaType.Image));
        }
    }

    private void LoadCollages()
    {
       
        
        int from = 1870;
        int to = 1876;
        
      
    }

    private List<ElementController> getMediaList(ChapterController cc, TimelineType type,  int from, int to)
    {
        List<ElementController> result = new List<ElementController>();
        foreach (var item in cc.elements)
        {
            if (item.elementType.Equals(type)& item.mediaType.Equals(MediaType.Image) &item.elemenetControllerType.Equals(ElementControllerType.Media))
            {
               
                if (item.from>=from && item.from<=to)
                {
                    result.Add(item);
                }
                
            }
        }
        return result;
    }

    public void LoadSubtitles()
    {
        for (int i = 0; i < chapterControllers.Length; i++)
        {
            builderController.BuildSubtitle(chapterControllers[i]);
        }
    }

    public void LoadTimelineElements(List<TimelineElement> elements)
    {

        for (int i = 0; i < chapterControllers.Length; i++)
        {

           
           
            
            //elements
            for (int j = 0; j < elements.Count; j++)
            {
                if (elements[j].chapterId== chapterControllers[i].chapterId)
                {
                    ElementController element = BuilderManager.instance.BuildTimelineElement(elements[j], chapterControllers[i]);
                    element.uuid= chapterControllers[i].elements.Count.ToString();
                    chapterControllers[i].elements.Add(element);
                    element.transform.parent = chapterControllers[i].elementsTransform;
                }
            }
           
        }
      
    }

    public void RedistributeChaptersHorizontally()
    {
        for (int i = 0; i < chapterControllers.Length; i++)
        {
            chapterControllers[i].transform.position = new Vector3(i, 0, 0);
        }

        //translate int 1 to int 7 to 0f and 1f
        float index = Mathf.InverseLerp(1f, 6f, chapterControllers.Length);
        Camera.main.transform.position = Vector3.Lerp(ConfiguratorManager.instance.originCameraPosition, ConfiguratorManager.instance.sixTracksPosition, index);
    }

    public void ResizeTrackLengthBasedOnContent()
    {
        //for every chapter space  timeline elements
        for (int i = 0; i < chapterControllers.Length; i++)
        {
            chapterControllers[i].pointB.localPosition = new Vector3(0, 0, chapterControllers[i].elements.Count * config.depthSeparation);
        }


    }

    public void RecenterTimelineElements()
    {
      
    }

    private List<ElementController> GetElementsPerType(List<ElementController> list, TimelineType tlt)
    {
        List<ElementController> tempList = new List<ElementController>();
        foreach (var item in list)
        {
            if (item.elementType == tlt)
            {
                tempList.Add(item);
            }
        }
        return tempList;

    }

    private void Update()
    {

        BuilderManager.instance.ApplyInitialChapterOffset(chapterControllers, config.depthSeparation);

    }








}
