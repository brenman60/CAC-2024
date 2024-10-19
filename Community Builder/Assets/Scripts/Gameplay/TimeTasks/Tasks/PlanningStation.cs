using UnityEngine;

public class PlanningStation : TimeTask
{
    [Header("PlanningStation References")]
    [SerializeField] private Transform personPathfindingAnchor;

    private float personCooldown;

    protected override void Start()
    {
        base.Start();

        CreateNewPerson();
    }

    protected override void Update()
    {
        base.Update();

        personCooldown -= Time.deltaTime;
    }

    public override void OnCompletion()
    {
        base.OnCompletion();

        CreateNewPerson();
    }

    private void CreateNewPerson()
    {
        if (personCooldown > 0) return;
        personCooldown = 20f;

        PersonManager.Instance.SpawnPerson(new System.Collections.Generic.List<PersonNode>()
        {
            new PersonNode(taskRoom.roomDoor.transform.position, Vector2.zero, 0f, Quaternion.identity, PersonAction.Walking, PersonAction.Walking, false),
            new PersonNode(personPathfindingAnchor.position, Vector2.zero, 25f, Quaternion.Inverse(personPathfindingAnchor.rotation), PersonAction.Walking, PersonAction.Idle, false),
            new PersonNode(taskRoom.roomDoor.transform.position, Vector2.zero, 0f, Quaternion.identity, PersonAction.Walking, PersonAction.Walking, false),
            new PersonNode(PersonManager.Instance.GetRandomSpawnPoint().position, Vector2.zero, 0f, Quaternion.identity, PersonAction.Walking, PersonAction.Walking, true),
        });
    }
}
