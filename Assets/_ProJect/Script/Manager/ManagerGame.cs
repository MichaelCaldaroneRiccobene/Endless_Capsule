using UnityEngine;
using UnityEngine.SceneManagement;

public class ManagerGame : GenericSingleton<ManagerGame>
{
    private int totCoin;
    private int coinTakeInGame;
    private int maxScoreInMenu = 5;

    private SaveData saveData = new SaveData();

    protected override void Awake()
    {
        base.Awake();
        Load();
    }

    private void Load()
    {
        saveData = SaveSystem.Load();

        if (saveData == null)
        {
            saveData = new SaveData();

            saveData.hasDoubleLife = false;
            saveData.coin = 0;

            saveData.scoresArray = new float[maxScoreInMenu];
        }

        totCoin = saveData.coin;
        if (ManagerMenu.Instance != null)
        {
            ManagerMenu.Instance.UpdateCoin(saveData.coin);
            ManagerMenu.Instance.AddScoreArray(saveData.scoresArray);
        }
    }

    private void Start()
    {
        if (Player_UI.Instance != null) Player_UI.Instance.UpdateCoinText(0);
    }

    private void Update()
    {
        if (Input.GetKeyUp(KeyCode.Alpha9))
        {
            saveData.coin += 10;
            SaveSystem.Save(saveData);
            if (ManagerMenu.Instance != null) ManagerMenu.Instance.UpdateCoin(saveData.coin);
        }
    }

    public void AddCoin() { coinTakeInGame++;totCoin++; if (Player_UI.Instance != null) Player_UI.Instance.UpdateCoinText(coinTakeInGame); }

    public void SaveGame(float score)
    {
        SaveScore(score);
        SaveCoin();

        SaveSystem.Save(saveData);
    }

    public void SaveScore(float score)
    {
        for (int i = 0; i < saveData.scoresArray.Length; i++)
        { 
            if (score > saveData.scoresArray[i])
            {
                for (int j = saveData.scoresArray.Length - 1; j > i; j--)
                {
                    saveData.scoresArray[j] = saveData.scoresArray[j - 1];
                }

                saveData.scoresArray[i] = score;             
                return;
            }
        }     
    }

    public void SaveCoin() => saveData.coin = totCoin;

    public void SavePowerUp(PowerUp powerUp)
    {
        saveData.UnlockPowerUp(powerUp);
        SaveSystem.Save(saveData);
    }

    public bool HasPlayer2Life() => saveData.hasDoubleLife;


    public int GetMoney() => saveData.coin;

    public void UpDateCoin(int value)
    {
        saveData.coin += value;
        if(ManagerMenu.Instance != null) ManagerMenu.Instance.UpdateCoin(saveData.coin);

        SaveSystem.Save(saveData);
    }

    public SaveData GetSaveData() => saveData;

    public void OnResetSave()
    {
        SaveSystem.Delete();
        saveData = new SaveData();

        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
