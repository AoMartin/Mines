using UnityEngine;

public class VidaRot : MonoBehaviour
{
    Quaternion localrot;

    private void Start()
    {
        localrot = transform.parent.localRotation;
    }

    private void Update()
    {
        transform.rotation = localrot;
    }
}
