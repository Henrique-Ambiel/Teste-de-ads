using TMPro;
using UnityEngine;
using UnityEngine.UI;

// Classe responsável por gerenciar o menu e atualizar a interface relacionada às moedas do jogador
public class MenuManager : MonoBehaviour
{
    // Referência ao gerenciador de anúncios (ads)
    public TestAds adsManager;

    // Referência ao texto da UI que exibe a quantidade de moedas
    public TextMeshProUGUI uiCoinsText;

    // Método chamado automaticamente quando o objeto é iniciado na cena
    private void Start()
    {
        // Procura automaticamente um objeto do tipo TestAds na cena e armazena a referência
        adsManager = FindAnyObjectByType<TestAds>();

        // Adiciona o método AdsManager_OnRewardedCompleted como ouvinte do evento de recompensa do anúncio
        adsManager.OnRewardedCompleted += AdsManager_OnRewardedCompleted;

        // Atualiza o texto das moedas na interface assim que o jogo inicia
        UpdateCoinsText();
    }

    // Método chamado automaticamente quando o objeto é destruído (ex: ao sair da cena)
    private void OnDestroy()
    {
        // Remove o método do evento para evitar erros ou vazamentos de memória
        adsManager.OnRewardedCompleted -= AdsManager_OnRewardedCompleted;
    }

    // Método executado quando o jogador completa um anúncio recompensado
    private void AdsManager_OnRewardedCompleted()
    {
        // Recupera o valor atual das moedas, adiciona 10 e salva novamente
        int COINS = PlayerPrefs.GetInt("COINS") + 10;
        PlayerPrefs.SetInt("COINS", COINS);
        PlayerPrefs.Save();

        // Atualiza o texto na interface para mostrar o novo total de moedas
        UpdateCoinsText();
    }

    // Atualiza o texto na UI com a quantidade atual de moedas
    private void UpdateCoinsText()
    {
        // Pega o número de moedas armazenado nas preferências do jogador
        int COINS = PlayerPrefs.GetInt("COINS");

        // Atualiza o texto exibido na interface com o valor atual das moedas
        uiCoinsText.text = "COINS: " + COINS;
    }
}
