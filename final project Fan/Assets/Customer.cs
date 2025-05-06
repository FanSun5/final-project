using UnityEngine;
using UnityEngine.UI;

public class Customer : MonoBehaviour
{
    
    public ItemType[] possibleOrders = {
        ItemType.IcedTea,
        ItemType.LemonTea,
        ItemType.BobaMilkTea,
    };

    
    public Sprite[] iconSprites;

    
    public Image iconImage;

    
    public ItemType orderType;  // ����ʱ���ѡ���Ķ���

    void Start()
    {
        // 1. ����� possibleOrders ��ѡһ��
        if (possibleOrders == null || possibleOrders.Length == 0)
        {
            Debug.LogError("Customer: possibleOrders ���鲻��Ϊ�գ�");
            return;
        }
        int idx = Random.Range(0, possibleOrders.Length);
        orderType = possibleOrders[idx];
        Debug.Log("�¹˿͵㵥: " + orderType);

        // 2. ��ͷ��ͼ�긳ֵ
        if (iconSprites == null || iconSprites.Length <= idx)
        {
            Debug.LogWarning("Customer: iconSprites ���鳤�Ȳ��㣬�޷���ʾͼ��");
        }
        else if (iconImage == null)
        {
            Debug.LogWarning("Customer: δ�� iconImage���޷���ʾͼ��");
        }
        else
        {
            iconImage.sprite = iconSprites[idx];
        }
    }
}
