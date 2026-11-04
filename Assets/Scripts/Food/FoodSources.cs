using System.Collections.Generic;

public static class FoodSources
{
    private static readonly List<IFoodSource> all = new List<IFoodSource>();

    public static IReadOnlyList<IFoodSource> All { get { return all; } }

    public static void Register(IFoodSource source) { all.Add(source); }
    public static void Unregister(IFoodSource source) { all.Remove(source); }
}