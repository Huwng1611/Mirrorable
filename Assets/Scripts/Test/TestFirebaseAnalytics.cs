using Firebase;
using Firebase.Analytics;
using Firebase.Extensions;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestFirebaseAnalytics : MonoBehaviour
{
    private void Awake()
    {
        DontDestroyOnLoad(this);
    }
    // Start is called before the first frame update
    void Start()
    {
        FirebaseAnalytics.SetAnalyticsCollectionEnabled(true);
    }

    void EventSignUp()
    {
        FirebaseAnalytics.LogEvent(FirebaseAnalytics.EventSignUp);
    }

    void EventLogin()
    {
        FirebaseAnalytics.LogEvent(FirebaseAnalytics.EventLogin);
    }

    void EventShare()
    {
        FirebaseAnalytics.LogEvent(FirebaseAnalytics.EventShare);
    }

    void EventJoinGroup()
    {
        FirebaseAnalytics.LogEvent(FirebaseAnalytics.EventJoinGroup, FirebaseAnalytics.ParameterGroupId, "");
    }

    void EventSearch()
    {
        FirebaseAnalytics.LogEvent(FirebaseAnalytics.EventSearch);
    }
}
