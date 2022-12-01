using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LerpTest : MonoBehaviour
{
    public Transform PointA;
    public Transform PointB;
    public Transform PointC;

    public Transform PointP;
    public Transform PointQ;

    public Transform PointX;


    [Range(0.0f, 1.0f)]
    public float lerpPosition;

    

    // Start is called before the first frame update
    void Start()
    {
        lerpPosition = 0;
    }

    // Update is called once per frame
    void Update()
    {
      
        lerpPosition = lerpPosition <= 1 ? (lerpPosition + Time.deltaTime) : 0;


        PointP.position = Vector3.Lerp(PointA.position, PointB.position, lerpPosition);
        PointQ.position = Vector3.Lerp(PointB.position, PointC.position, lerpPosition);

        PointX.position = Vector3.Lerp(PointP.position, PointQ.position, lerpPosition);

    }
}
