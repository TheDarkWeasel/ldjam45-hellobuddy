﻿using System.Collections.Generic;
using UnityEngine;

public class Docker : MonoBehaviour
{
    public Rigidbody rb;
    public float forceMagnitude = 100;

    private List<FixedJoint> joints = new List<FixedJoint>();

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

        if (distance <= 1.6 && !dockable.IsDocked())
        {
            dockable.SetDocked(true);
            FixedJoint fixedJoint = gameObject.AddComponent<FixedJoint>();
            joints.Add(fixedJoint);

            fixedJoint.connectedBody = otherRigidbody;
        }
    }
}
