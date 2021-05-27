using UnityEngine;
using System.Collections;
public class CharacterController : MonoBehaviour
{
    public float force;
    public float max;
    Rigidbody rb;

    private static CharacterController instance;

    public static CharacterController Instance { get { return instance; } }
    // { get; set; }
    public float globalGravity;
    public float gravityScale;

    float maxJumpRotation;
    float minJumpRotation;

    float time;

    float speed;

    public bool isGround = false;
    bool isGetRecord = true;

    Animator anim;

    public static Animator ring { get; set; }
    ParticleSystem particale { get; set; }
    ParticleSystemRenderer psr { get; set; }

    Vector3 character;

    /// <summary>///玩家死亡时的位置/// </summary>
    Vector3 m_ptDeath;

    float jump = 0;


    private void Start()
    {
        if (instance == null)
            instance = this;
        Init();

        rb = GetComponent<Rigidbody>();
        rb.useGravity = false;
        anim = GetComponentInChildren<Animator>();
        particale = GetComponentInChildren<ParticleSystem>();
        Transform tmpTF = transform.GetChild(1);
        if(null != tmpTF)
        {
            tmpTF = tmpTF.GetChild(0);
            psr = tmpTF.GetComponentInChildren<ParticleSystemRenderer>();            
        }
        jump = transform.rotation.y;
    }

    private void FixedUpdate()
    {
        Vector3 gravity = globalGravity * gravityScale * Vector3.up;
        rb.AddForce(gravity, ForceMode.Acceleration);

        if (InputManager.Instance.IsJump && isGround)
        {
            SoundManager.Play(SfxArrayEnum.Jump, transform.position);
            ProfileManager.Instance.CurrentRing.transform.position = transform.position;
            anim.Play("character_jump");//跳跃            
            isGround = false;
            InputManager.Instance.IsJump = false;
            rb.AddForce(Vector3.up * force, ForceMode.Impulse);
        }

        rb.velocity = Vector3.ClampMagnitude(rb.velocity, max);

    }

    int i;
    private void Update()
    {
        if (GameLogic.Instance.previousStack)
        {
            character = new Vector3(GameLogic.Instance.Swing(), transform.position.y);

            // 完美模式  玩家跟着摇摆
            if (GameLogic.Instance.challengeType == eChallengeType.PerfectJump)
                transform.position = character;
        }
        if (InputManager.Instance.IsJump && isGround)
        {

            i = Random.Range(0, 2);
            if ((i == 0 || minJumpRotation >= jump) && (maxJumpRotation > jump && maxJumpRotation != jump))
                jump += 5f;
            else if ((i == 1 || maxJumpRotation <= jump) && (minJumpRotation < jump && minJumpRotation != jump))
                jump -= 5f;

            transform.localRotation = Quaternion.Euler(0f, jump, 0f);
        }
    }

    // 玩家死亡
    public void Death(Collision collision = null)
    {
        
        // 玩家死亡前的位置
        m_ptDeath = transform.position;

        SoundManager.Play(SfxArrayEnum.Death, transform.position);
        ParticleManager.Instance.gameObject.SetActive(true);
        ParticleManager.Instance.death.Play();
        StartCoroutine(ParticleManager.Instance.StopParticle(3f));
        MenuManager.Instance.GameOver(collision);
        ProfileManager.Instance.UnlockItems(eUnlockType.Point, GameLogic.Instance.Score);
        isGround = true;

        StartCoroutine("sendGameOver");
    }

    IEnumerator sendGameOver()
    {   
        yield return new WaitForSeconds(0.02f);
        EventDispatcher.Instance.Dispatch(EventKey.OnGameOver);
    }

    private void OnCollisionEnter(Collision collision)
    {
        // 障碍物或者轮胎块
        if ((collision.gameObject.layer == LayerMask.NameToLayer("Ground") && Mathf.Abs(collision.GetContact(0).normal.y) < 0.9) || collision.gameObject.layer == LayerMask.NameToLayer("Obstacle"))
        {
            if (GameLogic.Instance.InGame)
            {
                if(collision.gameObject.layer == LayerMask.NameToLayer("Obstacle"))
                {
                    GameLogic.Instance.IsBrarrierDie = true;                    
                }
                Death(collision);
            }
        }

        if (collision.gameObject.layer == LayerMask.NameToLayer("Ground") && Mathf.Abs(collision.GetContact(0).normal.y) > 0.9)
        {
            isGround = true;
            if (GameLogic.Instance.InGame)
            {
                if (!GameLogic.Instance.FirstStart)
                {
                    //ProfileManager.Instance.CurrentBlobs.transform.localScale = Vector3.one;
                    anim.Play("character_land");//落地
                    if (null != ring)
                        ring.Play("ring_diffuse", -1, 0f);
                    SoundManager.Play(SfxArrayEnum.Landing, transform.position);
                    particale.Play(true);
                    if(GameLogic.comboCount>=5)
                        psr.material = GameLogic.Instance.gSplash;
                    else
                        psr.material = GameLogic.Instance.splash;
                }
            }
            if (GameLogic.Instance.challengeType == eChallengeType.None)
            {
                int score = GameLogic.Instance.Score;
                int highScore = ProfileManager.Instance.Record;

                if (score != 0 && score >= highScore && isGetRecord && !ProfileManager.Instance.FirstPlay)
                {
                    MenuManager.Instance.inGameNewRecord.SetActive(true);
                    ParticleManager.Instance.confetti.Play();
                    isGetRecord = false;
                }
            }
            GameLogic.Instance.FirstStart = false;
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        // if (collision.gameObject.layer == LayerMask.NameToLayer("Ground"))
        // isGround = false;
    }

    private void Init()
    {
        force = GameLogic.Instance.Force;
        max = GameLogic.Instance.MaxJump;
        globalGravity = GameLogic.Instance.GlobalGravity;
        gravityScale = GameLogic.Instance.GravityScale;
        minJumpRotation = GameLogic.Instance.MinJumpRotate;
        maxJumpRotation = GameLogic.Instance.MaxJumpRotate;
        speed = GameLogic.Instance.RotationSpeed;
        time = 0.25f;
    }

    /// <summary>
    /// 复活时
    /// </summary>
    public void GameResurgence()
    {
        if(transform)
        {        
            transform.position = new Vector3(0, GameLogic.Instance.CurrentPlatformCount * 0.34f + 0.5f, 0);
        }
    }

}
