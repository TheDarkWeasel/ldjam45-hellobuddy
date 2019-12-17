using System.Collections.Generic;
using UnityEngine;

public class Docker : MonoBehaviour
{
    public Rigidbody rb;
    private EvolutionManager evolutionManager;
    private PlayerSounds playerSounds;
    private GameOverManager gameOverManager;

    private List<DockedObject> dockedObjects = new List<DockedObject>();

    private bool isPlayer = false;

    void Start()
    {
        isPlayer = name == "Player";
        evolutionManager = FindObjectOfType<EvolutionManager>();
        playerSounds = FindObjectOfType<PlayerSounds>();
        gameOverManager = FindObjectOfType<GameOverManager>();
        if (evolutionManager == null)
        {
            throw new UnityException("No evolution manager found!");
        }
    }

    void FixedUpdate()
    {
        foreach (Dockable dockable in Dockable.dockables)
        {
            if (!dockable.IsDestroyed())
                TryDocking(dockable);
        }
    }

    private void TryDocking(Dockable dockable)
    {
        Rigidbody otherRigidbody = dockable.rb;
        Vector3 direction = rb.position - otherRigidbody.position;
        float distance = direction.magnitude;

        Collider collider = gameObject.GetComponentInChildren<Collider>();
        if (collider == null)
            return;

        float playerWidth = collider.bounds.size.x;

        //Sometimes the joints won't dock. This is preventing it.
        float safetyMargin = 0.05f;

        if (distance <= playerWidth + safetyMargin && !dockable.IsDocked())
        {
            dockable.SetDocked(true);
            FixedJoint fixedJoint = gameObject.AddComponent<FixedJoint>();

            DockedObject docked = new DockedObject
            {
                dockable = dockable,
                joint = fixedJoint
            };

            dockedObjects.Add(docked);

            fixedJoint.connectedBody = otherRigidbody;
            evolutionManager.OnAddedAtom();

            playerSounds.OnDock();
        }
    }

    public void OnHitEnemy()
    {
        if (isPlayer)
        {
            playerSounds.OnHit();
        }

        if (isPlayer && dockedObjects.Count == 0)
        {
            Destroy(gameObject);
            gameOverManager.OnGameOver();
        }
        else
        {
            List<DockedObject> copy = new List<DockedObject>(dockedObjects);
            foreach (DockedObject docked in copy)
            {
                docked.joint.connectedBody = null;
                Destroy(docked.joint);
                docked.dockable.SetDocked(false);
            }

            dockedObjects.RemoveAll(t => copy.Contains(t));

            evolutionManager.ClearAtoms();
        }

        Debug.Log("Hit enemy");
    }
}
