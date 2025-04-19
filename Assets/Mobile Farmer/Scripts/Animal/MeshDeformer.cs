using System;
using System.Collections;
using UnityEngine;

public class MeshDeformer : MonoBehaviour
{
    public float deformRadius = 1f;
    public float deformStrength = 0.5f;
    public float deformationThreshold = 0.8f;
    public Action OnThresholdReached;
    public Action OnMeshRestored;

    [SerializeField] private Mesh mesh;
    private Vector3[] originalVertices;
    private Vector3[] modifiedVertices;
    private int deformedCount = 0;
    private bool isRestoring;
    [SerializeField] float restoreSpeed = 1f;

    [Header("Deform Settings")]
    public Transform shrinkTarget; // NEW: target transform to shrink toward

    void Start()
    {
        mesh = GetComponent<MeshFilter>().mesh;
        originalVertices = mesh.vertices;
        modifiedVertices = mesh.vertices;
    }

    public void DeformMesh(Vector3 hitPoint)
    {
        if (shrinkTarget == null) return;

        int affectedCount = 0;
        Vector3 targetLocal = transform.InverseTransformPoint(shrinkTarget.position); // Convert target to local space

        for (int i = 0; i < modifiedVertices.Length; i++)
        {
            Vector3 worldPos = transform.TransformPoint(originalVertices[i]);
            float distance = Vector3.Distance(worldPos, hitPoint);

            if (distance < deformRadius)
            {
                float deformation = Mathf.Lerp(deformStrength, 0, distance / deformRadius);
                Vector3 directionToTarget = (targetLocal - modifiedVertices[i]).normalized;

                modifiedVertices[i] += directionToTarget * deformation;
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

       
    }

    public void RestoreMesh()
    {
        if (!isRestoring)
        {
            StartCoroutine(RestoreVerticesGradually());
        }
    }

    private IEnumerator RestoreVerticesGradually()
    {
        isRestoring = true;
        float progress = 0f;

        Vector3[] startVertices = new Vector3[modifiedVertices.Length];
        Array.Copy(modifiedVertices, startVertices, modifiedVertices.Length);

        while (progress < 1f)
        {
            progress += Time.deltaTime * restoreSpeed;
            progress = Mathf.Clamp01(progress);

            for (int i = 0; i < modifiedVertices.Length; i++)
            {
                modifiedVertices[i] = Vector3.Lerp(startVertices[i], originalVertices[i], progress);
            }

            mesh.vertices = modifiedVertices;
            mesh.RecalculateNormals();
            yield return null;
        }

        isRestoring = false;
        OnMeshRestored?.Invoke();
        deformedCount = 0;
    }

}
