﻿using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Collections.Generic;

public class GameplayClient : NetworkBehaviour
{

    private GameplayServer myServer;
    public int teamID;
	private EndGame mEndGameCanvas;

    public override void OnStartServer()
    {
        myServer = GameObject.Find("Gameplay_Server").GetComponent<GameplayServer>();
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


    public void UpdateTime(float t)
    {
        RpcUpdateTime(t);
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

    public void EndGame(int winner)
    {
        RpcEndGame(winner);
    }

    [ClientRpc]
    void RpcEndGame(int winner)
    {
        if (winner == teamID)
        {
            // win   
            GameObject.Find("WinLoseCanvas").GetComponent<EndGame>().Win();
        }
        else if (winner == 10)
        {
             GameObject.Find ("WinLoseCanvas").GetComponent<EndGame> ().Draw();
        }
        else
        {
             GameObject.Find ("WinLoseCanvas").GetComponent<EndGame> ().Lose();
        }
    }

    [ClientRpc]
    void RpcUpdateTime(float t)
    {
        GameObject.Find ("TimePanel").GetComponent<Timer> ().UpdateTime(t);
    }

	public void AttributeChange(bool success, string name, int cardID, float maxHealthChange, float attackChange, float newSpeed, float newRadius, float newAttackSpeed){
		CmdAttributeChange (success, name, cardID, maxHealthChange, attackChange, newSpeed, newRadius, newAttackSpeed);
	}

	[Command]
	public void CmdAttributeChange(bool success, string name, int cardID, float maxHealthChange, float attackChange, float newSpeed, float newRadius, float newAttackSpeed){
		myServer.ChangeAttribute (success, name, cardID, maxHealthChange, attackChange, newSpeed, newRadius, newAttackSpeed);
	}

	[Command]
	public void CmdSetSpys(){
		myServer.SetSpys ();
	}
}
