using System.Collections.Generic;
using UnityEngine;

public class Docker : MonoBehaviour
{
    [SerializeField]
    private Rigidbody rb;
    private EvolutionManager evolutionManager;
    private PlayerSounds playerSounds;
    private GameOverManager gameOverManager;

    private readonly object syncLock = new object();

    private List<DockedObject> dockedObjects = new List<DockedObject>();

    private bool isPlayer = false;

    private bool active = true;

    public Rigidbody RidgidBody { get => rb; set => rb = value; }
    public bool Active { get => active; set => active = value; }

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
        if (active)
        {
            foreach (Dockable dockable in Dockable.Dockables)
            {
                if (!dockable.IsDestroyed())
                    TryDocking(dockable);
            }
        }
    }

    private void TryDocking(Dockable dockable)
    {
        Rigidbody otherRigidbody = dockable.RidgidBody;
        Vector3 direction = rb.position - otherRigidbody.position;
        float distance = direction.magnitude;

        Collider playerCollider = gameObject.GetComponentInChildren<Collider>();
        if (playerCollider == null)
            return;

        Collider otherCollider = otherRigidbody.GetComponentInChildren<Collider>();
        if (otherCollider == null)
            return;

        float playerRadius = playerCollider.bounds.size.x / 2;
        float otherRadius = otherCollider.bounds.size.x / 2;

        //Sometimes the joints won't dock. This is preventing it.
        const float safetyMargin = 0.05f;

        if (distance <= playerRadius + otherRadius + safetyMargin && !dockable.IsDocked())
        {
            lock (syncLock)
            {
                if (active && !dockable.IsDestroyed())
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
        }
    }

    public void OnHitEnemy()
    {
        if (active)
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
                lock (syncLock)
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
            }

            Debug.Log("Hit enemy");
        }
    }
}
