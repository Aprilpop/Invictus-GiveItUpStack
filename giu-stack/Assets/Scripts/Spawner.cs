using System.CodeDom.Compiler;
using System.Collections;
using System.Collections.Generic;
using System.Security.Permissions;
using UnityEngine;

[System.Serializable]
public class Pool
{
    public string tag;
    public int size;
    public string platform;
}


/// <summary>
/// 飞盘
/// </summary>
public class Spawner : MonoBehaviour
{
    //public float spawnerStep;

    public GameObject[] spawners;

    public GameObject tower;

    public GameObject spawnerCenter;
    public GameObject[] indexArr = new GameObject[20];
    private static Spawner instance;

    public static Spawner Instance
    {
        get
        {
            if (instance == null)
                Instantiate(Resources.Load("Spawner"));
            return instance;
        }
    }

    public List<Pool> pools;
    private Dictionary<string, CirDeque<GameObject>> poolDictionary;

    private void Awake()
    {
        if (instance == null)
            instance = this;
    }

    private void Start()
    {        
        poolDictionary = new Dictionary<string, CirDeque<GameObject>>();

        foreach (var pool in pools)
        {
            CirDeque<GameObject> objectPool = new CirDeque<GameObject>();

            for (int i = 0; i < pool.size; i++)
            {
                GameObject obj = Instantiate(Resources.Load<GameObject>("Platforms/" + pool.platform), tower.transform);
                if("BasicPlatform" == pool.platform)
                {
                    obj.name = "grid_" + i;
                    indexArr[i] = obj;//取索引
                }
                obj.SetActive(false);
                objectPool.EnterRear(obj);                
            }

            poolDictionary.Add(pool.tag, objectPool);
        }
        //CreatePlatform();
    }

    int count = 0;

    /*GameObject obstacleGoal;
    public GameObject ObstacleGoal { get { return obstacleGoal; } }
    */

    Vector3 obsGoal;
    public Vector3 ObsGoal { get { return obsGoal; } }

    int random;

    public void CreateSaw()
    {
        int randomNumber = Random.Range(0, 2);
        if (randomNumber == 0 || count >= 2)
        {
            SpawnFromPool("Platform", spawners[random].transform.position, Quaternion.identity, tower.transform);
            count = 0;
        }
        else
        {
            SpawnFromPool("Obstacle", spawners[random].transform.position, Quaternion.identity, tower.transform);
            count++;
        }
    }

    public void CreateSaw(eStartType startType)
    {
        int randomNumber;

        switch (startType)
        {
            case eStartType.None:
                SpawnFromPool("Platform", spawners[random].transform.position, Quaternion.identity, tower.transform);
                break;
            case eStartType.IsEasy:
                randomNumber = Random.Range(0, 10);
                if (randomNumber <= 1 && count < 1)
                {
                    SpawnFromPool("Obstacle", spawners[random].transform.position, Quaternion.identity, tower.transform);
                    count++;
                }
                else
                {
                    SpawnFromPool("Platform", spawners[random].transform.position, Quaternion.identity, tower.transform);
                    count = 0;
                }
                break;
            case eStartType.IsMedium:
                randomNumber = Random.Range(0, 10);
                if (randomNumber <= 3 && count < 2)
                {
                    SpawnFromPool("Obstacle", spawners[random].transform.position, Quaternion.identity, tower.transform);
                    count++;
                }
                else
                {
                    SpawnFromPool("Platform", spawners[random].transform.position, Quaternion.identity, tower.transform);
                    count = 0;
                }
                break;
            case eStartType.IsHard:
                CreateSaw();
                break;
            default:
                break;
        }

    }

