using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class FishPanel : MonoBehaviour
{
    public static FishPanel instance;
    public FishStats fishStats;
    [SerializeField] private GameObject panel;
    [SerializeField] private TextMeshProUGUI fishName;
    [SerializeField] private TextMeshProUGUI fishDescription;
    [SerializeField] private TextMeshProUGUI fishPrice;
    [SerializeField] private Camera fishCam;
    [SerializeField] private Transform fishTraitsRoot;
    [SerializeField] private Transform fishTraitsPrefab;
    public void SetUp(FishStats newFish)
    {
        fishStats = newFish;

        ScreenManager.instance.OpenScreen(panel);
        panel.SetActive(true);

        fishName.text = fishStats.baseFish.fishName;
        fishDescription.text = fishStats.baseFish.diet.ToString();
        fishPrice.text = fishStats.baseFish.price.ToString();
        fishCam.orthographicSize = fishStats[StatsEnum.baseSize];

        for (int i = 0; i < fishStats.traits.Count; i++)
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
            Color c = fishStats.traits[i].traitColor;
            trait.GetComponent<Image>().color = fishStats.traits[i].traitColor;
            TextMeshProUGUI text = trait.GetChild(0).GetComponent<TextMeshProUGUI>();
            text.text = fishStats.traits[i].traitName;
            text.color = ((c.r * 0.299f + c.g * 0.587f + c.b * 0.114f) > 0.5f) ? Color.black : Color.white;
        }

        if (fishTraitsRoot.childCount > fishStats.traits.Count)
        {
            for (int i = fishStats.traits.Count; i < fishTraitsRoot.childCount; i++)
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
        if (fishStats == null)
        {
            return;
        }
        fishCam.transform.position = fishStats.transform.position + Vector3.back * 1;
    }
}
