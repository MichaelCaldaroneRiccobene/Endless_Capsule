using System;
using UnityEngine;

public class Player_Input : MonoBehaviour
{
    public event Action OnGoLeft;
    public event Action OnGoRight;
    public event Action OnGoUp;
    public event Action OnGoDown;

    public event Action OnShield;

    private void Update() => PlayerInput();

    private void PlayerInput()
    {
        if (Input.GetKeyDown(KeyCode.A)) OnGoLeft?.Invoke();
        if (Input.GetKeyDown(KeyCode.D)) OnGoRight?.Invoke();

        if (Input.GetKeyDown(KeyCode.Space)) OnGoUp?.Invoke();
        if (Input.GetKeyDown(KeyCode.S)) OnGoDown?.Invoke();

        if (Input.GetKeyDown(KeyCode.Q)) OnShield?.Invoke();

        if (Input.GetKeyDown(KeyCode.Escape)) if(Player_UI.Instance != null) Player_UI.Instance.OnPause();

    }
}
