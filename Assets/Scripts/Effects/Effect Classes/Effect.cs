using UnityEngine;

public abstract class Effect : ScriptableObject
{
    [Tooltip("The effect is required by the effect group")]
    public bool IsRequired;

    [Tooltip("The value of the effect (1-10)")]
    [Range(1, 10)]
    public int Value;

    [Tooltip("The number of turns the effect lasts, 0 for permanent")]
    [Range(0, 5)]
    public int Countdown;

    [Header("Target Description")]
    [Tooltip("A description of the target")]
    [TextArea]
    public string TargetDescription;

    [Header("Effect Sound")]
    [Tooltip("The sound played when this effect resolves")]
    public string EffectSound;

    public virtual void LoadEffect(Effect effect)
    {
        IsRequired = effect.IsRequired;
        Value = effect.Value;
        Countdown = effect.Countdown;
        TargetDescription = effect.TargetDescription;
    }
}
