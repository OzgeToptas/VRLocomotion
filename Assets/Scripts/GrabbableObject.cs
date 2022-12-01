using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrabbableObject : MonoBehaviour
{
    [SerializeField]private Color hoveredColor;

    private Color defaultColor;
    private Material material;

    // Start is called before the first frame update
    void Start()
    {
        material = GetComponent<Renderer>().material;
        defaultColor = material.color;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public virtual void OnHoverStart()
    {
        material.color = hoveredColor;
    }

    public virtual void OnHoverEnd()
    {
        material.color = defaultColor;
    }

    public virtual void OnGrabStart(Grabber hand)
    {
        transform.SetParent(hand.transform);
        GetComponent<Rigidbody>().useGravity = false;
        GetComponent<Rigidbody>().isKinematic = true;
    }

    public virtual void OnGrabEnd()
    {
        transform.SetParent(null);
        GetComponent<Rigidbody>().useGravity = true;
        GetComponent<Rigidbody>().isKinematic = false;
    }
}
