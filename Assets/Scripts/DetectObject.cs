using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class DetectObject : MonoBehaviour {

    
    //when user looks at object, adds time while they're looking at it
    //when user looks at other object, adds object to list and starts adding time
    //when user looks back at object alreafy listed, references ID and adds new time amount
    //for this need class that takes ID and time (class because dynamic)
    [SerializeField]
   GameObject availableObjectsParent;
    [SerializeField]
    Scrollbar scroll;
    [SerializeField]
    Image img;

    [SerializeField] Material redMat;
    [SerializeField] Material greenMat; 

    List<ObjectID> availableObjects;
    List<LookedAt> myLookedAts;
    LookedAt lastLookedAt;
    Quaternion lastRotation;
    Camera myCam;
	bool moveToObj=false;
	int lastObjID = -1;//so still picks up an id of 0
    bool currentlylookingAtObj = false; //so that the timer ticks up when they're looking at an object, but still only casts rays, when the rotation of the camera has changed

    float lookingAtTimer; //checking that the player is actually looking and not just moving their vision past
    float hasLookedAtDuration = .2f;

    RaycastHit lastRayHit;
    int lastRayHitID=0;
    bool lastRayHitSomething = false;
    WriteToImage writeToImg;
    DBHandler db;

    public List<LookedAt> GetLookedAt()
    {
        return myLookedAts;
    }

    void Start () {
        lastRayHit = new RaycastHit();
        availableObjects = new List<ObjectID>();
        availableObjects.AddRange(availableObjectsParent.GetComponentsInChildren<ObjectID>());
        myLookedAts = new List<LookedAt>(); // the index of this is the order
        for (int i = 0; i < availableObjects.Count; i++)
        {
            availableObjects[i].ID = i;

        } //list full of objects that can be looked at

        myCam = GetComponent<Camera>();
        lastRotation = transform.rotation;
        writeToImg = GetComponent<WriteToImage>();
        db = GetComponent<DBHandler>();
        //db.Read(myLookedAts);
        //print("la count" + myLookedAts.Count);
    }
	
	//look at something for a long enough period of time and you move towards it
    // when you want to recal, it will move you back in reverse order
	void Update () {

        CollectObjects();

        if (currentlylookingAtObj)
        {
            lastLookedAt.AddTime(Time.deltaTime); //looked at this object, for this amount of time at once
            FillBar(lastLookedAt.GetTime());
            writeToImg.WriteToPixel(lastRayHit);
            //print("c_obj: " +  availableObjects[myLookedAts[myLookedAts.Count-1].GetID()].name  + ", Ctime: " + myLookedAts[myLookedAts.Count-1].GetTime() + "total time: "+ availableObjects[lA.GetID()].GetComponent<ObjectID>().GetTotalTime());
        } else if(lastLookedAt!=null) {

        }
        if (Input.GetKey(KeyCode.Space))
        {
            db.Write(myLookedAts);
        }

        lastRotation = transform.rotation;

	}
    void CollectObjects()
    {
        if (/*Input.GetKey(KeyCode.Space) &&*/ transform.rotation != lastRotation)
        { //only casting a ray, and overwriting global variables, when they move their head
            Ray ray = new Ray(transform.position, transform.forward);
            lastRayHitSomething = Physics.Raycast(ray, out lastRayHit, 100f);
            if (lastRayHitSomething)
            {
                lastRayHitID = lastRayHit.transform.GetComponent<ObjectID>().ID;
            }

            //if (myLookedAts.Count > 0) lastLookedAt = myLookedAts[myLookedAts.Count - 1];
        }


        if (lastRayHitSomething)
        { //if ray hits an object
            //print("last is ray? " + (lastRayHitID == lastObjID));
            if (lastRayHitID == lastObjID)
            {
                if (scroll.size >= .99f)
                {
					img.material = redMat;
                }
                if (currentlylookingAtObj == false) //if already looking at an object, nothing needs to be done here
                {
                    lookingAtTimer += Time.deltaTime;
                    //print("timer: " + lookingAtTimer);
                    if (lookingAtTimer > hasLookedAtDuration)
                    {
                        //add to the amount of times the object has been looked at
                        myLookedAts.Add(new LookedAt(lastRayHitID, lookingAtTimer));
                        lastLookedAt = myLookedAts[myLookedAts.Count - 1]; //the newest lookAt
                        currentlylookingAtObj = true;
                        //print("all: " + Resources.FindObjectsOfTypeAll<Material>().Length);
						img.material = greenMat;
                    }
                }
            }
            else {
                lastObjID = lastRayHitID;
                lookingAtTimer = 0; //resetting for the next object
            }

            
        }
        else //if the player moves their head and there's nothing for a ray to hit
        {
            lookingAtTimer = 0; //resetting for the next object
            if (currentlylookingAtObj && myLookedAts.Count > 0) //if they just looked away from an object
            {
                LookedAt lA = myLookedAts[myLookedAts.Count - 1];
                availableObjects[lA.GetID()].SetTotalTime(lA.GetTime()); //adding to the total time of the previous object before counting for the next

                print("sSize" + scroll.size);
                if (scroll.size < .99f)
                {
                    availableObjects[lastLookedAt.GetID()].SetScale();
                }
                else if(scroll.size>=1)
                {
					
                }
                ResetBar();

				img.material = redMat;
                lastObjID = -1; //letting know that the user was just looking at nothing
                currentlylookingAtObj = false;
            }

        }
    }

    void DrawThroughObjects()
    {
		LineRenderer rend = gameObject.AddComponent<LineRenderer>();
		rend.material = greenMat;
		rend.SetColors (Color.cyan, Color.black);
		rend.SetPosition (0, availableObjects [myLookedAts.Count - 1].transform.position);
		rend.SetPosition (1, availableObjects [myLookedAts.Count - 2].transform.position);
    }
    //build reflection of player as 
    void FillBar(float amount)
    {
        scroll.size = Mathf.Lerp(scroll.size, Mathf.Clamp01(amount), 10 * Time.deltaTime);
    }
    void ResetBar()
    {
        scroll.size = 0;
    }
    //void writeAllToDB()
    //{
    //    db.Write(myLookedAts);
    //}
}
