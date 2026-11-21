using UnityEngine;

public class FishVisualController : MonoBehaviour, IFishBehaviour
{
    public FishStats stats;
    public SpriteRenderer sprite;
    public SpriteRenderer highlight;
    public Color noHighlighColor;
    public Color hightlightColor;

    public int Order => 4;

    public void Init(FishStats stats)
    {
        this.stats = stats;
        sprite = GetComponent<SpriteRenderer>();
        highlight = transform.GetChild(0).GetComponent<SpriteRenderer>();

        sprite.sprite = stats.baseFish.sprite;
        highlight.sprite = stats.baseFish.sprite;
    }

    public void UpdateFish()
    {
        if (stats.transform.rotation.eulerAngles.z % 360f > 90f && stats.transform.rotation.eulerAngles.z % 360f < 270f)
        {
            sprite.flipY = true;
            highlight.flipY = true;
        }
        else
        {
            sprite.flipY = false;
            highlight.flipY = false;
        }
    }

    public void Highlight(bool high)
    {
        highlight.color = high ? hightlightColor : noHighlighColor;
    }
}
