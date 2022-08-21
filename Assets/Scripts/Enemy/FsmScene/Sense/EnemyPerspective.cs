using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// �Ӽ���֪�ࣺģ������������֪ϵͳ
/// </summary>
public class EnemyPerspective : Sense
{
    public static EnemyPerspective instance;

    #region ������֪��ز���
    int FieldOfView = 60;//�Ӷȴ�С
    int ViewDistance = 20;//�ɼ�����
    int enemyLayerMask;//����������ײ�Ĳ㼶
    #endregion

    #region �����д��ڵ�����
    GameObject player;
    GameObject[] guns;
    GameObject[] potions;
    GameObject[] boxs;
    GameObject[] walls;
    #endregion

    #region ������������
    Vector3 enemyToPlayerDir;    //���������֮����������
    Vector3 enemyToGunDir;      //������ǹе֮����������
    Vector3 enemyToPotionDir;    //������ҩƿ֮����������
    float distance; //����볡������Ʒ�ľ���
    Player playerScript;//��ҵ�Player.cs�ű�����
    Transform playerTrans;//��ҵ�Transform���
    #endregion
    Dictionary<float, GameObject> distanceFromEnemyDic = new Dictionary<float, GameObject>();//�������λ���ֵ䣬GameObject��ʾ������˵���Ϸ����float��ʾ����˵ľ���

    #region ��ʶ����״̬�ı���
    public bool isInPickState = false;
    public bool isInRunAwayState = false;
    public bool isInHideState = false;
    public bool isInEscapeState = false;
    #endregion

    #region �����������
    int hpValue;
    int magicValue;
    int currentHPValue;
    int currentMagicValue;
    #endregion


    #region ��ʼ��
    protected override void Initialize()
    {
        instance = this;
        //��ȡ��Ϸ������ǹе����Һ�ҩƿ����
        guns = ItemManager.instance.guns;
        player = ItemManager.instance.player;
        potions = ItemManager.instance.potions;
        

        //��ȡ���˶����fsmEnemy�ű�������״̬������
        fsmEnemy = transform.GetComponent<FsmEnemy>();
        fsm = fsmEnemy.fsm;

        enemyLayerMask = LayerMask.NameToLayer("Enemy");//Ҫ���˵����߼��㼶

        //��ȡ���Player�ű�������transform
        playerScript = player.GetComponent<Player>();
        playerTrans = player.transform;
        

        hpValue = fsmEnemy.hpValue;
        magicValue = fsmEnemy.magicValue;
        
    }
    #endregion







    #region ����
    protected override void UpdateSense()
    {
        elapsedTime += Time.deltaTime;
        
        ///״̬���ȼ�������״̬>����״̬>����״̬>ʰȡ״̬>����״̬>����״̬>Ѳ��״̬
        ///��FsmEnmey����ײ������ж�����Ѫ�������Ѫ��Ϊ0��������״̬��ֱ��������Ҷ��󣬲��ö�����״̬�����жϣ����Ѫ��С�ڰٷ�֮ʮ����������״̬
        ///���Ѫ��С�ڰٷ�֮�壬��������״̬������״̬�����������������ֱ������ݴԽ�������״̬������״̬������Ѫ���ٷ�֮��ʮ�����л���Ѳ��״̬
        ///Ϊʲô���ڵ��˸�֪ϵͳ���ж����Ѫ����ԭ���ǵ��˸�֪ϵͳ1��ż��һ��
        if(elapsedTime >= detectionRate)
        {
            if (isInRunAwayState)//�ж��Ƿ�������״̬����������״̬ʱ���л�����״̬�����ǽ���ݴ��л�������״̬
            {
                elapsedTime = 0;
            }
            else if (isInHideState)//�������������״̬���ж��Ƿ�������״̬����������״̬ʱ�ж�����Ѫ���Ƿ���ڰٷ�֮��ʮ�������л�Ѳ��״̬
            {
                DetectSelf();
                elapsedTime = 0;
            }
            else if (isInPickState)//����������״̬������״̬���ж��Ƿ���ʰȡ״̬������ʰȡ״̬���̲�����������
            {
                elapsedTime = 0;
            }
            else
            {
                DetectPlayer();//�����ң����ܽ�������״̬�͹���״̬
                if (isInEscapeState)
                {
                    elapsedTime = 0;
                }
                else//�������������״̬,���ǹе
                {
                    DetectGuns();
                    if (isInPickState)//�����⵽ǹе�������ʰȡ״̬
                    {
                        elapsedTime = 0;
                    }
                    else
                    {
                        DetectPotions();//�����ⲻ��ǹе������ҩƿ
                        elapsedTime = 0;
                    }
                }
            } 
        }
    }
    #endregion





    

