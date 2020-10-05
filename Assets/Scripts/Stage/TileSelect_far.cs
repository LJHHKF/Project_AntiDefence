using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileSelect_far : MonoBehaviour
{
    private StageManager sm;
    private MeshRenderer m_mesh;

    // Start is called before the first frame update
    void Awake()
    {
        sm = GameObject.FindGameObjectWithTag("StageMObject").GetComponent<StageManager>();
        m_mesh = gameObject.GetComponent<MeshRenderer>();

         m_mesh.material = sm.Get_FarTile();
    }

}
