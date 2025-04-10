using UnityEngine;
using UnityEngine.Advertisements;
using System;

public class TestAds : MonoBehaviour, 
    IUnityAdsLoadListener,
    IUnityAdsShowListener,
    IUnityAdsInitializationListener
{
    [Header("---------- ANDROID IDs")]
    public string ANDROID_GAME_ID;
    public string ANDROID_INTERSTITIAL_ID = "Interstitial_Android";
    public string ANDROID_REWARDED_ID = "Rewarded_Android";

    [Header("---------- iOS IDs")]
    public string iOS_GAME_ID;
    public string iOS_INTERSTITIAL_ID = "Interstitial_iOS";
    public string iOS_REWARDED_ID = "Rewarded_iOS";

    private string GAME_ID;
    private string INTERSTITIAL_ID;
    private string REWARDED_ID;

    public event Action OnRewardedCompleted;

    public void ShowInterstitial()
    {
        //esse evento deve ser chamado para mostrar um interstitial na tela
        Advertisement.Show(INTERSTITIAL_ID, this);
    }

    public void ShowRewarded()
    {
        //esse evento deve ser chamado para mostrar um rewarded na tela
        Advertisement.Show(REWARDED_ID, this);
    }

    void Awake()
    {
#if UNITY_ANDROID
        GAME_ID = ANDROID_GAME_ID;
        INTERSTITIAL_ID = ANDROID_INTERSTITIAL_ID;
        REWARDED_ID = ANDROID_REWARDED_ID;
#else
        GAME_ID = iOS_GAME_ID;
        INTERSTITIAL_ID = iOS_INTERSTITIAL_ID;
        REWARDED_ID = iOS_REWARDED_ID;
#endif
    }

    void Start()
    {
        DontDestroyOnLoad(this);
        if (!Advertisement.isInitialized && Advertisement.isSupported)
        {
            Advertisement.Initialize(GAME_ID, true, this);
        }
    }

    public void OnInitializationComplete()
    {
        //assim que completa o start dos Ads vamos carregar um
        //pra deixar prontinho pra ser mostrado
        Debug.Log("OnInitializationComplete");
        Advertisement.Load(INTERSTITIAL_ID, this);
        Advertisement.Load(REWARDED_ID, this);
    }

    public void OnInitializationFailed(UnityAdsInitializationError error, string message)
    {
        Debug.Log($"Unity Ads Initialization Failed: {error.ToString()} - {message}");
    }
    public void OnUnityAdsAdLoaded(string placementId)
    {
        Debug.Log("OnUnityAdsAdLoaded: " + placementId);
    }
    public void OnUnityAdsFailedToLoad(string placementId, UnityAdsLoadError error, string message)
    {
        Debug.Log($"Error loading Ad Unit: {placementId} - {error.ToString()} - {message}");
    }
    public void OnUnityAdsShowClick(string placementId)
    {

    }

    public void OnUnityAdsShowComplete(string placementId, UnityAdsShowCompletionState showCompletionState)
    {
        Debug.Log($"OnUnityAdsShowComplete {placementId}:{showCompletionState}");
        Advertisement.Load(placementId, this);

        if (placementId == REWARDED_ID &&
            showCompletionState == UnityAdsShowCompletionState.COMPLETED &&
            OnRewardedCompleted != null
            )
        {
            OnRewardedCompleted();
        }
    }

    public void OnUnityAdsShowFailure(string placementId, UnityAdsShowError error, string message)
    {
        Debug.Log($"Error showing Ad Unit {placementId}: {error.ToString()} - {message}");
    }

    public void OnUnityAdsShowStart(string placementId)
    {
    }
}
