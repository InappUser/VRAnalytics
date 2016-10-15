using UnityEngine;
using System.Collections;
[ExecuteInEditMode]
public class BaseMaterialHandler : MonoBehaviour {
    [SerializeField]
    Color32 baseCol;
    Material[] mats;
	// Use this for initialization
	void Start () {
       // mats = GetComponent<MeshRenderer>().sharedMaterials;
	}
	
	// Update is called once per frame
	void Update () {
        //print("run");
        //foreach (Material m in mats)
        //{
        //    if (m.color != baseCol)
        //    {
        //        m.color = baseCol;
        //    }
        //}
    }
}
