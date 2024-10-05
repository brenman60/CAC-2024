using UnityEngine;

public class SignUpStation : TimeTask
{
    [Header("SignUpStation References")]
    [SerializeField] private GameObject personPrefab;

    protected override void Start()
    {
        base.Start();

        CreateNewPerson();
    }

    public override void OnCompletion()
    {
        base.OnCompletion();

        CreateNewPerson();
    }

    private void CreateNewPerson()
    {
        GameObject newPerson = Instantiate(personPrefab, transform);
        Person person = newPerson.GetComponent<Person>();
        person.use = PersonUse.MoveToAndPerformThenLeave;
        person.spawnedRoom = taskRoom;
        newPerson.transform.position = taskRoom.roomDoor.transform.position;
        person.UpdateTargetPosition(transform.position + new Vector3(0, 5f), 3f);
    }
}
