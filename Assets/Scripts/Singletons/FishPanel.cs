using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class FishPanel : MonoBehaviour
{
    public static FishPanel instance;
    public FishController fish;
    [SerializeField] private GameObject panel;
    [SerializeField] private TextMeshProUGUI fishName;
    [SerializeField] private TextMeshProUGUI fishDescription;
    [SerializeField] private TextMeshProUGUI fishPrice;
    [SerializeField] private Camera fishCam;
    [SerializeField] private Transform fishTraitsRoot;
    [SerializeField] private Transform fishTraitsPrefab;
    public void SetUp(FishController newFish)
    {
        fish = newFish;

        ScreenManager.instance.OpenScreen(panel);
        panel.SetActive(true);

        fishName.text = fish.data.fishName;
        fishDescription.text = fish.data.diet.ToString();
        fishPrice.text = fish.Price.ToString();
        fishCam.orthographicSize = fish.Size;

        for (int i = 0; i < fish.traits.Count; i++)
        {
            Transform trait = fishTraitsRoot;
            if (fishTraitsRoot.childCount > i)
            {
                trait = fishTraitsRoot.GetChild(i);
            }
            else
            {
                trait = Instantiate(fishTraitsPrefab, fishTraitsRoot);
            }
            trait.gameObject.SetActive(true);
            trait.GetComponent<Image>().color = fish.traits[i].traitColor;
            trait.GetChild(0).GetComponent<TextMeshProUGUI>().text = fish.traits[i].traitName;
        }

        if (fishTraitsRoot.childCount > fish.traits.Count)
        {
            for (int i = fish.traits.Count; i < fishTraitsRoot.childCount; i++)
            {
                fishTraitsRoot.GetChild(i).gameObject.SetActive(false);
            }
        }
    }
    private void Awake()
    {
        instance = this;
    }
    private void Start()
    {
        fishCam = InputManager.Instance.transform.GetChild(0).GetComponent<Camera>();
    }
    private void Update()
    {
        if (fish == null)
        {
            return;
        }
        fishCam.transform.position = fish.transform.position + Vector3.back * 1;
    }
}
