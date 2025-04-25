using TMPro;
using UnityEngine;
using UnityEngine.UI;

// Classe responsável por gerenciar o menu e atualizar a interface relacionada às moedas do jogador
public class MenuManager : MonoBehaviour
{
    public TestAds adsManager;
    public TMP_Text uiCoinsText;

    public Button buttonInterstitial;
    public Button buttonRewarded;
    public Button buttonShowBanner;
    public Button buttonHideBanner;

    private void Start()
    {
        adsManager = FindAnyObjectByType<TestAds>();
        adsManager.OnRewardedCompleted += AdsManager_OnRewardedCompleted;
        UpdateCoinsText();
    }
    private void Update()
    {
        buttonShowBanner.interactable = adsManager.bannerLoaded;
        buttonHideBanner.interactable = adsManager.bannerLoaded;
    }
    private void OnDestroy()
    {
        adsManager.OnRewardedCompleted -= AdsManager_OnRewardedCompleted;
    }

    private void AdsManager_OnRewardedCompleted()
    {
        //vamos premiar o jogador pois assistiu um video rewarded
        int COINS = PlayerPrefs.GetInt("COINS") + 10;
        PlayerPrefs.SetInt("COINS", COINS);
        PlayerPrefs.Save();
        UpdateCoinsText();
    }

    private void UpdateCoinsText()
    {
        int COINS = PlayerPrefs.GetInt("COINS");
        uiCoinsText.text = "COINS: " + COINS;
    }
}
