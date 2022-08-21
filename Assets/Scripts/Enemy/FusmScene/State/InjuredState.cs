using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ����״̬�ࣺ����ͬʱ��������״̬�ͽ���״̬������״̬�ʹ���״̬
/// </summary>
public class InjuredState : FusmBaseState
{
    #region ����������
    #region ģ��״̬�������뿪��ģ������״̬���Ľű�����
    private Fsm fsm;//��ȡ��������״̬������
    private Fusm fusm;//��ȡ����ģ��״̬������
    private FsmEnemy fsmEnemy;//��ȡ���˶���ű�����
    #endregion

    private float activation;//�����ڱ�״̬�ĳ̶ȣ�0.0f��ʾ��ȫ�����ڴ�״̬��1.0f��ʾ��ȫ���ڴ�״̬
    int hpValue;//Ѫ������
    int currentHPValue;//��ǰѪ��
    float timer;//��ʱ��
    float hpPercentage;//Ѫ���ٷֱ�
    #endregion
    #region ���췽��
    public InjuredState(Fusm fusm)
    {
        //Debug.Log("ע������״̬");
        this.fusm = fusm;
        this.fsmEnemy = fusm.GetFusmEnemy();

    }
    #endregion
    #region ���㼤������
    /// <summary> 
    /// �� Fusm ģ��״̬���� UpdateFusm ����ģ��״̬�������е��� Evaluate ���жϸ�״̬�� activation �Ƿ����0
    /// ���Ѫ���ٷֱ�Ϊ�ٷ�֮��ʮ��˵������ȫȫ��������״̬
    /// ���Ѫ���ٷֱȴ��ڰٷ�֮��ʮС�ڰٷ�֮��ʮ��˵��ͬʱ��������״̬�봹��״̬
    /// ���Ѫ���ٷֱȴ��ڰٷ�֮��ʮС�ڰٷ�֮��ʮ��˵��ͬʱ��������״̬�뽡��״̬
    /// </summary>
    public float Evaluate()
    {
        hpValue = fsmEnemy.hpValue;
        currentHPValue = fsmEnemy.currentHPValue;
        float hpPercentage = (float)Math.Round((decimal)currentHPValue / hpValue, 2);//�õ���ǰѪ���ٷֱ�
        if (hpPercentage > 0.2 && hpPercentage <= 0.4)
            activation = 0.05f   * (hpPercentage * 100) - 1;
        else if(hpPercentage >= 0.4 && hpPercentage < 0.8)
            activation = -0.025f * (hpPercentage * 100) + 2;
        CheckBounds();//��� activation �Ϸ���
        if (activation > 0)
        {
            Debug.Log("��ǰ��������״̬������������״̬�ĳ̶�Ϊ��" + activation.ToString());
        }
        if (hpPercentage > 0.2 && hpPercentage <= 0.4)
        {
            if (activation >= 0.34)
            {
                Debug.Log("��ǰѪ������������״̬�ĳ̶ȸ��ߣ����������������ҩƿ������ʰȡģʽ");
                /*GameObject[] potions = ItemManager.instance.potions;
            foreach(GameObject potion in potions)//��ȡ����������ҩƿ������ҩƿ�����ҩƿ�ָܻ�HP�������ʰȡ״̬
            {
                if (potion && (potion.GetType().Name == "HealthyPotion" || potion.GetType().Name == "RejuvenationPotion"))
                {
                    fsm.SetEnemyState(new PickState(fsm, potion));
                    //fsmEnemy.GetComponent<EnemyPerspective>().isInRunAwayState = true;
                    break;
                }
            }*/
            }
            else if (activation >= 0 && activation < 0.34)
            {
                Debug.Log("��ǰѪ�������ڴ���״̬�ĳ̶ȸ��ߣ���������״̬");
                //fsm.SetEnemyState(new RunAwayState(fsm));
                //fsmEnemy.GetComponent<EnemyPerspective>().isInRunAwayState = true;
            }
        }
        else if (hpPercentage >= 0.4 && hpPercentage < 0.8)
        {

            if (activation >= 0.25)
            {
                Debug.Log("��ǰѪ������������״̬�ĳ̶ȸ��ߣ����������������ҩƿ������ʰȡģʽ");
                /*GameObject[] potions = ItemManager.instance.potions;
            foreach(GameObject potion in potions)//��ȡ����������ҩƿ������ҩƿ�����ҩƿ�ָܻ�HP�������ʰȡ״̬
            {
                if (potion && (potion.GetType().Name == "HealthyPotion" || potion.GetType().Name == "RejuvenationPotion"))
                {
                    fsm.SetEnemyState(new PickState(fsm, potion));
                    //fsmEnemy.GetComponent<EnemyPerspective>().isInRunAwayState = true;
                    break;
                }
            }*/
            }
            else if(activation > 0 && activation < 0.25)
            {
                Debug.Log("��ǰѪ�������ڽ���״̬�ĳ̶ȸ��ߣ��������κβ���");
            }
        }
        return activation;//���� activation
    }
    #endregion



    public void Update()
    {

    }

    #region ��⼤������ֵ�ĺϷ���
    /// <summary>
    /// ��⼤������ֵ�ĺϷ���
    /// </summary>
    /// <param name="lowerBound"></param>
    /// <param name="upperBound"></param>
    public void CheckBounds(float lowerBound = 0, float upperBound = 1)
    {
        CheckLowerBound(lowerBound); 
        CheckUpperBound(upperBound);
    }
    /// <summary>
    /// �����������޺Ϸ���
    /// </summary>
    /// <param name="lowerBound"></param>
    public void CheckLowerBound(float lowerBound = 0)
    {
        if (activation < lowerBound)
        {
            activation = lowerBound;
        }
    }
    /// <summary>
    /// �����������޺Ϸ���
    /// </summary>
    /// <param name="upperBound"></param>
    public void CheckUpperBound(float upperBound = 1)
    {
        if (activation > upperBound)
        {
            activation = upperBound;
        }
            
    }
    #endregion


































    public void Enter()
    {

    }

    public void Exit()
    {

    }

    public void Init()
    {

    }



}
