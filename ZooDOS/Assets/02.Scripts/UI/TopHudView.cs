using UnityEngine;
using TMPro;

public class TopHudView : MonoBehaviour
{
    [Header("참조")]
    [SerializeField] private GameManager gameManager;  
    [SerializeField] private TMP_Text enemyText;      
    [SerializeField] private TMP_Text lifeText;       

    private void OnEnable()                            
    {
        gameManager.OnHudDataChanged += RefreshAll;    
        RefreshAll();                                  
    }

    private void OnDisable()                          
    {
        gameManager.OnHudDataChanged -= RefreshAll;    
    }

 
    private void RefreshAll()
    {
        RefreshEnemyCounter();                       
        RefreshLife();                                
    }

    private void RefreshEnemyCounter()
    {
        int total = gameManager.TotalEnemyCount;   
        int left = gameManager.LeftEnemyCount;    
        int processed = total - left;                 

      

        enemyText.text = processed.ToString() + " / " + total.ToString();
    }

 
    private void RefreshLife()
    {
        int life = gameManager.LeftLifeCount;
       // if (life < 0) life = 0;
        lifeText.text = life.ToString();
    }
}
