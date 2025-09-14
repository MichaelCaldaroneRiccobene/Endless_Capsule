using System.Collections;
using UnityEngine;

public class Player_PowerUp : MonoBehaviour
{
    [Header("Setting")]
    [SerializeField] private GameObject shiedlPreFab;
    [SerializeField] private float timeForShield = 0.2f;

    private Player_Input player_Input;

    private bool isOnShield;

    private void Start()
    {
        player_Input = GetComponent<Player_Input>();
        SetUpAction();
    }

    private void SetUpAction()
    {
        if(player_Input != null)
        {
            player_Input.OnShield += UseShield;
        }
    }

    private void UseShield() 
    {
        if (ManagerGame.Instance != null)
        {
            if(ManagerGame.Instance.GetSaveData().hasShield)
            {
                if (!isOnShield) StartCoroutine(UseShieldRoutione());
            }
        } 
    }

    private IEnumerator UseShieldRoutione()
    {
        shiedlPreFab.SetActive(true);
        isOnShield = true;

        yield return new WaitForSeconds(timeForShield);

        shiedlPreFab.SetActive(false);
        isOnShield = false;
    }

    private void OnDisable()
    {
        if(player_Input != null)
        {
            player_Input.OnShield -= UseShield;
        }
    }
}
