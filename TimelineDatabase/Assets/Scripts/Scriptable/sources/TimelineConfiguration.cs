using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class TimelineConfiguration : ScriptableObject
{
    [Header("Chapters Included")]
    public bool loadAllChapters = false;
    public int loadChapterFrom = 0;
    public int loadChapterTo = 6;

    [Header("Initial Positions")]
    public AnimationCurve curve;
    public float spreadFactor;

    [Header("Prefabs")]
    public GameObject TrackElementPrefab;
    public GameObject TitleElementPrefab;
    public GameObject TextElementPrefab;
    public GameObject SubtitleElementPrefab;
    public GameObject ImageElementPrefab;
    public GameObject UIChapterPanelPrefab;
    public GameObject CollagePrefab;

    [Header("Elements Colors")]
    public Color color1;
    public Color color2;
    public Color color3;
    public Color color4;

    [Header("Line")]
    public Material lineMaterial;
    public Color lineColor;

    [Header("Language")]
    public Languages defaultLanguage;

    [Header("Chapters ScreenPosition")]
    public float marginToEdge = 0.5f;
    public float depthSeparation = 12.0f;

    [Header("Materials")]
    public Material imageMaterial;



}
