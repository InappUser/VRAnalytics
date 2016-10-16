using UnityEngine;
using System.Collections;

public class LookedAt {


    int id; //refering to the ObjectID class attached to GOs
    float timeLookedAt = 0f;


    public LookedAt(int objID, float initTime)
    {
        id = objID;
        timeLookedAt = initTime;
    }
    public int GetID()
    {
        return id;
    }
    public float GetTime()
    {
        return timeLookedAt;
    }
    public void AddTime(float time)
    {
        timeLookedAt += time;
    }
    
}
