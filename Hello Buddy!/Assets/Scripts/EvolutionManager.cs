using UnityEngine;
using UnityEngine.UI;

public class EvolutionManager : MonoBehaviour
{
    private string HIGHSCOREKEY = "HIGHSCOREKEY";

    [SerializeField]
    private Text multiplierUIText;
    [SerializeField]
    private Text scoreUIText;
    [SerializeField]
    private Text atomsRemainingText;
    [SerializeField]
    private Text highscoreText;
    [SerializeField]
    private Text evolutionsDoneText;

    [SerializeField]
    private int scorePerAtom = 1;
    [SerializeField]
    private int scorePerEvolution = 100;
    [SerializeField]
    private int evolutionPossibleWithAtomCount = 5;

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
        player.transform.localScale += new Vector3(sizeChange, 0, sizeChange);
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
        multiplierUIText.text = "x" + Mathf.Max(1, atomCounter - evolutionPossibleWithAtomCount + 1);
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
