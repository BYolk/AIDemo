using MathNet.Numerics.LinearAlgebra;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

/// <summary>
/// ��������ű�˼·��
///     1���ܼ����� Dense Matrix����ϡ������Ӧ����һ�������� 0 Ԫ�ص�����ԶԶ���ڷ� 0 Ԫ�ص���������þ���Ϊϡ����󣬷�֮Ϊ�ܼ�����
///     
/// 
///     2��ƫ����൱�ں����Ľؾ� b����Ӧ��бʽ y=kx+b ��y �൱����һ�����Ԫ��x �൱�ڵ�ǰ�����Ԫ��k�൱��Ȩ�أ�b �൱��ƫ��ֵ��ƫ�������ÿ����Ԫ��ƫ��ֵ��ƫ��Ȩ�صĳ˻�
///     
/// 
///     3������������ã������ʹ�ü��������������������ô���ӣ�����Ӧ�ĺ�����ͼ�ϻ������������Եģ�������Ķ���һ��������ϡ�ֻ��ͨ�������������ܵõ������Ե���ϣ�����������ʾ�����Ե�ͼ�Σ����������������û�������������ı��������������������Ԫ���������Ԫ��
///         3.1��Sigmoid ����������S�ͺ�����Ҳ��ΪS���������ߣ��������������У���ֵӳ�䵽[0,1]�����ϣ�����Ϊ y = 1 / (1 + e^-x)
///         3.2��Tanh ����������˫�����к�������ֵӳ�䵽[-1,1]�����ϣ�����Ϊ y = tanhx = sinhx / coshx = (e^x - e^-x) / (e^x + e^-x)
///     
/// 
///     4��ÿһ�����ز���Ԫ���������
///     
/// 
///     5��������
///         5.1��������������㡢���ز���������ɣ�ÿһ�㶼�ɾ����ʾ��
///         5.2����Ϊ�������������ȷ���ģ������������������Ԫ���ֱ����������������������ǽ��λ�ã�����������������Ԫ���ֱ����������μ�����ת������Ϊ����㴴��һ��һ�����е��ܼ�����Ϊ����㴴��һ��һ�����е��ܼ�����
///         5.3����Ϊ���ز��ǲ�ȷ���ģ�������Ҫ����һ�����󼯺ϣ����ڱ���ÿһ���������ز�ľ���
///         5.4��ÿ�������ڵĲ�֮�����Ȩ�أ�����Ϊ���ز㲻ȷ����������Ҫ����һ�����󼯺��������ÿһ������Ȩ�صľ���
///         5.5�����������֮��ÿһ�㶼����һ��ƫ��ֵ��������Ҫ����һ�����������ÿһ������ƫ��ֵ�ĸ�����
///         5.6�������Ҫһ���������͵ı�������������������Ӧ��
///         
///     
///     
/// 
///     6��RunNetwork(float a, float b, float c) �������÷����� CarController �� FixedUpdate ������ÿ�� 0.02s ����һ��
///         6.1����Ϊ�÷����� CarController �ű��� FixedUpdate �����е��ã����Ա��ű�������ִ�еķ������� RunNetwork ����
///         6.2���÷�����Ҫ�����������������������������������ϰ����λ�ã����������������
///         6.3�����������������ֵ��������������������Ԫ
///         6.4��ͨ������� Tanh �����������ֵȫ��ӳ�䵽[-1��1]�����ϣ�Ϊ��������������Ԫ�أ�ע��˴�����ʹ�� Sigmoid ���������Ϊ�����֮ǰ�����е�ֵ��������ת���йأ�������ת����Ҫ�ø�����ʾ����ת
///         6.5���������� ���� ����������ز�ĵ�һ��֮���Ȩ�ؾ��� + ���ز��һ���ƫ��ֵ = ���ز��һ������ٽ�����ͨ������� Tanh ����һ������ֵ��ӳ�䵽[-1��1]�����ϣ�Ϊ�����������Ԫ��
///         6.6���������ز���˵�һ��֮������в㼶����Ϊ��һ�����������������Ҫ���⴦������ͬ����ÿһ���������ز�ľ���
///         6.7���������ز�����һ�㣬�������������
///         6.8������������������ݣ�ע���ڷ���֮ǰ��Ҫͨ��������������������ݣ���һ�����ݴ�����ٶȣ���Ҫͨ�� Sigmoid ������ת����[0,1]���䣬�ڶ������ݴ���ת����Ҫͨ�� Tanh ������ӳ�䵽[-1,1]����
///         
/// 
///     7��Sigmoid ������
///         7.1������������ Sigmoid����Ϊ��� MathNet.Numerics ��ֻ�м������� Tanh��������Ҫ�����д Sigmoid 
///         7.2���������� Sigmoid �㷨Ϊ��(1 / (1 + Mathf.Exp(-s)))������ s ��ʾҪת����ֵ���� x ��Mathf.Exp(s)������e��s�η�
///         
/// 
///     8��NNet �ű������з����ĵ��������
///         8.1��Sigmoid() ����ͨ�� RunNetwork() �������ã�RunNetwork ������ CarContorller�ű��� FixedUpdate �����е��á�
///         8.2��Initialise() ��RandomiseWeights() ���������������ڳ�ʼ��������ķ�����RandomiseWeights ������ Initialise �����ڵ��ã�Initialise ������ GeneticManager ����������е���
///         8.3��InitialiseCopy() ��InitialiseHidden() �������������ڿ�������ó�ʼ��������ĸ�����InitialiseHidden ������ InitialiseCopy �����ڵ��ã�InitialiseCopy ������ GeneticManager ����������е���
///         8.4��Ҫ�������������ʼ������������Ҫ��� GeneticManager �ű������
///         
///         
///     
///     9����ʼ�������緽����Initialise() ��RandomiseWeights()
///         9.1��Initialise(int hiddenLayerCount, int hiddenNeuronCount) ��
///             9.1.1����Ҫ��������������һ�������ز�������һ�������ز���Ԫ����
///             9.1.2�����þ���ͼ��ϵ� Clear() ����������㡢���ز㡢����㡢Ȩ�غ�ƫ����ȫ�����
///             9.1.3��ѭ�������ز����� + 1���Σ�
///                 9.1.3.1��ÿѭ��һ�δ���һ���ܼ����󣬾���ά��Ϊ��һ�У������ز���Ԫ�ڵ������С��������ݸ��������ز����������ز����Ԫ����������Ӧ��������Ӧά�Ⱦ��󣬲�����Щ������ӵ��Ѿ���յ����ز㼯�� hiddenLayers ��
///                 9.1.3.2���ٸ������ز�����������ƫ��ֵ��ƫ��ֵ�� -1 �� 1 ֮�����ȡһ������ֵ��
///                 9.1.3.3��ע������ǵ�һ�����ز㣬��Ϊ�����������������Ҫ���⴦����ҪΪ�䴴��һ�������У������ز���Ԫ�ڵ������С����ܼ��������ڴ��Ȩ�أ���Ϊ�����Ϊ��һ�У����С��ľ��󡣽����������ӵ���Ȩ�ؾ��󼯺��С�
///                 9.1.3.4��Ϊ�������ز㴴���������ز���Ԫ�ڵ������У������ز���Ԫ�ڵ������С��������ڴ�ų��˵�һ�����ز�֮����������Ȩ�أ�������Щ������ӵ���Ȩ�ؾ��󼯺��С�
///             9.1.4��ѭ�����֮��Ϊ����㴴���������ز���Ԫ�ڵ������У�2�С��ľ������ڴ�����һ�����ز�������Ȩ�ؾ��󲢽�����ӵ���Ȩ�ؾ��󼯺��С�����Ϊ��������ƫ��ֵ��ͬ���� -1 �� 1 ֮�����ȡһ������ֵ
///             9.1.5������ RandomiseWeights ����Ϊ���е�Ȩ��ֵ�����ֵ
///         9.2��RandomiseWeights()��ΪȨ�������ֵ
///             9.2.1������Ȩ�ؾ��󼯺����������Ȩ�ؾ���
///             9.2.2������ÿһ��Ȩ�ؾ����ÿһ��
///             9.2.3������ÿһ��Ȩ�ؾ����ÿһ�е�ÿһ��
///             9.2.4��ΪȨ�ؾ����ÿһ�е�ÿһ�е�Ԫ�������һ�� -1 �� 1 ֮��ĸ���ֵ
///         9.3����ʼ��������Ĺ�����ΪʲôҪ��Ȩ�غ�ƫ��ֵ��ȡ���ֵ��
///             ����֮��ᴴ�������������磬ÿ�������綼���Ȩ�����ƫ��ֵ��Ȼ�󾭹�ѵ������Щ��������ɸѡ��������õ������磬��Ȩ��ֵ��ƫ��ֵ����ʵ�������
///             
/// 
///     10��������ʼ�������縱��������InitialiseCopy() ��InitialiseHidden() 
///         10.1��InitialiseCopy(int hiddenLayerCount, int hiddenNeuronCount) ������
///             10.1.1����Ҫ�����������������ز����������ز����Ԫ����
///             10.1.2������һ���µ�������
///             10.1.3��Ϊ�µ������紴��һ���µ�Ȩ�ؾ��󼯺�
///             10.1.4��������ǰ������Ȩ�ؼ����е�ÿһ��Ȩ�ؾ��󣬴���һ���µ�Ȩ�ؾ��󣬾���ά��Ϊ��������Ȩ�ؾ������ڿ�����������Ȩ�ؾ��󡣱��������ÿһ��ÿһ�У�����������Ȩ�ؾ����ֵ��Ӧ��ֵ���µ�Ȩ�ؾ������Ȩ�ؾ���Ŀ�������������ӵ��µ�Ȩ�ؾ��󼯺�
///             10.1.5�������µ�ƫ��ֵ���϶��󣬲�����ǰ��ƫ��ֵ���Ͽ������µļ��϶�����
///             10.1.6��������������Ȩ�ؾ����ƫ�ü��϶���ֵ���µ�������
///             10.1.7������ InitialiseHidden ������ʼ��ʥ����������ز�
///             10.1.8���������������縱�����ظ������ߣ��� GeneticManager �������������
///         10.2��InitialiseHidden(int hiddenLayerCount, int hiddenNeuronCount) ������
///             10.1.1����Ҫ�����������������ز����������ز����Ԫ����
///             10.1.2����������������㡢���ز�������
///             10.1.3��ͨ�� for ѭ��Ϊ���ز㴴����Ӧ�����ľ��󣬲���������ӵ����ز���󼯺���
///         10.3��ͨ�� InitialiseCopy �� InitialiseHidden ����������ǰ�����������Ȩ��ֵ��ƫ��ֵ�����ز������ṹ�����Ǹ�������Ľṹ������ֵ����ݵ����ж��ı䣬ͨ�����������ṹ�����еõ����ֵ������������Ŀ��������������������㣩
///             
/// 
///     11��ע�⣺�� GeneticManager �ű����� Initialise ������ InitialiseCopy ����ʱ����Ҫ���������������������������Ķ����е�һ�������� hiddenLayerCount �����ڵ����Ǵ��ݹ�����ʵ���� CarController �ű������ LAYERS���� LAYERS ��ֵΪ 1 ʱ��������������ѭ��ʱ��� Layer + 1�������ز�����Ϊ 2��
/// </summary>
public class ANN 
{
    #region ������������
    public Matrix<float> inputLayer = Matrix<float>.Build.Dense(1, 3);//��������
    public List<Matrix<float>> hiddenLayers = new List<Matrix<float>>();//���ز���󼯺�(���ܴ��ڶ�����ز�)
    public Matrix<float> outputLayer = Matrix<float>.Build.Dense(1, 2);//��������
    public List<Matrix<float>> weights = new List<Matrix<float>>();//Ȩ�صľ��󼯺�
    public List<float> biases = new List<float>();//���ڴ��ƫ��ֵ
    public float fitness;//��Ӧ�ȱ���
    #endregion

