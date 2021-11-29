using System;
using DG.Tweening;
using DG.Tweening.Plugins.Options;
using Newtonsoft.Json;
using Polar;
using UnityEngine;
using UnityEngine.UI;

public class UIControllerMainMenu : MonoBehaviour
{
    [SerializeField] private Text username;
    [SerializeField] private Text markersCount;
    [SerializeField] private Text level; //TODO LevelCOunter
    [SerializeField] private RectTransform arrow;
    [SerializeField] private RectTransform arrowBox;
    private Button arrowButton;

    private UserController uc;
    private bool isArrowOpen;

    private void Start()
    {
        isArrowOpen = false;
        arrowButton = arrow.GetComponent<Button>();
        uc = UserController.instance;

        if (UserController.instance.IsEmpty)
        {
            Debug.Log("ПРОИЗОШЕЛ КОЛЛАПС, ЮЗЕР ПУСТОЙ, А МЫ В МЕНЮ");
            ServerController.instance.Profile();
        }
        SetData();
    }

    public void ToggleArrow()
    {
        isArrowOpen = !isArrowOpen;
    
        var time = 0.2f;
        arrowButton.interactable = !isArrowOpen;
        arrow.DORotate(new Vector3(0, 0, 180), time).OnComplete(SetInteractableTrue);
        var move = isArrowOpen? -140 : 140;
        arrowBox.DOMoveY(move, time);
    }

    private void SetInteractableTrue()
    {
        arrowButton.interactable = true;
    }

    private void SetData()
    {
        username.text = uc.userData.username;
        markersCount.text = $"Markers: {uc.GetMarkersCount()}/42";
    }

    private void OnDestroy()
    {
        
    }
}
