using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Player_Controller : MonoBehaviour
{
    [Header("Setting")]
    [SerializeField] private MeshRenderer meshRenderPlayer;
    [SerializeField] private float mapLargeSize = 5;

    private bool has2Life;

    private void Start()
    {
        if(meshRenderPlayer != null) meshRenderPlayer.material.color = Color.blue;
        if (ManagerGame.Instance != null) has2Life = ManagerGame.Instance.HasPlayer2Life();
    }

    private void FixedUpdate() { if (Mathf.Abs(transform.position.x) >= mapLargeSize) ReloadGame(); }

    private void ReloadGame()
    {
        ManagerGame.Instance.SaveGame(transform.position.z);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out Obstacle obstacle))
        {
            if (!has2Life) ReloadGame();
            else StartCoroutine(LifeDownRoutine());
        }
    }

    private IEnumerator LifeDownRoutine()
    {
        Player_UI.Instance.UpdateDamage();  
        StartCoroutine(LifeDownVisualRoutine());
        yield return new WaitForSeconds(0.5f);

        has2Life = false;
    }

    private IEnumerator LifeDownVisualRoutine()
    {
        for (int i = 0; i < 3; i++)
        {
            meshRenderPlayer.material.color = Color.red;
            yield return new WaitForSeconds(0.1f);
            meshRenderPlayer.material.color = Color.blue;
            yield return new WaitForSeconds(0.1f);
        }
    }
}
