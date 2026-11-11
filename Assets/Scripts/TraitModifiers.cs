using System.Collections.Generic;

public class TraitModifiers
{
    public float Size { get; private set; } = 1f;
    public float Speed { get; private set; } = 1f;
    public float TurnSpeed { get; private set; } = 1f;
    public float HungerDecay { get; private set; } = 1f;
    public float Price { get; private set; } = 1f;

    public TraitModifiers(IEnumerable<TraitSO> traits)
    {
        if (traits == null) return;

        foreach (TraitSO trait in traits)
        {
            if (trait == null) continue;

            Size *= trait.sizeMultiplier;
            Speed *= trait.speedMultiplier;
            TurnSpeed *= trait.turnSpeedMultiplier;
            HungerDecay *= trait.hungerDecayMultiplier;
            Price *= trait.priceMultiplier;
        }
    }
}