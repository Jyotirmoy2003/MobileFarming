using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class SeaControl : MonoBehaviour
{
    //public static SeaControl Instance { get; set; }
    
    [SerializeField] List<Fish> allFish = new List<Fish>();
    public float radius = 5f;
    public float height = 10f;

    private void Awake()
    {
        //Instance = this;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.blue;
        var position = transform.position;
        Gizmos.DrawWireSphere(position, radius);
        Gizmos.DrawLine(new Vector3(position.x, position.y - height / 2, position.z),
            new Vector3(position.x, position.y + height / 2, position.z));
    }


    public Fish GetARandomFishHooked()
    {
        return allFish[Random.Range(0, allFish.Count)];
    }
}
