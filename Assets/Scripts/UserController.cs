using System;
using Newtonsoft.Json;
using Polar;
using UnityEngine;

public class UserController : MonoBehaviour
{
    public User userData { get; private set; }
    public static UserController instance;
    public bool IsEmpty { get; private set; }
    private void Awake()
    {
        IsEmpty = true;
        SetInstance();
    }

    void Start() => ServerController.instance.OnProfile += U_OnProfile;

    private void U_OnProfile(object sender, RespondArgs e)
    {
        if (e.method == RequestMethod.Profile && !e.error)
        {
            var answer = JsonConvert.DeserializeObject<ProfileAnswer>(e.text);
            userData = new User(answer);
            IsEmpty = false;
            Debug.Log("New profile data! " + e.text);
        }
    }
    
    public int GetMarkersCount()
    {
        int counter = 0;
        for (int i = 0; i < userData.areas.Count; i++)
        {
            counter += userData.areas[i].markers.Count;
        }

        return counter;
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
}
