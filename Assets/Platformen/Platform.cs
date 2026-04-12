using Unity.VisualScripting;
using UnityEngine;

public class Platform : MonoBehaviour
{
    [Header("Settings")]
    public float speed;
    public Transform startingPoint;
    public Transform endpoint;

    [Header("Sound Settings")]
    [Tooltip("The sound played when the platform starts moving.")]
    [SerializeField] private AudioClip startSound;
    [Tooltip("The sound played when the platform stops moving (reaches destination).")]
    [SerializeField] private AudioClip stopSound;
    [Tooltip("The volume of the sound played.")]
    [SerializeField, Range(0f, 1f)] private float volume = 1f;

    [Header("References")]
    public Button relButton;

    private int i;
    //private bool isMoving = false;
    private bool wasMoving = false;

    void Start()
    {
        transform.position = startingPoint.position;
    }

    void Update()
    {
        
        /*
        if (Vector2.Distance(transform.position, points[i].position) < 0.02f)
        {
            i++;
            if (i == points.Length)
            {
                i = 0;
            }
        }
        */
        //Vector2.Distance(transform.position, startingPoint.position) < 0.02f

        Vector3 targetPosition = relButton.isPressed ? endpoint.position : startingPoint.position;
        bool isMoving = Vector3.Distance(transform.position, targetPosition) > 0.001f;

        if (isMoving && !wasMoving)
        {
            PlaySound(startSound);
        }
        else if (!isMoving && wasMoving)
        {
            PlaySound(stopSound);
        }

        wasMoving = isMoving;

        transform.position = Vector2.MoveTowards(transform.position, targetPosition, speed * Time.deltaTime);
    }

    private void PlaySound(AudioClip clip)
    {
        if (clip != null)
        {
            AudioSource.PlayClipAtPoint(clip, transform.position, volume);
        }
    }

    public void startPlatform(){
        //isMoving = true;
    }
}
