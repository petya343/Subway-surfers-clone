using UnityEngine;
public interface IBoots
{
    bool BootsActive { get; }
    float BootsJumpForce { get; }
    void PlayBootsJumpEffects();
    int PosUI();
}