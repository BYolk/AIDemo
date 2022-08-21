using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


/// <summary>
/// �˵������У������ť��ת����Ӧ����Ϸ����
/// </summary>
public class MenuOptions : MonoBehaviour
{
    #region

    #endregion
    
    /// <summary>
    /// ��ת������׼ģʽ������
    /// </summary>
    public void StrandardScene()
    {
        SceneManager.LoadScene(1);
    }
    /// <summary>
    /// ��ת��FSM����
    /// </summary>
    public void FSMScene()
    {
        SceneManager.LoadScene(2);
    }
    /// <summary>
    /// ��ת��FuSM����
    /// </summary>
    public void FuSMScene()
    {
        SceneManager.LoadScene(3);
    }
    /// <summary>
    /// ��ת��ANN����
    /// </summary>
    public void ANNScene()
    {
        SceneManager.LoadScene(4);
    }
    public void Exit()
    {
        Application.Quit();
    }
}
