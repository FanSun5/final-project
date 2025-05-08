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
            Debug.LogError("HeldItemDisplayTMP: 没有关联 TextMeshProUGUI 组件！");
    }

    void Update()
    {
        if (player == null || itemText == null) return;

        // 每帧刷新当前手持物品名称
        string heldName = player.GetHeldItemName();
        itemText.text = $"Object:{heldName}";
    }
}
