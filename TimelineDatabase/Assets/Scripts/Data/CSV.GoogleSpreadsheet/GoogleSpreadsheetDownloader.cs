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
using System.IO;
using UnityEngine;
using UnityEngine.Networking;

[CreateAssetMenu]
[System.Serializable]
public class GoogleSpreadsheetDownloader : ScriptableObject
{
    [Header("Gdocs Spreadsheet Content")]
    public string SpreadsheetId = "";
    public string TabId = "";

    [Header("Gdocs Spreadsheet Images")]
    public string imagesSpreadsheetId = "";
    public string imagesTabId = "";

    public string lastContent;
    public string lastImages;

    [Header("Download CSV")]
    private UnityWebRequest www;
    private UnityWebRequestAsyncOperation _asyncOperation;

    private string getTime()
    {
        return DateTime.Now.Year.ToString() + "." + DateTime.Now.Month.ToString() + "." + DateTime.Now.Day.ToString() + "." + DateTime.Now.Hour.ToString() + "." + DateTime.Now.Minute.ToString();
    }
   
    public void LogString(string message)
    {
        Debug.Log(message);
    }

    public void DownloadURL(string url)
    {
        LogString("...downloading " + url);
        www = UnityWebRequest.Get(url);
        _asyncOperation = www.SendWebRequest();
        _asyncOperation.completed += OnComplete;
    }

    public void DownloadContent()
    {
        
        string url = @"https://docs.google.com/spreadsheets/d/" + SpreadsheetId
            + "/export?format=csv"
            + "&gid=" + TabId;
        DownloadURL(url);


    }

    public void DownloadImages()
    {
       
        string url = @"https://docs.google.com/spreadsheets/d/" + imagesSpreadsheetId
            + "/export?format=csv"
            + "&gid=" + imagesTabId;

        DownloadURL(url);
    }

    private void OnComplete(AsyncOperation obj)
    {
        _asyncOperation.completed -= OnComplete;
        if (www.isHttpError||www.isNetworkError)
        {
            LogString("Error trying to update CSV database from google spreadsheet");
            LogString(www.error);
        }
        else
        {
            string responseHeader = www.GetResponseHeader("Content-Disposition").Replace("; ",";");
            
            string[] responseSplited = responseHeader.Split(";".ToCharArray());
            string filename = responseSplited[1].Split("=".ToCharArray())[1].Trim();
            char c = (char)34;
            if ((filename.StartsWith(c.ToString())) && filename.EndsWith(c.ToString()))
            {
                filename = filename.Substring(1, filename.Length - 2);
            }
            
            LogString("filename " + filename);
            filename = getTime() + "." + filename;
            
            File.WriteAllText("Assets/data/" + filename, www.downloadHandler.text);
            if (www.uri.ToString().Contains(TabId))
            {
                lastContent = www.downloadHandler.text;
            }
            else if (www.uri.ToString().Contains(imagesTabId))
            {
                lastImages  = www.downloadHandler.text;
            }
        }

    }
    
}
