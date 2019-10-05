using System.Collections.Generic;
using UnityEngine;

public class Dockable : MonoBehaviour
{
    public Rigidbody rb;

    public static List<Dockable> dockables;

    private bool isDocked = false;

    void OnEnable()
    {
        if (dockables == null)
        {
            dockables = new List<Dockable>();
        }

        dockables.Add(this);
    }

    void OnDisable()
    {
        if (dockables != null)
        {
            dockables.Remove(this);
        }
    }

    public bool IsDocked()
    {
        return isDocked;
    }

    public void SetDocked(bool docked)
    {
        isDocked = docked;
        if (!docked)
        {
            OnUndocked();
        }
    }

    private void OnUndocked()
    {
        Debug.Log("Undocked!");
        Destroy(gameObject);

        //TODO do this with a nice animation
    }
}
