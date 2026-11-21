using UnityEngine;

public class DietFoodConversion
{
    public static bool CanEat(FishStats stats, FoodSource source)
    {
        if (stats[StatsEnum.baseSize] < source.size)
        {
            return false;
        }
        if (!source.IsAvailable())
        {
            return false;
        }
        switch (stats.diet)
        {
            case FishDiet.Herbivore:
                {
                    if (source.type == FoodType.Plant)
                    {
                        return true;
                    }
                }
                break;
            case FishDiet.Carnivore:
                {
                    if (source.type == FoodType.Meat)
                    {
                        return true;
                    }
                }
                break;
            case FishDiet.Omnivore:
                {
                    if (source.type == FoodType.Plant || source.type == FoodType.Meat)
                    {
                        return true;
                    }
                }
                break;
            default:
                break;
        }

        return false;
    }
}
