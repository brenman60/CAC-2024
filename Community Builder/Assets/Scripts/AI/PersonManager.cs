using System.Collections.Generic;
using UnityEngine;

public class PersonManager : MonoBehaviour
{
    public static PersonManager Instance { get; private set; }

    [SerializeField] private GameObject personPrefab;
    [SerializeField] private List<Transform> personSpawnPoints = new List<Transform>();

    private List<Person> personPool = new List<Person>();

    private void Awake()
    {
        Instance = this;
    }

    public Transform GetRandomSpawnPoint()
    {
        return personSpawnPoints[Random.Range(0, personSpawnPoints.Count)];
    }

    public void SpawnPerson(List<PersonNode> nodes)
    {
        Person personObject = GetPersonInPool();
        personObject.SetMovementNodes(nodes);
        personObject.transform.position = GetRandomSpawnPoint().position;
    }

    public Person GetPersonInPool()
    {
        foreach (Person person in personPool)
            if (!person.gameObject.activeSelf)
            {
                person.gameObject.SetActive(true);
                return person;
            }

        GameObject newPersonObj = Instantiate(personPrefab, transform);
        Person newPerson = newPersonObj.GetComponent<Person>();
        personPool.Add(newPerson);
        return newPerson;
    }
}
