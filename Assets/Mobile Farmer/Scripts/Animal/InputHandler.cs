using UnityEngine;

public class InputHandler : MonoBehaviour
{
    [SerializeField] LayerMask deformLayer;
   public Camera mainCamera;
    public MeshDeformer meshDeformer;

    void Update()
    {
        if (Input.GetMouseButton(0)) // Detects mouse click or touch
        {
            Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out RaycastHit hit,Mathf.Infinity,deformLayer))
            {
                Vector3 cameraDirection = mainCamera.transform.forward; // Get camera direction
                meshDeformer.DeformMesh(hit.point);
                //  Debug.Log("Mesh Deforming");
            }
        }
    }


    public void ListentoOnSheepModeChange(Component sender, object data)
    {
        if((bool)data && sender is Sheep)
        {
            meshDeformer = (sender as Sheep).sheepDeformerMesh;
            this.enabled = true;
        }else{
            this.enabled = false;
        }
    }
    
}
