using UnityEngine;
using System.Collections;

public class EnemyAtomCreateCommand : AtomCreateCommand
{

    public EnemyAtomCreateCommand(AtomPool atomPool) : base(atomPool)
    {

    }

    protected override GameObject InternalCreate()
    {
        return atomPool.CreateEnemyAtom();
    }
}
