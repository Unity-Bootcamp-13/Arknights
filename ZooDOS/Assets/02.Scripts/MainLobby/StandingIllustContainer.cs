using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class StandingIllustContainer
{
    public List<Sprite> StandingIllusts { get; }

    static private string path = "MainLobby";

    public StandingIllustContainer()
    {
        StandingIllusts = new List<Sprite>();

        for(int i =1; i<=Constants.HOW_MANY_PLAYERUNIT_EXIST; i++)
        {
            Sprite sprite = Resources.Load<Sprite>(Path.Combine(path, $"Standing{i.ToString()}"));
            StandingIllusts.Add(sprite);
        }
    }
}