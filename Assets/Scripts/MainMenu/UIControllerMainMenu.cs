using System;
using DG.Tweening;
using DG.Tweening.Plugins.Options;
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
    private Button arrowButton;

    private bool isArrowOpen;

    public void ToggleArrow()
    {
        isArrowOpen = !isArrowOpen;
    
        var time = 0.2f;
        arrowButton.interactable = false;
        var rotate = isArrowOpen? 180 : 0;
        arrow.DORotate(new Vector3(0, 0, rotate), time).OnComplete(SetInteractableTrue);
        var move = isArrowOpen? -126 : 126;
        if (isArrowOpen)
        {
            arrowBox.gameObject.SetActive(true);
            arrowBox.DOAnchorPosY(move, time);
        }
        else
        {
            arrowBox.DOAnchorPosY(move, time).OnComplete(SetActiveFalseBox);
        }
        
        
    }

    private void Start()
    {
        arrowButton = arrow.GetComponent<Button>();
        isArrowOpen = true;
        ToggleArrow();
        UserController.instance.OnGetUserData += SetData;
        UserController.instance.GetUserData();
        
    }

    public void Scan()
    {
        SceneManager.LoadScene(2);
    }

    private void SetInteractableTrue()
    {
        arrowButton.interactable = true;
    }

    private void SetActiveFalseBox()
    {
        arrowBox.gameObject.SetActive(false);
    }

    private void SetData(object sender, UserArgs args)
    {
        username.text = args.user.username;
        markersCount.text = $"Markers: {args.markersCount}/42";
    }

    private void OnDestroy()
    {
        
    }
}
