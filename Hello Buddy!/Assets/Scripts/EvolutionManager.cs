using UnityEngine;
using UnityEngine.UI;

public class EvolutionManager : MonoBehaviour
{
    public Text atomsUIText;
    public Text scoreUIText;

    public int scorePerAtom = 1;
    public int scorePerEvolution = 100;
    public int evolutionPossibleWithAtomCount = 5;

    private int atomCounter = 0;
    private int score = 0;

    private GameObject player;

    void Start()
    {
        player = GameObject.Find("Player");
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
        //Lose all atoms
        player.GetComponent<Docker>().OnHitEnemy();
        player.transform.localScale += new Vector3(0.1f, 0.1f, 0.1f);
        ClearAtoms();
    }

    public void OnAddedAtom()
    {
        atomCounter++;
        score += scorePerAtom;
        UpdateAtomsAndScore();
    }

    public void ClearAtoms()
    {
        atomCounter = 0;
        UpdateAtomsAndScore();
    }

    private void UpdateAtomsAndScore()
    {
        atomsUIText.text = "" + atomCounter;
        scoreUIText.text = "" + score;
    }
}
