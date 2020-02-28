using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
[System.Serializable]
public class TimelineChapters : ScriptableObject
{
    [SerializeField]
    public List<TimeLineChapter> elements  =new List<TimeLineChapter>();

    public void AddChapter(TimeLineChapter chapter)
    { elements.Add(chapter); }

    public void RemoveChapter(TimeLineChapter chapter)
    { elements.Add(chapter); }

    public void ClearChapters() { elements.Clear(); }
}
