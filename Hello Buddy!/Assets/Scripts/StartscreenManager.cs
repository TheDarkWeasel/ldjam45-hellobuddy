using UnityEngine;
using UnityEngine.SceneManagement;

public class StartscreenManager : MonoBehaviour
{
    [SerializeField]
    private Canvas explanationCanvas;
    [SerializeField]
    private Canvas startscreenCanvas;

    private bool inExplanation = false;

    void Update()
    {
        if (Input.GetKeyUp(KeyCode.Return))
        {
            if (!inExplanation)
            {
                inExplanation = true;
                startscreenCanvas.gameObject.SetActive(false);
                explanationCanvas.gameObject.SetActive(true);
            }
            else
            {
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
            }
        }
        else if (Input.GetKeyUp(KeyCode.Escape))
        {
            Application.Quit();
        }
    }
}
