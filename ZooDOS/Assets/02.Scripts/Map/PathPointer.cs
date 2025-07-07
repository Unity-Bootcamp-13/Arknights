using System.Collections.Generic;
using UnityEngine;

public class PathPointer : EnemyUnit
{
    
    void Update()
    {
        MoveStep();
    }
    
    /// <summary>
    /// 다음 경로를 향해 이동
    /// </summary>
    protected override void MoveStep()
    {
        Vector3 current = transform.position;
        Vector3 target =_path[currentPathIndex + 1];
        
        transform.position = Vector3.MoveTowards(current, target, EnemyUnitData.MoveSpeed * Time.deltaTime);

        if (Vector3.Distance(current, target) < 0.05f)
        {
            currentPathIndex++;
            if (currentPathIndex >= _path.Count - 1)
            {
                OnArriveAtEnd();
            }
        }
    }

    public override void Init(List<Vector3> path)
    {
        _path = path;
        currentPathIndex = 0;
        transform.position = _path[0]; 
    }

    protected override void OnArriveAtEnd()
    {
        Destroy(gameObject);
    }
}
