using UnityEngine;
using TMPro;

public class HeldItemDisplayTMP : MonoBehaviour
{
    
    public playerController player;
   
    public TextMeshProUGUI itemText;

    void Start()
    {
        if (player == null)
            player = FindObjectOfType<playerController>();

        if (itemText == null)
            Debug.LogError("HeldItemDisplayTMP: û�й��� TextMeshProUGUI �����");
    }

    void Update()
    {
        if (player == null || itemText == null) return;

        // ÿ֡ˢ�µ�ǰ�ֳ���Ʒ����
        string heldName = player.GetHeldItemName();
        itemText.text = $"Object:{heldName}";
    }
}
