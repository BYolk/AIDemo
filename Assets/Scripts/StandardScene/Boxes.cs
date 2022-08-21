using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

/// <summary>
/// �ӵ���������ײ�������ӵ������ӣ����ʵ����ҡ�������ҩˮ
/// ��ȡ���ӵ���Ƭ��ͼ������Start�����ڳ�ʼ��
/// </summary>
public class Boxes : MonoBehaviour
{
    #region ������ҩˮԤ�������ü���
    public List<GameObject> guns = new List<GameObject>();
    public List<GameObject> potions = new List<GameObject>();
    #endregion

    #region ����
    Tilemap boxTilemap;     
    //GameObject[] tileMapGameObject;//������������кܶ࣬��Ҫ��һ�������������Щ���ӣ����ӵ�boxTilemap������ײʱ���ж���ײ�������ǲ��ǰ����������棬�����˵��������
    #endregion

    private void Start()
    {
        boxTilemap = GameObject.Find("Grid").transform.Find("Boxes").GetComponent<Tilemap>();
        
    }

    /// <summary>
    /// ��ʼ������
    /// </summary>
    private void InitializeBoxes()
    {

    }

    /// <summary>
    /// ����ӵ���Boxes(Tilemap)����Ƭ������ײ��������λ����Ƭ
    /// ����һ����ά�������ڴ����ײλ��
    /// �ж���boxTilemap������ײ�������ǲ����ӵ��������Ƕ����ӵ��Ĳ㼶������㼶��Ӧ�����ӵ������ȡ�ӵ���ײ��collision.contacts
    /// ���ӵ���ײ���x���y�ᱣ�浽hitPosition��
    /// ����boxTilemap��WorldToCell������hitPosition����ת��Ϊ��ͼ�������꣬������Tilemap��SetTile���������������Ƭ����Ϊnull���������ӣ�
    /// 
    /// ���ʵ���������ҩˮ
    /// </summary>
    /// <param name="collision">��ײ������</param>
    private void OnCollisionEnter2D(Collision2D collision)
    {
        Vector3 hitPosition = Vector3.zero; //��ײ������
        if(boxTilemap != null)
        {
            if (collision.gameObject.layer == 15 || collision.gameObject.layer == 14)
            {
                foreach (ContactPoint2D hit in collision.contacts)//������ײ�Ӵ���
                {
                    hitPosition.x = hit.point.x - 0.01f * hit.normal.x;
                    hitPosition.y = hit.point.y - 0.01f * hit.normal.y;
                    boxTilemap.SetTile(boxTilemap.WorldToCell(hitPosition), null);//����ײ��λ��ת������������λ�ã���������Ϊ��
                }
                Destroy(collision.gameObject);  //�����ӵ�����

                int num1 = Random.Range(1, 101);//�������һ��0��100����
                if(num1 > 90)
                {
                    int num2 = Random.Range(1, 100);//���������һ��0��100����
                    if(num2 < 50)
                    {
                        Instantiate(guns[Random.Range(0, guns.Count - 1)], boxTilemap.WorldToCell(hitPosition), this.transform.rotation);//�������һ������
                    }
                    else
                    {
                        Instantiate(potions[Random.Range(0, potions.Count - 1)], boxTilemap.WorldToCell(hitPosition), this.transform.rotation);//�������ҩˮ
                    }
                }
            }
            
        }
    }
}
