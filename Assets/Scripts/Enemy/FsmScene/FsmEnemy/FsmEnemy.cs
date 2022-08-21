using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// �������˵�����״̬��
/// </summary>
public class FsmEnemy : MonoBehaviour
{
    public Fusm fusm;//ģ��״̬��

    //���˵�����״̬��
    public Fsm fsm;
    //����
    public FsmEnemy fsmEnemy;




    /// <summary>
    /// ����״̬
    /// </summary>
    public enum FsmEnemyStateEnum
    {
        PatrolState,
        AttackState,
        DeadState,
        EscapeState,
        HideState,
        PickState,
        RunAwayState
    }
    public GameObject[] fsmEnemyPrefabs;//���й����Ԥ����









    #region ����������ر���������
    //������Ѫ����ħ��ֵ���ƶ��ٶȣ�����ȼ�,������������
    public int hpValue = 100;
    public int magicValue = 100;
    public int currentHPValue = 100;
    public int currentMagicValue = 100;
    public int damage = 5;
    public int consumeMagic = 0;
    public float moveSpeed = 5;
    public float shootSpeed = 1f;
    public int level = 0;
    public bool isRage = false;



    //���ã��������;�������;����ģ�Ͷ���
    [HideInInspector]
    public Rigidbody2D rig;
    [HideInInspector]
    public Transform target;//���˹���Ŀ�꣬�����
    #endregion









    #region ������ر�������
    //����������ٶȣ������ʱ������ǰ��ҿ�ʰȡ����������ǰ����ֳ������Ƿ�Ϊ���������ӵ����㼶�����߼��㼶;����λ��
    public List<GameObject> pickableItems = new List<GameObject>();


    //���ã�������������������ʼ����Ԥ���壻��ǰ�ֳ�װ�����ӵ����λ��;�ӵ�Ԥ����
    public Transform shootPos;
    public int lowestLevelDamage = 5;    
    public float lowestLevelShootSpeed = 1f;
    #endregion







    #region ������ת��ر���
    public float horizontal;
    public float vertical;
    #endregion







    #region ��ʼ��
    private void Start()
    {
        fsmEnemy = this;
        //��ʼ����ǰ��Ϸ����ĸ��������ģ�Ͷ���
        rig = this.GetComponent<Rigidbody2D>();
        target = GameObject.Find("Player").transform;
        shootPos = this.transform.Find("Model").transform.Find("ShootPos").transform;

        Debug.Log("��ʼ������״̬��");
        fsm = new Fsm(fsmEnemy);//ʵ�������˵�����״̬��
        fsm.SetEnemyState(new EscapeState(fsm));//Ĭ�Ͻ���Ѳ��״̬



        #region ģ��״̬����ش���
        fusm = new Fusm(fsmEnemy);//ʵ��������ģ��״̬��
        fusm.AddState(new HealthyState(fusm));
        fusm.AddState(new DyingState(fusm));
        fusm.AddState(new InjuredState(fusm));
        #endregion
    }
    #endregion








    #region ����
    void Update()
	{
        fsm.Update();
        fusm.Update();
    }
    #endregion







    #region ��ײ������Ѫ������ײǽ�ı��ƶ����򷽷�
    /// <summary>
    /// ������˺�����ӵ�������ײ�����ٵ���Ѫ�����������Ѫ������0�������״̬����Ϊ����
    /// ������˺�ǽ������ײ����ȡ���˵�ǰ�ƶ����򲢸ı���˷���
    /// </summary>
    /// <param name="collision"></param>
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "PlayerBullet")
        {
            Debug.Log("�ܵ�����");
            currentHPValue -= collision.gameObject.GetComponent<Bullet>().damage;
            if (currentHPValue <= 0)
            {
                currentHPValue = 0;
                fsm.SetEnemyState(new DeadState(fsm));//������ֱ�����ٶ���
            }
            else if((float)Math.Round((decimal)currentHPValue / hpValue, 2) < 0.1)//(float)Math.Round((decimal)currentHPValue / hpValue, 2)��������λС��
            {
                Debug.Log("Ѫ������ٷ�֮10");
                fsm.SetEnemyState(new RunAwayState(fsm));//��������״̬
                EnemyPerspective.instance.isInRunAwayState = true;
            }
            Debug.Log("��ǰѪ��Ϊ" + currentHPValue.ToString());

            //ģ��״̬��
            List<FusmBaseState> states = fusm.states;//��ȡģ��״̬������״̬
            float hpPercentage = (float)Math.Round((decimal)currentHPValue / hpValue, 2);//�õ���ǰѪ���ٷֱ�
            Debug.Log("��ǰѪ���ٷֱ�Ϊ" + hpPercentage);
            foreach (FusmBaseState state in fusm.states)
            {
                state.Evaluate();//����ģ��״̬���ļ���ȼ�
            }
            fusm.UpdateFusm();//���㼤��ȼ����ģ��״̬�����и���
        }



        else if ((collision.gameObject.tag == "Wall" || collision.gameObject.tag == "Boxes" || collision.gameObject.tag == "Obstacle") && fsm.GetEnemyState().GetType().Name == "PatrolState")
        {
            Debug.Log("ײǽ���ı䷽��");
            PatrolState.EnemyHitWallChangeDir();
        }


        //��������ϰ��ﲢ�Ҵ�������״̬������ ANNController �ű������ Death ����
        else if ((collision.gameObject.tag == "Wall" || collision.gameObject.tag == "Boxes" || collision.gameObject.tag == "Obstacle") && fsm.GetEnemyState().GetType().Name == "EscapeState")
        {
            ANNController controller = transform.GetComponent<ANNController>();
            //Debug.Log("����ǰ���������Ӧ��Ϊ��" + controller.overallFitness.ToString());
            controller.Death();
            Debug.Log("��������״̬");
        }

        //�������״̬�������˴����ţ����ʾ��ǰ�������ǡ��ϸ񡱵�������
        else if (collision.gameObject.tag == "Door" && fsm.GetEnemyState().GetType().Name == "EscapeState")
        {
            Debug.Log("�ɹ�����");
            GameObject.Find("Empty_GeneticManager").GetComponent<GeneticManager>().exceptSuccessed += 1;
            ANNController controller = transform.GetComponent<ANNController>();
            controller.Death();
            Debug.Log("��������״̬");
        }
    }
    #endregion








    #region ������������
    public void Dead()
    {
        Destroy(this.gameObject);
    }
    #endregion







    #region ɾ����ʰȡ�б��������Ϸ����
    public void DestroyItem(GameObject item)
    {
        Destroy(item);
    }
    #endregion








    #region ��״̬���п���Э�̷���
    public void StartCoroutineInState(string coroutineName)
    {
        StartCoroutine(coroutineName);
    }





    /// <summary>
    /// ��ҩƿʧЧЭ�̷���
    /// </summary>
    /// <returns></returns>
    IEnumerator Calm()
    {
        Debug.Log("�������ˡ��侲��Э��");
        //��10��
        yield return new WaitForSeconds(10f);
        isRage = false;
        this.moveSpeed -= 5;
        this.shootSpeed += 0.1f;
        this.damage -= 10;
        Debug.Log("ִ�����");
    }
    #endregion


}
