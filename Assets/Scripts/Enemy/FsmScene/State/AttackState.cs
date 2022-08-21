using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackState : EnemyBaseState
{
    #region ����״̬�������뿪������״̬���Ľű�����
    private Fsm fsm;//��ȡ��������״̬������
    private FsmEnemy fsmEnemy;//��ȡ���˶���ű�����
    #endregion

    #region ������ת�ƶ���ر�������
    //����
    float shootTimer = 0;//�����ʱ��
    float shootSpeed;//����ٶ�
    int magicValue;//��ħ��ֵ
    int currentMagicValue;//��ǰħ��ֵ
    int damage;//�˺�
    int consumeMagic;//����ħ��ֵ
    int lowestLevelDamage = 5;//����˺�ֵ����ħ��ֵ����ʱ��ʹ������˺���
    //float lowestLevelShootSpeed = 1;//����˺�ֵ����ħ��ֵ����ʱ��ʹ��������٣�

    //����
    Transform shootPos;//���λ��
    Transform target;//����Ŀ��Transform���
    static Transform enemyTransform;//����Transform���
    #endregion

    #region ���췽��
    /// <summary>
    /// ���췽��
    /// </summary>
    /// <param name="fsmEnemy">���칥��״̬ʱ����Ҫ����һ�����˵Ľű�����FsmEnemy</param>
    public AttackState(Fsm fsm)
    {
        Debug.Log("���빥��״̬");
        this.fsm = fsm;
        this.fsmEnemy = fsm.GetFsmEnemy();

        shootSpeed = fsmEnemy.shootSpeed;
        magicValue = fsmEnemy.magicValue;
        currentMagicValue = fsmEnemy.currentMagicValue;
        damage = fsmEnemy.damage;
        consumeMagic = fsmEnemy.consumeMagic;
        lowestLevelDamage = fsmEnemy.lowestLevelDamage;
        enemyTransform = fsmEnemy.transform;
        shootPos = fsmEnemy.shootPos;
        target = fsmEnemy.target;  
    }
    #endregion

    #region ����
    //Handle��Update����������StartFsm��Update�����е��ã���Handle��Update�������൱��Unity��Update������ÿ֡����һ��
    public void Handle()
    {
        //FindTarget();//�������---�÷����ڸ�֪ϵͳ��ʵ��
    }
    public void Update()
    {
        if (shootTimer >= shootSpeed)
        {
            Attack();
        }
        else
        {
            shootTimer += Time.deltaTime;
        }
    }
    #endregion

    #region Ѱ����ҷ���
    /// <summary>
    /// ���˹���״̬Ѱ�����Ƿ���:�ڹ���״̬���Ҳ������ǽ���Ѳ��״̬
    /// </summary>
    private void FindTarget()
    {
        Vector3 pos = target.position;
        float angle = 0;
        if (enemyTransform.rotation.y == 180)//������ﳯ�ҿ�
        {
            angle = Vector3.Angle(-enemyTransform.right, pos - enemyTransform.position);//����������ǰ��������˺���������ļн�
        }
        else if (enemyTransform.rotation.y == 0)//������ﳯ����
        {
            angle = Vector3.Angle(-enemyTransform.right, pos - enemyTransform.position);
        }

        if (!(angle < 120 && Vector3.Distance(enemyTransform.position, target.position) < 20))//���Ƕ�С��120�㣬����С��20��˵��Ŀ�겻�ڵ����Ӿ���Χ
        {
            fsm.SetEnemyState(new PatrolState(fsm));//�ı�״̬ΪѲ��״̬
        }
        else
        {
            //��ȡ��ҵ�ǰ�ֳ������ȼ����������ȼ��������������3������˽�������״̬
            Player playerScript = GameObject.Find("Player").GetComponent<Player>();//��ȡ���Player�ű����
            int gunLevel = playerScript.currentHandle.gameObject.GetComponent<Gun>().level;
            int enemyLevel = enemyTransform.gameObject.GetComponent<FsmEnemy>().level;

            //�����Ҵ��ڿ�״̬������˽�������״̬
            bool playerIsRage = playerScript.isRage;
            if (gunLevel - enemyLevel >= 3 || playerIsRage == true)
            {
                fsm.SetEnemyState(new EscapeState(fsm));//�ı�״̬Ϊ����״̬
            }
        }
    }
    #endregion

    #region ������ط���
    /// <summary>
    /// ֻ�����������������������˲Ż�ӹ���״̬�л���Ѳ��״̬��������״̬
    /// ������˹�������ֵС�ڵ�ǰħ��ֵ������й�������֮�����˹���ģʽ�л�����ȼ�Ϊ0��������ͬ��
    /// </summary>
    private void Attack()
    {
        shootTimer = 0;
        Vector3 shootDir = (target.position - enemyTransform.position).normalized;
        if (consumeMagic <= currentMagicValue)//������˺���ֵ���ã�ʹ�õ�ǰ����
        {
            GetBullet(shootDir);
            Bullet.instance.damage = damage;  //��ȡ�����˺�ֵ��ֵ���ӵ�
            currentMagicValue -= consumeMagic;
        }
        else//������˺���ֵ������ʹ�ó�������
        {
            GetBullet(shootDir);
            Bullet.instance.damage = lowestLevelDamage;  //��ȡ�����˺�ֵ��ֵ���ӵ�
        }

    }

    /// <summary>
    /// ��ȡ�ӵ�����
    /// </summary>
    /// <param name="shootDir">�ӵ�Ҫ����ķ���</param>
    private void GetBullet(Vector3 shootDir)
    {
        GameObject bullet = EnemyBulletPoolManager.instance.getBullet();
        bullet.transform.position = shootPos.position;
        if (shootDir.y > 0)
        {
            bullet.transform.eulerAngles = new Vector3(0, 0, Vector3.Angle(shootDir, new Vector2(1, 0)));
        }
        else
        {
            bullet.transform.eulerAngles = new Vector3(0, 0, 360 - Vector3.Angle(shootDir, new Vector2(1, 0)));
        }
        bullet.GetComponent<Rigidbody2D>().AddForce(shootDir * Bullet.instance.speed);
    }
#endregion
}
