using UnityEngine;
using UnityEngine.Advertisements;
using UnityEngine.UI;

// Classe respons�vel por gerenciar an�ncios da Unity Ads
public class TestAds : MonoBehaviour, IUnityAdsInitializationListener, IUnityAdsLoadListener, IUnityAdsShowListener
{
    [SerializeField] private string androidGameId; // ID do jogo no Android
    [SerializeField] private string iosGameId; // ID do jogo no iOS
    [SerializeField] private string adUnitIdAndroid = "Interstitial_Android"; // Nome do Ad Unit para Android
    [SerializeField] private string adUnitIdIOS = "Interstitial_iOS"; // Nome do Ad Unit para iOS
    [SerializeField] private Button adButton; // Refer�ncia ao bot�o que ativa o an�ncio

    private string gameId; // ID do jogo que ser� definido conforme a plataforma
    private string adUnitId; // ID do an�ncio que ser� carregado
    private bool isAdReady = false; // Flag que indica se o an�ncio est� pronto para exibi��o

    private void Awake()
    {
        // Define o Game ID e o Ad Unit ID de acordo com a plataforma em que o jogo est� rodando
        gameId = (Application.platform == RuntimePlatform.IPhonePlayer) ? iosGameId : androidGameId;
        adUnitId = (Application.platform == RuntimePlatform.IPhonePlayer) ? adUnitIdIOS : adUnitIdAndroid;

        // Inicializa a Unity Ads
        Advertisement.Initialize(gameId, true, this);
    }

    // Callback chamado quando a inicializa��o da Unity Ads � conclu�da com sucesso
    public void OnInitializationComplete()
    {
        Debug.Log("Unity Ads inicializado com sucesso.");
        Advertisement.Load(adUnitId, this); // Carrega o an�ncio ap�s a inicializa��o
    }

    // Callback chamado quando a inicializa��o da Unity Ads falha
    public void OnInitializationFailed(UnityAdsInitializationError error, string message)
    {
        Debug.LogError($"Falha na inicializa��o dos Ads: {error} - {message}");
    }

    // Callback chamado quando um an�ncio � carregado com sucesso
    public void OnUnityAdsAdLoaded(string placementId)
    {
        if (placementId == adUnitId)
        {
            Debug.Log("An�ncio carregado com sucesso.");
            isAdReady = true; // Define que o an�ncio est� pronto para ser exibido
            adButton.interactable = true; // Habilita o bot�o de an�ncio
        }
    }

    // Callback chamado quando um an�ncio falha ao carregar
    public void OnUnityAdsFailedToLoad(string placementId, UnityAdsLoadError error, string message)
    {
        Debug.LogError($"Erro ao carregar an�ncio {placementId}: {error} - {message}");
    }

    // M�todo para exibir o an�ncio
    public void ShowAd()
    {
        if (isAdReady) // Verifica se o an�ncio est� pronto
        {
            Advertisement.Show(adUnitId, this); // Exibe o an�ncio
        }
        else
        {
            Debug.LogWarning("An�ncio ainda n�o est� pronto.");
        }
    }

    // Callback chamado quando o an�ncio � conclu�do
    public void OnUnityAdsShowComplete(string placementId, UnityAdsShowCompletionState showCompletionState)
    {
        if (placementId == adUnitId && showCompletionState == UnityAdsShowCompletionState.COMPLETED)
        {
            Debug.Log("An�ncio conclu�do! Pode recompensar o jogador.");
            Advertisement.Load(adUnitId, this); // Recarrega o an�ncio para futuras exibi��es
        }
    }

    // Callback chamado quando ocorre um erro na exibi��o do an�ncio
    public void OnUnityAdsShowFailure(string placementId, UnityAdsShowError error, string message)
    {
        Debug.LogError($"Erro ao exibir an�ncio {placementId}: {error} - {message}");
    }

    // Callback chamado quando o an�ncio come�a a ser exibido
    public void OnUnityAdsShowStart(string placementId)
    {
        Debug.Log("An�ncio come�ou.");
    }

    // Callback chamado quando o jogador clica no an�ncio
    public void OnUnityAdsShowClick(string placementId)
    {
        Debug.Log("An�ncio foi clicado.");
    }
}
