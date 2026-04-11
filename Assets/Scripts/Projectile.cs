using UnityEngine;

[RequireComponent(typeof(Collider))]
public class Projectile : MonoBehaviour
{
    [SerializeField] float velocity;
    [HideInInspector] public Vector3 initialDir;
    [HideInInspector] public string initialHeldKeys;

    // Update is called once per frame
    void Update()
    {
        transform.Translate(Time.deltaTime * velocity * initialDir);
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Enemy"))
        {
            other.gameObject.GetComponent<EnemyBehaviour>().Hit(initialHeldKeys);
        }
        Destroy(gameObject);
    }
}