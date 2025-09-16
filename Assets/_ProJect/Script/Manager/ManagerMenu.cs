using Cinemachine;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
public class ManagerMenu : GenericSingleton<ManagerMenu>
{
    [Header("Setting")]
    [SerializeField] private string nameLevel;

    [Header("Setting Camere")]
    [SerializeField] private CinemachineVirtualCamera cameraMenu;
    [SerializeField] private CinemachineVirtualCamera cameraShop;

    [Header("Setting Score")]
    [SerializeField] private TextMeshProUGUI coinTotal;
    [SerializeField] private TextMeshProUGUI[] textScore;

    public int coin;
    public float[] score;
   

    private void Start()
    {
        if (score.Length > 1)
        {
            for (int i = 0; i < textScore.Length; i++) textScore[i].text = score[i].ToString("F2");
        }
        coinTotal.text = coin.ToString();

        cameraShop.Priority = 1;
    }

    public void OnGoPlay() => SceneManager.LoadScene(nameLevel);

    public void OnGoToShoop() => cameraShop.Priority = 11;

    public void OnGoToMenu() => cameraShop.Priority = 1;

    public void AddCoin(int coin) => this.coin = coin;

    public void AddScoreArray(float[] index)
    {
        if(index.Length < 1) return;

        score = new float[index.Length];

        for (int i = 0; i < index.Length; i++) score[i] = index[i];
    }

    public void OnResetSave() { if(ManagerGame.Instance != null) ManagerGame.Instance.OnResetSave(); }

    public void QuitGame() => Application.Quit();
}
    