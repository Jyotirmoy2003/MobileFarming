
using jy_util;
using UnityEngine;

public class Sheep : AnimalBase,IInteractable
{
    [Space]
    [Header("Sheep")]
   public MeshDeformer sheepDeformerMesh;
   [SerializeField] ParticleSystem sheepWoolParticel;
   [SerializeField] Transform sheepTargetPos;

    [Header("Camera Settings")]
   [SerializeField] Vector3 cameraBodyOffset,cameraAimOffset;



    void InitializeSheep()
    {
        _GameAssets.Instance.OnSheepModeChangedEvent.Raise(this,true);
        sheepDeformerMesh.OnThresholdReached += OnSuccessfullySearAllWool;

        StartRotation();
    }

    /// <summary>
    /// When user sucessfully sears all the wool from sheep
    /// </summary>
    void OnSuccessfullySearAllWool()
    {
        canInteract = false;
        StopRotation();
        BackToGround();
        

        sheepDeformerMesh.OnThresholdReached -= OnSuccessfullySearAllWool;
        PlayerVisualManager.Instance.SetPlayerRendererShowStatus(true);
        _GameAssets.Instance.OnViewChangeEvent.Raise(this,false);
        CameraManager.Instance.SwitchCamera();
        _GameAssets.Instance.OnSheepModeChangedEvent.Raise(this,false);
        //show particels
        Instantiate(sheepWoolParticel,sheepTargetPos.position,Quaternion.identity);

        sheepDeformerMesh.RestoreMesh();
        sheepDeformerMesh.OnMeshRestored += WoolResotred;
    }

    void BackToGround()
    {
        InventoryManager.Instance.AddItemToInventory(E_Inventory_Item_Type.Wool,3);
        shouldStartMovement = true;
        StartMovemnt();
    }

    void WoolResotred()
    {
        sheepDeformerMesh.OnMeshRestored -= WoolResotred;
        canInteract = true;
    }


    void StartRotation()
    {
        CameraManager.Instance.SwitchCamera(this.transform,cameraBodyOffset,cameraAimOffset);
        CameraManager.Instance.StartRotating(this.transform,this.transform.position);
    }

    void StopRotation()
    {
        CameraManager.Instance.StopRotating();
    }

    public GameObject sheepRenderer; // The object to rotate
    private bool isRotating = false; // Flag to control rotation
    public float rotationSpeed = 100f; // Speed of rotation

   

 










    #region  INTERFACE
    public void InIntreactZone(GameObject interactingObject)
    {
        if(!canInteract) return;

        HapticManager.Instance.LightHaptic();

        PlayerVisualManager.Instance.SetPlayerRendererShowStatus(false);
        StopMovement();
        _GameAssets.Instance.OnViewChangeEvent.Raise(this,true);
        CameraManager.Instance.SwitchCamera(this.transform,cameraBodyOffset,cameraAimOffset);
        Invoke(nameof(InitializeSheep),2f);
    }

    public void Interact(GameObject interactingObject)
    {
       
    }



    public void OutIntreactZone(GameObject interactingObject)
    {
       
    }

    public void ShowInfo(bool val)
    {
        
    }

    #endregion
}
