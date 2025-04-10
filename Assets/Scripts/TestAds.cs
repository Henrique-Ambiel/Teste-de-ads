using UnityEngine;
using UnityEngine.Advertisements;
using System;

// Classe responsável por gerenciar os anúncios Unity Ads no jogo
public class TestAds : MonoBehaviour,
    IUnityAdsLoadListener,            // Interface para lidar com eventos de carregamento de anúncios
    IUnityAdsShowListener,            // Interface para lidar com eventos de exibição de anúncios
    IUnityAdsInitializationListener   // Interface para lidar com eventos de inicialização dos anúncios
{
    // IDs de anúncios para a plataforma Android
    [Header("---------- ANDROID IDs")]
    public string ANDROID_GAME_ID;
    public string ANDROID_INTERSTITIAL_ID = "Interstitial_Android";
    public string ANDROID_REWARDED_ID = "Rewarded_Android";

    // IDs de anúncios para a plataforma iOS
    [Header("---------- iOS IDs")]
    public string iOS_GAME_ID;
    public string iOS_INTERSTITIAL_ID = "Interstitial_iOS";
    public string iOS_REWARDED_ID = "Rewarded_iOS";

    // Variáveis que armazenarão os IDs corretos em tempo de execução
    private string GAME_ID;
    private string INTERSTITIAL_ID;
    private string REWARDED_ID;

    // Evento que será chamado quando um anúncio recompensado for completado com sucesso
    public event Action OnRewardedCompleted;

    // Método público que exibe um anúncio intersticial (anúncio simples, sem recompensa)
    public void ShowInterstitial()
    {
        // Mostra o anúncio intersticial usando o ID definido
        Advertisement.Show(INTERSTITIAL_ID, this);
    }

    // Método público que exibe um anúncio recompensado (dá algo ao jogador após assistir)
    public void ShowRewarded()
    {
        // Mostra o anúncio recompensado usando o ID definido
        Advertisement.Show(REWARDED_ID, this);
    }

    // Define os IDs corretos com base na plataforma (Android ou iOS)
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

    // Inicializa o sistema de anúncios Unity
    void Start()
    {
        // Mantém esse objeto entre as cenas
        DontDestroyOnLoad(this);

        // Se ainda não estiver inicializado e for suportado, inicia os anúncios
        if (!Advertisement.isInitialized && Advertisement.isSupported)
        {
            Advertisement.Initialize(GAME_ID, true, this);
        }
    }

    // Chamado quando a inicialização dos anúncios é concluída com sucesso
    public void OnInitializationComplete()
    {
        Debug.Log("OnInitializationComplete");

        // Carrega os anúncios intersticial e recompensado para deixá-los prontos
        Advertisement.Load(INTERSTITIAL_ID, this);
        Advertisement.Load(REWARDED_ID, this);
    }

    // Chamado caso a inicialização falhe
    public void OnInitializationFailed(UnityAdsInitializationError error, string message)
    {
        Debug.Log($"Unity Ads Initialization Failed: {error.ToString()} - {message}");
    }

    // Chamado quando um anúncio é carregado com sucesso
    public void OnUnityAdsAdLoaded(string placementId)
    {
        Debug.Log("OnUnityAdsAdLoaded: " + placementId);
    }

    // Chamado quando falha ao carregar um anúncio
    public void OnUnityAdsFailedToLoad(string placementId, UnityAdsLoadError error, string message)
    {
        Debug.Log($"Error loading Ad Unit: {placementId} - {error.ToString()} - {message}");
    }

    // Chamado quando o jogador clica no anúncio (opcional de usar)
    public void OnUnityAdsShowClick(string placementId)
    {
        // Sem ação no clique por enquanto
    }

    // Chamado quando um anúncio termina de ser exibido
    public void OnUnityAdsShowComplete(string placementId, UnityAdsShowCompletionState showCompletionState)
    {
        Debug.Log($"OnUnityAdsShowComplete {placementId}:{showCompletionState}");

        // Recarrega o anúncio após ser exibido, mantendo ele disponível para a próxima vez
        Advertisement.Load(placementId, this);

        // Se for um anúncio recompensado, exibido completamente, aciona a recompensa
        if (placementId == REWARDED_ID &&
            showCompletionState == UnityAdsShowCompletionState.COMPLETED &&
            OnRewardedCompleted != null)
        {
            OnRewardedCompleted();
        }
    }

    // Chamado quando falha ao exibir um anúncio
    public void OnUnityAdsShowFailure(string placementId, UnityAdsShowError error, string message)
    {
        Debug.Log($"Error showing Ad Unit {placementId}: {error.ToString()} - {message}");
    }

    // Chamado quando um anúncio começa a ser exibido (opcional de usar)
    public void OnUnityAdsShowStart(string placementId)
    {
        // Sem ação no início por enquanto
    }
}
