using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ҩƿ��
/// </summary>
public class Potion : MonoBehaviour
{
    #region ��ҩƿ������صı���
    public string potionName;
    #endregion

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            collision.gameObject.GetComponent<Player>().pickableItems.Add(this.gameObject);
        }
        if(collision.gameObject.tag == "Enemy")
        {
            if (collision.gameObject.GetComponent<Enemy>())
            {
                collision.gameObject.GetComponent<Enemy>().pickableItems.Add(this.gameObject);
            }
            else if (collision.gameObject.GetComponent<FsmEnemy>())
            {
                collision.gameObject.GetComponent<FsmEnemy>().pickableItems.Add(this.gameObject);
            }
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            collision.gameObject.GetComponent<Player>().pickableItems.Remove(this.gameObject);
        }
        if (collision.gameObject.tag == "Enemy")
        {
            if (collision.gameObject.GetComponent<Enemy>())
            {
                collision.gameObject.GetComponent<Enemy>().pickableItems.Remove(this.gameObject);
            }
            else if (collision.gameObject.GetComponent<FsmEnemy>())
            {
                collision.gameObject.GetComponent<FsmEnemy>().pickableItems.Remove(this.gameObject);
            }
        }
    }
}
