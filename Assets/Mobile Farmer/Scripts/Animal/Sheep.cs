
using jy_util;
using UnityEngine;

public class Sheep : AnimalBase,IInteractable
{
    [Space]
    [Header("Sheep")]
   public MeshDeformer sheepDeformerMesh;
   [SerializeField] ButtonInfo buttonInfo;

   [SerializeField] Vector3 cameraBodyOffset,cameraAimOffset;



    void InitializeSheep()
    {
        _GameAssets.Instance.OnSheepModeChangedEvent.Raise(this,true);
        sheepDeformerMesh.OnThresholdReached += OnSuccessfullySearAllWool;
    }

    /// <summary>
    /// When user sucessfully sears all the wool from sheep
    /// </summary>
    void OnSuccessfullySearAllWool()
    {
        sheepDeformerMesh.OnThresholdReached -= OnSuccessfullySearAllWool;
        _GameAssets.Instance.OnViewChangeEvent.Raise(this,false);
        CameraManager.Instance.SwitchCamera();
        _GameAssets.Instance.OnShakeInitiateEvent.Raise(this,false);
    }












    #region  INTERFACE
    public void InIntreactZone(GameObject interactingObject)
    {
        UIManager.Instance.SetupIntreactButton(buttonInfo,true);
    }

    public void Interact(GameObject interactingObject)
    {
        StopMovement();
        _GameAssets.Instance.OnViewChangeEvent.Raise(this,true);
        CameraManager.Instance.SwitchCamera(this.transform,cameraBodyOffset,cameraAimOffset);
        Invoke(nameof(InitializeSheep),2f);
    }



    public void OutIntreactZone(GameObject interactingObject)
    {
        UIManager.Instance.SetupIntreactButton(buttonInfo,true);
    }

    public void ShowInfo(bool val)
    {
        
    }

    #endregion
}
