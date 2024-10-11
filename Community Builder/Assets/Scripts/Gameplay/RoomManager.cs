using Newtonsoft.Json;
using System.Collections.Generic;
using UnityEngine;

public class RoomManager : MonoBehaviour, ISaveData
{
    [Header("Customization")]
    public string roomName = "Room Name";
    [Header("References")]
    [SerializeField] private List<TimeTask> tasks = new List<TimeTask>();
    public GameObject roomDoor;

    public TimeTask HasUnboughtTask()
    {
        foreach (TimeTask task in tasks)
            if (!task.gameObject.activeSelf)
                return task;

        return null;
    }

    public void BuyTask()
    {
        TimeTask nextPurchasable = HasUnboughtTask();

        float currentMoney = SaveSystemManager.Instance.gameData.GetData<float, string>("Money");
        SaveSystemManager.Instance.gameData.SetData("Money", currentMoney - nextPurchasable.purchaseCost);

        nextPurchasable.gameObject.SetActive(true);

        AstarPath.active.Scan();
    }

    public string GetSaveData()
    {
        List<string> tasksData = new List<string>();
        foreach (TimeTask task in tasks)
        {
            string[] taskData = new string[3]
            {
                task.name,
                task.gameObject.activeSelf.ToString(),
                task.GetSaveData(),
            };

            tasksData.Add(JsonConvert.SerializeObject(taskData, SaveSystem.serializeSettings));
        }

        return JsonConvert.SerializeObject(tasksData, SaveSystem.serializeSettings);
    }

    public void PutSaveData(string saveData)
    {
        List<string> tasksData = JsonConvert.DeserializeObject<List<string>>(saveData, SaveSystem.serializeSettings);
        foreach (string taskData_ in tasksData)
        {
            string[] taskData = JsonConvert.DeserializeObject<string[]>(taskData_, SaveSystem.serializeSettings);
            foreach (TimeTask task in tasks)
            {
                if (task.name == taskData[0])
                {
                    task.gameObject.SetActive(bool.Parse(taskData[1]));
                    task.PutSaveData(taskData[2]);
                    break;
                }
            }
        }

        AstarPath.active.Scan();
    }
}
