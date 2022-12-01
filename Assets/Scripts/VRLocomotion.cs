using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class VRLocomotion : MonoBehaviour
{
    public Transform XRRig;
    public Transform head;

    public bool canSmoothMove = false;
    public float moveSpeed = 20;

    public bool canSmoothRotate = false;
    public float rotateSpeed = 20;

    public bool canTeleport = false;
    public Transform teleportingHand;
    public LineRenderer line;
    public Vector3 curveHeight;
    public int lineResolution;
    public Transform reticle;


    public float fadeTime;
    public RawImage fader;

    public Color validStart;
    public Color validEnd;
    public Color invalidStart;
    public Color invalidEnd;

    private string verticalAxis;
    private string horizontalAxis;
    private string triggerButton;
    private bool teleportLock;


    private void Awake()
    {
        verticalAxis = "XRI_Left_Primary2DAxis_Vertical";
        horizontalAxis = "XRI_Left_Primary2DAxis_Horizontal";
        triggerButton = "XRI_Left_TriggerButton";
    }


    void Start()
    {
        line.positionCount = lineResolution;
        fader.color = Color.clear;
        teleportLock = false;
    }

    
    void Update()
    {
        var verticalValue = Input.GetAxis(verticalAxis);
        var horizontalValue = Input.GetAxis(horizontalAxis);

        if (canSmoothMove) 
            SmoothMove(verticalValue);

        if (canSmoothRotate)
            SmoothRotate(horizontalValue);

        if (canTeleport)
            Teleport();

    }

    void SmoothMove(float axisValue)
    {
        Vector3 lookDirection = new Vector3(head.forward.x, 0, head.forward.z);
        lookDirection.Normalize();
        
        XRRig.position += Time.deltaTime * lookDirection * axisValue * moveSpeed * -1;
    }

    void SmoothRotate(float axisValue)
    {
        XRRig.Rotate(Vector3.up, rotateSpeed * Time.deltaTime * axisValue);
    }

    void Teleport()
    {
        Ray ray = new Ray(teleportingHand.position, teleportingHand.forward);

        if(Physics.Raycast(ray, out RaycastHit hit))
        {
            line.enabled = true;
            reticle.gameObject.SetActive(true);

            bool validTarget = hit.collider.CompareTag("teleportable");

            if (validTarget)
            {
                line.startColor = validStart;
                line.endColor = validEnd;
            }
            else
            {
                line.startColor = invalidStart;
                line.endColor = invalidEnd;
            }


            Vector3 startPoint = teleportingHand.position;
            Vector3 endPoint = hit.point;
            Vector3 midPoint = ((endPoint - startPoint) / 2) + startPoint;
            midPoint += curveHeight;

            // Smooth movement of the curve
            Vector3 desiredPosition = endPoint - reticle.position;
            Vector3 smoothVectoDesired = (desiredPosition / 0.2f) * Time.deltaTime;
            Vector3 reticleEndpoint = reticle.position + smoothVectoDesired;

            reticle.position = reticleEndpoint;
            reticle.transform.up = hit.normal;


            for(int i=0; i<lineResolution; i++)
            {
                float t = i / (float)lineResolution;

                Vector3 startToMid = Vector3.Lerp(startPoint, midPoint, t);
                Vector3 midToEnd = Vector3.Lerp(midPoint, reticleEndpoint, t);

                Vector3 curvePosition = Vector3.Lerp(startToMid, midToEnd, t);

                line.SetPosition(i, curvePosition);

            }

            // This is what we used for a straight line!! :D
            //line.SetPosition(0, teleportingHand.position);
            //line.SetPosition(1, hit.point);

            if (!teleportLock &&  validTarget && Input.GetButtonDown(triggerButton))
            {
                StartCoroutine(FadeTeleport(endPoint));
            }
        }
        else
        {
            line.enabled = false;
            reticle.gameObject.SetActive(false);
        }
    }


    private IEnumerator FadeTeleport(Vector3 newPosition)
    {
        teleportLock = true;

        // Fadein

        float timer = 0;

        while(timer < fadeTime)
        {
            fader.color = Color.Lerp(Color.clear, Color.black, timer);
            yield return new WaitForEndOfFrame();

            timer += Time.deltaTime;
        }

        fader.color = Color.black;

        //Teleport
        XRRig.position = newPosition;

        yield return new WaitForSeconds(fadeTime);


        //Fadeout
        timer = 0;

        while (timer < fadeTime)
        {
            fader.color = Color.Lerp(Color.black, Color.clear, timer);
            yield return new WaitForEndOfFrame();

            timer += Time.deltaTime;
        }

        fader.color = Color.clear;

        teleportLock = false;
    }
}
