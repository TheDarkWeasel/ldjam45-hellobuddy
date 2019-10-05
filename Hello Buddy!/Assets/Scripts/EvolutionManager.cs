using UnityEngine;
using UnityEngine.UI;

public class EvolutionManager : MonoBehaviour
{
    public Text atomsUIText;

    private int atomCounter = 0;

    public void OnAddedAtom()
    {
        atomCounter++;
        UpdateAtomText();
    }

    public void ClearAtoms()
    {
        atomCounter = 0;
        UpdateAtomText();
    }

    private void UpdateAtomText()
    {
        atomsUIText.text = "" + atomCounter;
    }
}
