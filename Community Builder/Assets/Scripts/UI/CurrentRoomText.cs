using System.Collections;
using TMPro;
using UnityEngine;

public class CurrentRoomText : MonoBehaviour
{
    private TextMeshProUGUI roomNameText;
    private Coroutine changeNameText;

    private void Start()
    {
        roomNameText = GetComponent<TextMeshProUGUI>();

        RoomChanged(GameManager.Instance.currentRoom);
        GameManager.roomChanged += RoomChanged;
    }

    private void RoomChanged(RoomManager newRoom)
    {
        if (newRoom == null) return;

        if (changeNameText != null) StopCoroutine(changeNameText);
        changeNameText = StartCoroutine(ChangeNameText(newRoom.roomName));
    }

    private IEnumerator ChangeNameText(string newRoomText)
    {
        while (roomNameText.text.Length > 0)
        {
            roomNameText.text = roomNameText.text.Remove(roomNameText.text.Length - 1);
            yield return new WaitForSeconds(0.025f);
        }

        yield return new WaitForSeconds(0.05f);

        for (int character = 0; character < newRoomText.Length; character++)
        {
            roomNameText.text += newRoomText[character];
            yield return new WaitForSeconds(0.05f);
        }
    }
}
