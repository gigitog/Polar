using System.Collections.Generic;
using DG.Tweening;
using Newtonsoft.Json;
using Polar;
using UnityEngine;
using UnityEngine.UI;

public class UIControllerLogin : MonoBehaviour
{
    [SerializeField] private InputField mail;
    [SerializeField] private InputField password;
    [SerializeField] private Button signInButton;
    [SerializeField] private GameObject textInvalid;
    [SerializeField] private List<Sprite> inputFieldsSprites;
    [SerializeField] private Image loginImage;
    [SerializeField] private Image passImage;
    [SerializeField] private GameObject loadingPanel;
    private CanvasGroup loadingPanelGroup;


    private void Start()
    {
        loadingPanelGroup = loadingPanel.GetComponent<CanvasGroup>();
        ServerController.instance.OnLogin += UI_OnLogin;
        ServerController.instance.OnProfile += UI_OnProfile;

        password.asteriskChar = '•';
        ServerController.instance.Profile();
    }

    private void OnDestroy()
    {
        ServerController.instance.OnLogin -= UI_OnLogin;
        ServerController.instance.OnProfile -= UI_OnProfile;
    }

    private void UI_OnLogin(object sender, RespondArgs args)
    {
        if (args.method != RequestMethod.Login) return;

        var answer = JsonConvert.DeserializeObject<LoginAnswer>(args.text);

        if (answer == null)
        {
            ShowError("Server error! We're fixing it(");
            Debug.Log("INTERNAL ERROR. SERVER GAVE EMPTY ANSWER");
            return;
        }

        if (answer.succeeded)
        {
            Debug.Log("OK! UI LOGIN" + JsonConvert.SerializeObject(answer));
            ServerController.instance.Profile();
            SceneLoader.LoadScene(1);
        }
        else
        {
            if (answer.errors[0].code != "InvalidCredentials") return;

            // show
            ShowError("Invalid login or password");

            // activate
            EnableButton();

            Debug.Log("Invalid ! UI LOGIN");
        }
    }

    // function to check token
    private void UI_OnProfile(object sender, RespondArgs args)
    {
        if (args.method != RequestMethod.Profile) return;

        if (!args.error)
        {
            Debug.Log("OK! Token correct! Main Menu");
            SceneLoader.LoadScene(1);
        }
        else
        {
            loadingPanelGroup.DOFade(0f, 1f).OnComplete(DeactivateLP);
            Debug.Log("ERROR! Token expired! Main Menu");
            ShowError("Session expired!");
        }
    }

    public void Login()
    {
        // UI
        if (IsFormEmpty())
        {
            ShowError("Fill all fields");
            return;
        }

        DisableButton();

        ServerController.instance.Login(new LoginRequest
        {
            login = mail.text,
            password = password.text
        });
    }

    private void DisableButton()
    {
        signInButton.interactable = false;
        signInButton.GetComponent<Image>().color = new Color(0.7f, 0.7f, 0.7f);
    }

    private void EnableButton()
    {
        signInButton.interactable = true;
        signInButton.GetComponent<Image>().color = new Color(1f, 1f, 1f);
    }

    private void ShowError(string error)
    {
        loginImage.sprite = inputFieldsSprites[0];
        passImage.sprite = inputFieldsSprites[1];
        textInvalid.GetComponent<Text>().text = error;
        textInvalid.SetActive(true);
    }

    private bool IsFormEmpty()
    {
        return mail.text.Length == 0 || password.text.Length == 0;
    }

    private void DeactivateLP()
    {
        loadingPanel.SetActive(false);
    }
}