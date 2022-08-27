using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterController_2D : Selectable
{    
    Rigidbody2D m_rigidbody;
    Animator m_Animator;
    Transform m_tran;

    private float h = 0;
    private float v = 0;

    public float MoveSpeed = 40;

    public SpriteRenderer[] m_SpriteGroup;

    public bool Once_Attack = false;


    // Use this for initialization
    void Start () {
        transform.eulerAngles = new Vector3(90, 0, 0);
        m_rigidbody = this.GetComponent<Rigidbody2D>();
        m_Animator = this.transform.Find("BURLY-MAN_1_swordsman_model").GetComponent<Animator>();
        m_tran = this.transform;
        m_SpriteGroup = this.transform.Find("BURLY-MAN_1_swordsman_model").GetComponentsInChildren<SpriteRenderer>(true);

  
    }
	
	// Update is called once per frame
	void Update () {


        spriteOrder_Controller();


        if (Input.GetKeyDown(KeyCode.Alpha9))
        {
            Debug.Log("1");
            m_Animator.Play("Hit");
        }
        else if (Input.GetKeyDown(KeyCode.Alpha0))
        {
            Debug.Log("2");
            m_Animator.Play("Die");
        }


        if (m_Animator.GetCurrentAnimatorStateInfo(0).IsName("Attack") || m_Animator.GetCurrentAnimatorStateInfo(0).IsName("Die")||
            m_Animator.GetCurrentAnimatorStateInfo(0).IsName("Hit")|| m_Animator.GetCurrentAnimatorStateInfo(0).IsName("Attack2"))
            return;

        Move_Fuc();



 
    }
    public void Attack1(bool secondAttack=false)
    {
        Once_Attack = false;

        if(!secondAttack)
            m_Animator.SetTrigger("Attack");
        else
            m_Animator.SetTrigger("Attack2");

        m_rigidbody.velocity = new Vector3(0, 0, 0);
    }

    public int sortingOrder = 0;
    public int sortingOrderOrigine = 0;

    private float Update_Tic = 0;
    private float Update_Time = 0.1f;

    void spriteOrder_Controller()
    {

        Update_Tic += Time.deltaTime;

        if (Update_Tic > 0.1f)
        {
            sortingOrder = Mathf.RoundToInt(this.transform.position.y * 100);
            //Debug.Log("y::" + this.transform.position.y);
            //  Debug.Log("sortingOrder::" + sortingOrder);
            for (int i = 0; i < m_SpriteGroup.Length; i++)
            {

                m_SpriteGroup[i].sortingOrder = sortingOrderOrigine - sortingOrder;

            }

            Update_Tic = 0;
        }

     

    }

    // character Move Function
    void Move_Fuc()
    {
        Vector3 deltaPos=transform.position;
        if (Input.GetKey(KeyCode.A))
        {
          //  Debug.Log("Left");
            transform.position+=(Vector3.left * MoveSpeed)*Time.deltaTime;
            if (!B_FacingRight)
                Filp();


        }
        else if (Input.GetKey(KeyCode.D))
        {
            //  Debug.Log("Right");
            transform.position += (Vector3.right * MoveSpeed) * Time.deltaTime;
            if (B_FacingRight)
                Filp();
        }

        if (Input.GetKey(KeyCode.W))
        {
            // Debug.Log("up");
            transform.position += (Vector3.forward * MoveSpeed) * Time.deltaTime;

        }
        else if (Input.GetKey(KeyCode.S))
        {
            // Debug.Log("Down");
            transform.position += (Vector3.back * MoveSpeed) * Time.deltaTime;


        }
        deltaPos = transform.position - deltaPos;
        float deltaPosSqrm = deltaPos.sqrMagnitude/(Time.deltaTime*Time.deltaTime);
        if(deltaPos.sqrMagnitude>0)
            m_Animator.SetFloat("MoveSpeed", 1);
        else
            m_Animator.SetFloat("MoveSpeed", 0);


    }


    // character Filp 
    bool B_Attack = false;
    bool B_FacingRight = true;

    void Filp()
    {
        B_FacingRight = !B_FacingRight;

        Vector3 theScale = transform.localScale;
        theScale.x *= -1;

        m_tran.localScale = theScale;
    }


 
    //   Sword,Dagger,Spear,Punch,Bow,Gun,Grenade


  

  
}
