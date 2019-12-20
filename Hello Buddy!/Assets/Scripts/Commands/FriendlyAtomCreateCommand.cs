using UnityEngine;
using System.Collections;

public class FriendlyAtomCreateCommand : AtomCreateCommand
{
    public FriendlyAtomCreateCommand(AtomPool atomPool) : base(atomPool)
    {

    }
    protected override GameObject InternalCreate()
    {
        return atomPool.CreateFriendlyAtom();
    }
}
