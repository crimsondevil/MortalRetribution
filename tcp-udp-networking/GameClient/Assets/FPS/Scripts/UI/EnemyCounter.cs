﻿using UnityEngine;
using UnityEngine.UI;

public class EnemyCounter : MonoBehaviour
{
    [Header("Enemies")]
    [Tooltip("Text component for displaying enemy objective progress")]
    public Text enemiesText;

    FPsEnemyManager m_EnemyManager;

    void Awake()
    {
        m_EnemyManager = FindObjectOfType<FPsEnemyManager>();
        DebugUtility.HandleErrorIfNullFindObject<EnemyManager, EnemyCounter>(m_EnemyManager, this);
    }

    void Update()
    {
        enemiesText.text = m_EnemyManager.numberOfEnemiesRemaining + "/" + m_EnemyManager.numberOfEnemiesTotal;
    }
}