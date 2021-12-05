using System;
using DG.Tweening;
using Polar;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartController : MonoBehaviour
{
    [SerializeField] private CanvasGroup loadingPanel;

    private void Start()
    {
        MarkerController.instance.onMarkersGot += OnCheck;
    }

    private void OnDestroy()
    {
        MarkerController.instance.onMarkersGot -= OnCheck;
    }

    private void OnCheck(object sender, ServerConnection args)
    {
        if (args.hasConnection)
        {
            // load login or tutorial scene
            SceneManager.LoadScene(UserController.instance.passedTutorial ? 1 : 5);
        }
        else
        {
            loadingPanel.DOFade(0f, 1f).OnComplete(DeactivateLP);
        }
    }

    public void Check()
    {
        SceneManager.LoadScene(0); // reload this scene
    }
    
    private void DeactivateLP() => loadingPanel.gameObject.SetActive(false);
}
