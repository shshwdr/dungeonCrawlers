using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Monster : HPObject
{
    [SerializeField]
    string playerName = "chest";
    [SerializeField]
    float awakeRange;
    public int level = 1;
    public int color = 0;
    public string displayName;
    Animator animator;
    public GameObject battleCamera;
    public GameObject attackCamera;
    bool isAwake = false;
    public MonsterStatus monsterStatus;

    public Vector3 popupPositionAdjust;
    public Vector3 monsterCenter;

    AudioSource audioSource;

    public AudioClip appearClip;
    public AudioClip attackClip;
    public AudioClip dieClip;
    public AudioClip getDamageClip;


    NPC npc;
    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
        Init(BattleCharacters.Instance.monsterStatusDict[getMonsterId()]);
        monsterCenter = transform.Find("monsterCenter").position;
        npc = GetComponent<NPC>();
        if (npc)
        {
            npc.enabled = false;
        }
        audioSource = GetComponent<AudioSource>();
    }

    public string getName()
    {
        return displayName;
    }
    public string getAbsorbId()
    {
        return BattleCharacters.Instance.monsterAbsorbDict[status.playerName + color].absorbAbility;
    }
    string getMonsterId()
    {
        return playerName + level;
    }

    protected override void Init(BattleCharacterStatus s)
    {
        animator = GetComponentInChildren<Animator>();
        stateUI = HUD.Instance.enemyUI;
        monsterStatus = (MonsterStatus)s;
        base.Init(s);
    }


    // Update is called once per frame
    void Update()
    {
        if (!isAwake&&ShouldNotice(transform.forward,awakeRange))
        {
            noticePlayer();
        }
    }

    bool ShouldNotice(Vector3 direction,float awakeRange)
    {
        Ray myRay = new Ray(transform.position, direction);
        RaycastHit hit;
        LayerMask layerMask = LayerMask.GetMask("Player");
        Debug.DrawRay(myRay.origin, myRay.direction, Color.red);
        if (Physics.Raycast(myRay, out hit, awakeRange, layerMask))
        {
            return true;
        }
        return false;
    }

    public void noticePlayer()
    {

        animator.SetTrigger("wakeup");
    }


    public void getIntoBattle()
    {

        Init(BattleCharacters.Instance.monsterStatusDict[getMonsterId()]);
        animator.SetTrigger("wakeup");
        animator.SetTrigger("battle");
        battleCamera.SetActive(true);
        initStatusUI();
        StartCoroutine(playAppear());
    }

    IEnumerator playAppear()
    {
        yield return new WaitForSeconds(0.5f);
        audioSource.PlayOneShot(appearClip);
    }

    public  override void takeDamage(int damage)
    {
        //attackCamera.SetActive(true);
        if (!GameManager.Instance.enemyImmortal)
        {
            base.takeDamage(damage);
        }
        animator.SetTrigger("takeDamage");
        audioSource.PlayOneShot(getDamageClip);
    }

    public void finishDamage()
    {

        //attackCamera.SetActive(false);
    }

    protected override void die()
    {
        base.die();

        animator.SetTrigger("die");
        audioSource.PlayOneShot(dieClip);
    }

    public void dieIdle()
    {
        animator.SetTrigger("dieIdle");
        if (npc)
        {
            npc.enabled = true;
        }
        Destroy(this);
    }

    public void fullyDie()
    {
        animator.SetTrigger("fullyDie");
    }

    public void startAttackCamera()
    {
        if (attackCamera)
            attackCamera.gameObject.SetActive(true);
    }
    public void finishAttackCamera()
    {
        if(attackCamera)
        attackCamera.gameObject.SetActive(false);
    }

    public override void attack(HPObject attakee, int damage)
    {
        //base.attack(attakee);
        //random choose attack

        audioSource.PlayOneShot(attackClip);

        attakee.takeDamage(damage);
        //battleCamera.SetActive(true);
        var rand = Random.Range(0, 2);
        if(rand == 0)
        {
            attack1(attakee);
        }
        else
        {
            attack2(attakee);
        }
    }

    void attack1(HPObject attakee)
    {

        animator.SetTrigger("attack1");
    }

    void attack2(HPObject attakee)
    {

        animator.SetTrigger("attack2");
    }

    public void clearCamera()
    {
        attackCamera.SetActive(false);
        battleCamera.SetActive(false);
    }

    protected override void initStatusUI()
    {
        stateUI.updateLevel("Level: "+level);
        updateBuffUI();
        base.initStatusUI();
        stateUI.updateName(getName());
    }


    public void updateBuffUI()
    {
        stateUI.updateBuff(BattleSystem.Instance.monsterBuffDict);
    }
}
