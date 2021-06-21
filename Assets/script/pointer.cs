using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using Valve.VR;
public class pointer : MonoBehaviour
{
    public float mlength = 5.0f;
    public GameObject Dot;
    public VRInput m_input;

    private LineRenderer line = null;
    private void Awake()
    {
        line = GetComponent<LineRenderer>();
    }


    // Update is called once per frame
   private void Update()
    {
        UpdateLine();
    }
    private void UpdateLine()
    {


        float targetLength = mlength;

        RaycastHit hit = CreateRaycast(targetLength);

        Vector3 endPosittion = transform.position + (transform.forward * targetLength);

        if (hit.collider != null)
            endPosittion = hit.point;

        Dot.transform.position = endPosittion;
        line.SetPosition(0, transform.position);
        line.SetPosition(1, endPosittion);

    }
    private RaycastHit CreateRaycast(float lenght)
    {
        RaycastHit hit;
        Ray ray = new Ray(transform.position, transform.forward);
        Physics.Raycast(ray, out hit, mlength);
        return hit;
    }
}
