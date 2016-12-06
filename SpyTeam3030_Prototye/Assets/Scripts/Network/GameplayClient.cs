using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Collections.Generic;

public class GameplayClient : NetworkBehaviour
{

    private GameplayServer myServer;
    private int teamID;
	[SyncVar]
	public float time;
	public Timer mTimer;

    public override void OnStartServer()
    {
        myServer = GameObject.Find("Gameplay_Server").GetComponent<GameplayServer>();
		time = 240f;
    }

	void Update(){
		if (myServer != null && myServer.GetPlayerCount() == 2) {
			Debug.Log("timer");
			time -= Time.deltaTime;
		}
		GameObject.Find ("TimePanel").GetComponent<Timer> ().UpdateTime (time);
	}

    public override void OnStartLocalPlayer()
    {
        Debug.Log("OnStartLocalPlayer");
        CmdRoateCamera();
        CmdSpawnSpy();
    }

    void OnDisconnectedFromServer(NetworkDisconnection info) 
    {
        SceneManager.LoadScene("Main_l");
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
//        Camera.main.GetComponent<Transform>().RotateAround(Vector3.zero, Vector3.up, 180.0f);
		Camera.main.GetComponent<Transform> ().position = new Vector3 (5.1f, 27.4f, 10.1f);
		Camera.main.GetComponent<Transform> ().eulerAngles = new Vector3 (66.798f, 180f, 0f);
    }
}
