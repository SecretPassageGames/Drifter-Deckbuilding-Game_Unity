using UnityEngine;

[CreateAssetMenu(fileName = "New GiveNextUnit Effect", menuName = "Effects/GiveNextUnit")]
public class GiveNextUnitEffect : Effect
{
    public Effect[] Effects;
    public int Multiplier;

    public override void LoadEffect(Effect effect)
    {
        base.LoadEffect(effect);
        GiveNextUnitEffect gnfe = effect as GiveNextUnitEffect;
        Effects = gnfe.Effects;
        Multiplier = gnfe.Multiplier;
    }
}
