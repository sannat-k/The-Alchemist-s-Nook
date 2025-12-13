using UnityEngine;

public class SmoothFollow : MonoBehaviour
{
    public Transform target;
    public Vector3 offset = new Vector3(0, 5, -10);
    public Vector3 rotationOffset = new Vector3(17, 0, 0);

    void LateUpdate()
    {
        if (target == null) return;
        transform.position = target.position + offset;
        transform.rotation = Quaternion.Euler(rotationOffset);
    }
}
