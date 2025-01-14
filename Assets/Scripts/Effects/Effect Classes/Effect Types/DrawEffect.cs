using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Draw Effect", menuName = "Effects/Effect/Draw")]
public class DrawEffect : Effect
{
    public bool IsDiscardEffect;
    public bool DiscardAll;
    public bool IsMulliganEffect;
    public List<Effect> AdditionalEffects;

    public override void LoadEffect(Effect effect)
    {
        base.LoadEffect(effect);
        var de = effect as DrawEffect;
        IsDiscardEffect = de.IsDiscardEffect;
        DiscardAll = de.DiscardAll;
        IsMulliganEffect = de.IsMulliganEffect;
        AdditionalEffects = de.AdditionalEffects;
    }
}
