using UnityEngine;
using UnityEngine.UI;

public class EvolutionManager : MonoBehaviour
{
    private string HIGHSCOREKEY = "HIGHSCOREKEY";

    public Text atomsUIText;
    public Text scoreUIText;
    public Text atomsRemainingText;
    public Text highscoreText;
    public Text evolutionsDoneText;

    public int scorePerAtom = 1;
    public int scorePerEvolution = 100;
    public int evolutionPossibleWithAtomCount = 5;

    private int atomCounter = 0;
    private int score = 0;
    private int highscore = 0;
    private int evolutionsDone = 0;

    private Animator evolutionUIAnimator;
    private GameObject player;

    void Start()
    {
        highscore = PlayerPrefs.GetInt(HIGHSCOREKEY, 0);
        player = GameObject.Find("Player");
        evolutionUIAnimator = atomsRemainingText.GetComponentInParent<Animator>();
        UpdateAtomsAndScore();
    }

    void Update()
    {
        if (atomCounter >= evolutionPossibleWithAtomCount && Input.GetKeyUp(KeyCode.F))
        {
            Evolve();
        }
    }

    private void Evolve()
    {
        //Get a bonus, when you have more atoms than needed
        score += scorePerEvolution * (atomCounter - evolutionPossibleWithAtomCount + 1);
        evolutionsDone++;
        //Lose all atoms
        player.GetComponent<Docker>().OnHitEnemy();
        float sizeChange = 0.2f;
        player.transform.localScale += new Vector3(sizeChange, sizeChange, sizeChange);
        ClearAtoms();
    }

    private void OnEvolutionReady()
    {
        evolutionUIAnimator.ResetTrigger("EvoReady");
        evolutionUIAnimator.ResetTrigger("EvoDone");
        evolutionUIAnimator.SetTrigger("EvoReady");
    }

    private void OnEvolutionDone()
    {
        evolutionUIAnimator.ResetTrigger("EvoReady");
        evolutionUIAnimator.ResetTrigger("EvoDone");
        evolutionUIAnimator.SetTrigger("EvoDone");
    }

    public void OnAddedAtom()
    {
        atomCounter++;
        score += scorePerAtom;
        UpdateAtomsAndScore();

        if (atomCounter >= evolutionPossibleWithAtomCount)
        {
            OnEvolutionReady();
        }
    }

    public void ClearAtoms()
    {
        atomCounter = 0;
        UpdateAtomsAndScore();
        OnEvolutionDone();
    }

    private void UpdateAtomsAndScore()
    {
        atomsUIText.text = "" + atomCounter;
        scoreUIText.text = "" + score;
        evolutionsDoneText.text = "" + evolutionsDone;
        int atomsRemainingForEvo = evolutionPossibleWithAtomCount - atomCounter;
        if (atomsRemainingForEvo > 0)
        {
            atomsRemainingText.text = "" + (evolutionPossibleWithAtomCount - atomCounter);
        }
        else
        {
            atomsRemainingText.text = "Press 'F' to evolve";
        }

        UpdateHighscore();
    }

    private void UpdateHighscore()
    {
        if (score > highscore)
        {
            highscore = score;
            PlayerPrefs.SetInt(HIGHSCOREKEY, highscore);
        }

        highscoreText.text = "" + highscore;
    }
}