    #region ���£��÷������� CarController ���˿������ű��е� FixedUpdate ������ÿ�� 0.02s ����һ��
    /// <summary>
    /// ���������緽��
    /// </summary>
    /// <param name="a"></param>
    /// <param name="b"></param>
    /// <param name="c"></param>
    /// <returns>����һ��һ�ж��еľ��󣬷ֱ��ʾ���˼��ٶȺ͵���ת��</returns>
    public (float, float) RunNetwork(float a, float b, float c)
    {
        //���������������Ԫ��ֵ
        inputLayer[0, 0] = a;
        inputLayer[0, 1] = b;
        inputLayer[0, 2] = c;
        inputLayer = inputLayer.PointwiseTanh();//ͨ��Tanh������ÿ���ڵ��ֵת����[-1��1]����
        //����Ϊʲô������ͨ��Sigmoid����ת����[0��1]������Ϊ���ֵ��x��Ҫ�ø�ֵ��ʾ��ת

        //��һ�����ز�����=��������*Ȩ�ؾ���+ƫ��ֵ
        //�ٽ���һ�����ز�����ͨ��Tanh����ת����[-1��1]����
        hiddenLayers[0] = ((inputLayer * weights[0]) + biases[0]).PointwiseTanh();

        //���������м����ز㣨��Ϊ��һ�����ز���������������Ҫ���⴦��
        for (int i = 1; i < hiddenLayers.Count; i++)
        {
            hiddenLayers[i] = ((hiddenLayers[i - 1] * weights[i]) + biases[i]).PointwiseTanh();
        }

        //���������=���һ�����ز����*���һ��Ȩ�ؾ���+���һ��ƫ��ֵ
        //�ٽ�����ͨ��Tanh����ת����[-1��1]����
        outputLayer = ((hiddenLayers[hiddenLayers.Count - 1] * weights[weights.Count - 1]) + biases[biases.Count - 1]).PointwiseTanh();

        //��һ�����������x���꣬�ڶ������������y����
        return ((float)Math.Tanh(outputLayer[0, 0]), Sigmoid(outputLayer[0, 1]));
    }
    #endregion







