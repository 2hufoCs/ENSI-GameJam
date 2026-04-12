using System;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class Projectile : MonoBehaviour
{
    [SerializeField] float velocity;
    [SerializeField] SpriteRenderer spriteRenderer;
    [SerializeField] Sprite[] spriteSheet;
    [SerializeField] float interval;
    float intervalTimer;

    [HideInInspector] public Vector3 initialDir;
    [HideInInspector] public string initialHeldKeys;
    bool destroy;

    // Update is called once per frame
    void Update()
    {
        if (destroy)
        {
            intervalTimer += Time.deltaTime;

            if (intervalTimer > interval)
            {
                if (Array.IndexOf(spriteSheet, spriteRenderer.sprite) == spriteSheet.Length - 1)
                {
                    Destroy(gameObject);
                    return;
                }

                intervalTimer = 0;
                spriteRenderer.sprite = spriteSheet[Array.IndexOf(spriteSheet, spriteRenderer.sprite) + 1];
            }
            return;
        }

        transform.Translate(Time.deltaTime * velocity * initialDir);

        if (spriteRenderer.sprite == spriteSheet[3]) return;

        // Projectile gets bigger
        intervalTimer += Time.deltaTime;
        if (intervalTimer > interval)
        {
            intervalTimer = 0;
            int newSpriteIndex = Array.IndexOf(spriteSheet, spriteRenderer.sprite) + 1;
            spriteRenderer.sprite = newSpriteIndex == 2 ? spriteSheet[newSpriteIndex + 1] : spriteSheet[newSpriteIndex];
        }
    }

    void Start()
    {
        spriteRenderer.sprite = null;      
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Enemy"))
        {
            other.gameObject.GetComponent<EnemyBehaviour>().Hit(initialHeldKeys);
        }
        intervalTimer = 0;
        destroy = true;
    }
}