using UnityEngine;
using UnityEngine.UI;

public class StandingIllustUI : MonoBehaviour
{
    [SerializeField] Image StandingIllust;

    void Start()
    {
        StandingIllustContainer standingIllustContainer = new StandingIllustContainer();
        int randInt = Random.Range(0, Constants.HOW_MANY_PLAYERUNIT_EXIST);
        StandingIllust.sprite = standingIllustContainer.StandingIllusts[randInt];
    }
}
