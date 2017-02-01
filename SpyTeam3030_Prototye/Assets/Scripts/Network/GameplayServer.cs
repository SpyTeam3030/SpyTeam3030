﻿using UnityEngine;
using UnityEngine.Networking;
using System.Collections;
using System.Collections.Generic;


public enum LineType
{
    LeftLine = 0,
    Middle = 1,
    RightLine = 2
};

public class SpyInfo
{
    public int teamID;
    public LineType lineID;
    public GameObject spy;

    public SpyInfo(int id, LineType line, GameObject obj)
    {
        teamID = id;
        lineID = line;
        spy = obj;
    }
};

public class GameplayServer : NetworkBehaviour
{
    [Header("Spy Pool")]
    public List<GameObject> spyTypeList;
    public List<GameObject> towerTypeList;
    public List<GameObject> baseTypeList;
    public List<Transform> spawnPosList;
    public List<Transform> towerSpanPosList;
    public List<Transform> basePosList;
	public int winner;

    private int playerCount = 0;
    private int rotate = 0;
    public float time;
    GameObject[] clients;
    int[] towerCount = { 0, 0 };

    private List<SpyInfo> allSpyList;

	public List<GameObject> alltheSpys;

    void Start()
    {
        allSpyList = new List<SpyInfo>();
		alltheSpys = new List<GameObject> ();
		winner = 10;
        time = 240.0f;
		clients = null;
    }

    public int rotateCamera()
    {
        int copy = rotate;
        rotate++;
        return copy;
    }

    void Update()
    {
		if (clients == null || clients.Length != 2) {
			return;
		}
        if (winner == 10 && time > 0.0f)
        {
            time -= Time.deltaTime;
            for (int i = 0; i < clients.Length; i++)
            {
//                Debug.Log(i + "time");
                clients[i].GetComponent<GameplayClient>().UpdateTime(time);
            }

        }
        else
        {
            if (winner == 10)
            {
                if (towerCount[0] > towerCount[1])
                {
                    winner = 1;
                }
                else if (towerCount[0] < towerCount[1])
                {
                    winner = 0;
                }
            }
            for (int i = 0; i < clients.Length; i++)
            {
//                Debug.Log(i + "win");
                clients[i].GetComponent<GameplayClient>().EndGame(winner);
                clients[i].GetComponent<GameplayClient>().UpdateTime(0f);
            }
        }
    }

    public float GetTime()
    {
        return time;
    }

    public void SetWinner(int id)
    {
        winner = id;
    }

    public void UpdateTowerCount(int index)
    {
        towerCount[index]++;
    }

	public int GetPlayerCount(){
		return playerCount;
	}

    public void SpawnSpy(int id)
    {
        Debug.Log("spawn spy");
        playerCount++;

        // Instantiate the spys for the team with the id "id"
        for (int i = 0; i < 3; i++)
        {
            allSpyList.Add(new SpyInfo(id, (LineType)i, null));
        }

        if(playerCount == 2)
        {
            clients = GameObject.FindGameObjectsWithTag("Client");
            var posbase = basePosList[0].position;
            var rotationbase = basePosList[0].rotation;

            GameObject base0 = (GameObject)Instantiate(baseTypeList[0], posbase, rotationbase);
            base0.GetComponent<TowerController>().InitiID(0);
            NetworkServer.Spawn(base0);

            posbase = basePosList[1].position;
            rotationbase = basePosList[1].rotation;

            GameObject base1 = (GameObject)Instantiate(baseTypeList[1], posbase, rotationbase);
            base0.GetComponent<TowerController>().InitiID(1);
            NetworkServer.Spawn(base1);
            
            int[] index = {0,0};
            for(int i = 0; i < 6; i++)
            {
                // spawn the spay
                int localId = allSpyList[i].teamID;
				int num = localId * 3 + index[localId];
//				Debug.Log(allSpyList[i].lineID + " " + localId + " " + num);
				var pos = spawnPosList[num].position;
				var rotation = spawnPosList[num].rotation;

                GameObject enemy = (GameObject)Instantiate(spyTypeList[num], pos, rotation);
                enemy.GetComponent<SpyController>().InitilizeSpy(pos, basePosList[localId].position, localId);
                NetworkServer.Spawn(enemy);
				alltheSpys.Add (enemy);

                // spawn the related tower
				pos = towerSpanPosList[num].position;
				rotation = towerSpanPosList[num].rotation;
				GameObject tower = (GameObject)Instantiate(towerTypeList[localId], pos, rotation);
                tower.GetComponent<TowerController>().InitiID(localId);
                NetworkServer.Spawn(tower);

                index[localId]++;
            }
			Debug.Log ("alltheSpys " + alltheSpys.Count);
			GameObject.Find ("CardDisplay").GetComponent<CardDisplay>().SetSpys ();
        }

    }

	public void ChangeAttribute(string name, float maxHealthChange = 0.0f, float attackChange = 0.0f, float newSpeed = 0.0f, float newRadius = 0.0f, float newAttackSpeed = 0.0f){
		CombatController cc = GameObject.Find (name).GetComponent<CombatController> ();
		if (cc == null) {
			return;
		}
		cc.AttributeChange (maxHealthChange, attackChange, newSpeed, newRadius, newAttackSpeed);
	}
}

