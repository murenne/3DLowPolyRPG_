using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class QuestManager : Singleton<QuestManager>
{
    [System.Serializable]
    public class Quest
    {
        public QuestData_SO _questData;
        public bool IsStarted
        {
            get
            {
                return _questData.isStarted;
            }
            set
            {
                _questData.isStarted = value;
            }
        }

        public bool IsCompleted
        {
            get
            {
                return _questData.isCompleted;
            }

            set
            {
                _questData.isCompleted = value;
            }
        }

        public bool IsFinished
        {
            get
            {
                return _questData.isFinished;
            }

            set
            {
                _questData.isFinished = value;
            }
        }
    }

    public List<Quest> questList = new();

    public bool HaveQuest(QuestData_SO data)
    {
        if (data != null)
        {
            return questList.Any(q => q._questData.questName == data.questName);
        }
        else
        {
            return false;
        }
    }

    public Quest GetTask(QuestData_SO data)
    {
        return questList.Find(q => q._questData.questName == data.questName);
    }
}
