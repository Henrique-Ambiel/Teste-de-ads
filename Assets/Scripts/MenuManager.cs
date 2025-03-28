using UnityEngine;
using UnityEngine.UI;

public class MenuManager : MonoBehaviour
{

    public Text uiCoinsText;

    void Update()
    {
        uiCoinsText.text = "COINS: " + PlayerPrefs.GetInt("COINS");
    }
}
