using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class WriteToImage : MonoBehaviour {
    public class Pair {
        Vector3 dir;
        string mat;
        public Pair(Vector3 myDir, string myMat)
        {
            dir = myDir;
            mat = myMat;
        }

       public string GetString() { return mat; }
        public Vector3 GetVector() { return dir; }
    }
    public class Direction {
        Pair[] pairs;

        public Direction() {
            pairs = new Pair[6];
            pairs[0] = new Pair(-Vector3.right, "Right");
            pairs[1] = new Pair(Vector3.right, "Left");
            pairs[2] = new Pair(Vector3.up, "Front");
            pairs[3] = new Pair(-Vector3.up, "Back");
            pairs[4] = new Pair(Vector3.forward, "Top");
            pairs[5] = new Pair(-Vector3.forward, "Bottom");
        }

        public string Find(Vector3 v)
        {
            foreach (Pair p in pairs)
            {
                if (p.GetVector() == v) {
                    return p.GetString();
                }
            }
            Debug.LogAssertion("no vector could be matched!");
            return "";
        }
        
    }
    public Texture2D img;
    Color32[] allPixels;
    Color32 black;
    Direction cubeSide;
    void Start()
    {
        cubeSide = new Direction();
        //Vector3 n = new Vector3(-1.0f, 0.0f, 0.0f);
        //cubeSide = new Dictionary<Vector3, string>();
        //cubeSide.Add();
        //cubeSide.Add(n, "Left");
        //cubeSide.Add();
        //cubeSide.Add();
        //cubeSide.Add();
        //cubeSide.Add();
        black = new Color32(0, 0, 0, 1);
        //allPixels = new Color32[img.width * img.height];
        //for (int i = 0; i < allPixels.Length; i++)
        //{
        //    allPixels[i] = black;
        //}

        //// img = new Texture2D(2, 2);
        //print("Red pixel00: " + img.GetPixel(4, 10));
        //img.SetPixel(4, 10, black);
        ////print("Black pixel00: " + img.GetPixel(4, 10));
        //img.Apply();
    }
    public void WriteToPixel(RaycastHit hit)
    {

        ////use local
        Ray myRay = new Ray(transform.position, transform.forward);
        if (Physics.Raycast(myRay, out hit))
        {
            //print("awdawd1");
            Vector3 normal = hit.normal;
            normal = hit.transform.TransformDirection(normal);
            Vector3 nor = RebuildNormal(hit.transform.InverseTransformDirection(hit.normal));
            Texture2D img= new Texture2D(2,2);
            img.SetPixel(1, 1, new Color(255, 0, 0, 1));
            img.Apply();
            Material[] mats = hit.transform.GetComponent<MeshRenderer>().sharedMaterials;
            foreach (Material m in mats)
            {
                //print(m.name+", "+ cubeSide.Find(nor) +"eual?"+ (m.name == cubeSide.Find(nor)));
                if (m.name == (cubeSide.Find(nor) +" (Instance)"))
                {
                    //print("img name" + img.name + "col " + img.GetPixel(1, 1));
                    img = Resources.Load<Texture2D>(cubeSide.Find(nor));
                    //print("img name" + img.name +"col "+img.GetPixel(1,1));
                    break;
                }
                // print("no"+ hit.transform.InverseTransformDirection(hit.normal)+ "cu"+/*cubeSide[hit.transform.InverseTransformDirection(hit.normal)]*/"culef"+ cubeSide.Find(-Vector3.right));//)
                //{
                //    print("tex name" + m.mainTexture.name);
                //}
            }
            
            for (int x = 0; x < img.width; x++)
            {
                for (int y = 0; y < img.height; y++)
                {
                    img.SetPixel(x, y, black);
                }
            }
            img.Apply();
            hit.transform.position += (hit.normal / 1000);
        }
    }
    Vector3 RebuildNormal(Vector3 normal)
    {
        return new Vector3(-1.0f, 0.0f, 0.0f);
    }
    //Texture2D GetBoxSide(MeshRender string materialName)
    //{

    //}
}
