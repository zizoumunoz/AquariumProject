using System.Collections.Generic;
using UnityEngine;


[RequireComponent(typeof(FishMovement), typeof(FishFeeding), typeof(FishBreeding))]
[RequireComponent(typeof(FishFlee), typeof(FishWander))]
[RequireComponent(typeof(FishNeeds), typeof(FishGrowth))]
public class FishController : MonoBehaviour
{
    public FishSO data;

    public FishStatus statusMultiplier = new(1);

    private bool initialized;



    [SerializeField] private SpriteRenderer sr;
    [SerializeField] public SpriteRenderer highlight;
    private FishMovement movement;
    private FishFlee flee;
    private FishNeeds needs;
    private FishGrowth growth;

    private IFishBehaviour[] behaviours;

    public List<TraitSO> traits = new List<TraitSO>();


    public SpriteRenderer Visual { get { return sr; } }
    public FishSO Data { get { return data; } }
    public float Size { get { return data.fishStatus.baseSize * statusMultiplier.baseSize * AgeMultiplier; } }
    public int Price { get { return Mathf.RoundToInt(data.price * priceMultiplier * (growth.CurrentAge == Age.Adult ? 2 : 0.5f)); } }
    private float priceMultiplier = 1;
    private float AgeMultiplier { get { return growth.CurrentAge == Age.Young ? data.fishStatus.youngSizeMultiplier * statusMultiplier.youngSizeMultiplier : 1f; } }
    public float CurrentSpeed { get { return data.fishStatus.fishSpeed * statusMultiplier.fishSpeed; } }


    public Age CurrentAge { get { return growth.CurrentAge; } }
    public float GrowTimer { get { return growth.Timer; } }
    public float Happiness { get { return needs.Happiness; } }
    public bool IsHungry { get { return needs.IsHungry; } }
    public bool IsAfraid { get { return flee.IsAfraid; } }
    public float PoopInterval { get { return data.fishStatus.poopInterval * statusMultiplier.poopInterval; } }



    public void Init(FishSO so, List<TraitSO> newTraits = null, float growTimer = -1f)
    {
        data = so;
        FishCollection.Discover(so);    // Calls FishCollection event to add to album
        traits = new List<TraitSO>(newTraits);

        foreach (TraitSO trait in traits)
        {
            statusMultiplier *= trait.statusMultiplier;
            priceMultiplier *= trait.priceMultiplier;
        }

        initialized = true;

        needs.Setup();
        growth.Setup(growTimer != -1 ? data.fishStatus.ageTime * statusMultiplier.ageTime : growTimer);
        sr.sprite = data.sprite;
        highlight.sprite = data.sprite;

        transform.localScale = Vector3.one * Size;
        movement.Setup(sr.sprite);
    }

    public void Feed(float amount)
    {
        needs.Feed(amount);
    }

    private void Awake()
    {
        movement = GetComponent<FishMovement>();
        flee = GetComponent<FishFlee>();
        needs = GetComponent<FishNeeds>();
        growth = GetComponent<FishGrowth>();

        behaviours = new IFishBehaviour[]
        {
            flee,
            GetComponent<FishFeeding>(),
            GetComponent<FishBreeding>(),
            GetComponent<FishWander>(),
        };
    }

    private void Start()
    {
        if (data != null && !initialized) Init(data, traits);
    }

    private void Update()
    {
        for (int i = 0; i < behaviours.Length; i++)
        {
            if (behaviours[i].Tick()) break;
        }

        UpdateIcons();
    }

    private void LateUpdate()
    {
        if (data == null || data.iconRenderer == null || sr == null) return;

        Bounds b = sr.bounds;
        Transform t = data.iconRenderer.transform;
        t.rotation = Quaternion.identity;
        t.position = new Vector3(b.center.x, b.max.y + data.iconPadding, transform.position.z);
        t.localScale = Vector3.one * (data.iconSize / Size);
    }

    private void UpdateIcons()
    {
        if (data.iconRenderer == null) return;
        if (data.statusIcons == null)
        {
            Debug.LogWarning("missing " + name);
            return;
        }

        bool afraid = IsAfraid;
        bool hungry = IsHungry;

        if (afraid) data.iconRenderer.sprite = data.statusIcons.fear;
        else if (hungry) data.iconRenderer.sprite = data.statusIcons.hunger;

        data.iconRenderer.enabled = afraid || hungry;
    }
}