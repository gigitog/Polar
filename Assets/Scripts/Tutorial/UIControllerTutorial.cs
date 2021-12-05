using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIControllerTutorial : MonoBehaviour
{
    
    public int index = 0;
    [SerializeField] private Button nextButton;
    [SerializeField] private GameObject loadingPanel;
    [SerializeField] private RectTransform tutorials;
    [SerializeField] private GameObject startButton;

    private void Start()
    {
        loadingPanel.GetComponent<CanvasGroup>().DOFade(0f, 1f).OnComplete(DeactivateLP);
    }

    public void NextButton()
    {
        if (index == 1) return;
        index += 1;
        tutorials.DOAnchorPosX(index * -360f, 0.3f);
        startButton.SetActive(true);
    }

    public void PrevButton()
    {
        if (index == 0) return;
        index -= 1;
        tutorials.DOAnchorPosX(index * 360f, 0.3f);
    }

    public void StartButton()
    {
        SceneManager.LoadScene(1); // load Login Scene
        UserController.instance.PassTutorial();
    }
    
    private void DeactivateLP() => loadingPanel.SetActive(false);
}
