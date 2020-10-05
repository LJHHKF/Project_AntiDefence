using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileSelect_far : MonoBehaviour
{
    private StageManager sm;
    private MeshRenderer m_mesh;
    private int index;

    public Material[] tileMaterial;

    // Start is called before the first frame update
    void Awake()
    {
        sm = GameObject.FindGameObjectWithTag("StageMObject").GetComponent<StageManager>();
        m_mesh = gameObject.GetComponent<MeshRenderer>();

        index = sm.Get_FarTile();

       m_mesh.material = tileMaterial[index];
    }

}
