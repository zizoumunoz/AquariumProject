using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class FishPanel : MonoBehaviour
{
    public static FishPanel instance;
    public FishController fish;
    [SerializeField] private PanelAnimator panel;
    [SerializeField] private TextMeshProUGUI fishName;
    [SerializeField] private TextMeshProUGUI fishDescription;
    [SerializeField] private TextMeshProUGUI fishDiet;
    [SerializeField] private TextMeshProUGUI fishPrice;
    [SerializeField] private Slider fishHappiness;
    [SerializeField] private Slider fishHunger;
    [SerializeField] private Camera fishCam;
    [SerializeField] private Transform fishTraitsRoot;
    [SerializeField] private Transform fishTraitsPrefab;
    public void SetUp(FishController newFish)
    {
        fish = newFish;

        if (!panel.gameObject.activeSelf)
        {
            ScreenManager.instance.OpenScreen(panel);
        }

        fishName.text = fish.data.fishName;
        fishDescription.text = fish.data.description;
        fishDiet.text = fish.data.diet.ToString();
        fishPrice.text = fish.Price.ToString();

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
            Color c = fish.traits[i].traitColor;
            trait.GetComponent<Image>().color = c;
            TextMeshProUGUI text = trait.GetChild(0).GetComponent<TextMeshProUGUI>();
            text.text = fish.traits[i].traitName;
            text.color = ((c.r * 0.299f + c.g * 0.587f + c.b * 0.114f) > 0.5f) ? Color.black : Color.white;
            Debug.Log(text.text);
        }

        for (int i = fish.traits.Count; i < fishTraitsRoot.childCount; i++)
        {
            fishTraitsRoot.GetChild(i).gameObject.SetActive(false);
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
            if (panel.gameObject.activeSelf)
            {
                ScreenManager.instance.EmptyStack();
            }
            return;
        }
        fishCam.transform.position = fish.transform.position + Vector3.back * 1;
        fishCam.orthographicSize = fish.Size;
        fishHappiness.value = fish.Happiness;
        fishHunger.value = fish.Fullness;
    }
}
