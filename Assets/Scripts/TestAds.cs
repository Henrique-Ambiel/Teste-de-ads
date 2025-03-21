using UnityEngine;
using UnityEngine.Advertisements;
using UnityEngine.UI;

public class TestAds : MonoBehaviour, IUnityAdsInitializationListener, IUnityAdsLoadListener, IUnityAdsShowListener
{
    [SerializeField] private string androidGameId; // ID do jogo no Android
    [SerializeField] private string iosGameId; // ID do jogo no iOS
    [SerializeField] private string adUnitIdAndroid = "Interstitial_Android"; // Nome do Ad Unit no painel da Unity Ads
    [SerializeField] private string adUnitIdIOS = "Interstitial_iOS"; // Nome do Ad Unit no painel da Unity Ads
    [SerializeField] private Button adButton; // Botão para exibir o anúncio

    private string gameId;
    private string adUnitId;
    private bool isAdReady = false;

    private void Awake()
    {
        // Define o Game ID e o Ad Unit ID de acordo com a plataforma
        gameId = (Application.platform == RuntimePlatform.IPhonePlayer) ? iosGameId : androidGameId;
        adUnitId = (Application.platform == RuntimePlatform.IPhonePlayer) ? adUnitIdIOS : adUnitIdAndroid;

        // Inicializa Unity Ads
        Advertisement.Initialize(gameId, true, this);
    }

    public void OnInitializationComplete()
    {
        Debug.Log("Unity Ads inicializado com sucesso.");
        Advertisement.Load(adUnitId, this); // Carrega o anúncio
    }

    public void OnInitializationFailed(UnityAdsInitializationError error, string message)
    {
        Debug.LogError($"Falha na inicialização dos Ads: {error} - {message}");
    }

    public void OnUnityAdsAdLoaded(string placementId)
    {
        if (placementId == adUnitId)
        {
            Debug.Log("Anúncio carregado com sucesso.");
            isAdReady = true;
            adButton.interactable = true; // Ativa o botão quando o anúncio estiver pronto
        }
    }

    public void OnUnityAdsFailedToLoad(string placementId, UnityAdsLoadError error, string message)
    {
        Debug.LogError($"Erro ao carregar anúncio {placementId}: {error} - {message}");
    }

    public void ShowAd()
    {
        if (isAdReady)
        {
            Advertisement.Show(adUnitId, this);
        }
        else
        {
            Debug.LogWarning("Anúncio ainda não está pronto.");
        }
    }

    public void OnUnityAdsShowComplete(string placementId, UnityAdsShowCompletionState showCompletionState)
    {
        if (placementId == adUnitId && showCompletionState == UnityAdsShowCompletionState.COMPLETED)
        {
            Debug.Log("Anúncio concluído! Pode recompensar o jogador.");
            Advertisement.Load(adUnitId, this);
        }
    }

    public void OnUnityAdsShowFailure(string placementId, UnityAdsShowError error, string message)
    {
        Debug.LogError($"Erro ao exibir anúncio {placementId}: {error} - {message}");
    }

    public void OnUnityAdsShowStart(string placementId)
    {
        Debug.Log("Anúncio começou.");
    }

    public void OnUnityAdsShowClick(string placementId)
    {
        Debug.Log("Anúncio foi clicado.");
    }
}
