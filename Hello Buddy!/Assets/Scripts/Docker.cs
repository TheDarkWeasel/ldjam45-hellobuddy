using System.Collections.Generic;
using UnityEngine;

public class Docker : MonoBehaviour
{
    public Rigidbody rb;
    private EvolutionManager evolutionManager;

    public float distanceToDock = 1.6f;

    private List<DockedObject> dockedObjects = new List<DockedObject>();

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

        //Debug.Log("Distance: " + distance);

        if (distance <= distanceToDock && !dockable.IsDocked())
        {
            dockable.SetDocked(true);
            FixedJoint fixedJoint = gameObject.AddComponent<FixedJoint>();

            DockedObject docked = new DockedObject();
            docked.dockable = dockable;
            docked.joint = fixedJoint;

            dockedObjects.Add(docked);

            fixedJoint.connectedBody = otherRigidbody;
            evolutionManager.OnAddedAtom();
        }
    }

    public void OnHitEnemy()
    {
        foreach(DockedObject docked in dockedObjects)
        {
            docked.joint.connectedBody = null;
            Destroy(docked.joint);
            docked.dockable.SetDocked(false);
        }

        dockedObjects.Clear();

        evolutionManager.ClearAtoms();

        Debug.Log("Hit enemy");
    }
}
