using System;

[Serializable]
public class StageModel
{
    public int stage_id;
    public string stage_name;
    public int stage_clear_count;
}

[Serializable]
public class StageModelList
{
    public StageModel[] data;
}
