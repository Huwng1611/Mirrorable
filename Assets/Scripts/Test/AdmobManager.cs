using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GoogleMobileAds;
using GoogleMobileAds.Api;
using System;
using UnityEngine.SceneManagement;

public class AdmobManager : MonoBehaviour
{
    public static AdmobManager instance;

    public static string appID = "ca-app-pub-3746547126889687~9624707419";
    private BannerView bannerAd;
    private InterstitialAd interstitialdAd;
    private RewardBasedVideoAd rewardVideoAd;
    private bool videoIsDone;
    private void Awake()
    {
        if (!instance)
        {
            instance = this;
        }
        DontDestroyOnLoad(this);
    }

    private void Start()
    {
        RequestBanner();
        RequestInterstitial();
        RequestVideoAD();

        ShowRewardVideo();
    }

    void ShowRewardVideo()
    {
        // Get singleton reward based video ad reference.
        this.rewardVideoAd = RewardBasedVideoAd.Instance;

        // Called when an ad request has successfully loaded.
        rewardVideoAd.OnAdLoaded += HandleRewardBasedVideoLoaded;
        // Called when an ad request failed to load.
        rewardVideoAd.OnAdFailedToLoad += HandleRewardBasedVideoFailedToLoad;
        // Called when an ad is shown.
        rewardVideoAd.OnAdOpening += HandleRewardBasedVideoOpened;
        // Called when the ad starts to play.
        rewardVideoAd.OnAdStarted += HandleRewardBasedVideoStarted;
        // Called when the user should be rewarded for watching a video.
        rewardVideoAd.OnAdRewarded += HandleRewardBasedVideoRewarded;
        // Called when the ad is closed.
        rewardVideoAd.OnAdClosed += HandleRewardBasedVideoClosed;
        // Called when the ad click caused the user to leave the application.
        rewardVideoAd.OnAdLeavingApplication += HandleRewardBasedVideoLeftApplication;
    }

    void RequestBanner()
    {
        string bannerID = "ca-app-pub-3940256099942544/6300978111";
        bannerAd = new BannerView(bannerID, AdSize.SmartBanner, AdPosition.Top);

        //app thật
        //AdRequest request = new AdRequest.Builder().Build();

        //test
        AdRequest adRequest = new AdRequest.Builder().AddTestDevice("2077ef9a63d2b398840261c8221a0c9b").Build();
        bannerAd.LoadAd(adRequest);
    }

    void RequestInterstitial()
    {
        string interstitialdID = "ca-app-pub-3746547126889687/5346658857";
        interstitialdAd = new InterstitialAd(interstitialdID);

        //test
        AdRequest adRequest = new AdRequest.Builder().AddTestDevice("2077ef9a63d2b398840261c8221a0c9b").Build();
        interstitialdAd.LoadAd(adRequest);
    }

    void RequestVideoAD()
    {
        string videoID = "ca-app-pub-3940256099942544/1712485313";
        rewardVideoAd = RewardBasedVideoAd.Instance;

        //test
        AdRequest adRequest = new AdRequest.Builder().AddTestDevice("2077ef9a63d2b398840261c8221a0c9b").Build();

        rewardVideoAd.LoadAd(adRequest, videoID);
    }

    public void DisplayBanner()
    {
        bannerAd.Show();
    }

    public void DisplayInterstitialdAd()
    {
        if (interstitialdAd.IsLoaded())
        {
            interstitialdAd.Show();
        }
    }

    public void DisplayVideoAd()
    {
        if (rewardVideoAd.IsLoaded())
        {
            rewardVideoAd.Show();
        }
        else
        {
            Debug.LogWarning("<i>video reward not loaded.</i>");
        }
    }

    public void HandleOnAdLoaded(object sender, EventArgs args)
    {
        DisplayBanner();
    }

    public void HandleOnAdFailedToLoad(object sender, AdFailedToLoadEventArgs args)
    {
        RequestBanner();
    }

    public void HandleOnAdOpened(object sender, EventArgs args)
    {
        MonoBehaviour.print("HandleAdOpened event received");
    }

    public void HandleOnAdClosed(object sender, EventArgs args)
    {
        MonoBehaviour.print("HandleAdClosed event received");
    }

    public void HandleOnAdLeavingApplication(object sender, EventArgs args)
    {
        MonoBehaviour.print("HandleAdLeavingApplication event received");
    }

    public void HandleBannerADEvent(bool subscribe)
    {
        if (subscribe)
        {
            // Called when an ad request has successfully loaded.
            bannerAd.OnAdLoaded += HandleOnAdLoaded;
            // Called when an ad request failed to load.
            bannerAd.OnAdFailedToLoad += HandleOnAdFailedToLoad;
            // Called when an ad is clicked.
            bannerAd.OnAdOpening += HandleOnAdOpened;
            // Called when the user returned from the app after an ad click.
            bannerAd.OnAdClosed += HandleOnAdClosed;
            // Called when the ad click caused the user to leave the application.
            bannerAd.OnAdLeavingApplication += HandleOnAdLeavingApplication;
        }
        else
        {
            // Called when an ad request has successfully loaded.
            bannerAd.OnAdLoaded -= HandleOnAdLoaded;
            // Called when an ad request failed to load.
            bannerAd.OnAdFailedToLoad -= HandleOnAdFailedToLoad;
            // Called when an ad is clicked.
            bannerAd.OnAdOpening -= HandleOnAdOpened;
            // Called when the user returned from the app after an ad click.
            bannerAd.OnAdClosed -= HandleOnAdClosed;
            // Called when the ad click caused the user to leave the application.
            bannerAd.OnAdLeavingApplication -= HandleOnAdLeavingApplication;
        }
    }

    private void OnEnable()
    {
        HandleBannerADEvent(true);
    }

    private void OnDisable()
    {
        HandleBannerADEvent(false);
    }

    public void HandleRewardBasedVideoLoaded(object sender, EventArgs args)
    {
        MonoBehaviour.print("HandleRewardBasedVideoLoaded event received");
    }

    public void HandleRewardBasedVideoFailedToLoad(object sender, AdFailedToLoadEventArgs args)
    {
        MonoBehaviour.print(
            "HandleRewardBasedVideoFailedToLoad event received with message: "
                             + args.Message);
    }

    public void HandleRewardBasedVideoOpened(object sender, EventArgs args)
    {
        MonoBehaviour.print("HandleRewardBasedVideoOpened event received");
    }

    public void HandleRewardBasedVideoStarted(object sender, EventArgs args)
    {
        MonoBehaviour.print("HandleRewardBasedVideoStarted event received");
    }

    public void HandleRewardBasedVideoClosed(object sender, EventArgs args)
    {
        MonoBehaviour.print("HandleRewardBasedVideoClosed event received");
        //SceneManager.LoadScene("Menu");
        if (videoIsDone)
        {
            return;
        }
        else
        {
            UIManager.ui.GameOver();
        }
    }

    public void HandleRewardBasedVideoRewarded(object sender, Reward args)
    {
        string type = args.Type;
        double amount = args.Amount;
        MonoBehaviour.print(
            "HandleRewardBasedVideoRewarded event received for "
                        + amount.ToString() + " " + type);
        videoIsDone = true;
        SceneManager.LoadScene("Play");
    }

    public void HandleRewardBasedVideoLeftApplication(object sender, EventArgs args)
    {
        MonoBehaviour.print("HandleRewardBasedVideoLeftApplication event received");
    }
}
