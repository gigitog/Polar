using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using Newtonsoft.Json;
using Polar;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIControllerRegister : MonoBehaviour
{
    [SerializeField] private InputField mail;
    [SerializeField] private InputField username;
    [SerializeField] private InputField password;
    [SerializeField] private Button registerButton;
    [Header("Hello")]
    [SerializeField] private GameObject messageText;
    [SerializeField] private List<Sprite> inputFieldsSprites;
    [SerializeField] private Image loginImage;
    [SerializeField] private Image mailImage;
    [SerializeField] private Image passImage;
    [SerializeField] private GameObject loadingPanel;
    private CanvasGroup loadingPanelGroup;
    
    private string websiteUri = "https://polar-proj.westeurope.cloudapp.azure.com/";

    private string[] successMessages = new[] {"Successful registration", "Вдала реєстрація!"};
    private void Start()
    {
        loadingPanelGroup = loadingPanel.GetComponent<CanvasGroup>();
        loadingPanelGroup.DOFade(0f, 1f).OnComplete(DeactivateLP);
        ServerController.instance.OnRegister += UI_OnRegister;
        
        password.asteriskChar = '•';
    }

    private void OnDestroy()
    {
        ServerController.instance.OnRegister -= UI_OnRegister;
    }

    private void UI_OnRegister(object sender, RespondArgs args)
    {
        if (args.method != RequestMethod.Register) return;

        var answer = JsonConvert.DeserializeObject<RegisterAnswer>(args.text);

        if (answer == null)
        {
            ShowMessage("Server error!", true);
            Debug.Log("INTERNAL ERROR. SERVER GAVE EMPTY ANSWER");
            return;
        }

        if (answer.succeeded)
        {
            Debug.Log("OK! Registered " + JsonConvert.SerializeObject(answer));
            
            ShowMessage(successMessages[0], false);
            
            SceneManager.LoadScene(1); // load login scene
        }
        else
        {
            // show
            string errors = answer.errors.Aggregate("", (current, t) => current + (t.code + "\n"));

            ShowMessage($"{errors}", true);

            // activate
            EnableButton();
        }
    }

    public void RegisterButton()
    {
        // UI
        if (IsFormEmpty())
        {
            ShowMessage("Fill all fields", true);
            return;
        }

        DisableButton();

        ServerController.instance.Register(new RegisterRequest
        {
            UserName = username.text,
            Email = mail.text,
            Password = password.text
        });
    }

    private void DisableButton()
    {
        registerButton.interactable = false;
        registerButton.GetComponent<Image>().color = new Color(0.7f, 0.7f, 0.7f);
    }

    private void EnableButton()
    {
        registerButton.interactable = true;
        registerButton.GetComponent<Image>().color = new Color(1f, 1f, 1f);
    }

    private void ShowMessage(string message, bool isError)
    {
        if (isError)
        {
            mailImage.sprite = inputFieldsSprites[0];
            loginImage.sprite = inputFieldsSprites[1];
            passImage.sprite = inputFieldsSprites[2];
            messageText.GetComponent<Text>().color = new Color(0.99f, 0.77f, 0.61f);
        }
        else
        {
            mail.text = password.text = username.text = "";
            mailImage.sprite = inputFieldsSprites[3];
            loginImage.sprite = inputFieldsSprites[4];
            passImage.sprite = inputFieldsSprites[5];
            messageText.GetComponent<Text>().color = new Color(0.81f, 0.99f, 0.61f);
        }
        
        messageText.GetComponent<Text>().text = message;
        messageText.SetActive(true);
    }

    private bool IsFormEmpty()
    {
        return mail.text.Length == 0 || username.text.Length == 0 || password.text.Length == 0;
    }


    public void LoginButton()
    {
        SceneManager.LoadScene(1); // load login scene
    }
    
    public void ForgetButton()
    {
        Application.OpenURL(websiteUri);
    }
        
    private void DeactivateLP() => loadingPanel.SetActive(false);
    
}