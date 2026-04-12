using System.Collections.Generic;
using TMPro;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using System.Data;
using Unity.VisualScripting.Antlr3.Runtime.Tree;

public class UIManager : MonoBehaviour
{
    [SerializeField] Image healthBar;
    [SerializeField] TextRender targetWord;
    [SerializeField] TextMeshProUGUI heldKeysTxt;

    [Header("End Panels")]
    [SerializeField] GameObject winPanel;
    [SerializeField] GameObject losePanel;
    [SerializeField] float waitBeforeShowPanel = 2;
    [SerializeField] float waitBeforeCrash = 7;

    [Header("Gameplay Panels")]
    [SerializeField] RectTransform mainPanel;
    [SerializeField] RectTransform mainPanelBg;
    [SerializeField] RectTransform ramPanel;
    [SerializeField] RectTransform targetWordPanel;
    [SerializeField] AnimationCurve panelMoveX;
    [SerializeField] AnimationCurve panelMoveY;
    [SerializeField] float panelMoveStrength;
    List<RectTransform> panels;
    List<Vector2> initialPanelPositions; 
    float panelTimerX;
    float panelTimerY = .75f;

    void Start()
    {
        panels = new() { mainPanel, mainPanelBg, ramPanel, targetWordPanel };
        initialPanelPositions = new() { mainPanel.anchoredPosition, mainPanelBg.anchoredPosition, ramPanel.anchoredPosition, targetWordPanel.anchoredPosition };
    }


    void OnEnable()
    {
        Actions.OnPlayerHit += DecreaseHealth;
        Actions.OnWin += ShowWinMenu;
        Actions.OnPlayerDie += ShowLoseMenu;
    }

    void OnDisable()
    {
        Actions.OnPlayerHit -= DecreaseHealth;
        Actions.OnWin -= ShowWinMenu;
        Actions.OnPlayerDie -= ShowLoseMenu;
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

        AnimatePanels();
    }

    void AnimatePanels()
    {
        panelTimerX += Time.deltaTime;
        panelTimerY += Time.deltaTime;
        if (panelTimerX > panelMoveX.keys[^1].time) panelTimerX -= panelMoveX.keys[^1].time;
        if (panelTimerY > panelMoveY.keys[^1].time) panelTimerY -= panelMoveY.keys[^1].time;

        float offset = 0;

        for (int i = 0; i < panels.Count; i++)
        {
            float offsetTimerX = panelTimerX + offset;
            float offsetTimerY = panelTimerY + offset;
            if (offsetTimerX > panelMoveX.keys[^1].time) offsetTimerX -= panelMoveX.keys[^1].time;
            if (offsetTimerY > panelMoveY.keys[^1].time) offsetTimerY -= panelMoveY.keys[^1].time;

            Vector2 diff = new Vector2(panelMoveX.Evaluate(offsetTimerX), panelMoveY.Evaluate(offsetTimerY)) * panelMoveStrength;
            panels[i].anchoredPosition = initialPanelPositions[i] + diff;

            if (i > 0) offset += .8f;
        }
    }

    void DecreaseHealth(float normalizedHealth)
    {
        // TODO: health bar tweening
        healthBar.fillAmount = normalizedHealth;
    }

    void ShowLoseMenu()
    {
        // Waiting before showing menu
        Sequence loseSequence = DOTween.Sequence();
        loseSequence.AppendInterval(waitBeforeShowPanel);
        loseSequence.OnComplete(() => 
        {
            losePanel.SetActive(true);

            // Crashing after menu is shown
            Sequence crashSequence = DOTween.Sequence();
            crashSequence.AppendInterval(waitBeforeCrash);
            crashSequence.OnComplete(() => { Application.Quit(); });

            #if UNITY_EDITOR
                Debug.Log("Crashing game");
            #endif
        });
    }

    void ShowWinMenu()
    {
        // Waiting before showing menu
        Sequence winSequence = DOTween.Sequence();
        winSequence.AppendInterval(waitBeforeShowPanel);
        winSequence.OnComplete(() => 
        {
            winPanel.SetActive(true);

            // Crashing after menu is shown
            Sequence crashSequence = DOTween.Sequence();
            crashSequence.AppendInterval(waitBeforeCrash);
            crashSequence.OnComplete(() => { Application.Quit(); });

            #if UNITY_EDITOR
                Debug.Log("Crashing game");
            #endif
        });
    }
}
