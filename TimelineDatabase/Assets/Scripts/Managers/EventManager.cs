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
using UnityEngine.Events;

[System.Serializable]
public class EventDatabaseReady : UnityEvent<List<TimeLineChapter>, List<TimelineElement>, List<TimelineMedia>>
{

}
[System.Serializable]
public class EventPointsUpdate : UnityEvent<Transform, Transform>
{

}
[System.Serializable]
public class EventTrackGeometryUpdate : UnityEvent<Transform, Transform, ChapterController>
{

}

[System.Serializable]
public class MouseTouchingTrack : UnityEvent<bool, ChapterController>
{
    
}
[System.Serializable]
public class MouseTouchingElement : UnityEvent<bool, ChapterController, ElementController>
{
   
}
[System.Serializable]
public class SelectTimelineElement : UnityEvent<ChapterController, ElementController>
{

}

[System.Serializable]
public class CloseTopPanel : UnityEvent<UIChapterController>
{

}






public class EventManager : MonoBehaviour
{
    public static EventManager instance;

    public EventDatabaseReady eventDatabaseReady;

    public EventTrackGeometryUpdate eventTrackGeometryUpdate;

    public MouseTouchingTrack mouseTouchingTrack;

    public MouseTouchingTrack mouseDraggingTrack;

    public MouseTouchingElement mouseTouchingElement;

    public SelectTimelineElement selectTimelineElement;

    public CloseTopPanel closeTopPanel;

    private void Awake()
    {
        instance = this;
    }
}