    #region �����׶��Χ����Ʒ�ķ���
    /// <summary>
    /// ���������ҷ���
    /// </summary>
    private void DetectPlayer()
    {
        enemyToPlayerDir = player.transform.position - transform.position;//���˵���ҵ�����
        if (GetAngleWithItem(enemyToPlayerDir) < FieldOfView && GetDistanceWithItem(playerTrans) < ViewDistance)//���������Ӷȷ�Χ����������Χ�ڣ��������߼��
        {
            RaycastHit2D hit = Physics2D.Raycast(transform.position, enemyToPlayerDir, ViewDistance, ~(1 << enemyLayerMask));//~(1 << playerLayerMask):��ʾ�����layermask��һ��
            if (hit.collider)//�����⵽��Ʒ
            {
                string currentState = fsm.GetEnemyStateName();//��ȡ��ǰ״̬����
                Aspect aspect = hit.collider.GetComponent<Aspect>();//��ȡ��⵽�������aspect����
                if (aspect != null && aspect.aspectName == playerAspect)//�����⵽�����
                {
                    //��ȡ��ҵ�ǰ�ֳ������ȼ����������ȼ��������������3������˽�������״̬   �����Ҵ��ڿ�״̬������˽�������״̬
                    Debug.Log("��⵽���");
                    int enemyLevel = fsmEnemy.level;
                    bool playerIsRage = playerScript.isRage;

                    int playerGunLever = playerScript.currentHandle.GetComponent<Gun>().level;//��ȡ�����ȼ�
                    if (currentState == FsmEnemy.FsmEnemyStateEnum.PatrolState.ToString())//������˴���Ѳ��״̬
                    {

                        if (playerGunLever - enemyLevel >= 3 || playerIsRage == true)
                        {
                            Debug.Log("��������ȼ����߻��ڿ�״̬");
                            fsm.SetEnemyState(new EscapeState(fsm));//�ı�״̬Ϊ����״̬
                            isInEscapeState = true;
                        }
                        else
                        {
                            fsm.SetEnemyState(new AttackState(fsm));//�ı�״̬Ϊ����״̬
                        }
                    }
                    else if (currentState == FsmEnemy.FsmEnemyStateEnum.AttackState.ToString())//������˴��ڹ���״̬
                    {
                        if (playerGunLever - enemyLevel >= 3 || playerIsRage == true)
                        {
                            Debug.Log("��������ȼ����߻��ڿ�״̬");
                            fsm.SetEnemyState(new EscapeState(fsm));//�ı�״̬Ϊ����״̬
                            isInEscapeState = true;
                        }
                    }
       
                }
                else//����������˵��ʶ�𲻵����
                {
                    if (currentState == FsmEnemy.FsmEnemyStateEnum.AttackState.ToString())//������˴��ڹ���״̬
                    {
                        fsm.SetEnemyState(new PatrolState(fsm));
                    }
                }
            }
        }
    }







