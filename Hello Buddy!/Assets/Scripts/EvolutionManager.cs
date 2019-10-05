using UnityEngine;
using UnityEngine.UI;

public class EvolutionManager : MonoBehaviour
{
    public Text atomsUIText;
    public Text scoreUIText;

    public int scorePerAtom = 1;

    private int atomCounter = 0;
    private int score = 0;

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
