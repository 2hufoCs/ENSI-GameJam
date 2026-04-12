using NaughtyAttributes;
using UnityEngine;

public class UISimpleShake : MonoBehaviour
{
    public float shakeInterval = 0.5f;
    public float range;
    
    public Vector2 basePosition;
    private float _timer = 0f;

    void Start()
    {
        if (basePosition == Vector2.zero)
        {
            basePosition = transform.localPosition;
        }
    }
    void Update()
    {
        _timer += Time.deltaTime;
        if (_timer > shakeInterval)
        {
            _timer = 0;
            transform.localPosition = basePosition + new Vector2(Random.Range(-range, range), Random.Range(-range, range));
        }
    }
}
