using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [SerializeField] Image healthBar;
    [SerializeField] TextMeshProUGUI targetWord;
    [SerializeField] TextMeshProUGUI heldKeysTxt;

    [Header("Panels")]
    [SerializeField] RectTransform mainPanel;
    [SerializeField] RectTransform ramPanel;
    [SerializeField] RectTransform targetWordPanel;

    void OnEnable()
    {
        Actions.OnPlayerHit += DecreaseHealth;
    }

    void OnDisable()
    {
        Actions.OnPlayerHit -= DecreaseHealth;
    }

    // Update is called once per frame
    void Update()
    {
        // Show targeted enemy requirements
        if (EnemyManager.targetedEnemy)
        {
            targetWord.text = EnemyManager.targetedEnemy.keysRequirements;          
        }

        // Show held keys
        heldKeysTxt.text = PlayerInput.Instance.ListToString(PlayerInput.Instance.keysHeld);
    }

    void DecreaseHealth(float normalizedHealth)
    {
        // TODO: health bar tweening
        healthBar.fillAmount = normalizedHealth;
    }
}
