using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "QuestData", menuName = "QestData", order = 2)]
public class QuestData : ScriptableObject
{
    public List<TaskData> taskList = new List<TaskData>();
    public RewardData reward;   

    public string GetIdQuest()
    {
        return name;
    }
}
