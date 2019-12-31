using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class Dockable : MonoBehaviour
{
    [SerializeField]
    private Rigidbody rb;

    private static List<Dockable> dockables = new List<Dockable>();

    private Animator destroyAnimator;

    private bool isDocked = false;
    private bool isDestroyed = false;

    public static List<Dockable> Dockables { get => dockables; set => dockables = value; }
    public Rigidbody RidgidBody { get => rb; set => rb = value; }

    void Start()
    {
        destroyAnimator = GetComponent<Animator>();
    }

    void OnEnable()
    {
        dockables.Add(this);
    }

    void OnDisable()
    {
        dockables.Remove(this);
    }

    public bool IsDocked()
    {
        return isDocked;
    }

    public bool IsDestroyed()
    {
        return isDestroyed;
    }

    public void SetDocked(bool docked)
    {
        if (isDestroyed)
        {
            Debug.Log("Already destroyed, should be set to: " + docked);
            return;
        }

        isDocked = docked;
        if (!docked)
        {
            OnUndocked();
        }
        else
        {
            OnDocked();
        }
    }

    private void OnDocked()
    {
        //Deactivate the mover script
        gameObject.GetComponent<NonPlayableAtomMover>().Active = false;
        //object will become a docker itself
        gameObject.GetComponent<Docker>().Active = true;
    }

    private void OnUndocked()
    {
        isDestroyed = true;
        Debug.Log("Undocked!");
        if (gameObject.GetComponent<Docker>() != null)
        {
            gameObject.GetComponent<Docker>().OnHitEnemy();
            gameObject.GetComponent<Docker>().Active = false;
        }
        destroyAnimator.ResetTrigger("Reset");
        destroyAnimator.SetTrigger("Destroy");
        const int secondsTillDestruction = 1;
        AtomPool.GetInstance().DestroyFriendlyAtom(gameObject, secondsTillDestruction);
    }

    public void ResetDestroy()
    {
        destroyAnimator.ResetTrigger("Destroy");
        destroyAnimator.SetTrigger("Reset");
        isDestroyed = false;
    }
}
