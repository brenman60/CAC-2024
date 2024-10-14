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
    public static event Action<RoomManager> roomChanged;

    [SerializeField] private Vector3 cameraRoomOffset;
    [SerializeField] private Vector3 cameraRoomRotation;
    [Space(20), SerializeField] private RoomManager[] rooms;

    public RoomManager currentRoom { get; private set; }

    private Camera mainCam;

    private float tapCooldown;
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
        tapCooldown -= Time.deltaTime;

        bool dragging_ = SaveSystemManager.Instance.settings.inputClick.ReadValue<float>() != 0;
        if (dragging_ != dragging && !dragging) StartCoroutine(DragCamera());
        dragging = dragging_;

        if (!dragging)
        {
            mainCam.transform.position = Vector3.Lerp(mainCam.transform.position, new Vector3(currentRoom.transform.position.x, currentRoom.transform.position.y, currentRoom.transform.position.z) + cameraRoomOffset, Time.deltaTime * 5f);
            mainCam.transform.rotation = Quaternion.Slerp(mainCam.transform.rotation, Quaternion.Euler(cameraRoomRotation.x, cameraRoomRotation.y, cameraRoomRotation.z), Time.deltaTime * 5f);
        }
    }

    public void ScreenTouch(InputAction.CallbackContext obj)
    {
        if (tapCooldown > 0f) return;
        tapCooldown = 0.05f;

        screenTapped?.Invoke(this, currentRoom);
    }

    private IEnumerator DragCamera()
    {
        Vector2 startingPosition = Mouse.current.position.ReadValue();
        Vector2 completeDrag = Vector2.zero;
        while (SaveSystemManager.Instance.settings.inputClick.ReadValue<float>() != 0)
        {
            completeDrag = Mouse.current.position.ReadValue() - startingPosition;
            yield return new WaitForEndOfFrame();
        }

        completeDrag /= new Vector2(Screen.width, Screen.height);
        completeDrag = new Vector2(Mathf.Clamp(completeDrag.x, -0.2f, 0.2f), 0);

        SnapPositionToRoom(mainCam.transform.position - (new Vector3(completeDrag.x * 80f, 0)));
    }

    private void SnapPositionToRoom(Vector3 position)
    {
        RoomManager closestRoom = null;
        foreach (RoomManager room in rooms)
            if (closestRoom == null)
                closestRoom = room;
            else if ((room.transform.position - position).sqrMagnitude < (closestRoom.transform.position - position).sqrMagnitude)
                closestRoom = room;

        if (closestRoom != currentRoom)
            roomChanged?.Invoke(closestRoom);

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
