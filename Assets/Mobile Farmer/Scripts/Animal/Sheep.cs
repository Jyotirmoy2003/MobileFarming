
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

        LeanTween.move(sheepRenderer,sheepTargetPos, 2f).setOnComplete(StartRotation);
    }

    /// <summary>
    /// When user sucessfully sears all the wool from sheep
    /// </summary>
    void OnSuccessfullySearAllWool()
    {
        canInteract = false;
        RotateObject(false);
        LeanTween.move(sheepRenderer,transform, 2f).setOnComplete(BackToGround);
        LeanTween.rotate(sheepRenderer,Vector3.zero,2f);

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
        RotateObject(true);
    }

    void StopRotation()
    {
        RotateObject(false);
    }

    public GameObject sheepRenderer; // The object to rotate
    private bool isRotating = false; // Flag to control rotation
    public float rotationSpeed = 100f; // Speed of rotation

    void Update()
    {
        if (isRotating && sheepRenderer != null)
        {
            sheepRenderer.transform.Rotate(0, rotationSpeed * Time.deltaTime, 0);
        }
    }

    public void RotateObject(bool shouldRotate)
    {
        isRotating = shouldRotate;
    }











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
