using UnityEngine;
using UnityEngine.Advertisements;

public class TestAds : MonoBehaviour
{
    [SerializeField] private string androidGameId = "5818303";
    [SerializeField] private string iosGameId = "5818302";
    [SerializeField] private string interstitialIdAndroid = "Interstitial_Android";
    [SerializeField] private string rewardIdAndroid = "Rewarded_Android";
    [SerializeField] private string interstitialIdIOS = "Interstitial_iOS";
    [SerializeField] private string rewardIdIos = "Rewarded_iOS";
    [SerializeField] private bool testMode = true;

    private string gameId;
    private string interstitialAdUnitId;
    private string rewardedAdUnitId;

    private bool isInterstitialReady = false;
    private bool isRewardedReady = false;

    void Start()
    {
        // Define os IDs de acordo com a plataforma
        gameId = (Application.platform == RuntimePlatform.IPhonePlayer) ? iosGameId : androidGameId;
        interstitialAdUnitId = (Application.platform == RuntimePlatform.IPhonePlayer) ? interstitialIdIOS : interstitialIdAndroid;
        rewardedAdUnitId = (Application.platform == RuntimePlatform.IPhonePlayer) ? rewardIdIos : rewardIdAndroid;

        // Inicializa Unity Ads
        Advertisement.Initialize(gameId, testMode, new InitializationListener(this));
    }

    public void ShowInterstitial()
    {
        if (isInterstitialReady)
        {
            Advertisement.Show(interstitialAdUnitId, new AdsShowListener(this, false));
            isInterstitialReady = false;  // Reseta o status até carregar novamente
        }
        else
        {
            Debug.Log("Anúncio intersticial ainda não carregou.");
        }
    }

    public void ShowRewarded()
    {
        if (isRewardedReady)
        {
            Advertisement.Show(rewardedAdUnitId, new AdsShowListener(this, true));
            isRewardedReady = false;  // Reseta o status até carregar novamente
        }
        else
        {
            Debug.Log("Anúncio recompensado ainda não carregou.");
        }
    }

    private class InitializationListener : IUnityAdsInitializationListener
    {
        private TestAds adsScript;
        public InitializationListener(TestAds script) { adsScript = script; }

        public void OnInitializationComplete()
        {
            Debug.Log("Unity Ads inicializado com sucesso!");

            // Após a inicialização, carrega os anúncios
            Advertisement.Load(adsScript.interstitialAdUnitId, new AdsLoadListener(adsScript, false));
            Advertisement.Load(adsScript.rewardedAdUnitId, new AdsLoadListener(adsScript, true));
        }

        public void OnInitializationFailed(UnityAdsInitializationError error, string message)
        {
            Debug.LogError($"Falha na inicialização do Unity Ads: {error} - {message}");
        }
    }

    private class AdsLoadListener : IUnityAdsLoadListener
    {
        private TestAds adsScript;
        private bool isRewarded;

        public AdsLoadListener(TestAds script, bool rewarded)
        {
            adsScript = script;
            isRewarded = rewarded;
        }

        public void OnUnityAdsAdLoaded(string placementId)
        {
            if (isRewarded)
            {
                adsScript.isRewardedReady = true;
                Debug.Log("Anúncio recompensado carregado!");
            }
            else
            {
                adsScript.isInterstitialReady = true;
                Debug.Log("Anúncio intersticial carregado!");
            }
        }

        public void OnUnityAdsFailedToLoad(string placementId, UnityAdsLoadError error, string message)
        {
            Debug.LogError($"Erro ao carregar anúncio {placementId}: {error} - {message}");
        }
    }

    private class AdsShowListener : IUnityAdsShowListener
    {
        private TestAds adsScript;
        private bool isRewarded;

        public AdsShowListener(TestAds script, bool rewarded)
        {
            adsScript = script;
            isRewarded = rewarded;
        }

        public void OnUnityAdsShowComplete(string placementId, UnityAdsShowCompletionState showCompletionState)
        {
            if (showCompletionState == UnityAdsShowCompletionState.COMPLETED)
            {
                Debug.Log($"O usuário assistiu ao anúncio {placementId} até o final.");
                if (isRewarded)
                {
                    Debug.Log("O jogador ganhou a recompensa!");
                }
            }

            // Após exibir o anúncio, recarregamos ele
            Advertisement.Load(placementId, new AdsLoadListener(adsScript, isRewarded));
        }

        public void OnUnityAdsShowFailure(string placementId, UnityAdsShowError error, string message)
        {
            Debug.LogError($"Erro ao exibir anúncio {placementId}: {error} - {message}");
        }

        public void OnUnityAdsShowStart(string placementId) { }
        public void OnUnityAdsShowClick(string placementId) { }
    }
}
