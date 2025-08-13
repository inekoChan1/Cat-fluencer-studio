using UnityEngine;

public enum TaskType { PhotoSession, VideoSession }

[System.Serializable]
public class Assignment
{
    public Cat assignedCat;
    public TaskType taskType;
    public int endTotalHours;

    public Assignment(Cat cat, TaskType type, int endTime)
    {
        assignedCat = cat;
        taskType = type;
        endTotalHours = endTime;
    }
}