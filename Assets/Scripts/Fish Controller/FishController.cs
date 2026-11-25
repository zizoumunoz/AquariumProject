using System.Collections.Generic;
using UnityEngine;


[RequireComponent(typeof(FishMovement), typeof(FishFeeding), typeof(FishBreeding))]
[RequireComponent(typeof(FishFlee), typeof(FishWander))]
[RequireComponent(typeof(FishNeeds), typeof(FishGrowth))]
public class FishController : MonoBehaviour
{
    public FishSO data;
    public FishStatus statusMultiplier = new(1);

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
    public float Fullness { get { return needs.Fullness; } }
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

        needs.Setup();
        growth.Setup(growTimer == -1 ? data.fishStatus.ageTime * statusMultiplier.ageTime : growTimer);
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

    private void Update()
    {
        for (int i = 0; i < behaviours.Length; i++)
        {
            if (behaviours[i].Tick()) break;
        }
    }
}