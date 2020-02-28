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

[CreateAssetMenu]
public class BuilderController : ScriptableObject
{
    [Header("Configuration")]
    [SerializeField]
    TimelineConfiguration config;

    [Header("Data")]

    [SerializeField]
    TimelineMedias timelineMedias;
    [SerializeField]
    TimelineChapters timelineChapters;

   

    public ChapterController BuildChapter(int index)
    {
        Debug.Log("BuildChapter "+index);
        Debug.Log("timelineChapters total " + timelineChapters.elements.Count);
        
        //Gameobjects and Controller
        GameObject chapterObject = Instantiate(config.TrackElementPrefab, Vector3.zero, Quaternion.identity, null);
        ChapterController chapterController = chapterObject.GetComponent<ChapterController>();
        //Data
        chapterController.from = timelineChapters.elements[index].from;
        chapterController.to = timelineChapters.elements[index].to;
        chapterController.transform.name = "Chapter (" + timelineChapters.elements[index].id + ")";
        chapterController.chapterId = timelineChapters.elements[index].id;
        chapterController.title_DE = timelineChapters.elements[index].title_DE;
        chapterController.title_EN = timelineChapters.elements[index].title_EN;
        chapterController.subtitle_DE = timelineChapters.elements[index].subtitle_DE;
        chapterController.subtitle_EN = timelineChapters.elements[index].subtitle_EN;

        return chapterController;
    }

    public UIChapterController BuildUIChapterPanel(ChapterController chapterController, Transform parent)
    {
        GameObject panelObject = Instantiate(config.UIChapterPanelPrefab, Vector3.zero, Quaternion.identity, parent);
        UIChapterController uicc = panelObject.GetComponent<UIChapterController>();
        uicc.cc = chapterController;
        chapterController.uicc = uicc;
        //data
        uicc.BottomPanelYear.text = chapterController.from.ToString();
        uicc.BottomPanelTitle.text = chapterController.title_DE;


        return uicc;
    }

    public void BuildSubtitle(ChapterController cc)
    {
        GameObject elementObject = Instantiate(config.SubtitleElementPrefab, Vector3.zero, Quaternion.identity, cc.elementsTransform);
        ElementController ec = elementObject.GetComponent<ElementController>();
        ec.cc = cc;
        ec.elemenetControllerType = ElementControllerType.Subtitle;
        elementObject.transform.name = "c" + cc.chapterId + "_" + "subtitle";
        //Data
        ec.uuid = "1";
        ec.chapterId = cc.chapterId;
        ec.text.text = cc.subtitle_DE;
        ec.text_DE = cc.subtitle_DE;
        ec.text_EN = cc.subtitle_EN;
        ec.from = cc.from;
        ec.to = cc.to;
        cc.elements.Add(ec);
        
    }

}
