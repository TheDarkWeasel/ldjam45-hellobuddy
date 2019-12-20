using System;
using System.Collections.Concurrent;

/**
 * Generic object pool. Recycles objects, if available, creates new objects otherwise.
 **/ 
public class ObjectPool<T>
{
    private ConcurrentBag<T> objects;
    private Func<T> objectGenerator;
    private Action<T> objectActivator;
    private Action<T> objectDeactivator;

    public ObjectPool(Func<T> objectGenerator, Action<T> objectActivator, Action<T> objectDeactivator)
    {
        this.objects = new ConcurrentBag<T>();
        this.objectGenerator = objectGenerator ?? throw new ArgumentNullException("objectGenerator missing");
        this.objectActivator = objectActivator ?? throw new ArgumentNullException("objectActivator missing");
        this.objectDeactivator = objectDeactivator ?? throw new ArgumentNullException("objectDeactivator missing");
    }

    public T GetObject()
    {
        T item;
        if (objects.TryTake(out item))
        {
            objectActivator(item);
            return item;
        }
        return objectGenerator();
    }

    public void PutObject(T item)
    {
        objectDeactivator(item);
        objects.Add(item);
    }
}
