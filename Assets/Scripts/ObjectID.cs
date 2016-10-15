using UnityEngine;
using System.Collections;

public class ObjectID : MonoBehaviour {

    [SerializeField]
    float scaleMod = 2;
    int id;
    int amountLookedAt;
    float totalTimeLookedAt;
    Vector3 originalScale;
    bool scaling = false;
    float scaleTime = 10f;

    void Start()
    {
        originalScale = transform.localScale;
    }
    void Update()
    {
        if (scaling)
        {
            Vector3 toScale = originalScale * (1 / (1+totalTimeLookedAt / scaleMod));
            //print("ttim "+totalTimeLookedAt+" to " + toScale + ", orig" + originalScale);
            transform.localScale = Vector3.Lerp(transform.localScale, toScale, scaleTime*Time.deltaTime);

            //print(Vector3.Distance(transform.localScale, toScale));
            if (Vector3.Distance(transform.localScale, toScale) < 1) {
                scaling = false;
            }
        }
    }

    public int ID {
        get { return id; }
        set { id = value; }
    }
    public void SetScale() {
        scaling = true;
    }
    public void SetTotalTime(float tTime)
    {
        totalTimeLookedAt += tTime;
        //scaling = true;

    }

    public float GetTotalTime()
    {
        return totalTimeLookedAt;
    }
}