    #region Sigmoid �㷨
    /// <summary>
    /// ���ṩ��ֵת��Ϊ0��1֮���ֵ
    /// </summary>
    /// <param name="s"></param>
    /// <returns></returns>
    private float Sigmoid(float s)
    {
        return (1 / (1 + Mathf.Exp(-s)));
    }
    #endregion








    #region ��ʼ�������緽����������ʼ�������緽��
    /// <summary>
    /// ��ʼ�������磺ȷ���տ�ʼ��ʱ�����е�ֵ��������ģ�����ѵ��ѡ��Ч����õ�Ȩ��ֵ��
    /// </summary>
    /// <param name="hiddenLayerCount">��ʼ������������ز�����</param>
    /// <param name="hiddenNeuronCount">��ʼ������·��������Ԫ���񾭽ڵ㣩����</param>
    public void Initialise(int hiddenLayerCount, int hiddenNeuronCount)
    {
        //������е�����������㡢���ز㡢����㡢Ȩ�غ�ƫ��ֵ����
        inputLayer.Clear();
        hiddenLayers.Clear();
        outputLayer.Clear();
        weights.Clear();
        biases.Clear();


        //��ʼ�����������ز㣬ƫ������Ȩ��
        for (int i = 0; i < hiddenLayerCount + 1; i++)//�����������ص����������ز�
        {
            //��ÿһ�����ص����������ز����������Ԫ�������浽һ���¾����У���������ӵ�����������ز������� hiddenLayers ��
            Matrix<float> f = Matrix<float>.Build.Dense(1, hiddenNeuronCount);
            hiddenLayers.Add(f);
            biases.Add(Random.Range(-1f, 1f));//�������һ����-1��1�ĸ�������Ϊ�������ƫ��ֵ

            //���Ȩ��
            if (i == 0)//���i==0��˵����һ���Ȩ����������Ȩ�أ���Ϊ���ز��һ�������������
            {
                Matrix<float> inputToH1 = Matrix<float>.Build.Dense(3, hiddenNeuronCount);//��Ϊ�������������Ԫ������������3�������������ص���Ԫ�ڵ�����
                weights.Add(inputToH1);
            }
            //�����Ȩ����ר��Ϊ�������Ƶ�Ȩ��
            //��������Ȩ����Ϊ���������֮���Ȩ����Ƶģ��������ز㶼����ͬ��������Ԫ������
            Matrix<float> HiddenToHidden = Matrix<float>.Build.Dense(hiddenNeuronCount, hiddenNeuronCount);
            weights.Add(HiddenToHidden);
        }
        //���������Ȩ�ؾ���
        Matrix<float> OutputWeight = Matrix<float>.Build.Dense(hiddenNeuronCount, 2);//��Ϊ�����ֻ��������Ԫ������������2
        weights.Add(OutputWeight);
        biases.Add(Random.Range(-1f, 1f));//ͬ�����-1��1��һ�������������Ϊƫ��ֵ
        RandomiseWeights();//������е�Ȩ��ֵ
    }




