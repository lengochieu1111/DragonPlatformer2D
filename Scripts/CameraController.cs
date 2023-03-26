using UnityEngine;

public class CameraController : MonoBehaviour
{
    // Room Camera 
    [SerializeField] protected float speed;
    [SerializeField] protected float currentPosX;
    private Vector3 veclocity = Vector3.zero;

    // Follow Player
    [SerializeField] private Transform player;
    [SerializeField] private float aheadDistance;
    [SerializeField] private float cameraSpeed;
    private float lookAhead;


    private void Update()
    {
        // Room Camera 
        // transform.position = Vector3.SmoothDamp(transform.position, new Vector3(currentPosX, transform.position.y, transform.position.z), ref veclocity, speed);

        // Follow Player
        transform.position = new Vector3(player.position.x + lookAhead, transform.position.y, transform.position.z);
        lookAhead = Mathf.Lerp(lookAhead, (aheadDistance * transform.localScale.x), cameraSpeed * Time.deltaTime);

    }

    public void MoveToNewRoom(Transform _newRoom)
    {
        this.currentPosX = _newRoom.position.x;
    }

}
