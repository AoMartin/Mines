using UnityEngine;

public class Control_cam : MonoBehaviour {

    public Transform playpos;

    Vector3 offset;
    Vector3 velocity;
    Vector3 targetPosition;
    [SerializeField] float smoothTime = 0.5f;

    void Start () {
        offset = transform.position - playpos.position;
    } 
	
    void LateUpdate()
    {
        targetPosition = playpos.position + offset;
        transform.position = Vector3.SmoothDamp(transform.position,targetPosition,ref velocity,smoothTime);  
    }

}
