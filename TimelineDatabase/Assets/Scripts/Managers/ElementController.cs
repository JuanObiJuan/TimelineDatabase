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
using TMPro;

public enum ElementControllerType { Collage = 1, Title = 2 , Subtitle = 3, Media  =4, Other=5}

public class ElementController : MonoBehaviour
{

    [Header("Common Settings")]
    public MeshRenderer backgroundRenderer;
    public MeshRenderer cornerRenderer;
    public string uuid;
    public int chapterId;
    public ChapterController cc;
    public ElementMouse em;
    public TextMeshPro text;
    public ElementControllerType elemenetControllerType;


    public int from;
    public int to;
    public string text_DE;
    public string text_EN;
    [Header("Image Element")]
    public MeshRenderer mr;

   
    public MediaType mediaType;

    public TextMeshProUGUI textTitle;
    public TextMeshProUGUI textContent;
    public TimelineType elementType;
    public int width;
    public int height;


    [Header("Collage Element")]
    [SerializeField]
    public TextMeshPro collageTitle;
    public TextMeshPro collageYear;
    public GameObject collagePreview;
    public MeshRenderer[] mrArray;
    internal string path;
    internal bool isExpanded;
    internal bool isBeingAnimated;
    public Transform pivotOffset;
}
