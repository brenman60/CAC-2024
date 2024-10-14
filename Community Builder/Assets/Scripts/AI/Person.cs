using Pathfinding;
using System.Collections.Generic;
using UnityEngine;

public class Person : MonoBehaviour
{
    [HideInInspector] public RoomManager spawnedRoom;

    [SerializeField] private List<PersonPart> bodyParts;
    [SerializeField] private List<Color> personColors;

    private AIPath aiPath;
    private AIDestinationSetter destinationSetter;
    private Animator animator;
    private bool gotComponents;

    private Vector3 targetPosition;
    private float timeStill = -1f;

    private PersonAction currentAction;
    private List<PersonNode> movementNodes = new List<PersonNode>();
    private float timeAtCurrentNode;

    private void OnEnable()
    {
        if (!gotComponents) GetComponents();

        float randomScale = Random.Range(0.8f, 1.2f);
        transform.localScale = (Vector3.one * 0.6f) * randomScale;

        aiPath.maxSpeed = Random.Range(2.5f, 4f);
        animator.speed = aiPath.maxSpeed / 3f;

        Color randomColor = personColors[Random.Range(0, personColors.Count)];
        foreach (PersonPart bodyPart in bodyParts)
            bodyPart.part.material.color = randomColor - (bodyPart.faded ? new Color(0.05f, 0.05f, 0.05f) : new Color(0, 0, 0));
    }

    private void GetComponents()
    {
        gotComponents = true;

        animator = GetComponent<Animator>();
        aiPath = GetComponent<AIPath>();
        destinationSetter = GetComponent<AIDestinationSetter>();
    }

    private void Update()
    {
        destinationSetter.target.position = new Vector3(targetPosition.x, targetPosition.y, targetPosition.z);

        if (movementNodes.Count > 0)
        {
            PersonNode currentNode = movementNodes[0];
            if ((transform.position - destinationSetter.target.position).sqrMagnitude <= 2.5f * 2.5f)
            {
                currentAction = currentNode.waitingAction;

                timeAtCurrentNode += Time.deltaTime;
                if (timeAtCurrentNode >= currentNode.waitingTime)
                {
                    timeAtCurrentNode = 0f;
                    movementNodes.Remove(currentNode);

                    if (movementNodes.Count > 0)
                        UpdateTargetPosition(movementNodes[0].position, movementNodes[0].positionRadius.x, movementNodes[0].positionRadius.y, movementNodes[0].positionRadius.z);

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

    public void UpdateTargetPosition(Vector3 newPosition, float xPositionVariation, float yPositionVariation, float zPositionVariation)
    {
        targetPosition = newPosition;
        targetPosition += new Vector3(Random.Range(-xPositionVariation, xPositionVariation), Random.Range(-yPositionVariation, yPositionVariation), Random.Range(-zPositionVariation, zPositionVariation));
    }

    public void SetMovementNodes(List<PersonNode> nodes)
    {
        movementNodes = nodes;
        UpdateTargetPosition(movementNodes[0].position, movementNodes[0].positionRadius.x, movementNodes[0].positionRadius.y, movementNodes[0].positionRadius.z);
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
    public Vector3 positionRadius;
    public float waitingTime;
    public PersonAction movingToAction;
    public PersonAction waitingAction;
    public bool killPersonOnCompletion;

    public PersonNode(Vector3 position, Vector3 positionRadius, float waitingTime, PersonAction movingToAction, PersonAction waitingAction, bool killPersonOnCompletion)
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
