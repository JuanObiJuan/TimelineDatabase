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


public class BuilderManager : MonoBehaviour
{
    public static BuilderManager instance;

    [Header("Configuration")]
    [SerializeField]
    TimelineConfiguration config;

    [Header("Data")]
    [SerializeField]
    TimelineMedias timelineMedias;

    [Header("UI Hook")]
    [SerializeField]
    private Transform UIParentPanel;


    private void Awake()
    {
        instance = this;
    }

 

    

    public ElementController BuildTitle(ChapterController cc)
    {
        
        GameObject elementObject = Instantiate(config.TitleElementPrefab, Vector3.zero, Quaternion.identity, null);
        ElementController ec = elementObject.GetComponent<ElementController>();
        ec.cc = cc;
        ec.elemenetControllerType = ElementControllerType.Title;
        elementObject.transform.name = "c"+ cc.chapterId+"_"+"title";
        //Data
        ec.uuid = "0";
        ec.chapterId = cc.chapterId;
        ec.text.text = cc.title_DE;
        ec.text_DE = cc.title_DE;
        ec.text_EN = cc.title_EN;
        ec.from = cc.from;
        ec.to = cc.to;
   
        return ec;
    }

   

    public Texture2D getImageTexture(TimelineMedia tlm) {
        return (Texture2D)Resources.Load(tlm.resourcePath+ tlm.filename) as Texture2D;
    }

    internal ElementController BuildTimelineMedia(TimelineMedia tlm, ChapterController cc)
    {
        GameObject elementObject = Instantiate(config.ImageElementPrefab, Vector3.zero, Quaternion.identity, null);
        ElementController ec = elementObject.GetComponent<ElementController>();
        ec.cc = cc;
        ec.elemenetControllerType = ElementControllerType.Media;
        elementObject.transform.name = "c" + tlm.chapterId + "_" + "media" + "_id_" + tlm.uuid;
        //data
        ec.uuid = tlm.uuid;
        ec.chapterId = tlm.chapterId;
        ec.elementType = tlm.elementType;
        ec.mediaType = tlm.mediaType;
        ec.from = tlm.from;
        ec.to = tlm.to;
       
        ec.path = tlm.resourcePath + tlm.filename;
        ec.mr.material.mainTexture = (Texture2D)Resources.Load(tlm.resourcePath+ tlm.filename) as Texture2D;
        ec.width = tlm.width;
        ec.height = tlm.height;
        //Debug.Log(ec.width+"x"+ ec.height);
        float aspectRatio = ec.width*1.0f / ec.height;
        if (aspectRatio>1)
        {
            ec.mr.transform.localScale = new Vector3(0.9f, 0.9f / aspectRatio, 1f);
        }
        else
        {
            ec.mr.transform.localScale = new Vector3(0.9f * aspectRatio, 0.9f , 1f);
        }
        if (ec.elementType == TimelineType.Type1)
        {
            ec.cornerRenderer.material.color = config.color1;
            ec.backgroundRenderer.material.color = config.color1;
        }
        else if (ec.elementType == TimelineType.Type3)
        {
            ec.cornerRenderer.material.color = config.color4;
            ec.backgroundRenderer.material.color = config.color4;
        }
        else if (ec.elementType == TimelineType.Type2)
        {
            ec.cornerRenderer.material.color = config.color3;
            ec.backgroundRenderer.material.color = config.color3;
        }


        return ec;
    }

    public ElementController BuildTimelineElement(TimelineElement tle, ChapterController cc)
    {
        
        GameObject elementObject = Instantiate(config.CollagePrefab, Vector3.zero, Quaternion.identity, null);
        ElementController ec = elementObject.GetComponent<ElementController>();
        ec.cc = cc;
        ec.elemenetControllerType = ElementControllerType.Collage;
        elementObject.transform.name = "collage_" + tle.chapterId + "_" + "element"+"_id_"+tle.uuid;
        ec.pivotOffset.localPosition += Vector3.right * UnityEngine.Random.Range(-0.3f, 0.3f);
        //Data
        ec.from = tle.from;
        ec.to = tle.to;
        ec.uuid = tle.uuid;
        ec.chapterId = tle.chapterId;
        ec.elementType = tle.type;
        ec.collageTitle.text = tle.type.ToString();
        ec.collageYear.text = tle.from.ToString();
 
        ec.text_DE = tle.text_DE;
        ec.text_EN = tle.text_EN;

        if (ec.elementType==TimelineType.Type1)
        {
            ec.backgroundRenderer.material.color = config.color1;
            ec.cornerRenderer.material.color = config.color1;
            //ec.transform.GetChild(0).GetChild(0)
        }
        else if (ec.elementType == TimelineType.Type3)
        {
            ec.backgroundRenderer.material.color = config.color4;
            ec.cornerRenderer.material.color = config.color4;
        }
        else if (ec.elementType == TimelineType.Type2)
        {
            ec.backgroundRenderer.material.color = config.color3;
            ec.cornerRenderer.material.color = config.color3;
        }

        List<TimelineMedia> images = timelineMedias.GetMedia(tle.chapterId, tle.from, tle.to, tle.type, MediaType.Image);
        int iamgesToLoad = 3;
        if (images.Count<3)
        {
            iamgesToLoad = images.Count;
        }
        for (int i = 0; i < iamgesToLoad; i++)
        {
            ec.mrArray[i].material.mainTexture = getImageTexture(images[i]);
            
            
            if (images[i].width>images[i].height)
            {
                //reduce proportionally the local scale Y
                float ratio = 1.0f * images[i].width / images[i].height;

                ec.mrArray[i].transform.localScale = new Vector3(ec.mrArray[i].transform.localScale.x, ec.mrArray[i].transform.localScale.x / ratio, ec.mrArray[i].transform.localScale.z);
            }
            else if (images[i].width < images[i].height)
            {
                float ratio = 1.0f * images[i].height / images[i].width;
                ec.mrArray[i].transform.localScale = new Vector3(ec.mrArray[i].transform.localScale.y / ratio, ec.mrArray[i].transform.localScale.y, ec.mrArray[i].transform.localScale.z);

            }
        }
        return ec;
    }

    public ElementController BuildCollageElement(ChapterController cc, string title, List<ElementController> mediaList)
    {
   
        GameObject elementObject = Instantiate(config.CollagePrefab, Vector3.zero, Quaternion.identity, null);
        ElementController ec = elementObject.GetComponent<ElementController>();
        ec.cc = cc;
        ec.elemenetControllerType = ElementControllerType.Collage;
        ec.text.text = title;
        for (int i = 0; i < 3; i++)
        {
            
            if (mediaList[i]!=null && mediaList[i].mr!=null)
            {
                
                ec.mrArray[i].material.mainTexture = mediaList[i].mr.material.mainTexture;
            }
            else
            {
                ec.mrArray[i].enabled = false;
            }
            
        }
    
        return ec;
    }

   

    public void ApplyInitialChapterOffset(ChapterController[] chapterControllers, float distance)
    {


        
        for (int i = 0; i < chapterControllers.Length; i++)
        {

            float delta = 1.0f / chapterControllers.Length;
            
            Vector3 reference = new Vector3(config.curve.Evaluate(delta * i + delta * 0.5f)* config.spreadFactor, -12f, 173);
            Vector3 direction = reference - Vector3.zero;
            chapterControllers[i].pointB.localPosition = direction.normalized * chapterControllers[i].elements.Count * distance;
        }
    }

}