    public void CreatePlatform()
    {
        int platformSpawn = ProfileManager.Instance.Enviroment.StartRandomSpawn;

        //DebugManager.LogInfo("==>CreatePlatform " + platformSpawn + " / " + GameLogic.Instance.CurrentPlatformCount);

        if (platformSpawn > GameLogic.Instance.CurrentPlatformCount && platformSpawn / 2 > GameLogic.Instance.CurrentPlatformCount)
            random = Random.Range(4, 6);
        else if (platformSpawn > GameLogic.Instance.CurrentPlatformCount && platformSpawn / 2 < GameLogic.Instance.CurrentPlatformCount)
            random = Random.Range(2, 8);
        else if (platformSpawn < GameLogic.Instance.CurrentPlatformCount && platformSpawn / 2 < GameLogic.Instance.CurrentPlatformCount)
            random = Random.Range(0, spawners.Length);

        //DebugManager.LogInfo("调整类型："+GameLogic.Instance.challengeType);

        if (GameLogic.Instance.challengeType == eChallengeType.JumpOver)
        {
            CreateSaw();
        }
        else if (GameLogic.Instance.challengeType == eChallengeType.None)
        {            
            //Debug.Log(ProfileManager.Instance.enviroments[ProfileManager.Instance.EnviromentIndex].sawStart.StartType(GameLogic.Instance.CurrentPlatformCount));
            CreateSaw(ProfileManager.Instance.Enviroment.sawStart.StartType(GameLogic.Instance.CurrentPlatformCount));

            /*if (ProfileManager.Instance.enviroments[ProfileManager.Instance.EnviromentIndex].StartSaw > GameLogic.Instance.CurrentPlatformCount)
                SpawnFromPool("Platform", spawners[random].transform.position, Quaternion.identity, tower.transform);
            else
               CreateSaw();*/
        }
        else
            SpawnFromPool("Platform", spawners[random].transform.position, Quaternion.identity, tower.transform);
    }

    public GameObject objectSpawns;

    public GameObject SpawnFromPool(string tag, Vector3 position, Quaternion rotation, Transform parent)
    {
        if (!poolDictionary.ContainsKey(tag))
        {
            DebugManager.LogInfo("Pool with tag" + tag + "doesnt exist.");
            return null;
        }

        GameObject objectSpawn = poolDictionary[tag].OutFront();
        objectSpawn.SetActive(true);
        objectSpawn.transform.position = position;
        objectSpawn.transform.rotation = rotation;
        objectSpawn.transform.parent = parent;
  
        if (tag == "Obstacle")
            obsGoal = new Vector3(spawners[random].transform.position.x * -1, transform.position.y, spawners[random].transform.position.z * -1);

        Platform platform = objectSpawn.GetComponent<Platform>();
        if (platform != null)
        {

            if (platform.IsEmploy)
            {
                
                // 代表当前pool中全部用完  开始循环使用
                // 池底对象
                GameObject poolBottom = poolDictionary[tag].GetValue(0);
                Platform poolBottomPlatform = poolBottom.GetComponent<Platform>();
                if (poolBottomPlatform)
                {
                    // 让最下面的平台不受重力的影响往下掉
                    var tmpObj1 = poolBottomPlatform.Rigidbody;
                    if(null != tmpObj1) tmpObj1.isKinematic = true;
                }

            }
            // 清理为未使用状态
            platform.IsEmploy = false;



            int gravityPlatformCount = 13;
            // 只让最上方的N块感受到重力
            if(GameLogic.Instance.CurrentPlatformCount>gravityPlatformCount &&  poolDictionary[tag].Count() > gravityPlatformCount )
            {   int index=0;

                for (int i = 0; i < poolDictionary[tag].Count(); i++)
                {
                    GameObject item = poolDictionary[tag].GetValue(i);

                    index++;
                    platform = item.GetComponent<Platform>();
                    
                    // 只设定超出的部分不感受重力
                    if(index > poolDictionary[tag].Count() - gravityPlatformCount)
                    {
                        break;
                    }

                    if(platform.IsMoveOver)
                    {
                        var tmpObj2 = platform.Rigidbody;
                        if (null != tmpObj2) tmpObj2.isKinematic = true;
                    }
                }
            }

        }
        IPoolObject pooledObj = objectSpawn.GetComponent<IPoolObject>();

        if (pooledObj != null)
            pooledObj.OnSpawn();

        poolDictionary[tag].EnterRear(objectSpawn);

        return objectSpawn;

    }

