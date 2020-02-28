using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
[System.Serializable]
public class TimelineMedias : ScriptableObject
{
    [SerializeField]
    public List<TimelineMedia> elements = new List<TimelineMedia>();

    public void AddElement(TimelineMedia element) { elements.Add(element); }

    public void RemoveElement(TimelineMedia element) { elements.Add(element); }

    public void ClearElements() { elements.Clear(); }

    public List<TimelineMedia> GetMedia(int chapterId,int from, int to, TimelineType tlp, MediaType mt)
    {
        List<TimelineMedia> result = new List<TimelineMedia>();
        foreach (var media in elements)
        {
            if (media.chapterId == chapterId
               && media.elementType == tlp
               && media.mediaType == mt
               )
            {
                if (media.from >= from && media.from <= to)
                {
                    result.Add(media);
                }
            }
        }
        return result;
    }
    public List<TimelineMedia> GetMedia(int chapterId, MediaType mt)
    {
        List<TimelineMedia> result = new List<TimelineMedia>();
        foreach (var media in elements)
        {
            if (media.chapterId == chapterId && media.mediaType == mt)
            {
             result.Add(media);
            }
        }
        return result;
    }

}
