using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using Unity.VisualScripting;
using Unity.VisualScripting.FullSerializer;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.UIElements;

public static class GenerateMesh
{

    

    public static void UpdateMesh(float[,] noiseMap, float heightmultiplier, AnimationCurve curve, int levelOfDetail, int multi)
    {
        int width = noiseMap.GetLength(0);
        int height = noiseMap.GetLength(1);

       int reduceVertexFactor = (levelOfDetail == 0 ) ? 1 : levelOfDetail * 2;
       int vertexPerLine = ((noiseMap.GetLength(0) - 1) / reduceVertexFactor) + 1;

        Vector2[] uvArr = new Vector2[vertexPerLine * vertexPerLine];
        Vector3[] vertexArr = new Vector3[vertexPerLine * vertexPerLine];

        int[] triangleArr = new int[((vertexPerLine - 1) * (vertexPerLine - 1)) * 6];

        int j = 0;
        int k = 0;
        //generate vertices array + triangle array + uv array
        for (int x = 0; x < width; x += reduceVertexFactor)
        {
            for (int y = 0; y < height; y += reduceVertexFactor)
            {
                vertexArr[k] = new Vector3(x, (curve.Evaluate(noiseMap[x, y]) * heightmultiplier), y);
                uvArr[k] = new Vector2((float)x / width, (float)y / height);


                if (y < height - 1 && x < height - 1)
                {
                    //first triangle
                    triangleArr[j] = k;
                    triangleArr[j + 1] = k + vertexPerLine + 1;
                    triangleArr[j + 2] = k + vertexPerLine;
                    //second triangle
                    triangleArr[j + 3] = k + vertexPerLine + 1;
                    triangleArr[j + 4] = k;
                    triangleArr[j + 5] = k + 1;

                    j += 6;
                }

                ++k;
            }
        }



        //myMesh.vertices = vertexArr;
        //myMesh.triangles = triangleArr;
        //myMesh.uv = uvArr;

        GameObject tempobj = GameObject.CreatePrimitive(PrimitiveType.Plane);
        tempobj.GetComponent<MeshFilter>().sharedMesh.Clear();

        
        tempobj.GetComponent<MeshFilter>().sharedMesh.vertices = vertexArr;
        tempobj.GetComponent<MeshFilter>().sharedMesh.triangles = triangleArr;
        tempobj.GetComponent<MeshFilter>().sharedMesh.uv = uvArr;

        tempobj.GetComponent<MeshFilter>().sharedMesh.RecalculateBounds();
        tempobj.GetComponent<MeshFilter>().sharedMesh.RecalculateNormals();

        tempobj.transform.localScale = Vector3.one *multi ;

       
        tempobj.name = "test";

    }

    public static void TestMesh()
    {
       GameObject test = GameObject.Find("test");
        Vector3[] bigArr = test.GetComponent<MeshFilter>().sharedMesh.vertices;
        

        bigArr[0].y = -10f;
        bigArr[1].y = -10f;
        bigArr[(int)Mathf.Sqrt(bigArr.Length)].y = -10f;
        bigArr[(int)Mathf.Sqrt(bigArr.Length)+1].y = -10f;

        bigArr[2].y = 10f;
        bigArr[3].y = 10f;
        bigArr[(int)Mathf.Sqrt(bigArr.Length)+2].y = 10f;
        bigArr[(int)Mathf.Sqrt(bigArr.Length) + 3].y = 10f;

        test.GetComponent<MeshFilter>().sharedMesh.vertices=bigArr;

        int sqrlngth = (int)Mathf.Sqrt(bigArr.Length);
        Vector3[] smallArr = new Vector3[(sqrlngth-1)*(sqrlngth-1)*4];// 576length 169vertices
        int p = 0;
        int q = 0;
        for(int x = 1; x < sqrlngth-1;++x)
        {
            for(int y = 0; y < sqrlngth-1; ++y)
            {
                smallArr[p] = bigArr[q];

                smallArr[p+1] = bigArr[q+1];
                
                smallArr[p+2] = bigArr[sqrlngth+q];

                smallArr[p+3] = bigArr[sqrlngth+1 + q];
                p += 4;
                ++q;
            }
            ++q;
        }

        //split the smallArr example into chunks now so we can create and apply it

        GameObject test1 = GameObject.CreatePrimitive(PrimitiveType.Plane);
        test1.name = "test1";
        Mesh n = new();




        //smallArr[0] = bigArr[0];
        //smallArr[1] = bigArr[1];
        //smallArr[2] = bigArr[(int)Mathf.Sqrt(bigArr.Length)];
        //smallArr[3] = bigArr[(int)Mathf.Sqrt(bigArr.Length) + 1];

        //Vector2[]uv = new Vector2[4];
        //uv[0]=new Vector3(0,0);
        //uv[1]= new Vector3(1,0);
        //uv[2]= new Vector3(0,1);
        //uv[3]=new Vector3(1,1);

        //int[] testtri = new int[6];
        //testtri[0] = 0;
        //testtri[1] = 3;
        //testtri[2] = 2;
        //testtri[3] =0;
        //testtri[4] = 1;
        //testtri[5] = 3;

        //n.vertices = smallArr;
        //n.triangles = testtri;
        //n.uv = uv;
        

        //test1.GetComponent<MeshFilter>().sharedMesh = n;
        //test1.GetComponent<MeshCollider>().sharedMesh = n;
    }
  
}
