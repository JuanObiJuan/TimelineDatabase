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
using UnityEngine.UI;

public class UIChapterController : MonoBehaviour
{
    public ChapterController cc;
    [Header("Top Panel")]
    public RectTransform rtTopPanel;
    public TextMeshProUGUI TopPanelTitle;
    public TextMeshProUGUI TopPanelYear;
    public TextMeshProUGUI TextScroll;
    public RectTransform rtImageScroll;
    public GameObject scrollImagePrefab;
    public Button closeButton;

    [Header("Bottom Panel")]

    public TextMeshProUGUI BottomPanelTitle;
    public TextMeshProUGUI BottomPanelYear;
    public Slider BottomSlider;
    public Button[] buttons;
    public TextMeshProUGUI[] buttonstext;
    public float _sliderValue;
    // Start is called before the first frame update

    public void UpdateBotomSlider(float value) {
        //TODO this is the only way i can differenciate the change from drag a track from the change click in the scroll
        if (value== _sliderValue)
        {
            //This is an expected trigger
            //just update the slider
            BottomSlider.value = value;
        }
        else
        {
            AnimationManager.instance.MoveTrack(cc, value);
        }
        
    }

    private void OnEnable()
    {
        closeButton.onClick.AddListener(ClickOnCloseButton);
        BottomSlider.onValueChanged.AddListener(OnValueChanged);
        CloseTopPanel();
    }

    private void OnValueChanged(float value)
    {
        //can be the auto-update or can be because it was tocuhed by the user
        UpdateBotomSlider(value);
    }

    private void OnDisable()
    {
        closeButton.onClick.RemoveListener(ClickOnCloseButton);
    }

    private void ClickOnCloseButton()
    {
        EventManager.instance.closeTopPanel.Invoke(this);
        cc.isExpanded = false;
        CloseTopPanel();
    }

    public void CloseTopPanel()
    {
        
        rtTopPanel.gameObject.SetActive(false);

    }
    
    
}
