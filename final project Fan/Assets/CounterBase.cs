using UnityEngine;

public class CounterBase : MonoBehaviour
{
    public Transform holdPoint;

    // ��� holdPoint ���Ѿ��������壬����Ϊ��̨��ռ
    public bool HasItem => holdPoint != null && holdPoint.childCount > 0;
}
