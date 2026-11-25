using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SlotPanel : MonoBehaviour
{
    public SlotSerialize[] slots = new SlotSerialize[3];
    private void OnEnable()
    {
        CheckSave();
    }
    public void CheckSave()
    {
        for (int i = 0; i < slots.Length; i++)
        {
            slots[i].slotNumber.text = $"Slot {i + 1}";
            if (!SaveAndLoad.ExistSlot(i))
            {
                slots[i].info.SetActive(false);
                slots[i].deleteSave.interactable = false;
                continue;
            }

            SaveAndLoad.LoadData(i);
            slots[i].info.SetActive(true);

            SaveAndLoad.SaveFile save = SaveAndLoad.save;

            slots[i].moneyNumber.text = $"${save.money}";
            slots[i].tankNumber.text = $"{save.tankList.Count}";

            int fishCount = 0;
            int decoCount = 0;
            for (int j = 0; j < save.tankList.Count; j++)
            {
                fishCount += save.tankList[j].fishes.Count;
                decoCount += save.tankList[j].decorations.Count;
            }

            slots[i].fishNumber.text = $"{fishCount}";
            slots[i].decoNumber.text = $"{decoCount}";
            slots[i].deleteSave.interactable = true;
        }
    }
    public void DeleteSlot(int slot)
    {
        SaveAndLoad.DeleteData(slot);
        CheckSave();
    }
    public void EnterSlot(int slot)
    {
        SaveAndLoad.slot = slot;
        if (SaveAndLoad.ExistSlot(slot))
        {
            SceneFlow.LoadGame();
        }
        else
        {
            SceneFlow.NewGame();
        }
    }
}

[Serializable]
public class SlotSerialize
{
    public TextMeshProUGUI slotNumber;
    public GameObject info;
    public TextMeshProUGUI moneyNumber;
    public TextMeshProUGUI tankNumber;
    public TextMeshProUGUI fishNumber;
    public TextMeshProUGUI decoNumber;
    public Button deleteSave;
    public Button enterSave;
}