using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bush : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Enemy")//������˽���ݴ�
        {
            FsmEnemy fsmEnemy = collision.gameObject.GetComponent<FsmEnemy>();
            Fsm fsm = fsmEnemy.fsm;
            EnemyPerspective enemyPerspective = collision.gameObject.GetComponent<EnemyPerspective>();

            if(fsm.GetEnemyStateName() == "RunAwayState")//������˴�������״̬
            {
                enemyPerspective.isInRunAwayState = false;
                enemyPerspective.isInHideState = true;
                fsm.SetEnemyState(new HideState(fsm));//�л����˵�����״̬
            }
        }
    }
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Enemy")
        {
            FsmEnemy fsmEnemy = collision.gameObject.GetComponent<FsmEnemy>();
            Fsm fsm = fsmEnemy.fsm;
            EnemyPerspective enemyPerspective = collision.gameObject.GetComponent<EnemyPerspective>();

            int currentHPValue = fsmEnemy.currentHPValue;
            int hpValue = fsmEnemy.hpValue;
            if (fsm.GetEnemyStateName() == "HideState")//������˴�������״̬
            {
                if((float)Math.Round((decimal)currentHPValue / hpValue, 2) >= 0.5f)//���Ѫ�����ڰٷ�֮��ʮ���л�ΪѲ��״̬
                {
                    fsm.SetEnemyState(new PatrolState(fsm));
                    enemyPerspective.isInHideState = false;
                }
            }
        }
    }
}
