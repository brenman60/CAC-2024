using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class GameManager : MonoBehaviour, ISaveData
{
    public static GameManager Instance { get; private set; }

    public static event EventHandler<RoomManager> screenTapped;

    [SerializeField] private Vector3 cameraRoomOffset;
    [SerializeField] private Vector3 cameraRoomRotation;
    [Space(20), SerializeField] private RoomManager[] rooms;
    private RoomManager currentRoom;

    private Camera mainCam;

    private bool dragging;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        mainCam = Camera.main;
        currentRoom = rooms[0];

        SaveSystemManager.Instance.settings.inputClick.performed += ScreenTouch;
    }

    private void Update()
    {
        bool dragging_ = SaveSystemManager.Instance.settings.inputClick.ReadValue<float>() != 0;
        if (dragging_ != dragging && !dragging) StartCoroutine(DragCamera());
        else if (dragging_ != dragging && dragging) SnapPositionToRoom();
        dragging = dragging_;

        if (!dragging)
        {
            mainCam.transform.position = Vector3.Lerp(mainCam.transform.position, new Vector3(currentRoom.transform.position.x, currentRoom.transform.position.y, currentRoom.transform.position.z) + cameraRoomOffset, Time.deltaTime * 5f);
            mainCam.transform.rotation = Quaternion.Slerp(mainCam.transform.rotation, Quaternion.Euler(cameraRoomRotation.x, cameraRoomRotation.y, cameraRoomRotation.z), Time.deltaTime * 5f);
        }
    }

    public void ScreenTouch(InputAction.CallbackContext obj)
    {
        screenTapped?.Invoke(this, currentRoom);
    }

    private IEnumerator DragCamera()
    {
        Vector3 mousePos = Mouse.current.position.ReadValue();
        mousePos.z = 10f;
        Vector3 initialMousePosition = mainCam.ScreenToWorldPoint(mousePos);
        while (SaveSystemManager.Instance.settings.inputClick.ReadValue<float>() != 0)
        {
            mousePos = Mouse.current.position.ReadValue();
            mousePos.z = 10f;
            Vector3 currentMousePosition = mainCam.ScreenToWorldPoint(mousePos);
            Vector3 travel = currentMousePosition - initialMousePosition;
            travel.y = 0;

            mainCam.transform.position -= travel;

            yield return null;
        }
    }

    private void SnapPositionToRoom()
    {
        RoomManager closestRoom = null;
        foreach (RoomManager room in rooms)
            if (closestRoom == null)
                closestRoom = room;
            else if ((room.transform.position - mainCam.transform.position).sqrMagnitude < (closestRoom.transform.position - mainCam.transform.position).sqrMagnitude)
                closestRoom = room;

        currentRoom = closestRoom;
    }

    public string GetSaveData()
    {
        List<string> roomsData = new List<string>();
        foreach (RoomManager roomManager in rooms)
        {
            string[] roomData = new string[2]
            {
                roomManager.name,
                roomManager.GetSaveData(),
            };

            roomsData.Add(JsonConvert.SerializeObject(roomData, SaveSystem.serializeSettings));
        }

        return JsonConvert.SerializeObject(roomsData, SaveSystem.serializeSettings);
    }

    public void PutSaveData(string saveData)
    {
        List<string> roomsData = JsonConvert.DeserializeObject<List<string>>(saveData, SaveSystem.serializeSettings);
        foreach (string roomData_ in roomsData)
        {
            string[] roomData = JsonConvert.DeserializeObject<string[]>(roomData_, SaveSystem.serializeSettings);
            foreach (RoomManager roomManager in rooms)
            {
                if (roomManager.name == roomData[0])
                {
                    roomManager.PutSaveData(roomData[1]);
                    break;
                }
            }
        }
    }
}
