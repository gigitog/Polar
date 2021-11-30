using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using Newtonsoft.Json;
using Polar;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIControllerMainMenu : MonoBehaviour
{
    [SerializeField] private Text username;
    [SerializeField] private Text markersCount;
    [SerializeField] private Text level; //TODO LevelCOunter
    [SerializeField] private RectTransform arrow;
    [SerializeField] private RectTransform arrowBox;
    [SerializeField] private GameObject loadingPanel;
    [SerializeField] private GameObject MarkersPanel;
    [SerializeField] private GameObject MarkersContent;

    [SerializeField] private GameObject AreaListComponentPrefab;
    private List<GameObject> areaListComponents;

    [SerializeField] private GameObject ratingPanel;
    [SerializeField] private List<Text> usernamesTexts;
    [SerializeField] private List<Text> scoresTexts;
    [SerializeField] private Text userScoreText;

    private Button arrowButton;

    private bool isArrowOpen;
    private CanvasGroup loadingPanelGroup;


    private void Start()
    {
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
        if (!Input.GetKeyUp(KeyCode.Escape)) return;
        var activity = new AndroidJavaClass("com.unity3d.player.UnityPlayer")
            .GetStatic<AndroidJavaObject>("currentActivity");
        activity.Call<bool>("moveTaskToBack", true);
    }

    private void OnDestroy()
    {
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
        MarkersPanel.SetActive(true);
    }

    public void MarkersClose()
    {
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
        SceneManager.LoadScene(2);
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
        areaListComponents = new List<GameObject>();
        for (var i = 0; i < areas.Count; i++)
        {
            var areaObj = Instantiate<GameObject>(AreaListComponentPrefab, Vector3.zero, Quaternion.identity,
                MarkersContent.transform);
            var startId = (i + 1) * areas[i].totalMarkers;
            areaObj.GetComponent<AreaListComponent>().SetDefaultArea(areas[i]);
            areaListComponents.Add(areaObj);
        }
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