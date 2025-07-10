using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "StageTileInfoSO", menuName = "Scriptable Objects/StageTiles")]
public class StageTileInfoSO : ScriptableObject
{
    [SerializeField] private int mapWidth; 
    [SerializeField] private int mapHeight; 
    
    [SerializeField] private List<TileType> tileLayout;

    public int MapWidth => mapWidth;
    public int MapHeight => mapHeight;
    public List<TileType> TileLayout => tileLayout;

}

