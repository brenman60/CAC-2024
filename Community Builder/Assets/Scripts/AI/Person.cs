using Pathfinding;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class Person : MonoBehaviour
{
    [HideInInspector] public RoomManager spawnedRoom;

    [SerializeField] private List<PersonPart> bodyParts;
    [SerializeField] private List<Color> personColors;

    private AIPath aiPath;
    private AIDestinationSetter destinationSetter;
    private Animator animator;
    private SortingGroup sortingGroup;

    private Vector3 targetPosition = new Vector3(0, 0, 4);
    private float timeStill = -1f;

    private PersonAction currentAction;
    private List<PersonNode> movementNodes = new List<PersonNode>();
    private float timeAtCurrentNode;

    private void Start()
    {
        animator = GetComponent<Animator>();
        aiPath = GetComponent<AIPath>();
        destinationSetter = GetComponent<AIDestinationSetter>();
        sortingGroup = GetComponent<SortingGroup>();

        Color randomColor = personColors[Random.Range(0, personColors.Count)];
        foreach (PersonPart bodyPart in bodyParts)
            bodyPart.part.material.color = randomColor - (bodyPart.faded ? new Color(0.05f, 0.05f, 0.05f) : new Color(0, 0, 0));
    }

    private void Update()
    {
        sortingGroup.sortingOrder = -Mathf.RoundToInt(transform.position.y * 1000) - 10000;

        destinationSetter.target.position = new Vector3(targetPosition.x, targetPosition.y, transform.position.z);

        if (aiPath.desiredVelocity.x > 2f || aiPath.desiredVelocity.y > 2f)
        {
            float angle = Mathf.Atan2(aiPath.desiredVelocity.y, aiPath.desiredVelocity.x) * Mathf.Rad2Deg;
            if (angle >= -45 && angle < 45)
            {
                // Facing right
                transform.rotation = Quaternion.Euler(0, -90, 90);
            }
            else if (angle >= 45 && angle < 135)
            {
                // Facing up
                transform.rotation = Quaternion.Euler(90, -90, 90);
            }
            else if (angle >= 135 || angle < -135)
            {
                // Facing left
                transform.rotation = Quaternion.Euler(0, 90, -90);
            }
            else if (angle >= -135 && angle < -45)
            {
                // Facing down
                transform.rotation = Quaternion.Euler(-90, -90, 90);
            }
        }

        if (movementNodes.Count > 0)
        {
            PersonNode currentNode = movementNodes[0];
            if ((transform.position - destinationSetter.target.position).sqrMagnitude <= 3f * 3f)
            {
                currentAction = currentNode.waitingAction;

                timeAtCurrentNode += Time.deltaTime;
                if (timeAtCurrentNode >= currentNode.waitingTime)
                {
                    timeAtCurrentNode = 0f;
                    movementNodes.Remove(currentNode);

                    if (movementNodes.Count > 0)
                        UpdateTargetPosition(movementNodes[0].position, movementNodes[0].positionRadius.x, movementNodes[0].positionRadius.y);

                    if (currentNode.killPersonOnCompletion)
                        gameObject.SetActive(false);
                }
            }
            else
            {
                currentAction = currentNode.movingToAction;
            }
        }

        animator.SetBool("walking", currentAction == PersonAction.Walking);
        animator.SetBool("performingTask", currentAction == PersonAction.PerformingTask);
    }

    public void UpdateTargetPosition(Vector2 newPosition, float xPositionVariation, float yPositionVariation)
    {
        targetPosition = newPosition;
        targetPosition += new Vector3(Random.Range(-xPositionVariation, xPositionVariation), Random.Range(-yPositionVariation, yPositionVariation), 0);
    }

    public void SetMovementNodes(List<PersonNode> nodes)
    {
        movementNodes = nodes;
        UpdateTargetPosition(movementNodes[0].position, movementNodes[0].positionRadius.x, movementNodes[0].positionRadius.y);
    }

    [System.Serializable]
    private struct PersonPart
    {
        public MeshRenderer part;
        public bool faded;
    }
}

public struct PersonNode 
{
    public Vector3 position;
    public Vector2 positionRadius;
    public float waitingTime;
    public PersonAction movingToAction;
    public PersonAction waitingAction;
    public bool killPersonOnCompletion;

    public PersonNode(Vector3 position, Vector2 positionRadius, float waitingTime, PersonAction movingToAction, PersonAction waitingAction, bool killPersonOnCompletion)
    {
        this.position = position;
        this.positionRadius = positionRadius;
        this.waitingTime = waitingTime;
        this.movingToAction = movingToAction;
        this.waitingAction = waitingAction;
        this.killPersonOnCompletion = killPersonOnCompletion;
    }
}

public enum PersonAction
{
    Idle,
    Walking,
    PerformingTask,
}
