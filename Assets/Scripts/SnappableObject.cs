using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SnappableObject : ThrowableObject
{
    private bool isWithinSnappableArea = false;
    private float snapZ;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("snap"))
        {
            isWithinSnappableArea = true;
            snapZ = other.transform.position.z;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("snap"))
        {
            isWithinSnappableArea = false;
        }
    }

    public override void OnGrabStart(Grabber hand)
    {
        base.OnGrabStart(hand);

        GetComponent<Rigidbody>().isKinematic = false;
        GetComponent<Rigidbody>().useGravity = true;

    }

    public override void OnGrabEnd()
    {
        base.OnGrabEnd();

        if (isWithinSnappableArea)
        {
            GetComponent<Rigidbody>().isKinematic = true;
            GetComponent<Rigidbody>().useGravity = false;

            var tempPosition = new Vector3(transform.position.x, transform.position.y, snapZ);
            transform.position = tempPosition;
        }
        else
        {
            GetComponent<Rigidbody>().isKinematic = false;
            GetComponent<Rigidbody>().useGravity = true;
        }
    }
}
