using UnityEngine;

public class SpawnArea : MonoBehaviour
{
    [SerializeField] private Color GizmosColor = new Color(0.5f, 0.5f, 0.5f, 0.2f);
    [SerializeField] private float radius;
    
    void OnDrawGizmos()
    {
        Gizmos.color = GizmosColor;
        Gizmos.DrawSphere(transform.position, radius);
    }

    public Vector3 GetRandomPoint()
    {
        float a = Random.value * 2 * Mathf.PI;
        float r = (transform.localScale.x / 2.0f) * Mathf.Sqrt(Random.value);

        Vector3 randomPoint = new Vector3(r * Mathf.Cos(a), 0, r * Mathf.Sin(a));
        
        return transform.position + randomPoint;
    }
}
