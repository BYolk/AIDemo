using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// �����Žű������ڵ�������ʱ�ã������˽��봫���ţ���ʾ�������ܳɹ�
/// </summary>
public class Door : MonoBehaviour
{
    #region

    #endregion

    /// <summary>
    /// �����ŵ���ײ��ⷽ��������봫������ײ��ʱ���������ҵ��˴�������״̬
    /// </summary>
    /// <param name="collision"></param>
    private void OnCollisionEnter2D(Collision2D collision)
    {
        string tag = collision.gameObject.tag;
        
        /*if(tag == "Enemy" )
        {
            if(collision.GetComponent<FsmEnemy>().fsm.GetEnemyState().GetType().Name == "EscapeState")
            {
                Debug.Log(collision.gameObject.name.ToString() + "���ܳɹ�");
                Destroy(collision.gameObject);
            }
        }*/
        if(tag == "PlayerBullet" || tag == "EnemyBullet")
        {
            Destroy(collision.gameObject);
        }
    }


    
}
