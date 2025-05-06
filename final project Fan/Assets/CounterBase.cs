using UnityEngine;

public class CounterBase : MonoBehaviour
{
    public Transform holdPoint;

    // 如果 holdPoint 下已经有子物体，就认为柜台被占
    public bool HasItem => holdPoint != null && holdPoint.childCount > 0;
}
