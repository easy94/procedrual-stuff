using System;
using System.Collections;
using System.Collections.Generic;
using System.Xml.Schema;
using Unity.VisualScripting;
using UnityEngine;

public class GenerateMap : MonoBehaviour
{
    public AnimationCurve curve;
    [SerializeField] MeshFilter myMeshFilter;
    [SerializeField] MeshCollider meshCollider;
    DrawNoiseTexture noiseTexture;
    [SerializeField] float Scale;
    [Range(1, 145)]
    [SerializeField] int MapSizeMultiplier;
    public static int Mapwidth = 145;
    [Range(0, 4)]
    [SerializeField] int LevelOfDetail;
    [SerializeField] int octaves;
    [SerializeField] float freq;
    [SerializeField] float amp;
    [SerializeField] int seed;
    [SerializeField] float heightmultiplier;

    public bool autoUpdate;



    public void DrawMapData()
    {

        noiseTexture = FindObjectOfType<DrawNoiseTexture>();
        MapData mapData = new();

        mapData.NoiseValueData = GenerateNoiseMap();
        mapData.ColorData = noiseTexture.DrawTexture(mapData.NoiseValueData);
        GenerateMesh.UpdateMesh(mapData.NoiseValueData, heightmultiplier, curve, LevelOfDetail, MapSizeMultiplier);

        GenerateMesh.TestMesh();

        //MapData data = new MapData();
        //data.NoiseValueData = GenerateNoiseMap();

        //data.ColorData = noiseTexture.DrawTexture(data.NoiseValueData);



        //MeshFilter tileInstanceMesh = tileInstance.GetComponent<MeshFilter>();
        //tileInstanceMesh.mesh = GenerateMesh.UpdateMesh(data.NoiseValueData, heightmultiplier, myMeshFilter, curve, LevelOfDetail);

        //MeshCollider tileInstanceCollider = tileInstance.GetComponent<MeshCollider>();
        //tileInstanceCollider.sharedMesh = tileInstanceMesh.sharedMesh;


        //myMeshFilter.mesh = GenerateMesh.UpdateMesh(data.NoiseValueData, heightmultiplier, myMeshFilter, curve, LevelOfDetail);
        //MeshCollider meshCollider = myMeshFilter.GetComponent<MeshCollider>();
        //meshCollider.sharedMesh = myMeshFilter.sharedMesh;

    }

    public float[,] GenerateNoiseMap()
    {
        float[,] noiseMap = GenerateNoise.Generate(Mapwidth, Scale, octaves, freq, amp, seed);//call generate function from the Perlin noise class that creates an multiarr of                                                                                               //perlinvals
                                                                                              //call drawtexture function from DrawNoiseTexture class that creates+applytoplane
        return noiseMap;
    }


}


public struct MapData
{

    public float[,] NoiseValueData;
    public Color32[] ColorData;


    public MapData(float[,] noise, Color32[] color)
    {
        NoiseValueData = noise;
        ColorData = color;
    }
}
