using UnityEngine;
using UnityEngine.Networking;
using System.Collections;
using System.Collections.Generic;


public class GameplayServer : NetworkBehaviour
{
    enum LineType
    {
        LeftLine = 0,
        Middle = 1,
        RightLine = 2
    }

    struct SpyInfo
    {
        int teamID;
        LineType lineID;
        GameObject spy;

        public SpyInfo(int id, LineType line, GameObject obj)
        {
            teamID = id;
            lineID = line;
            spy = obj;
        }
    }

    [Header("Spy Pool")]
    public List<GameObject> spyTypeList;
    public List<Transform> spawnPosList;
    public List<Transform> basePosList;

    private int playerCount = 0;
    private int rotate = 0;

    private List<SpyInfo> allSpyList;


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

        for (int i = 0; i < 3; i++)
        {
            var pos = spawnPosList[id * 3 + i].position;
            var rotation = spawnPosList[id * 3 + i].rotation;

            var enemy = (GameObject)Instantiate(spyTypeList[0], pos, rotation);
            enemy.GetComponent<SpyController>().InitilizeSpy(pos, basePosList[id].position);
            SpyInfo info = new SpyInfo(id, (LineType)i, enemy);
            enemy.SetActive(false);

            NetworkServer.Spawn(enemy);
        }
    }
}
