using UnityEngine;
using System.Collections;

public class LookedAt {


    int id; //refering to the ObjectID class attached to GOs
    float timeLookedAt = 0f;
    Vector3 hitPoint; //only saved locally
    bool writtenToDB = false; //for ensuring that list items are only written to the DB once no matter how many times the list of LookedAt's is saved

    public LookedAt(int _id, float _timeLookedAt, Vector3 _hitPoint)
    {
        id = _id;
        timeLookedAt = _timeLookedAt;
        hitPoint = _hitPoint;
    }
    public int GetID()
    {
        return id;
    }
    public Vector3 GetHitPoint()
    {
        return hitPoint;
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
