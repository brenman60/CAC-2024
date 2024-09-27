using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class GameManager : MonoBehaviour
{
    [SerializeField] private Vector3 cameraRoomOffset;
    [Space(20), SerializeField] private RoomManager[] rooms;
    private RoomManager currentRoom;

    private Camera mainCam;

    private bool dragging;

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
            mainCam.transform.position = Vector3.Lerp(mainCam.transform.position, new Vector3(currentRoom.transform.position.x, mainCam.transform.position.y, mainCam.transform.position.z) + cameraRoomOffset, Time.deltaTime * 20f);
    }

    public void ScreenTouch(InputAction.CallbackContext obj)
    {
        print("Screen touched");
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
}
