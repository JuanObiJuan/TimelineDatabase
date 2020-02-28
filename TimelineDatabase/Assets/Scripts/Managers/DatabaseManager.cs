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
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;

public class DatabaseManager : MonoBehaviour
{
    public static DatabaseManager instance;

    [Header("CSV Text file from Google Spreadsheet")]
    public TextAsset csvChaptersAsset;
    public TextAsset csvAsset1;
    public TextAsset csvAsset2;
    public TextAsset csvAsset3;
    public TextAsset csvAsset4;

    [Header("Data")]
    [SerializeField]
    TimelineChapters timelineChapters;
    [SerializeField]
    TimelineElements elements1;
    [SerializeField]
    TimelineElements elements2;
    [SerializeField]
    TimelineElements elements3;
    [SerializeField]
    TimelineElements elementsAll;
    [SerializeField]
    TimelineMedias tlMedias;
    [SerializeField]
    TimelineMedias tlVideos;
    [SerializeField]
    TimelineMedias tlImages;

    [Header("Media")]
    public string pathToMedia;

    [Header("Events to Raise")]
    [SerializeField]
    GameEvent OnDatabaseReady;

    private void Start()
    {
        Invoke(nameof(CheckAndParse), 0.2f);
    }


    public void CheckAndParse()
    {
        if (DatabaseIsReady())
        {
            OnDatabaseReady.Raise();
        }
        else
        {
            ParseAndCheck();
        }


    }
    public bool DatabaseIsReady()
    {

        if (
           timelineChapters.elements.Count > 0
           && elements1.elements.Count > 0
           && elements2.elements.Count > 0
           && elements3.elements.Count > 0
           && tlMedias.elements.Count > 0
           && elementsAll.elements.Count > 0
           )
        {
            return true;
        }
        else
        {
            return false;
        }

    }

    public void ParseAndCheck()
    {
        ParseAll();
        if (DatabaseIsReady()&& OnDatabaseReady!=null)
        {
            OnDatabaseReady.Raise();

        }
        else
        {
            Debug.LogWarning("Database is not ready");
        }  
            
    }

    public void ParseAll()
    {
        ParseCSVChapters();
        ParseCSV01();
        ParseCSV02();
        ParseCSV03();
        ParseCSV04();
        elementsAll.ClearElements();
        elementsAll.elements.AddRange(elements3.elements);
        elementsAll.elements.AddRange(elements2.elements);
        elementsAll.elements.AddRange(elements1.elements);

    }

    private void ParseCSVChapters()
    {
        timelineChapters.ClearChapters();
        CSVChapter[] csvChapters = CSVParser.Deserialize<CSVChapter>(csvChaptersAsset.ToString());
        for (int i = 0; i < csvChapters.Length; i++)
        {
            TimeLineChapter c = new TimeLineChapter();
             c.id = csvChapters[i].chapter_id;
            c.from = csvChapters[i].from;
            c.to = csvChapters[i].to;
            c.title_DE = csvChapters[i].chapter_title_de;
            c.title_EN = csvChapters[i].chapter_title_en;
            c.subtitle_DE = csvChapters[i].chapter_subtitle_de;
            c.subtitle_EN = csvChapters[i].chapter_subtitle_en;
            timelineChapters.AddChapter(c);
        }
      
    }

    private void ParseCSV01()
    {
        elements1.ClearElements();
        CSVTimelineElement[] csvTLE = CSVParser.Deserialize<CSVTimelineElement>(csvAsset3.ToString());
        for (int i = 0; i < csvTLE.Length; i++)
        {
            TimelineElement element = new TimelineElement();

            element.type = TimelineType.Type2;
            element.chapterId = csvTLE[i].chapter_id;
            element.from = csvTLE[i].from;
            element.to = csvTLE[i].to;
            element.text_DE = csvTLE[i].text_de;
            element.text_EN = csvTLE[i].text_en;

            elements1.AddElement(element);
        }
       

       
    }

    private void ParseCSV02()
    {
        elements3.ClearElements();
        CSVTimelineElement[] csvTLE = CSVParser.Deserialize<CSVTimelineElement>(csvAsset1.ToString());
        for (int i = 0; i < csvTLE.Length; i++)
        {
            TimelineElement element = new TimelineElement();

            element.type = TimelineType.Type1;
            element.chapterId = csvTLE[i].chapter_id;
            element.from = csvTLE[i].from;
            element.to = csvTLE[i].to;
            element.text_DE = csvTLE[i].text_de;
            element.text_EN = csvTLE[i].text_en;

            elements3.AddElement(element);
        }

    }

    private void ParseCSV03()
    {
        elements2.ClearElements();
        CSVTimelineElement[] csvTLE = CSVParser.Deserialize<CSVTimelineElement>(csvAsset2.ToString());
        for (int i = 0; i < csvTLE.Length; i++)
        {
            TimelineElement element = new TimelineElement();

            element.type = TimelineType.Type3;
            element.chapterId = csvTLE[i].chapter_id;
            element.from = csvTLE[i].from;
            element.to = csvTLE[i].to;
            element.text_DE = csvTLE[i].text_de;
            element.text_EN = csvTLE[i].text_en;

            elements2.AddElement(element);
        }

    }

    private void ParseCSV04()
    {
        tlImages.ClearElements();
        CSVMediaElement[] csvME = CSVParser.Deserialize<CSVMediaElement>(csvAsset4.ToString());
        for (int i = 0; i < csvME.Length; i++)
        {
         
            string filenameWithNoExtension = Path.GetFileNameWithoutExtension(Application.dataPath + pathToMedia + csvME[i].filename);
            if (filenameWithNoExtension == null)
            {
                Debug.Log("file " + csvME[i].filename + " can not be found int hte image folder");
            }
            else
            {
            
            TimelineMedia mediaElement = new TimelineMedia();
            mediaElement.uuid = csvME[i].uuid;
            mediaElement.filename = filenameWithNoExtension;
            //subfolder under Resources?
            mediaElement.resourcePath = string.Concat(pathToMedia);
            mediaElement.chapterId = csvME[i].chapter_id;
            mediaElement.from = csvME[i].from;
            mediaElement.to = csvME[i].to;
            mediaElement.elementType = csvME[i].elementType=="type1"? TimelineType.Type1: csvME[i].elementType == "type2" ? TimelineType.Type3 : csvME[i].elementType == "type3" ? TimelineType.Type2 : TimelineType.Type4;
            mediaElement.width = csvME[i].width;
            mediaElement.height = csvME[i].height;
            mediaElement.size = csvME[i].size;
            mediaElement.description_DE = csvME[i].description_de;
            mediaElement.description_EN = csvME[i].description_en;
            mediaElement.copyright = csvME[i].copyright;

            tlImages.AddElement(mediaElement);
            }
        }

    }

   
}