    /// <summary>
    /// �������ǹе����
    /// </summary>
    private void DetectGuns()
    {
        InstantiateAndOrderDic(guns);//��ʼ���ֵ�
        List<float> keys = GetDicKeys(distanceFromEnemyDic);//��ȡ�ֵ������key
        keys.Sort();//��key�ļ��Ͻ�������

        for (int i = 0; i < keys.Count; i++)
        {
            GameObject gun;
            distanceFromEnemyDic.TryGetValue(keys[i],out gun);//����keyȡValue
            Transform gunTrans = gun.transform;//��ȡgun�����
            enemyToGunDir = gunTrans.position - transform.position;//��ȡgun�����˵ķ�������
            if (GetAngleWithItem(enemyToGunDir) < FieldOfView && GetDistanceWithItem(gunTrans) < ViewDistance)//���ǹе���Ӷȷ�Χ����������Χ�ڣ��������߼��
            {
                RaycastHit2D hit = Physics2D.Raycast(transform.position, enemyToGunDir, ViewDistance, ~(1 << enemyLayerMask));//~(1 << playerLayerMask):��ʾ�����layermask��һ��
                if (hit.collider)//�����⵽��Ʒ
                {
                    string currentState = fsm.GetEnemyStateName();//��ȡ��ǰ״̬����
                    Aspect aspect = hit.collider.GetComponent<Aspect>();//��ȡ��⵽�������aspect����

                    if (aspect != null && aspect.aspectName == gunAspect)
                    {
                        //������˲���������״̬����ǹе�ĵȼ����ڵ�������ȼ�3�������ʰȡ״̬
                        //�����������״̬�����������������ȼ�3����ʰȡǹе��С��3�������ʰȡ״̬
                        Debug.Log("��⵽ǹе");

                        int enemyLevel = fsmEnemy.level;//��ȡ���˵ȼ�
                        bool playerIsRage = playerScript.isRage;//��ȡ����Ƿ��ڿ�״̬
                        int gunLevel = gunTrans.GetComponent<Gun>().level;//��ȡ��⵽�������������ȼ�
                        int playerGunLevel = playerScript.currentHandle.GetComponent<Gun>().level;//��ȡ����ֳ������ȼ�

                        if (fsmEnemy.level < 5 && currentState != FsmEnemy.FsmEnemyStateEnum.EscapeState.ToString() && gunLevel >= enemyLevel)//������˲���������״̬�������ȼ���������ȼ�
                        {
                            fsm.SetEnemyState(new PickState(fsm, gun));//����ʰȡ״̬
                            isInPickState = true;
                            return;
                        }
                        else if (fsmEnemy.level < 5 && playerIsRage == false && currentState == FsmEnemy.FsmEnemyStateEnum.AttackState.ToString() && playerGunLevel - enemyLevel == 3)//���������Ϊ��������ȼ����߶���������״̬���ҵ���ʰȡ����������ȼ���1С��3�������ʰȡ״̬
                        {
                            fsm.SetEnemyState(new PickState(fsm, gun));//����ʰȡ״̬
                            isInPickState = true;
                            return;
                        }
                    }
                }
            }
        }
    }


    /// <summary>
    /// �������ҩƿ����
    /// </summary>
    private void DetectPotions()
    {
        InstantiateAndOrderDic(potions);//��ʼ���ֵ�
        List<float> keys = GetDicKeys(distanceFromEnemyDic);//��ȡ�ֵ������key
        keys.Sort();//��key�ļ��Ͻ�������

        for (int i = 0; i < keys.Count; i++)
        {
            GameObject potion;
            distanceFromEnemyDic.TryGetValue(keys[i], out potion);//����keyȡValue
            Transform potionTrans = potion.transform;//��ȡgun�����
            enemyToPotionDir = potionTrans.position - transform.position;
            if (GetDistanceWithItem(potionTrans) < ViewDistance && GetAngleWithItem(enemyToPotionDir) < FieldOfView)//���ǹе���Ӷȷ�Χ����������Χ�ڣ��������߼��
            {
                RaycastHit2D hit = Physics2D.Raycast(transform.position, enemyToPotionDir, ViewDistance, ~(1 << enemyLayerMask));//~(1 << playerLayerMask):��ʾ�����layermask��һ��
                if (hit.collider)//�����⵽��Ʒ
                {
                    string currentState = fsm.GetEnemyStateName();//��ȡ��ǰ״̬����
                    Aspect aspect = hit.collider.GetComponent<Aspect>();//��ȡ��⵽�������aspect����

                    if (aspect != null && aspect.aspectName == potionAspect)
                    {

                        Debug.Log("��⵽ҩƿ");

                        fsm.SetEnemyState(new PickState(fsm, potion));
                        isInPickState = true;
                        return;
                    }
                }
            }
        }


        /*for (int i = 0; i < potions.Length; i++)
        {
            Transform potionTrans = potions[i].transform;
            enemyToPotionDir = potionTrans.position - transform.position;
            if (GetDistanceWithItem(potionTrans) < ViewDistance  && GetAngleWithItem(enemyToPotionDir) < FieldOfView)//���ǹе���Ӷȷ�Χ����������Χ�ڣ��������߼��
            {
                RaycastHit2D hit = Physics2D.Raycast(transform.position, enemyToPotionDir, ViewDistance, ~(1 << enemyLayerMask));//~(1 << playerLayerMask):��ʾ�����layermask��һ��
                if (hit.collider)//�����⵽��Ʒ
                {
                    string currentState = fsm.GetEnemyStateName();//��ȡ��ǰ״̬����
                    Aspect aspect = hit.collider.GetComponent<Aspect>();//��ȡ��⵽�������aspect����

                    if (aspect != null && aspect.aspectName == potionAspect)
                    {
                        
                        Debug.Log("��⵽ҩƿ");

                        fsm.SetEnemyState(new PickState(fsm, potions[i]));
                        
                    }
                }
            }
        }*/
    }
    
