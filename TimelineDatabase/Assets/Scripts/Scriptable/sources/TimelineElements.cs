using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
[System.Serializable]
public class TimelineElements : ScriptableObject
{
    [SerializeField]
    public List<TimelineElement> elements  =new List<TimelineElement>();

    public void AddElement(TimelineElement element){ elements.Add(element); }
    public void RemoveElement(TimelineElement element){ elements.Add(element); }
    public void ClearElements() { elements.Clear(); }
    public List<TimelineElement> GetElements(int chapterId, TimelineType tlp)
    {
        List<TimelineElement> response = new List<TimelineElement>();
        foreach (var item in elements)
        {
            if (item.chapterId==chapterId && tlp.Equals(item.type))
            {
                response.Add(item);
            }
        }
        return response;
    }

}
