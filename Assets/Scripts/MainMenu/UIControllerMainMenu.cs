using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using DG.Tweening;
using Newtonsoft.Json;
using Polar;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIControllerMainMenu : MonoBehaviour
{
    [Header("Header")]
    [SerializeField] private Text username;
    [SerializeField] private Text markersCount;
    [SerializeField] private Text level; //TODO LevelCOunter
    [SerializeField] private RectTransform arrow;
    [SerializeField] private RectTransform arrowBox;
    [SerializeField] private GameObject loadingPanel;
    
    [Header("Markers")]
    [SerializeField] private GameObject MarkersPanel;
    [SerializeField] private GameObject MarkersContent;
    [SerializeField] private GameObject markerStoryPanel;
    [SerializeField] private Text markerInfoText;
    [SerializeField] private GameObject AreaListComponentPrefab;
    private List<GameObject> areaListComponents;
    
    [Header("Rating")]
    [SerializeField] private GameObject ratingPanel;
    [SerializeField] private List<Text> usernamesTexts;
    [SerializeField] private List<Text> scoresTexts;
    [SerializeField] private Text userScoreText;

    private Button arrowButton;
    private bool isArrowOpen;
    private CanvasGroup loadingPanelGroup;

    private List<List<int>> markersIds;
    private bool isMarkersOpen;
    private bool isMarkerInfoOpen;
    private void Start()
    {
        MarkersClose();
        RatingClose();
        CloseMarkerStory();
        FadeLoadingPanel();
        
        arrowButton = arrow.GetComponent<Button>();
        isArrowOpen = true;
        SetMarkersDefaultData(MarkerController.instance.GetAreasCompressed());

        ToggleArrow();
        ServerController.instance.OnRating += SetRating;
        UserController.instance.OnGetUserData += SetUserData;
        UserController.instance.GetUserData();
    }

    private void Update()
    {
        if (isMarkersOpen && !isMarkerInfoOpen)
        {
            if (EventSystem.current.currentSelectedGameObject == null) return;
            string buttonName = EventSystem.current.currentSelectedGameObject.name;
            if (!buttonName.StartsWith("M_")) return;
            
            Debug.Log("Clicked " + buttonName);
            var matches = Regex.Matches(buttonName, "[0-9]+");
            if (matches.Count == 3)
            {
                (int, int) a = (int.Parse(matches[0].Value), int.Parse(matches[1].Value));
                OpenMarkerStory(a);
            }
        }
        

        if (Input.GetKeyUp(KeyCode.Escape))
        {
            var activity = new AndroidJavaClass("com.unity3d.player.UnityPlayer")
                .GetStatic<AndroidJavaObject>("currentActivity");
            activity.Call<bool>("moveTaskToBack", true);
        }
        
    }

    private void OnDestroy()
    {
        ServerController.instance.OnRating -= SetRating;
        UserController.instance.OnGetUserData -= SetUserData;
    }

    #region UI Buttons

    public void ToggleArrow()
    {
        isArrowOpen = !isArrowOpen;

        var time = 0.2f;
        arrowButton.interactable = false;
        var rotate = isArrowOpen ? 180 : 0;
        arrow.DORotate(new Vector3(0, 0, rotate), time).OnComplete(SetArrowButtonInteractableTrue);
        var move = isArrowOpen ? -126 : 126;
        if (isArrowOpen)
        {
            arrowBox.gameObject.SetActive(true);
            arrowBox.DOAnchorPosY(move, time);
        }
        else
        {
            arrowBox.DOAnchorPosY(move, time).OnComplete(SetActiveFalseArrowBox);
        }
    }

    public void MarkersOpen()
    {
        isMarkersOpen = true;
        MarkersPanel.SetActive(true);
    }

    public void MarkersClose()
    {
        isMarkersOpen = false;
        MarkersPanel.SetActive(false);
    }

    public void RatingOpen()
    {
        ServerController.instance.Rating();
        ratingPanel.SetActive(true);
    }

    public void RatingClose()
    {
        ratingPanel.SetActive(false);
    }

    public void ScanButton()
    {
        SceneManager.LoadScene(4); // load ar scene
    }
    
    public void OpenMarkerStory((int, int) id)
    {
        isMarkerInfoOpen = true;
        markerStoryPanel.SetActive(true);
        if (id.Item1 == -1) return;
        string text = MarkerController.instance.areas[id.Item1].markers[id.Item2].storyText;
        markerInfoText.text = text;
    }

    public void CloseMarkerStory()
    {
        isMarkerInfoOpen = false;
        markerInfoText.text = "No text";
        markerStoryPanel.SetActive(false);
    }

    public void Logout()
    {
        UserController.instance.Logout();
        SceneManager.LoadScene(1); // load login scene
    }
    
    #endregion

    #region Set data on scene

    private void SetArrowButtonInteractableTrue()
    {
        arrowButton.interactable = true;
    }

    private void SetActiveFalseArrowBox()
    {
        arrowBox.gameObject.SetActive(false);
    }

    private void SetUserData(object sender, UserArgs args)
    {
        username.text = args.user.username;
        markersCount.text = $"Markers: {args.markersCount}/{args.user.areas.Sum(a => a.totalMarkers)}";

        UpdateMarkersData(args.user.areas, MarkerController.instance.GetAreasCompressed());
    }

    private void SetMarkersDefaultData(List<Area> areas)
    {
        markersIds = new List<List<int>>();
        areaListComponents = new List<GameObject>();
        for (var i = 0; i < areas.Count; i++)
        {
            var areaObj = Instantiate<GameObject>(AreaListComponentPrefab, Vector3.zero, Quaternion.identity,
                MarkersContent.transform);
            var startId = (i + 1) * areas[i].totalMarkers;
            areaObj.GetComponent<AreaListComponent>().SetDefaultArea(i, areas[i], markersIds);
            areaListComponents.Add(areaObj);
        }
        Debug.Log("Created List");
    }

    private void UpdateMarkersData(List<Area> userAreas, List<Area> defaultAreas)
    {
        var i = 0;
        foreach (var area in areaListComponents)
        {

            area.GetComponent<AreaListComponent>().UpdateMarkers(userAreas[i], defaultAreas[i]);
            i++;
        }
    }

    private void SetRating(object sender, RespondArgs args)
    {
        // if (args.method != RequestMethod.Rating) return;
        //
        // if (!args.error)
        // {
        //     var answer = JsonConvert.DeserializeObject<RatingAnswer>(args.text);
        //
        //     for (int i = 0; i < 3; i++)
        //     {
        //         scoresTexts[i].text = ;
        //         usernamesTexts[i].text = ;
        //     }
        //     
        //     userScoreText.text = $"Your place: {answer.place}\nYour score: {answer.score}";
        // }
        // else
        // {
        //     
        // }
    }

    #endregion

    private void DeactivateLP()
    {
        loadingPanel.SetActive(false);
    }

    private void FadeLoadingPanel()
    {
        loadingPanelGroup = loadingPanel.GetComponent<CanvasGroup>();
        loadingPanelGroup.DOFade(0f, 1.5f).SetEase(Ease.OutCubic).OnComplete(DeactivateLP);
    }

    
    
    
}