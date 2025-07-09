using UnityEngine;
using UnityEditor;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

public class WaveDataGenerator : MonoBehaviour
{
    [MenuItem("Tools/Generate WaveData From CSV")]
    public static void GenerateWaveDataFromCSV()
    {
        string csvPath = EditorUtility.OpenFilePanel("Select Wave CSV", Application.dataPath, "csv");
        if (string.IsNullOrEmpty(csvPath)) return;

        string[] lines = File.ReadAllLines(csvPath);
        if (lines.Length <= 1)
        {
            Debug.LogError("CSV 파일이 비어있거나 잘못됨");
            return;
        }

        List<EnemySpawnData> spawnDataList = new();

        for (int i = 1; i < lines.Length; i++) // 첫 줄은 헤더
        {
            string line = lines[i];
            string[] tokens = ParseCsvLine(line);

            if (tokens.Length < 3) {
                Debug.LogWarning($"Line {i+1} is invalid: {line}");
                continue;
            }

            string prefabPath = tokens[0].Trim('"');
            float spawnTime = float.Parse(tokens[1]);
            string[] pathTokens = tokens[2].Trim('"').Split(';');

            List<Position> path = new();
            foreach (string p in pathTokens)
            {
                Debug.Log($"p : {p}  ");
                
                string trimmed = p.Trim('(', ')');
                string[] coords = trimmed.Split(',');
                Debug.Log($"coords : {coords[0]}  ");
                Debug.Log($"coords : {coords[1]}  ");
                int x = int.Parse(coords[0]);
                int y = int.Parse(coords[1]);
                path.Add(new Position(x, y));
            }

            GameObject prefab = AssetDatabase.LoadAssetAtPath<GameObject>("Assets/" + prefabPath + ".prefab");
            if (prefab == null)
            {
                Debug.LogError($"프리팹 경로 오류: {prefabPath}");
                continue;
            }

            EnemySpawnData data = new EnemySpawnData
            {
                enemyPrefab = prefab,
                spawnTime = spawnTime,
                path = path
            };

            spawnDataList.Add(data);
        }

        WaveData waveData = ScriptableObject.CreateInstance<WaveData>();

        // Init 호출
        waveData.Init(spawnDataList);
        // 저장
        string assetPath = "Assets/WaveDataGeneratedSO.asset";
        AssetDatabase.CreateAsset(waveData, assetPath);
        AssetDatabase.SaveAssets();

        Debug.Log("WaveData 생성 완료: " + assetPath);
    }

    static string[] ParseCsvLine(string line)
    {
        var pattern = ",(?=(?:[^\"]*\"[^\"]*\")*[^\"]*$)";
        return Regex.Split(line, pattern).Select(s => s.Trim('"')).ToArray();
    }
}