    public void StepSpawnerUp()
    {
        transform.position = new Vector3(0f, transform.position.y + GameLogic.Instance.spawnerStep);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Ground") && GameLogic.Instance.InGame && GameLogic.Instance.challengeType == eChallengeType.None)
        {
            // 目的: 过滤掉角色弹跳而引发的一些非正规碰撞（开启了重力）
            bool isMoveed = other.gameObject.GetComponent<Platform>().IsMoveOver;
            if (isMoveed)
            {
                return;
            }
            StepSpawnerUp();
            CreatePlatform();
        }

        if (other.gameObject.layer == LayerMask.NameToLayer("Obstacle") && GameLogic.Instance.InGame /*&& GameLogic.Instance.challengeType == eChallengeType.JumpOver*/)
        {
            CreatePlatform();
        }

        if (other.gameObject.layer == LayerMask.NameToLayer("Ground") && GameLogic.Instance.InGame && GameLogic.Instance.challengeType != eChallengeType.None && GameLogic.Instance.GoalPlatform())
        {
            // 目的: 过滤掉角色弹跳而引发的一些非正规碰撞（开启了重力）
            bool isMoveed = other.gameObject.GetComponent<Platform>().IsMoveOver;
            if (isMoveed)
            {
                return;
            }

            StepSpawnerUp();
            CreatePlatform();
        }
    }

    /// <summary>
    /// 复活  因为需求的特殊性  所以必须引入循环队列才能高效解决该复杂问题
    /// </summary>
    public void GameResurgence()
    {

        GameLogic.Instance.PlatformCount--;
        if(!GameLogic.Instance.IsBrarrierDie)
        {
            GameLogic.Instance.CurrentPlatformCount--;
        }
        
        Platform poolBottomPlatform;
        int index = 0;              // 记重新排列个数(用于恢复时的其实位置)
        GameLogic.comboCount = 0;
        GameLogic.jumpIndex = 0;
        // 重新整理队列
        GameObject goPlatform;

        int tempCount = poolDictionary["Platform"].Count();
        for (int i = tempCount - 1; i > 0; i--)
        {
            var item = poolDictionary["Platform"].GetValue(tempCount-1);
            poolBottomPlatform = item.GetComponent<Platform>();
            if (poolBottomPlatform.IsEmploy && !poolBottomPlatform.IsMoveOver)
            {
                if (GameLogic.Instance.CurrentPlatformCount >= poolDictionary["Platform"].Count())
                    index++;
                goPlatform = poolDictionary["Platform"].OutRear();
                poolDictionary["Platform"].EnterFront(goPlatform);
              }
            else {
                break;
            }
        }

        int curPlatformCount = GameLogic.Instance.CurrentPlatformCount;

        if (GameLogic.Instance.CurrentPlatformCount >= poolDictionary["Platform"].Count() )
        {
            index += GameLogic.Instance.CurrentPlatformCount - poolDictionary["Platform"].Count();
            index --; 
        }
        else
        {
            index = 0;
        }

        // 将运动结束的全部显示出来
        for (int i = 0; i < poolDictionary["Platform"].Count(); i++)
        {
            var item = poolDictionary["Platform"].GetValue(i);        
            //
            poolBottomPlatform = item.GetComponent<Platform>();
            
            Debug.Log( "打印当前编号:"+ poolBottomPlatform.m_curNumber + "---"+Time.realtimeSinceStartup);
            // 清理掉碰撞失败的信息
            poolBottomPlatform.m_isCollisionFail = false;            
            if (poolBottomPlatform && (poolBottomPlatform.IsMoveOver))
            {
                poolBottomPlatform.gameObject.SetActive(true);
                index++;
                var tmpObj3 = poolBottomPlatform.Rigidbody;
                if(null != tmpObj3)
                {
                    tmpObj3.useGravity = false;
                    tmpObj3.isKinematic = true;//poolBottomPlatform.Rigidbody
                }
                
                item.transform.position = new Vector3(0, index * 0.34f + 0.03f, 0);                
                item.transform.rotation = Quaternion.identity;
            }
            
            poolBottomPlatform.m_moveOverPos = item.transform.position;

            //item.transform.position = new Vector3(0, poolBottomPlatform.m_moveOverPos.y, 0);
        }

        // transform.position = new Vector3(0f, GameLogic.Instance.spawnerStep * GameLogic.Instance.PlatformCount);
    }

}
