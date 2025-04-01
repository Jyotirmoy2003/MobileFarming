
using System;
using UnityEngine;

public class MeshDeformer : MonoBehaviour
{
   public float deformRadius = 1f;
    public float deformStrength = 0.5f;
    public float deformationThreshold = 0.8f; // 80% threshold
    public Action OnThresholdReached; // Event when threshold is met

    [SerializeField]private Mesh mesh;
    private Vector3[] originalVertices;
    private Vector3[] modifiedVertices;
    private int deformedCount = 0;

    void Start()
    {
        mesh = GetComponent<MeshFilter>().mesh;
        originalVertices = mesh.vertices;
        modifiedVertices = mesh.vertices;
    }

    public void DeformMesh(Vector3 hitPoint, Vector3 cameraDirection)
    {
        int affectedCount = 0;

        for (int i = 0; i < modifiedVertices.Length; i++)
        {
            Vector3 worldPos = transform.TransformPoint(originalVertices[i]); // Convert local to world space
            float distance = Vector3.Distance(worldPos, hitPoint);

            if (distance < deformRadius)
            {
                float deformation = Mathf.Lerp(deformStrength, 0, distance / deformRadius);
                Vector3 localDeformation = transform.InverseTransformDirection(cameraDirection.normalized * deformation); // Convert back to local

                modifiedVertices[i] += localDeformation; // Apply deformation in local space
                affectedCount++;
            }
        }

        mesh.vertices = modifiedVertices;
        mesh.RecalculateNormals();

        // Threshold check
        deformedCount += affectedCount;
        float deformedRatio = (float)deformedCount / modifiedVertices.Length;
        if (deformedRatio >= deformationThreshold)
        {
            OnThresholdReached?.Invoke();
        }
        Debug.Log("Deform ratio: "+deformedRatio);
    }
}