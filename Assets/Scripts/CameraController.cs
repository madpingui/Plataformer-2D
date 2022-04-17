using UnityEngine;

public class CameraController : MonoBehaviour
{
    private Transform cameraTr, playerTr;
    private float playerInicialPosX;
    public static bool EnterToMapLimits;

    void Awake()
    {
       cameraTr = Camera.main.transform;
       playerTr = FindObjectOfType<PlayerController>().transform;
       playerInicialPosX = playerTr.position.x;
    }

    void Update()
    {
        if(!EnterToMapLimits){
            cameraTr.position = new Vector3(playerTr.position.x - playerInicialPosX, cameraTr.position.y, cameraTr.position.z);
        }
    }
}