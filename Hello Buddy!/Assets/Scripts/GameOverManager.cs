using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOverManager : MonoBehaviour
{
    private Animator anim;

    private bool hasGameEnded = false;

    void Start()
    {
        anim = gameObject.GetComponent<Animator>();
    }

    public void OnGameOver()
    {
        anim.SetTrigger("GameOver");
        hasGameEnded = true;
    }

    void Update()
    {
        if (hasGameEnded && Input.GetKeyUp(KeyCode.R))
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }

        if (Input.GetKeyUp(KeyCode.Escape))
        {
            Application.Quit();
        }
    }
}
