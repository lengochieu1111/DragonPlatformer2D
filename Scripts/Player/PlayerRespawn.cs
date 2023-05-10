using UnityEngine;

public class PlayerRespawn : MonoBehaviour
{
    [SerializeField] AudioClip checkpointSound;
    [SerializeField] Transform currentChecpointPos;
    [SerializeField] Health healthPlayer;


    private void Awake()
    {
        healthPlayer = GetComponent<Health>();
    }

    public void Respawn()
    {
        transform.position = currentChecpointPos.position;

        healthPlayer.Respawn();

        Camera.main.GetComponent<CameraController>().MoveToNewRoom(currentChecpointPos);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.transform.tag == "Checkpoint")
        {
            currentChecpointPos = collision.transform;
            SoundManager.instance.PlaySound(checkpointSound);
            collision.GetComponent<Collider2D>().enabled = false;
            collision.GetComponent<Animator>().SetTrigger("appear");
        }
    }

}
