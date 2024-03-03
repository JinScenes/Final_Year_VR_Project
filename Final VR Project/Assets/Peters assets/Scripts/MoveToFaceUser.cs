using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveToFaceUser : MonoBehaviour
{
    [SerializeField] private Transform hmd;
    [SerializeField] private float canvasDistanceFromUser = 2f;
    [SerializeField] private float smoothTime = 1f;

    private Vector3 currentVelocity = new Vector3();

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 idealCanvasPosition = hmd.TransformPoint(new Vector3(0, 0, canvasDistanceFromUser));
        transform.position = Vector3.SmoothDamp(transform.position, idealCanvasPosition, ref currentVelocity, smoothTime);
        transform.LookAt(new Vector3(hmd.position.x, transform.position.y, hmd.position.z));
        transform.rotation *= Quaternion.Euler(0, 180f, 0);
    }
    //lecture code technically redundant 
}
