using UnityEngine;
using System.Collections;

public class LookedAt {


    int id; //refering to the ObjectID class attached to GOs
    float timeLookedAt = 0f;
    bool writtenToDB = false; //for ensuring that list items are only written to the DB once no matter how many times the list of LookedAt's is saved

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
    public bool WrittenToDB {
        get { return writtenToDB; }
        set { writtenToDB = value; }
    }
    
}
