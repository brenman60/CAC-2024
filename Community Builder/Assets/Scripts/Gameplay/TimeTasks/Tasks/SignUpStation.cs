using UnityEngine;

public class SignUpStation : TimeTask
{
    [Header("SignUpStation Customization")]
    [SerializeField] private int personSortingGroup;

    [Header("SignUpStation References")]
    [SerializeField] private GameObject personPrefab;

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
        personCooldown = 1.25f;

        PersonManager.Instance.SpawnPerson(new System.Collections.Generic.List<PersonNode>()
        {
            new PersonNode(taskRoom.roomDoor.transform.position, Vector2.zero, 0f, PersonAction.Walking, PersonAction.Walking, false),
            new PersonNode(transform.position + new Vector3(0, 2.5f), new Vector2(2.5f, 0.25f), 4f, PersonAction.Walking, PersonAction.PerformingTask, false),
            new PersonNode(taskRoom.roomDoor.transform.position, Vector2.zero, 0f, PersonAction.Walking, PersonAction.Walking, false),
            new PersonNode(PersonManager.Instance.GetRandomSpawnPoint().position, Vector2.zero, 0f, PersonAction.Walking, PersonAction.Walking, true),
        });
    }
}