    /// <summary>
    /// ����������������Ѫ�����ڰٷ�֮��ʮʱ������Ѳ��״̬
    /// </summary>
    private void DetectSelf()
    {
        currentHPValue = fsmEnemy.currentHPValue;
        currentMagicValue = fsmEnemy.currentMagicValue;
        if ((float)Math.Round((decimal)currentHPValue / hpValue, 2) >= 0.5f)
        {
            Debug.Log("�������");
            Debug.Log("��ǰѪ��" + currentHPValue.ToString());
            Debug.Log("Ѫ��" + hpValue.ToString());
            Debug.Log((float)Math.Round((decimal)currentHPValue / hpValue, 2));
            fsm.SetEnemyState(new PatrolState(fsm));
            isInHideState = false;
        }
    }
    #endregion








    #region ��ȡ����������֮��ľ���ͽǶ�
    /// <summary>
    /// ��ȡ������ľ���
    /// </summary>
    /// <param name="gameObject"></param>
    private float GetAngleWithItem(Vector3 dir)
    {
        float angle = 0;
        if (transform.rotation.y == 180)//������ﳯ�ҿ�
        {
            angle = Vector3.Angle(-transform.right, dir);//����������ǰ��������˺���������ļн�
        }
        else if (transform.rotation.y == 0)//������ﳯ����
        {
            angle = Vector3.Angle(-transform.right, dir);
        }

        return angle;
    }



    /// <summary>
    /// ��ȡ�����������ľ���
    /// </summary>
    /// <param name="gameObject"></param>
    private float GetDistanceWithItem(Transform trans)
    {
        return Mathf.Abs(Vector3.Distance(trans.position, transform.position));//���˵���ҵ���������
    }
    #endregion









    #region ���߷��������Ƶ���������Χ
    /// <summary>
    /// OnDrawGizmos�������������������ÿ֡����һ��
    /// ���ڻ��Ƶ�����GameObject����֮���������Χ
    /// </summary>
    /*private void OnDrawGizmos()
    {
        //���Ƶ��˵���ҵ�ֱ��
        Debug.DrawLine(transform.position, player.transform.position, Color.red);


        if (transform.rotation.y == 180)//������ﳯ�ҿ�
        {
            Vector3 frontRayPoint = transform.position + (-transform.right * ViewDistance);
            Vector3 leftRayPotin = frontRayPoint;
            leftRayPotin.y += FieldOfView;
            Vector3 rightRayPoint = frontRayPoint;
            rightRayPoint.y -= FieldOfView;


            Debug.DrawLine(transform.position, frontRayPoint, Color.green);
            Debug.DrawLine(transform.position, leftRayPotin, Color.green);
            Debug.DrawLine(transform.position, rightRayPoint, Color.green);
        }
        else if (transform.rotation.y == 0)//������ﳯ����
        {
            Vector3 frontRayPoint = transform.position + (-transform.right * ViewDistance);
            Vector3 leftRayPotin = frontRayPoint;
            leftRayPotin.y += FieldOfView;
            Vector3 rightRayPoint = frontRayPoint;
            rightRayPoint.y -= FieldOfView;


            Debug.DrawLine(transform.position, frontRayPoint, Color.green);
            Debug.DrawLine(transform.position, leftRayPotin, Color.green);
            Debug.DrawLine(transform.position, rightRayPoint, Color.green);
        }
    }*/
    #endregion









   

    /// <summary>
    /// ��ȡ��Ϸ��������Ҿ���
    /// </summary>
    /// <returns></returns>
    private float GetDistance(GameObject Object)
    {
        return Vector3.Distance(Object.transform.position, transform.position);
    }

    /// <summary>
    /// ��ʼ���ֵ�
    /// </summary>
    /// <param name="gameObjects"></param>
    private void InstantiateAndOrderDic(GameObject[] gameObjects)
    {
        distanceFromEnemyDic.Clear();//������ֵ�
        
        for(int i = 0; i < gameObjects.Length; i++)
        {
            if (gameObjects[i])
            {
                distanceFromEnemyDic.Add(GetDistance(gameObjects[i]), gameObjects[i]);
            }
            
        }
    }

    private List<float> GetDicKeys(Dictionary<float,GameObject> dic)
    {
        List<float> keys = new List<float>();
        foreach(KeyValuePair<float,GameObject> kvp in dic)
        {
            keys.Add(kvp.Key);
        }
        return keys;
    }
}
