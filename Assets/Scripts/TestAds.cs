using UnityEngine;
using UnityEngine.Advertisements;
using UnityEngine.UI;

// Classe responsável por gerenciar anúncios da Unity Ads
public class TestAds : MonoBehaviour, IUnityAdsInitializationListener, IUnityAdsLoadListener, IUnityAdsShowListener
{
    [SerializeField] private string androidGameId; // ID do jogo no Android
    [SerializeField] private string iosGameId; // ID do jogo no iOS
    [SerializeField] private string adUnitIdAndroid = "Interstitial_Android"; // Nome do Ad Unit para Android
    [SerializeField] private string adUnitIdIOS = "Interstitial_iOS"; // Nome do Ad Unit para iOS
    [SerializeField] private Button adButton; // Referência ao botão que ativa o anúncio

    private string gameId; // ID do jogo que será definido conforme a plataforma
    private string adUnitId; // ID do anúncio que será carregado
    private bool isAdReady = false; // Flag que indica se o anúncio está pronto para exibição

    private void Awake()
    {
        // Define o Game ID e o Ad Unit ID de acordo com a plataforma em que o jogo está rodando
        gameId = (Application.platform == RuntimePlatform.IPhonePlayer) ? iosGameId : androidGameId;
        adUnitId = (Application.platform == RuntimePlatform.IPhonePlayer) ? adUnitIdIOS : adUnitIdAndroid;

        // Inicializa a Unity Ads
        Advertisement.Initialize(gameId, true, this);
    }

    // Callback chamado quando a inicialização da Unity Ads é concluída com sucesso
    public void OnInitializationComplete()
    {
        Debug.Log("Unity Ads inicializado com sucesso.");
        Advertisement.Load(adUnitId, this); // Carrega o anúncio após a inicialização
    }

    // Callback chamado quando a inicialização da Unity Ads falha
    public void OnInitializationFailed(UnityAdsInitializationError error, string message)
    {
        Debug.LogError($"Falha na inicialização dos Ads: {error} - {message}");
    }

    // Callback chamado quando um anúncio é carregado com sucesso
    public void OnUnityAdsAdLoaded(string placementId)
    {
        if (placementId == adUnitId)
        {
            Debug.Log("Anúncio carregado com sucesso.");
            isAdReady = true; // Define que o anúncio está pronto para ser exibido
            adButton.interactable = true; // Habilita o botão de anúncio
        }
    }

    // Callback chamado quando um anúncio falha ao carregar
    public void OnUnityAdsFailedToLoad(string placementId, UnityAdsLoadError error, string message)
    {
        Debug.LogError($"Erro ao carregar anúncio {placementId}: {error} - {message}");
    }

    // Método para exibir o anúncio
    public void ShowAd()
    {
        if (isAdReady) // Verifica se o anúncio está pronto
        {
            Advertisement.Show(adUnitId, this); // Exibe o anúncio
        }
        else
        {
            Debug.LogWarning("Anúncio ainda não está pronto.");
        }
    }

    // Callback chamado quando o anúncio é concluído
    public void OnUnityAdsShowComplete(string placementId, UnityAdsShowCompletionState showCompletionState)
    {
        if (placementId == adUnitId && showCompletionState == UnityAdsShowCompletionState.COMPLETED)
        {
            Debug.Log("Anúncio concluído! Pode recompensar o jogador.");
            Advertisement.Load(adUnitId, this); // Recarrega o anúncio para futuras exibições
        }
    }

    // Callback chamado quando ocorre um erro na exibição do anúncio
    public void OnUnityAdsShowFailure(string placementId, UnityAdsShowError error, string message)
    {
        Debug.LogError($"Erro ao exibir anúncio {placementId}: {error} - {message}");
    }

    // Callback chamado quando o anúncio começa a ser exibido
    public void OnUnityAdsShowStart(string placementId)
    {
        Debug.Log("Anúncio começou.");
    }

    // Callback chamado quando o jogador clica no anúncio
    public void OnUnityAdsShowClick(string placementId)
    {
        Debug.Log("Anúncio foi clicado.");
    }
}
