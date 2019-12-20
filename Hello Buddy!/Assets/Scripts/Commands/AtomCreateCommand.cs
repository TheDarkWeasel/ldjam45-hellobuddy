using UnityEngine;
using System.Collections;

public abstract class AtomCreateCommand
{
    protected AtomPool atomPool;

    public AtomCreateCommand(AtomPool atomPool)
    {
        this.atomPool = atomPool;
    }

    protected abstract GameObject InternalCreate();

    public GameObject Create()
    {
        return InternalCreate();
    }
}
