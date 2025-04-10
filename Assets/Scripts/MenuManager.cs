using UnityEngine;
using UnityEngine.UI;

public class MenuManager : MonoBehaviour
{
    public TestAds adsManager;
    public Text uiCoinsText;

    private void Start()
    {
        adsManager = FindAnyObjectByType<TestAds>();
        adsManager.OnRewardedCompleted += AdsManager_OnRewardedCompleted;
        UpdateCoinsText();
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
