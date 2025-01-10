using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class _GameAssets : MonoSingleton<_GameAssets>
{
    [Header("Particel System")]
    public ParticleSystem seedParticel;
    public ParticleSystem waterParticel;

    [Header("Player Components")]
    public PlayerAnimator playerAnimator;
    public PlayerToolSelector playerToolSelector;

    [Header("3D object Ref")]
    public GameObject wateringCan;

    [Header("Material Ref")]
    public Material defaultCropTileMat;
    public Material wateredCropTileMat;
}
