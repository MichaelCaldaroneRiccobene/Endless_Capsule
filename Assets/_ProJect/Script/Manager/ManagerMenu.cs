using Cinemachine;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
public class ManagerMenu : GenericSingleton<ManagerMenu>
{
    [Header("Setting")]
    [SerializeField] private string nameLevel;
    [SerializeField] private AudioClip soundButton;

    [Header("Setting Camere")]
    [SerializeField] private CinemachineVirtualCamera cameraMenu;
    [SerializeField] private CinemachineVirtualCamera cameraShop;

    [Header("Setting Score")]
    [SerializeField] private TextMeshProUGUI coinTotal;
    [SerializeField] private TextMeshProUGUI[] textScore;

    public float[] score;
   

    private void Start()
    {
        if (score.Length > 1)
        {
            for (int i = 0; i < textScore.Length; i++) textScore[i].text = score[i].ToString("F2");
        }
        cameraShop.Priority = 1;
    }

    public void OnGoPlay()
    {
        ManagerAudio.Instance.PlayFXSound(soundButton, 0.1f);
        SceneManager.LoadScene(nameLevel);
    }

    public void OnGoToShoop()
    {
        ManagerAudio.Instance.PlayFXSound(soundButton, 0.1f);
        cameraShop.Priority = 11;
    }

    public void OnGoToMenu()
    {
        ManagerAudio.Instance.PlayFXSound(soundButton, 0.1f);
        cameraShop.Priority = 1;
    }

    public void UpdateCoin(int coin) => coinTotal.text = coin.ToString();

    public void AddScoreArray(float[] index)
    {
        if(index.Length < 1) return;

        score = new float[index.Length];

        for (int i = 0; i < index.Length; i++) score[i] = index[i];
    }

    public void OnResetSave() { if(ManagerGame.Instance != null) ManagerGame.Instance.OnResetSave(); ManagerAudio.Instance.PlayFXSound(soundButton, 0.1f);}

    public void QuitGame()
    {
        ManagerAudio.Instance.PlayFXSound(soundButton, 0.1f);
        Application.Quit();
    }
}
    