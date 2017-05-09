using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Collections.Generic;

public class GameplayClient : NetworkBehaviour
{

    private GameplayServer myServer;
    public int teamID = -1;
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
        gameObject.tag = "LocalClient";
    }

    void OnDisconnectedFromServer(NetworkDisconnection info) 
    {
        SceneManager.LoadScene("Main_l");
    }


    public void UpdateTime(float t)
    {
        RpcUpdateTime(t);
	}

    public void EndGame(int winner)
    {
        Debug.Log("winner is " + winner);
        if (winner == teamID)
        {
            RpcEndGame(0);
        }
        else if (winner == 10)
        {
            RpcEndGame(2);
        }
        else
        {
            RpcEndGame(1);
        }
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
        myServer.SpawnSpy(teamID, gameObject);
    }

    [ClientRpc]
    void RpcRotateCamera()
    {
        Debug.Log("rotate");
		Camera.main.GetComponent<Transform> ().position = new Vector3 (5.1f, 27.4f, 10.1f);
		Camera.main.GetComponent<Transform> ().eulerAngles = new Vector3 (66.798f, 180f, 0f);
    }

    [ClientRpc]
    public void RpcEndGame(int result)
    {
        if (result == 0)
        {
            GameObject.Find("WinLoseCanvas").GetComponent<EndGame>().Lose();
        }
        else if (result == 2)
        {
             GameObject.Find ("WinLoseCanvas").GetComponent<EndGame> ().Draw();
        }
        else if(result == 1)
        {
             GameObject.Find ("WinLoseCanvas").GetComponent<EndGame> ().Win();
        }
    }

    [ClientRpc]
    void RpcUpdateTime(float t)
    {
        GameObject.Find ("TimePanel").GetComponent<Timer> ().UpdateTime(t);
    }

	[Command]
	public void CmdAttributeChange(string name, int cardID, float maxHealthChange, float attackChange, float newSpeed, float newRadius, float newAttackSpeed, float damage){
			myServer.ChangeAttribute (name, cardID, maxHealthChange, attackChange, newSpeed, newRadius, newAttackSpeed, damage);
	}

	[Command]
	public void CmdDisableHUD(int id){
		myServer.DisableHUD (id);
	}

	[Command]
	public void CmdSetSpys(){
		myServer.SetSpys ();
	}

    [Command]
    public void CmdDoDamageToSingleTower(float damage, int id)
    {
        myServer.doDamageToSingleTower(damage, id);
    }

	[ClientRpc]
	public void RpcFindAndDisableHUD(){
		GameObject.Find ("Canvas").GetComponent<CardHUD> ().DisableHUD ();
	}
}
