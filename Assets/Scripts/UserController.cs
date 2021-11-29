using System;
using System.Linq;
using Newtonsoft.Json;
using Polar;
using UnityEngine;

public class UserController : MonoBehaviour
{
    public static UserController instance;
    private bool isEmpty;
    public event EventHandler<UserArgs> OnGetUserData;
    private User userData;
    
    private void Awake()
    {
        isEmpty = true;
        SetInstance();
    }

    private void Start() => ServerController.instance.OnProfile += U_OnProfile;

    public void GetUserData()
    {
        if (isEmpty)
        { // send request
            ServerController.instance.Profile();
        }
        else
        { // invoke event, which gives User's Data
            OnGetUserData?.Invoke(this, new UserArgs(){user = userData, markersCount = GetMarkersCount()});
        }
    }
    
    private void U_OnProfile(object sender, RespondArgs e)
    {
        if (e.method != RequestMethod.Profile || e.error) return;
        
        // get answer from args and deserialize
        var answer = JsonConvert.DeserializeObject<ProfileAnswer>(e.text);
        userData = new User(answer);
        isEmpty = false;
        // invoke event because data was changed
        OnGetUserData?.Invoke(this, new UserArgs(){user = userData, markersCount = GetMarkersCount()});
        Debug.Log("Updated profile data! " + e.text);
    }

    private int GetMarkersCount() => userData.areas.Sum(t => t.markers.Count);

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
