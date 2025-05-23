using UnityEngine;

public class CameraController : MonoBehaviour
{
    [Header("Position relative to player will be kept except the height magic.")]

    Transform Player;

    Vector3 StartPos;
    Vector3 PlayerStartPos;

    Vector3 Offset;
    Vector3 Position;

    void Start()
    {
        Player = GameObject.Find("Player").transform;
        StartPos = transform.position;
        PlayerStartPos = Player.position;

        Offset = StartPos - PlayerStartPos;
    }

    void Update()
    {
        Position = Offset + Player.position;

        gameObject.transform.position = new Vector3(Position.x, StartPos.y + ((Player.position.y - PlayerStartPos.y) / 3f), Position.z);
    }
}
