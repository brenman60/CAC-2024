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

        GameObject newPerson = Instantiate(personPrefab, transform);
        Person person = newPerson.GetComponent<Person>();
        person.use = PersonUse.MoveToAndPerformThenLeave;
        person.spawnedRoom = taskRoom;
        newPerson.transform.position = taskRoom.roomDoor.transform.position;
        person.UpdateTargetPosition(transform.position + new Vector3(0, 2.5f), 2.5f, 0.25f);
    }
}
