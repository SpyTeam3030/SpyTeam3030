using UnityEngine;
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
    public List<Transform> spawnPosList;
    public List<Transform> towerSpanPosList;
    public List<Transform> basePosList;

    private int playerCount = 0;
    private int rotate = 0;

    private List<SpyInfo> allSpyList;

    void Start()
    {
        allSpyList = new List<SpyInfo>();
    }

    public int rotateCamera()
    {
        int copy = rotate;
        rotate++;
        return copy;
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

                // spawn the related tower
				pos = towerSpanPosList[num].position;
				rotation = towerSpanPosList[num].rotation;
                GameObject tower = (GameObject)Instantiate(towerTypeList[0], pos, rotation);
                tower.GetComponent<TowerController>().InitiID(localId);
                NetworkServer.Spawn(tower);

                index[localId]++;
            }
        }

    }
}

