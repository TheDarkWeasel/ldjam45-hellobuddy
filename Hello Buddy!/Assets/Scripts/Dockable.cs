using System.Collections.Generic;
using UnityEngine;

public class Dockable : MonoBehaviour
{
    public Rigidbody rb;

    public static List<Dockable> dockables = new List<Dockable>();

    private Animator destroyAnimator;

    private bool isDocked = false;
    private bool isDestroyed = false;

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
            return;

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
        //Destroy the mover script
        Destroy(gameObject.GetComponent<NonPlayableAtomMover>());
        //object will become a docker itself
        gameObject.AddComponent<Docker>().rb = gameObject.GetComponent<Rigidbody>();
    }

    private void OnUndocked()
    {
        isDestroyed = true;
        Debug.Log("Undocked!");
        if (gameObject.GetComponent<Docker>() != null)
        {
            gameObject.GetComponent<Docker>().OnHitEnemy();
        }
        destroyAnimator.ResetTrigger("Destroy");
        destroyAnimator.SetTrigger("Destroy");
        //Destroy collider, so we don't lose when this object hits an enemy
        DestroyChildColliders();
        Destroy(gameObject, 1);
    }

    private void DestroyChildColliders()
    {
        Collider[] childColliders = gameObject.GetComponentsInChildren<Collider>();
        foreach (Collider collider in childColliders)
        {
            Destroy(collider);
        }
    }
}
