using NaughtyAttributes;
using UnityEngine;

public class UISimpleShake : MonoBehaviour
{
    public float shakeInterval = 0.5f;
    public float range;
    
    private Vector2 _basePosition;
    private float _timer = 0f;

    void Start()
    {
        _basePosition = transform.position;
    }
    void Update()
    {
        _timer += Time.deltaTime;
        if (_timer > shakeInterval)
        {
            _timer = 0;
            transform.position = _basePosition + new Vector2(Random.Range(-range, range), Random.Range(-range, range));
        }
    }
}
