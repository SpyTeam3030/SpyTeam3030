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

public enum EffectType
{
    LINE,
    ALL,
    SINGLE
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
    
public class SpecialEffectInfo
{
    public EffectType m_type;
    public LineType lineID;
    public int m_enemyTeamID;
    public int m_myTeamID;
    public int [] spyID = new int[6];

    // values[0] = damage, values[1] = health, values[2] = aux
    public int [] values = {0, 0, 0}; 
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

    public List<GameObject> alltheTowers;

    private ConsoleView m_consoleView;

    void Start()
    {
        allSpyList = new List<SpyInfo>();
        alltheSpys = new List<GameObject> ();
        m_consoleView = GameObject.FindGameObjectWithTag("DebugConsole").GetComponent<ConsoleView>();
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
        if (clients == null || clients.Length != 2) 
        {
            return;
        }
        if (winner == 10 && time > 0.0f)
        {
            time -= Time.deltaTime;
            for (int i = 0; i < clients.Length; i++)
            {
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

    public int GetPlayerCount()
    {
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
                int localId = allSpyList[i].teamID;
                int num = localId * 3 + index[localId];
                var pos = spawnPosList[num].position;
                var rotation = spawnPosList[num].rotation;

                GameObject enemy = (GameObject)Instantiate(spyTypeList[num], pos, rotation);
                enemy.GetComponent<SpyController>().InitilizeSpy(pos, basePosList[localId].position, localId, i);
                NetworkServer.Spawn(enemy);
                alltheSpys.Add (enemy);

                pos = towerSpanPosList[num].position;
                rotation = towerSpanPosList[num].rotation;
                GameObject tower = (GameObject)Instantiate(towerTypeList[localId], pos, rotation);
                tower.GetComponent<TowerController>().InitiID(localId);
                NetworkServer.Spawn(tower);

                index[localId]++;
            }
            Debug.Log ("alltheSpys " + alltheSpys.Count);
            CallSetSpys ();
        }

        allSpyList.Clear();

    }

    public void SetSpys()
    {
        GameObject.Find ("CardDisplay").GetComponent<CardDisplay>().SetSpys ();
    }

    public void ChangeAttribute(bool success, string name, int cardID, float maxHealthChange = 0.0f, float attackChange = 0.0f, float newSpeed = 0.0f, float newRadius = 0.0f, float newAttackSpeed = 0.0f)
    {
        CombatController cc = GameObject.Find (name).GetComponent<CombatController> ();
        if (cc == null) 
        {
            success = false;
            return;
        }
        cc.AttributeChange (cardID, maxHealthChange, attackChange, newSpeed, newRadius, newAttackSpeed);
        for (int i = 0; i < alltheSpys.Count; ++i)
        {
            CombatController m_cc = alltheSpys[i].GetComponent<CombatController>();
            m_consoleView.inputField.text = m_cc.name + " (Health: " + m_cc.health + " Attack: " + m_cc.attackPower + "/" + m_cc.attackRadius
                + "/" + m_cc.attackSpeed +" )";
            m_consoleView.runCommand();
        }
    }

    public void CallSetSpys()
    {
        GameObject[] obs = GameObject.FindGameObjectsWithTag ("Client");
        if (obs.Length == 0) 
        {
            return;
        }
        foreach(var gameobject in obs)
        {
            if(gameobject.GetComponent<GameplayClient>().isLocalPlayer)
            {
                gameobject.GetComponent<GameplayClient> ().CmdSetSpys();
            }
        }

    }


    // handler for taking effect to all enemies
    public void doDamageToAllEnemies(ref SpecialEffectInfo handler)
    {
        foreach (GameObject go in alltheSpys)
        {
            CombatController cc = go.GetComponent<CombatController>();
            if (cc.getID() != handler.m_myTeamID)
            {
                cc.TakeDamge(handler.values[0]);
                cc.TakeHealth(handler.values[1]);
            }
        }
    }

    // handler for doing damage to one single enemy
    public void doDamageToSingleEnemy(ref SpecialEffectInfo handler)
    {
        foreach (GameObject go in alltheSpys)
        {
            CombatController cc = go.GetComponent<CombatController>();
            if (cc.isEqualByID(handler.spyID[0]))
            {
                cc.TakeDamge(handler.values[0]);
                cc.TakeHealth(handler.values[1]);
                break;
            }
        }
    }


    public void takeSpecialEffect(ref SpecialEffectInfo handler)
    {
        switch(handler.m_type)
        {
            case EffectType.LINE:
                //TODO
                break;
            case EffectType.ALL:
                //TODO
                break;
            case EffectType.SINGLE:
                //TODO
                break;
            default:
                //TODO
                break;
        }
    }
}

