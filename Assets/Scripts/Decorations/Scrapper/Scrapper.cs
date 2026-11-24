    using UnityEngine;

public class Scrapper : MonoBehaviour
{
    public float size;
    public void Start()
    {
        float totalAlgea = AlgaeVisuals.instance.transform.childCount;
        float newTotalAlgea = AlgaeVisuals.instance.transform.childCount;

        for (int i = 0; i < totalAlgea; i++)
        {
            if (AlgaeVisuals.instance.transform.GetChild(i).position.x > transform.position.x - 0.5f * size &&
                AlgaeVisuals.instance.transform.GetChild(i).position.x < transform.position.x + 0.5f * size)
            {
                Destroy(AlgaeVisuals.instance.transform.GetChild(i).gameObject);
                newTotalAlgea--;
            }
        }


        TankManager.instance.database.activeTank.currentAttibutes.moldLevel
            = newTotalAlgea / AlgaeVisuals.instance._maxSpots * 100;

        Destroy(gameObject);
    }
}
