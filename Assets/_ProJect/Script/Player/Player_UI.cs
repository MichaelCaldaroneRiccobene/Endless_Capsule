using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Player_UI : GenericSingleton<Player_UI>
{
    [Header("Setting")]
    [SerializeField] private Transform player;

    [SerializeField] private GameObject menu;
    [SerializeField] private string menuName = "Menu";

    [Header("Setting Life")]
    [SerializeField] private GameObject imageLife;

    [Header("Setting Score")]
    [SerializeField] private TextMeshProUGUI textScoreDistance;
    [SerializeField] private TextMeshProUGUI textScoreCoin;

    private bool isOnMenu;

    private void Start()
    {
        if(menu != null) menu.SetActive(false);
        if (ManagerGame.Instance != null) imageLife.gameObject.SetActive(ManagerGame.Instance.HasPlayer2Life());

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void FixedUpdate() => UpdateDistance();

    #region Menu
    public void OnPause()
    {
        isOnMenu = !isOnMenu;

        if (isOnMenu) OpenMenu();
        else CloseMenu();
    }

    public void GoOnMenu()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene(menuName);
    }

    public void OpenMenu()
    {
        Time.timeScale = 0;
        if (menu != null) menu.SetActive(true);

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    public void CloseMenu()
    {
        Time.timeScale = 1;
        if (menu != null) menu.SetActive(false);

        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }
    #endregion

    #region GenericUI
    public void UpdateDamage() { if (imageLife != null) imageLife.gameObject.SetActive(false); }

    private void UpdateDistance() { if (player != null) textScoreDistance.text = player.transform.position.z.ToString("F2"); }
    public void UpdateCoinText(int coin) { if (textScoreCoin != null) textScoreCoin.text = coin.ToString(); }
    #endregion  


    public void QuitGame() => Application.Quit(); 
}
