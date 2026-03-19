using UnityEngine;


public class TurretAim : MonoBehaviour
{
    [SerializeField] private Transform turret;
    [SerializeField] private float _rotationSpeed = 10f;

    public void Tick()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out RaycastHit hit))
        {
            Vector3 direction = hit.point - turret.position;
            direction.y = 0;

            if (direction == Vector3.zero)
                return;

            Quaternion targetRotation = Quaternion.LookRotation(direction);

            turret.rotation = Quaternion.RotateTowards(
                turret.rotation,
                targetRotation,
                _rotationSpeed * Time.deltaTime * 100f
            );
        }
    }
}