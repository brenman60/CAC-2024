using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] private RoomManager[] rooms;
    private RoomManager currentRoom;

    private void Start()
    {
        currentRoom = rooms[0];

        SaveSystemManager.Instance.settings.inputTouch.performed += ScreenTouch;
    }

    public void ScreenTouch(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {
        print("Screen touched");
    }
}
