using UnityEngine;
using UnityEngine.Networking;
using System.Collections;
using System.Collections.Generic;

public class GameplayClient : NetworkBehaviour
{

    private GameplayServer myServer;
    private int teamID;

    public override void OnStartServer()
    {
        myServer = GameObject.Find("Gameplay_Server").GetComponent<GameplayServer>();
    }

    public override void OnStartLocalPlayer()
    {
        Debug.Log("hjgjk");
        CmdRoateCamera();
        CmdSpawnSpy();
    }

    [Command]
    void CmdRoateCamera()
    {
        if((teamID = myServer.rotateCamera()) == 0)
        {
            RpcRotateCamera();
        }
    }

    [Command]
    void CmdSpawnSpy()
    {
        myServer.SpawnSpy(teamID);
    }

    [ClientRpc]
    void RpcRotateCamera()
    {
        Debug.Log("rotate");
        Camera.main.GetComponent<Transform>().RotateAround(Vector3.zero, Vector3.up, 180.0f);
    }
}
