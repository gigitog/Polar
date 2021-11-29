using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Newtonsoft.Json;
using Polar;
using UnityEngine;
using UnityEngine.Networking;

public class ServerController : MonoBehaviour
{
    public static ServerController instance;

    public event EventHandler<RespondArgs> OnLogin;
    public event EventHandler<RespondArgs> OnRegister;
    public event EventHandler<RespondArgs> OnProfile;
    public event EventHandler<RespondArgs> OnRating;
    private string token;

    private string uriLogin = "https://polar-proj.westeurope.cloudapp.azure.com/api/user/login";
    private string uriProfile = "https://polar-proj.westeurope.cloudapp.azure.com/api/user/profile";
    private string uriRating = "https://polar-proj.westeurope.cloudapp.azure.com/api/home/rating";
    private string uriRegister = "https://polar-proj.westeurope.cloudapp.azure.com/api/home/register";
    
    private void Awake()
    {
        token = PlayerPrefs.GetString("Token");
        Debug.Log("Saved Token: " + token);
        
        OnLogin += Sc_OnLogin;
        SetInstance();
    }
    

    public void Register(RegisterRequest data)
    {
    
    }

    public void Login(LoginRequest data)
    {
        var body = JsonUtility.ToJson(data);
        
        UnityWebRequest request = new UnityWebRequest(uriLogin);
        request.uploadHandler   = new UploadHandlerRaw(Encoding.ASCII.GetBytes(body));
        request.downloadHandler = new DownloadHandlerBuffer();
        request.method          = UnityWebRequest.kHttpVerbPOST;
        request.SetRequestHeader("Content-Type", "application/json");
        
        StartCoroutine( SendRequest(request, RequestMethod.Login));
    }
    
    public void Profile()
    {
        // request
        UnityWebRequest request = new UnityWebRequest(uriProfile);
        request.method          = UnityWebRequest.kHttpVerbGET;
        request.downloadHandler = new DownloadHandlerBuffer();
        request.SetRequestHeader("Authorization", "Bearer " + token);

        StartCoroutine( SendRequest(request, RequestMethod.Profile));
    }
    
    public void Rating()
    {
        // request
        UnityWebRequest request = new UnityWebRequest(uriProfile);
        request.method          = UnityWebRequest.kHttpVerbGET;
        request.downloadHandler = new DownloadHandlerBuffer();

        StartCoroutine( SendRequest(request, RequestMethod.Profile));
    }
    
    private IEnumerator SendRequest(UnityWebRequest request, RequestMethod method)
    {
        // send it
        yield return request.SendWebRequest();
        
        var arg = new RespondArgs
        {
            method = method,
            text = request.downloadHandler.text,
            error = false
        };
        
        
        if(request.isNetworkError || request.isHttpError) {
            Debug.Log("ERROR!\n" + request.error);
            Debug.Log(request.downloadHandler.text);
            arg.error = true;
        }
        else 
        {
            // Show results as text
            Debug.Log("No errors\n" + request.downloadHandler.text);
        }
        
        InvokeEvent(this, arg);
    }                                                   

    private void Sc_OnLogin(object sender, RespondArgs args)
    {
        if (args.method == RequestMethod.Login)
        {
            LoginAnswer answer =  JsonConvert.DeserializeObject<LoginAnswer>(args.text);
            if (answer.token != null)
            {
                token = answer.token;
                Debug.Log("new Token " + token);
                PlayerPrefs.SetString("Token", token);
                PlayerPrefs.Save();
            }
        }
    }
    
    private void InvokeEvent(object sender, RespondArgs arg)
    {
        
        switch (arg.method)
        {
            case RequestMethod.Login:
                OnLogin?.Invoke(sender, arg);
                break;
            case RequestMethod.Register:
                OnRegister?.Invoke(sender, arg);
                break;
            case RequestMethod.Profile:
                OnProfile?.Invoke(sender, arg);
                break;
            case RequestMethod.Rating:
                OnRating?.Invoke(sender, arg);
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
    }
    
    private void SetInstance()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(instance);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void OnDestroy()
    {
        OnLogin -= Sc_OnLogin;
    }
    // public static string JsonPrettify(string json)
    // {
    //     using (var stringReader = new StringReader(json))
    //     using (var stringWriter = new StringWriter())
    //     {
    //         var jsonReader = new JsonTextReader(stringReader);
    //         var jsonWriter = new JsonTextWriter(stringWriter) { Formatting = Formatting.Indented };
    //         jsonWriter.WriteToken(jsonReader);
    //         return stringWriter.ToString();
    //     }
    // }
    
}
