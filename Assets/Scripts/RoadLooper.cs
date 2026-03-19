using UnityEngine;
using UnityEngine.UIElements;


public class RoadLooper : MonoBehaviour
{
    [SerializeField] private Transform playerCar; 
    [SerializeField] private Transform[] roadSegments;
    private float[] segmentLengths;                     

    private void Start()
    {
         segmentLengths = new float[roadSegments.Length];

        for (int i = 0; i < roadSegments.Length; i++)
        {
            MeshRenderer mr = roadSegments[i].GetComponentInChildren<MeshRenderer>();
            if (mr != null)
            {
                segmentLengths[i] = mr.bounds.size.z;
            }
            
        }
    }

    private void Update()
    {
        for (int i = 0; i < roadSegments.Length; i++)
        {
            float segLen = segmentLengths[i];

            if (playerCar.position.z - roadSegments[i].position.z > segLen)
            {
                float maxZ = GetMaxZ();
                roadSegments[i].position = new Vector3(
                    roadSegments[i].position.x,
                    roadSegments[i].position.y,
                    maxZ + segLen
                );
            }
        }
    }

    private float GetMaxZ()
    {
        float max = roadSegments[0].position.z;
        foreach (var seg in roadSegments)
        {
            if (seg.position.z > max)
                max = seg.position.z;
        }
        return max;
    }
}


