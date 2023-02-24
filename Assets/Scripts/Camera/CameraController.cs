using UnityEngine;
using System.Collections;

public class CameraController : MonoBehaviour
{

    public Transform target;

    [Range(0.0f, 1.0f)]
    public float smoothSpeed = 0.125f;
    public Vector3 offset;

    private void FixedUpdate()
    {
        Vector3 targetPosition = target.position + offset;
        Vector3 smoothedPosition = Vector3.Lerp(transform.position, targetPosition, smoothSpeed);
        transform.position = smoothedPosition;
    }

    public IEnumerator Shake(float duration, float magnitude, float speed)
    {

        float elapsed = 0.0f;
        float delay = 1 / speed;

        Vector3 targetPosition;

        while (elapsed < duration)
        {
            targetPosition = target.position + offset;

            float x = Random.Range(-1, 1) * magnitude;
            float y = Random.Range(-1, 1) * magnitude;

            transform.localPosition = new Vector3(targetPosition.x + x, targetPosition.y + y, transform.localPosition.z);

            elapsed += delay;

            yield return new WaitForSeconds(delay);
        }
    }
}
