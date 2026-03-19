using UnityEngine;
using UnityEngine.UIElements;


public class RoadLooper : MonoBehaviour
{
    [SerializeField] private Transform playerCar;       // car
    [SerializeField] private Transform[] roadSegments;  // segment prefabs
    private float[] segmentLengths;                     // length of each segment

    private void Start()
    {
        //Determine the length of each segment automatically
         segmentLengths = new float[roadSegments.Length];

        for (int i = 0; i < roadSegments.Length; i++)
        {
            MeshRenderer mr = roadSegments[i].GetComponentInChildren<MeshRenderer>();
            if (mr != null)
            {
                segmentLengths[i] = mr.bounds.size.z; // actual length of the segment in Z
            }
            //else
            //{
            //    Debug.LogWarning("MeshRenderer не знайдено у сегменті: " + roadSegments[i].name);
            //    segmentLengths[i] = 20f; // запасне значення
            //}
        }
    }

    private void Update()
    {
        for (int i = 0; i < roadSegments.Length; i++)
        {
            float segLen = segmentLengths[i];

            // if the segment is behind the car → move it forward
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

    // returns the most distant segment in Z
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


