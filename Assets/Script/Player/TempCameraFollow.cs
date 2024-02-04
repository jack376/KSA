using UnityEngine;

public class TempCameraFollow : MonoBehaviour
{
    public Transform player;
    public Vector3 offset;

    private void LateUpdate()
    {
        transform.position = player.position + offset;
    }
}