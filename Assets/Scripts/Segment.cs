using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Segment : MonoBehaviour
{
    public int SegId { set; get; }
    public bool transition;

    public int length;
    public int beginY1, beginY2, beginY3;
    public int endY1, endY2, endY3;

    private PieceSpawner[] pieces;

    private void Awake()
    {
        pieces = gameObject.GetComponentsInChildren<PieceSpawner>();
        //This checks for each piece on the scene
        for (int i = 0; i < pieces.Length; i++)
        {
            // This is to get the MeshRender for each piece in the pool on the scene
            foreach(MeshRenderer mr in pieces[i].GetComponentsInChildren<MeshRenderer>())
            {
                mr.enabled = LevelManager.Instance.SHOW_COLLIDER; // this either true of false
            }
        }
    }

    public void Spawn()
    {
        gameObject.SetActive(true);

        //this check sfr the assetsfor the objects in the scene

        for (int i = 0; i < pieces.Length; i++)
        {
            pieces[i].Spawn();        
        }
    }

    public void DeSpawn()
    {
        gameObject.SetActive(false);

        //The same to despawn off the scene

        for (int i = 0; i < pieces.Length; i++)
        {
            pieces[i].DeSpawn();
        }
    }
}
