using Newtonsoft.Json;
using System.Collections.Generic;
using UnityEngine;

public class RoomManager : MonoBehaviour, ISaveData
{
    [SerializeField] private List<TimeTask> tasks = new List<TimeTask>();

    public string GetSaveData()
    {
        List<string> tasksData = new List<string>();
        foreach (TimeTask task in tasks)
        {
            string[] taskData = new string[3]
            {
                task.name,
                task.enabled.ToString(),
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
                    task.enabled = bool.Parse(taskData[1]);
                    task.PutSaveData(taskData[2]);
                    break;
                }
            }
        }
    }
}
