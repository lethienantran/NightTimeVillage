using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraControllerHome : MonoBehaviour
{
    public Transform target;
    public float smoothing;
    public Vector2 minPos;
    public Vector2 maxPos;

    public LobbyManager lobbyManager;
    public HomeCharacterSelection homeCharacterSelection;

    // Start is called before the first frame update
    void Start()
    {

    }

    public void startFindingTarget(GameObject playerHome)
    {
        target = playerHome.transform;
    }

    // Update is called once per frame
    void LateUpdate()
    {
        //transform.position = new Vector3(target.transform.position.x, target.transform.position.y, transform.position.z);
        if (lobbyManager.playerCreated && target != null)
        {
            if (transform.position != target.position)
            {
                Vector3 targetPos = new Vector3(target.position.x, target.position.y, transform.position.z);

                targetPos.x = Mathf.Clamp(target.position.x, minPos.x, maxPos.x);
                targetPos.y = Mathf.Clamp(target.position.y, minPos.y, maxPos.y);

                transform.position = Vector3.Lerp(transform.position, targetPos, smoothing);
            }
        }
    }
}
