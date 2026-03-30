using System.Collections;
using UnityEngine;
using UnityEngine.UIElements;
using Zenject;

public class RoadLooper : MonoBehaviour
{
    [Inject] private EndGamePresenter _presenter;

    [SerializeField] private Transform playerCar;
    [SerializeField] private Transform[] roadSegments;
    [SerializeField] private CarMovement _carMovement;

    [Header("Level Settings")]
    [SerializeField] private int maxLoops = 3;

    private float[] segmentLengths;
    private int _currentLoops;
    private bool _isFinished;

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
        if (_isFinished)
            return;

        for (int i = 0; i < roadSegments.Length; i++)
        {
            float segLen = segmentLengths[i];

            if (playerCar.position.z - roadSegments[i].position.z > segLen * 1.1f)
            {
                if (_currentLoops >= maxLoops)
                {
                    _isFinished = true;

                    StartCoroutine(StopAfterLastSegment());

                    return;
                }

                float maxZ = GetMaxZ();

                var segment = roadSegments[i];
                SetRenderers(segment, false);

                segment.position = new Vector3(
                    segment.position.x,
                    segment.position.y,
                    maxZ + segLen - 0.01f
                );
                SetRenderers(segment, true);

                _currentLoops++;
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

    private IEnumerator StopAfterLastSegment()
    {
        yield return new WaitForSeconds(75f / 5f);

        _carMovement.StopMove();
        _presenter.Win();
        Time.timeScale = 0f;
    }

    private void SetRenderers(Transform segment, bool state)
    {
        var renderers = segment.GetComponentsInChildren<MeshRenderer>();

        foreach (var r in renderers)
            r.enabled = state;
    }

    public float GetProgress()
    {
        float totalLength = (2 + maxLoops) * 75f;

        float currentZ = playerCar.position.z;

        return Mathf.Clamp01(currentZ / totalLength);
    }
}