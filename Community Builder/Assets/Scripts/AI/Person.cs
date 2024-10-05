using Pathfinding;
using UnityEngine;

public class Person : MonoBehaviour
{
    public PersonUse use;
    public RoomManager spawnedRoom;

    [SerializeField] private Animator shadowAnimator;

    private AIPath aiPath;
    private AIDestinationSetter destinationSetter;
    private Animator animator;

    private Vector3 targetPosition = new Vector3(0, 0, 4);
    private float timeStill = -1f;

    private PersonState state = PersonState.MovingToTask;
    private bool movedToPosition;
    private bool finishedTask;

    private void Start()
    {
        animator = GetComponent<Animator>();
        aiPath = GetComponent<AIPath>();
        destinationSetter = GetComponent<AIDestinationSetter>();
    }

    private void Update()
    {
        destinationSetter.target.position = targetPosition;

        animator.SetBool("walking", aiPath.desiredVelocity != Vector3.zero);
        shadowAnimator.SetBool("walking", aiPath.desiredVelocity != Vector3.zero);

        animator.SetBool("performingTask", state == PersonState.PerformingTask);
        shadowAnimator.SetBool("performingTask", state == PersonState.PerformingTask);

        if (aiPath.velocity == Vector3.zero)
            timeStill += Time.deltaTime;
        else
            timeStill = 0f;

        if (state == PersonState.PerformingTask && timeStill >= 4f)
        {
            timeStill = -1f;
            aiPath.canMove = true;
            state = PersonState.MovingToDoor;
            UpdateTargetPosition(spawnedRoom.roomDoor.transform.position, 0f);
        }
        else if (state == PersonState.MovingToTask && timeStill > 0)
        {
            state = PersonState.PerformingTask;
            aiPath.canMove = false;
        }
        else if (state == PersonState.MovingToDoor && timeStill > 0.5f)
        {
            Destroy(gameObject);
        }
    }

    public void UpdateTargetPosition(Vector2 newPosition, float positionVariation)
    {
        targetPosition = newPosition;
        targetPosition += new Vector3(Random.Range(-positionVariation, positionVariation), Random.Range(-positionVariation, positionVariation), 0);
    }
}

public enum PersonUse
{
    Wander,
    MoveToThenLeave,
    MoveToAndPerformThenLeave,
}

public enum PersonState
{
    MovingToTask,
    PerformingTask,
    MovingToDoor,
}
