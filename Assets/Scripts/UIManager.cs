using System.Collections.Generic;
using TMPro;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [SerializeField] Image healthBar;
    [SerializeField] TextRender targetWord;
    [SerializeField] TextMeshProUGUI heldKeysTxt;

    [Header("Panels")]
    [SerializeField] RectTransform mainPanel;
    [SerializeField] RectTransform ramPanel;
    [SerializeField] RectTransform targetWordPanel;
    [SerializeField] AnimationCurve panelMoveX;
    [SerializeField] AnimationCurve panelMoveY;
    List<RectTransform> panels;
    float panelTimer;

    void Start()
    {
        panels = new() { mainPanel, ramPanel, targetWordPanel };
    }


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
            targetWord.inputText = EnemyManager.targetedEnemy.keysRequirements;
            targetWord.UpdateText();   
        }

        // Show held keys
        heldKeysTxt.text = PlayerInput.Instance.ListToString(PlayerInput.Instance.keysHeld);

        //AnimatePanels();
    }

    void AnimatePanels()
    {
        panelTimer += Time.deltaTime;


    }

    void DecreaseHealth(float normalizedHealth)
    {
        // TODO: health bar tweening
        healthBar.fillAmount = normalizedHealth;
    }
}
