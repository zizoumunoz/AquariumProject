using UnityEngine;

public class Food : MonoBehaviour
{
    [SerializeField] private FoodSO data;

    public float FullnessRestore { get { return data.fullnessRestore; } }

    public void Init(FoodSO so)
    {
        data = so;
        SpriteRenderer sr = GetComponent<SpriteRenderer>();
        if (sr != null) sr.sprite = data.sprite;
    }

    private void Start()
    {
        if (data != null) Init(data);
    }
}