using UnityEngine;

public class NonPlayableAtomMover : MonoBehaviour
{
    [SerializeField]
    private float movementPerSecond = 3f;

    private const float zBottomOfScreen = -19;

    private bool active = true;

    public bool Active { get => active; set => active = value; }

    void Update()
    {
        if (active)
        {
            float deltaTime = Time.deltaTime;

            Vector3 movement = new Vector3(0, 0, -movementPerSecond * deltaTime);
            gameObject.transform.Translate(movement);

            //Destroy unneeded object
            if (gameObject.activeSelf == true && gameObject.transform.position.z < zBottomOfScreen)
            {
                if (gameObject.GetComponentInChildren<EnemyController>() == null)
                {
                    //it's a friendly atom
                    AtomPool.GetInstance().DestroyFriendlyAtom(gameObject, 0);
                }
                else
                {
                    //it's an enemy
                    AtomPool.GetInstance().DestroyEnemyAtom(gameObject, 0);
                }
            }
        }
    }
}