    /// <summary>
    /// �������Ȩ��ֵ��
    /// </summary>
    public void RandomiseWeights()
    {
        //��������Ȩ�ؾ�������Ȩ�ؾ��󶼱����ڼ����ڣ�
        for (int i = 0; i < weights.Count; i++)
        {
            //����ÿһ��Ȩ�ؾ������
            for (int x = 0; x < weights[i].RowCount; x++)
            {
                //����ÿһ��Ȩ�ؾ���ÿһ�е�ÿһ��
                for (int y = 0; y < weights[i].ColumnCount; y++)
                {
                    weights[i][x, y] = Random.Range(-1f, 1f);//��-1��1�����һ����������Ϊ�þ�����и��е�ֵ
                }
            }
        }
    }
    #endregion







    #region ���Ƴ�ʼ���������緽�������õ���ʼ��������ĸ���
    /// <summary>
    /// ��ʼ�����ƣ���ʼ����������������
    /// </summary>
    /// <param name="hiddenLayerCount">���ز�����</param>
    /// <param name="hiddenNeuronCount">���ز���Ԫ����</param>
    /// <returns></returns>
    public ANN InitialiseCopy(int hiddenLayerCount, int hiddenNeuronCount)
    {
        ANN n = new ANN();//����һ���µ�������
        List<Matrix<float>> newWeights = new List<Matrix<float>>();//����һ���µ�������Ȩ�ؾ�������

        //������ǰÿһ��������Ȩ�ؾ���
        for (int i = 0; i < this.weights.Count; i++)
        {
            Matrix<float> currentWeight = Matrix<float>.Build.Dense(weights[i].RowCount, weights[i].ColumnCount);//�����µ�Ȩ�ؾ���
            for (int x = 0; x < currentWeight.RowCount; x++)//�����µ�Ȩ�ؾ����ÿһ��
            {
                for (int y = 0; y < currentWeight.ColumnCount; y++)//�����µ�Ȩ�ؾ����ÿһ��
                {
                    currentWeight[x, y] = weights[i][x, y];//���µ�Ȩ�ؾ����ÿ��ÿ�е�Ԫ�ظ�ֵ������ֵ�ǵ�ǰҪ������Ȩ�ؾ����Ӧ��Ԫ��ֵ
                }
            }
            newWeights.Add(currentWeight);//���µ�Ȩ�ؾ�����ӵ��µ�Ȩ�ؾ��󼯺���
        }

        List<float> newBiases = new List<float>();//�����µ�ƫ��ֵ����

        newBiases.AddRange(biases);//��ԭ����ƫ��ֵ���ϸ�ֵ���µ�ƫ��ֵ����
        n.weights = newWeights;//����Ȩ�ظ��µ�������
        n.biases = newBiases;//����ƫ��ֵ���µ�������
        n.InitialiseHidden(hiddenLayerCount, hiddenNeuronCount);//��ʼ������������ز�
        //���������Ŀ���
        return n;//���ؿ����ŵ�������
    }




    /// <summary>
    /// ��ʼ������������ز�
    /// </summary>
    /// <param name="hiddenLayerCount">���ز�����</param>
    /// <param name="hiddenNeuronCount">���ز����Ԫ����</param>
    public void InitialiseHidden(int hiddenLayerCount, int hiddenNeuronCount)
    {
        inputLayer.Clear();//�����������������
        hiddenLayers.Clear();//�������������ز�
        outputLayer.Clear();//���������������


        //����ÿһ�����������ز�
        for (int i = 0; i < hiddenLayerCount + 1; i++)
        {
            Matrix<float> newHiddenLayer = Matrix<float>.Build.Dense(1, hiddenNeuronCount);//Ϊÿһ�����������ز㴴���������ڴ����Ԫ
            hiddenLayers.Add(newHiddenLayer);//��������ӵ����ز���
        }

    }

    #endregion



}
