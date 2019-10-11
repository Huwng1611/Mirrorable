using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Facebook.Unity;

public class FBAnalyticManager : MonoBehaviour
{
    public static FBAnalyticManager instance;
    void Awake()
    {
        if (FB.IsInitialized)
        {
            FB.ActivateApp();
        }
        else
        {
            //Handle FB.Init
            FB.Init(() =>
            {
                FB.ActivateApp();
            });
        }
        if (!instance)
        {
            instance = this;
        }
    }

    // Unity will call OnApplicationPause(false) when an app is resumed
    // from the background
    void OnApplicationPause(bool pauseStatus)
    {
        // Check the pauseStatus to see if we are in the foreground
        // or background
        if (!pauseStatus)
        {
            //app resume
            if (FB.IsInitialized)
            {
                FB.ActivateApp();
            }
            else
            {
                //Handle FB.Init
                FB.Init(() =>
                {
                    FB.ActivateApp();
                });
            }
        }
    }

    public void LoginFB()
    {
        var permission = new List<string>() { "public_profile", "email", "user_friends" };
        FB.LogInWithReadPermissions(permission);
    }

    public void LogoutFB()
    {
        FB.LogOut();
    }

    public void ShareOnFB()
    {
        FB.ShareLink(new System.Uri("https://www.google.com"), "", "", new System.Uri("https://www.google.com"));
    }
}