using System.Collections.Generic;
using UnityEngine;

public class Docker : MonoBehaviour
{
    public Rigidbody rb;
    private EvolutionManager evolutionManager;

    public float distanceToDock = 1.6f;

    private List<FixedJoint> joints = new List<FixedJoint>();

    void Start()
    {
        evolutionManager = FindObjectOfType<EvolutionManager>();
        if(evolutionManager == null)
        {
            throw new UnityException("No evolution manager found!");
        }
    }

    void FixedUpdate()
    {
        foreach (Dockable dockable in Dockable.dockables)
        {
            TryDocking(dockable);
        }
    }

    private void TryDocking(Dockable dockable)
    {
        Rigidbody otherRigidbody = dockable.rb;
        Vector3 direction = rb.position - otherRigidbody.position;
        float distance = direction.magnitude;

        Debug.Log("Distance: " + distance);

        if (distance <= distanceToDock && !dockable.IsDocked())
        {
            dockable.SetDocked(true);
            FixedJoint fixedJoint = gameObject.AddComponent<FixedJoint>();
            joints.Add(fixedJoint);

            fixedJoint.connectedBody = otherRigidbody;
            evolutionManager.OnAddedAtom();
        }
    }
}
