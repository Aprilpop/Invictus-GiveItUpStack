using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class Platform : MonoBehaviour, IPoolObject
{
    public float speed;

    float positionx;
    float positionz;

    bool isTouched = false;

    bool isMove = false;

    public Renderer renderer;

    private Renderer vfx_render;

    ParticleSystem particle;
    ParticleSystemRenderer psr;

    public Vector3 m_moveOverPos;    // 保存运动结束后的点
    Vector3 platform;
    Vector3 platform2;

    Vector3 spawnerCenterPosition;

    float frequency;

    float sinY;

    bool m_isMoveOver;

    bool m_isFerfectJumpFail;
    // 当前平台编号
    public int m_curNumber = 0;
    // 是否显示过新手提示
    public bool m_isShowGreenHandTipOne = false;
    public bool m_isShowGreenHandTipTwo = false;
    public bool m_isShowGreenHandTipThree = false;
    public bool m_isShowGreenHandTipFour = false;

    public bool m_isCollisionFail = false;
    // 正在运动的()    
    // 刚体
    private Rigidbody m_rigidbody;
    public Rigidbody Rigidbody { get { return m_rigidbody; } }

    // 碰撞盒
    private BoxCollider m_BoxCollider;
    // 是否运动结束
    public bool IsMoveOver { get { return m_isMoveOver; } }
    public float SinY { get { return sinY; } }

    // 是否在使用
    private bool m_isEmploy = false;
    public bool IsEmploy { set { m_isEmploy = value; } get { return m_isEmploy; } }

    private bool isGoldColor { get; set; }

    private void Awake()
    {
        particle = GetComponentInChildren<ParticleSystem>();
        psr = GetComponentInChildren<ParticleSystemRenderer>();
        m_rigidbody = GetComponent<Rigidbody>();
        m_BoxCollider = GetComponent<BoxCollider>();

        EventDispatcher.Instance.AddEventListener(EventKey.OnGreenHandStabilize, OnGreenHandStabilize);
        EventDispatcher.Instance.AddEventListener(EventKey.OnCharacterPosChange, OnCharacterPosChange);
        EventDispatcher.Instance.AddEventListener(EventKey.OnGameOver, OnGameOver);

        vfx_render = transform.GetChild(1).GetComponent<Renderer>();
    }

    void OnEnable()
    {
        // 注册事件
    }

    void OnDisable()
    {

    }

    void OnDestroy()
    {
        // 注册事件

        EventDispatcher.Instance.RemoveEventListener(EventKey.OnGreenHandStabilize, OnGreenHandStabilize);
        EventDispatcher.Instance.RemoveEventListener(EventKey.OnCharacterPosChange, OnCharacterPosChange);
        EventDispatcher.Instance.RemoveEventListener(EventKey.OnGameOver, OnGameOver);
    }

    IEnumerator StartDelay(float time)
    {
        yield return new WaitForSeconds(time);
        isMove = true;
    }

    public void OnSpawn()
    {
        // 每次运动之前 清空
        // m_moveOverPos = Vector3.zero;
        m_isFerfectJumpFail = false;
        m_isCollisionFail = false;

        m_isEmploy = true;
        // 完美模式 关闭重力 开启运动学
        if(null != m_rigidbody) m_rigidbody.isKinematic = true;
        // 非完美模式 运动后开启重力 关闭运动学
        if (GameLogic.Instance.challengeType != eChallengeType.PerfectJump)
        {
            // 刚创建出来时取消重力
            if (null != m_rigidbody) m_rigidbody.useGravity = false;
            // m_rigidbody.isKinematic = false;
            if (null != m_rigidbody) m_rigidbody.isKinematic = true;
        }

        spawnerCenterPosition = Spawner.Instance.transform.position;

        Vector3 lookPos = spawnerCenterPosition - transform.position;
        transform.rotation = Quaternion.LookRotation(lookPos) * Quaternion.AngleAxis(-90, Vector3.up);

        if (GameLogic.Instance.challengeType == eChallengeType.ScalePlatform)
        {
            float scale = Random.Range(0.5f, 1.5f);
            transform.localScale = new Vector3(scale, 1f, scale);
        }
        else if (GameLogic.Instance.challengeType == eChallengeType.None)
        {
            int randomNumber;

            //Debug.Log(ProfileManager.Instance.enviroments[ProfileManager.Instance.EnviromentIndex].scaleStart.StartType(GameLogic.Instance.CurrentPlatformCount));

            switch (ProfileManager.Instance.Enviroment.scaleStart.StartType(GameLogic.Instance.CurrentPlatformCount))
            {
                case eStartType.None:
                    break;
                case eStartType.IsEasy:
                    randomNumber = Random.Range(0, 10);
                    if (randomNumber <= 1)
                    {
                        float scale = Random.Range(0.5f, 1.5f);
                        transform.localScale = new Vector3(scale, 1f, scale);
                    }
                    break;
                case eStartType.IsMedium:
                    randomNumber = Random.Range(0, 10);
                    if (randomNumber <= 3)
                    {
                        float scale = Random.Range(0.5f, 1.5f);
                        transform.localScale = new Vector3(scale, 1f, scale);
                    }
                    break;
                case eStartType.IsHard:
                    randomNumber = Random.Range(0, 10);
                    if (randomNumber <= 5)
                    {
                        float scale = Random.Range(0.5f, 1.5f);
                        transform.localScale = new Vector3(scale, 1f, scale);
                    }
                    break;
                default:
                    break;
            }
        }

        GameLogic.Instance.PlatformCount++;
        GameLogic.Instance.CurrentPlatformCount++;
        m_curNumber = GameLogic.Instance.CurrentPlatformCount;
        
        if (GameLogic.Instance.challengeType == eChallengeType.None)
            ProfileManager.Instance.MaxPlatform = GameLogic.Instance.PlatformCount;

        GameLogic.Instance.SetDifficulty();

        GameLogic.Instance.stack = this;
        renderer.enabled = true;

        if (renderer.sharedMaterial != ProfileManager.Instance.NormalMaterial())
            renderer.sharedMaterial = ProfileManager.Instance.NormalMaterial();

        speed = Random.Range(GameLogic.Instance.minPlatformSpeed, GameLogic.Instance.maxPlatformSpeed);

        positionx = transform.position.x;
        positionz = transform.position.z;

        isTouched = false;
        isMove = false;
        m_isMoveOver = false;

        float delayTime = GameLogic.Instance.MaxDelayTime;

        if (ProfileManager.Instance.PlayedGames <= 1 && m_curNumber == 1 || m_curNumber == 2)
        {
            delayTime = 0;
            if (m_curNumber == 1)
            {
                setPosition();
            }
        }

        StartCoroutine(StartDelay(Random.Range(GameLogic.Instance.MinDelayTime, delayTime)));

        particle.Play();
    }

    private void Update()
    {

        if (!isTouched)
        {
            if (isMove && GameLogic.Instance.InGame)
            {
                transform.position = Vector3.MoveTowards(transform.position, spawnerCenterPosition, Time.deltaTime * speed);
            }

            platform = transform.position;
            if(GameLogic.comboCount >= 5)
            {
                renderer.material = GameLogic.Instance.gGrid;
                psr.material = GameLogic.Instance.gSplash;
                isGoldColor = true;
            }
            else
            {
                renderer.material = GameLogic.Instance.normal;
                psr.material = GameLogic.Instance.splash;
                isGoldColor = false;
            }
        }
        else
        {
            platform2 = platform;
            platform2.x += GameLogic.Instance.Swing(sinY, transform.position.y);

            // 完美模式  轮胎不受重力影响 但是会左右摇摆
            if (GameLogic.Instance.challengeType == eChallengeType.PerfectJump)
                transform.position = platform2;
        }

        // 如果不是完美模式，开启物理引擎模式
        // 运动结束后
        if (GameLogic.Instance.InGame && m_isMoveOver &&
            GameLogic.Instance.challengeType != eChallengeType.PerfectJump
            )
        {
            // x z 平面的偏移量
            float distance = Vector2.Distance(new Vector2(m_moveOverPos.x, m_moveOverPos.z), new Vector2(transform.position.x, transform.position.z));
            // y轴偏移量
            float yChcekOffset = m_BoxCollider.size.y; // y轴偏移

            // 如果数量多了之后

            if (GameLogic.Instance.ChallengePlatformCount > 4)
            {
                yChcekOffset = 2 * yChcekOffset;
            }

            // 发现严重偏移 游戏结束
            if (m_moveOverPos.y - transform.position.y > yChcekOffset || distance > m_BoxCollider.size.x)
            {
                string strLog = "y轴偏移" + (m_moveOverPos.y - transform.position.y).ToString();
                strLog += "\r\n" + "xz平面偏移量：" + distance;
                strLog += "\r\n" + "碰撞盒子：" + m_BoxCollider.size.ToString();
                strLog += "\r\n" + "当前位置：" + transform.position;
                Debug.Log(strLog);
                DebugManager.LogInfo("发生坍塌 游戏结束");
                GameLogic.Instance.IsCollapse = true;
                // 如果位置下滑那么 表示坍塌 游戏结束
                ProfileManager.Instance.CharacterController.Death();
                //Death
            }
            else if (distance > m_BoxCollider.size.x / 5)
            {
                DebugManager.LogInfo("危险提示！");
                UIManager.Instance.ShowStabilizationTip(true);
            }

        }

        #region 新手引导
        
        // 第一局  第一块距离中心点一定距离新手提示
        if (ProfileManager.Instance.PlayedGames <= 1)
        {
            float distance = Vector3.Distance(transform.position, Spawner.Instance.spawnerCenter.transform.position);
            if (distance < 1.5 && m_curNumber == 1 && !m_isShowGreenHandTipOne)
            {
                m_isShowGreenHandTipOne = true;
                //UIManager.Instance.ShowGreenHandTipMsg("点击屏幕可以使点点跳起来！", true, () => { });

                UIManager.Instance.ShowGreenHandCoveringTipMsg("点击屏幕可以使点点跳起来！",
                    () =>
                    {
                        InputManager.Instance.IsJump = true;
                    });
            }

            if (distance < 1.5 && m_curNumber == 2 && !m_isShowGreenHandTipTwo)
            {

                m_isShowGreenHandTipTwo = true;
                // 新手提示                
                UIManager.Instance.ShowGreenHandCoveringTipMsg("将飞盘堆叠在中心位置，获得perfect会将得到更高的分数",
                    () =>
                    {
                        InputManager.Instance.IsJump = true;
                    });
            }

            if (distance < 2.5 && m_curNumber == 3 && !m_isShowGreenHandTipThree)
            {
                m_isShowGreenHandTipThree = true;                
                GameLogic.Instance.m_isShowGreenHandTipOffset = true;
                UIManager.Instance.ShowGreenHandCoveringTipMsg("如果飞盘偏移太远，会导致叠柱不稳定，甚至可能会倒塌哟！！",
                () =>
                {
                    InputManager.Instance.IsJump = true;
                });
            }

            if (distance < 1.5 && m_curNumber == 4 && !m_isShowGreenHandTipFour)
            {
                m_isShowGreenHandTipFour = true;
                UIManager.Instance.ShowGreenHandCoveringTipMsg("但是！你可以将后面的飞盘往偏移飞盘的反方向叠放来减缓叠柱的不稳定！",
                () =>
                {
                    InputManager.Instance.IsJump = true;
                });
            }

        }
        #endregion

    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Player") && Mathf.Abs(collision.GetContact(0).normal.y) < 0.9)
        {
            isTouched = true;
            gameObject.SetActive(false);
            m_isCollisionFail = true;
        }

        if (collision.gameObject.layer == LayerMask.NameToLayer("Player") && !isTouched && Mathf.Abs(collision.GetContact(0).normal.y) > 0.9)
        {
            sinY = Mathf.Sin(transform.position.y * 0.5f);
            m_isFerfectJumpFail = GameLogic.Instance.SuccesJump(this);


            if (!isTouched && GameLogic.Instance.InGame)
            {
                m_isMoveOver = true;

                // 如果不是完美模式，开启物理引擎模式
                if (GameLogic.Instance.challengeType != eChallengeType.PerfectJump)
                {
                    // 保存运动结束后的位置
                    m_moveOverPos = this.transform.position;

                    // 设置重力
                    if (null != m_rigidbody)
                    {
                        m_rigidbody.useGravity = false;// true;
                        m_rigidbody.isKinematic = true;//false
                    }                    
                }

                //数值发生变化  通知注册当前 金币发生变化的 界面
                EventDispatcher.Instance.Dispatch(EventKey.OnCharacterPosChange, m_curNumber.ToString());

                GameLogic.Instance.ChallengePlatformCount++;
            }

            if (GameLogic.Instance.challengeType != eChallengeType.None && !GameLogic.Instance.GoalPlatform())
            {
                if (GameLogic.Instance.InGame)
                    MenuManager.Instance.Win(GameLogic.Instance.challengeLevelNumber + 1);

                if (GameLogic.Instance.stack && GameLogic.Instance.stack != this)
                    GameLogic.Instance.stack.gameObject.SetActive(false);


                if (GameLogic.Instance.challengeLevelNumber != 11)
                    ProfileManager.Instance.UnlockChallenge(GameLogic.Instance.challengeType, GameLogic.Instance.challengeLevelNumber + 1);
            }

            isTouched = true;
            particle.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
            GameLogic.Instance.stack = null;
        }
    }

    void OnCharacterPosChange(string name)
    {
        // 新手提示
        // 第一块完之后 设置第二块的位置
        if (ProfileManager.Instance.PlayedGames <= 1 && m_curNumber == 2 && !m_isShowGreenHandTipTwo)
        {
            setPosition();
        }

        // 第二块完之后 设置第三块的位置
        if (ProfileManager.Instance.PlayedGames <= 1 && m_curNumber == 3 && !m_isShowGreenHandTipThree)
        {
            isMove = true;
            setPosition(2.5f);
        }

        // 第二块完之后 设置第三块的位置
        if (ProfileManager.Instance.PlayedGames <= 1 && m_curNumber == 4 && !m_isShowGreenHandTipFour)
        {
            isMove = true;
            setPosition(1.5f);
        }

        // 第四块完成
        if (ProfileManager.Instance.PlayedGames <= 1 && name == "4")
        {
            StartCoroutine("GreenHandEnd");
        }
    }

    // 
    public IEnumerator GreenHandEnd()
    {
        yield return new WaitForSeconds(0.5f);
        UIManager.Instance.ShowTipMsg("新手引导结束");        
    }
    /// <summary>
    /// 飞盘不稳定
    /// </summary>
    void OnGreenHandStabilize( string index)
    {
        if (index==m_curNumber.ToString())
        {
            isMove = true;
            setPosition();
        }
    }

    /// <summary>
    /// 强制设置位置
    /// </summary>
   void  setPosition(float value=1.5f)
    {
        float x = transform.position.x;
        if (x > 0)
        {
            x = value;
        }
        else
        {
            x = -value;
        }
        transform.position = new Vector3(x, transform.position.y, transform.position.z);
    }

    /// <summary>
    /// 游戏结束
    /// </summary>
    void OnGameOver(string msg = "")
    {
        // 找到结束前最后一块
        if ((m_isCollisionFail && !m_isMoveOver)
            || (GameLogic.Instance.IsCollapse && m_curNumber == GameLogic.Instance.PlatformCount && !m_isMoveOver)
            || m_isFerfectJumpFail && GameLogic.Instance.challengeType == eChallengeType.PerfectJump
            )

        {
            Spawner.Instance.transform.position = new Vector3(Spawner.Instance.transform.position.x, transform.position.y);
        }

        // 将所有未移动结束的全部隐藏
        if (!m_isMoveOver)
        {
            gameObject.SetActive(false);
        }
        //连击数归零
        GameLogic.comboCount = 0;
        GameLogic.jumpIndex = 0;
    }
    private float time = 0.1f;
    public void PlayVFX()
    {
        transform.GetChild(0).gameObject.SetActive(false);
        transform.GetChild(1).gameObject.SetActive(true);
        vfx_render.material.color = new Color(1f, 1f, 1f);
        Invoke("PlayVFX2", time);
    }

    public void PlayVFX2()
    {
        if (isGoldColor)
            vfx_render.material.color = new Color((224f / 255f), (217f / 255f), (135f / 255f));
        else
            vfx_render.material.color = new Color(0.5f, 0.5f, 0.5f);
        Invoke("PlayVFX3", time);
    }

    public void PlayVFX3()
    {
        if (isGoldColor)
            vfx_render.material.color = new Color((209f / 255f), (197f / 255f), (75f / 255f));
        else
            vfx_render.material.color = new Color(0.25f, 0.25f, 0.25f); 
        Invoke("Recover", time);
    }

    private void Recover()
    {
        transform.GetChild(0).gameObject.SetActive(true);
        transform.GetChild(1).gameObject.SetActive(false);
    }
}
