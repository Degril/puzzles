using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SphereGenerator : MonoBehaviour
{
    [SerializeField] private GameObject _empty;
    [SerializeField] private GameObject _emptyWithMesh;
    
    public IEnumerable<AbstractDetail> Generate(int squeres, int devideNumber, float thickness, float radius)
    {
        var table = GeneratePosTable(squeres, radius);
        return GenerateObjects(table, squeres, devideNumber, thickness,radius);
    }

    void FindSperePos(ref float posx, ref float posy,ref float posz, int finderpos, float radius = 1)
    {
        if (finderpos == 0)
        {
            posx = (float)Math.Sqrt(radius* radius - (double)(posy * posy + posz * posz));
        }
        else if (finderpos == 1)
        {
            posy = (float)Math.Sqrt(radius * radius - (posx * posx + posz * posz));
        }
        else if (finderpos == 2)
        {
            posz = (float)Math.Sqrt(radius * radius - (posx * posx + posy * posy));
        }
    }

    private float leftPos(float radius) => -Mathf.Sqrt((radius * radius) / 3f);
    private float RightPos(float radius) => Mathf.Sqrt((radius * radius) / 3f);

    private Vector3[,,] GeneratePosTable(int squeres, float radius = 1)
    {
        float posx = 0;
        float posy = 0;
        float posz = 0;

        List<float> poses = new List<float>();

        for (int i = 0; i <= squeres; i++)
        {
            float pos = Mathf.Lerp(leftPos(radius), RightPos(radius), i / (float)squeres);
            float posOne = pos;

            float posTwoRight = Mathf.Sqrt((radius* radius - (posOne * posOne)) / 2);
            poses.Add(posTwoRight);
        }

        Vector3[,,] positionsTable = new Vector3[6,squeres + 1, squeres + 1];
        for (int posOneNumber = 0; posOneNumber <= squeres; posOneNumber++)
        {
            for (int PosTwoNumber = 0; PosTwoNumber <= squeres; PosTwoNumber++)
            {
                float posOne = Mathf.Lerp(-poses[posOneNumber], poses[posOneNumber], PosTwoNumber / (float)squeres);
                float posTwo = Mathf.Lerp(-poses[PosTwoNumber], poses[PosTwoNumber], posOneNumber / (float)squeres);
                if ((posOneNumber == 0 || posOneNumber == poses.Count - 1) && (PosTwoNumber == 0 || PosTwoNumber == poses.Count - 1))
                { }
                else
                {
                    if (posOneNumber == 0 || posOneNumber == poses.Count - 1)
                        posTwo = Math.Sign(posTwo) * (float)Math.Sqrt((radius * radius - (posOne * posOne)) / 2);
                    if (PosTwoNumber == 0 || PosTwoNumber == poses.Count - 1)
                        posOne = Math.Sign(posOne) * (float)Math.Sqrt((radius * radius - (posTwo * posTwo)) / 2);
                }

                posx = posOne; posy = posTwo; posz = 0;
                FindSperePos(ref posx, ref posy, ref posz, 2, radius);
                positionsTable[1, posOneNumber, PosTwoNumber] = new Vector3(posx, posy, posz);
                positionsTable[0, posOneNumber, PosTwoNumber] = new Vector3(posx, posy, -posz);

                posx = posOne; posy = 0; posz = posTwo;
                FindSperePos(ref posx, ref posy, ref posz, 1, radius);
                positionsTable[2, posOneNumber, PosTwoNumber] = new Vector3(posx, posy, posz);
                positionsTable[3, posOneNumber, PosTwoNumber] = new Vector3(posx, -posy, posz);

                posx = 0; posy = posOne; posz = posTwo;
                FindSperePos(ref posx, ref posy, ref posz, 0, radius);
                positionsTable[5, posOneNumber, PosTwoNumber] = new Vector3(posx, posy, posz);
                positionsTable[4, posOneNumber, PosTwoNumber] = new Vector3(-posx, posy, posz);
            }
        }
        return positionsTable;
    }

    private IEnumerable<AbstractDetail> GenerateObjects(Vector3[,,] positionsTable, int squres, int devideNumber, float thickness, float radius)
    {
        vertsAll = new List<Vector3>();
        uvsAll = new List<Vector2>();
        trisAll = new List<int>();
        var objects = new List<AbstractDetail>();
        for (int i = 0; i < 6; i++)
        {
            for (int j = 0; j < squres; j++)
            {
                for (int k = 0; k < squres; k++)
                {
                    var vertices = new Vector3[4];
                    vertices[0] = positionsTable[i, j, k];
                    vertices[1] = positionsTable[i, j + 1, k];
                    vertices[2] = positionsTable[i, j, k + 1];
                    vertices[3] = positionsTable[i, j + 1, k + 1];
                    var detail = GenerateDetail(vertices, i, devideNumber, radius, thickness);
                    AbstractDetail detailComponent;
                    if (thickness == 0)
                        detailComponent = detail.transform.GetChild(0).gameObject.AddComponent<SlotComponent>();
                    else detailComponent = detail.transform.GetChild(0).gameObject.AddComponent<DetailComponent>();
                    detailComponent.Angles = vertices;
                    objects.Add(detailComponent);
                }
            }
        }

        return objects;
        //FileObjectCreater.CreateFileObj(uvsAll.ToArray(), vertsAll.ToArray(), trisAll.ToArray());
    }

    List<Vector3> vertsAll;
    List<Vector2> uvsAll;
    List<int> trisAll;
    private GameObject GenerateDetail(Vector3[] vertices, int brinkNumber, int devideNumber, float radius, float thickness)
    {
        var emptyRoot = Instantiate(_empty, transform);
        var emptyObject = Instantiate(_emptyWithMesh, emptyRoot.transform);

        var meshFilter = emptyObject.GetComponent<MeshFilter>();
        var mesh = new Mesh();
        var tris = new List<int>();
        var endVerices = new List<Vector3>();

        var newVericesFrom = GenerateDeatilPolygons(vertices, devideNumber, radius + thickness).ToList();

        var meanPosFrom = new Vector3(
            vertices.Average(x => x.x),
            vertices.Average(x => x.y),
            vertices.Average(x => x.z));

        newVericesFrom = newVericesFrom.Select(vert => vert - meanPosFrom).ToList();
        endVerices.AddRange(newVericesFrom);

        if (thickness > 0)
        {
            var newVericesTo = GenerateDeatilPolygons(vertices, devideNumber, radius).ToList();
            var meanPosTo = new Vector3(
                newVericesTo.Average(x => x.x),
                newVericesTo.Average(x => x.y),
                newVericesTo.Average(x => x.z));
            newVericesTo = newVericesTo.Select(vert => vert - meanPosTo).ToList();
            endVerices.AddRange(newVericesTo);


            for (int posOneNumber = 0; posOneNumber < devideNumber; posOneNumber++)
            {
                for (int PosTwoNumber = 0; PosTwoNumber < devideNumber; PosTwoNumber++)
                {
                    if ((posOneNumber == 0 || posOneNumber == devideNumber - 1) && (PosTwoNumber != devideNumber - 1))
                    {
                        var newTrees = new List<int>();
                        newTrees.Add(posOneNumber * devideNumber + PosTwoNumber);
                        newTrees.Add(posOneNumber * devideNumber + PosTwoNumber + 1);
                        newTrees.Add(newVericesFrom.Count + posOneNumber * devideNumber + PosTwoNumber + 1);
                        newTrees.Add(posOneNumber * devideNumber + PosTwoNumber);
                        newTrees.Add(newVericesFrom.Count + posOneNumber * devideNumber + PosTwoNumber + 1);
                        newTrees.Add(newVericesFrom.Count + posOneNumber * devideNumber + PosTwoNumber);

                        if ((posOneNumber != 0) != (brinkNumber % 2 == 1))
                            newTrees.Reverse();
                        tris.AddRange(newTrees);
                    }
                    if ((PosTwoNumber == 0 || PosTwoNumber == devideNumber - 1) && (posOneNumber != devideNumber - 1))
                    {
                        var newTrees = new List<int>();

                        newTrees.Add(posOneNumber * devideNumber + PosTwoNumber);
                        newTrees.Add((posOneNumber + 1) * devideNumber + PosTwoNumber);
                        newTrees.Add(newVericesFrom.Count + (posOneNumber + 1) * devideNumber + PosTwoNumber);
                        newTrees.Add(posOneNumber * devideNumber + PosTwoNumber);
                        newTrees.Add(newVericesFrom.Count + (posOneNumber + 1) * devideNumber + PosTwoNumber);
                        newTrees.Add(newVericesFrom.Count + posOneNumber * devideNumber + PosTwoNumber);


                        if ((PosTwoNumber == 0) != (brinkNumber % 2 == 1))
                            newTrees.Reverse();
                        tris.AddRange(newTrees);
                    }
                }
            }
        }

        //emptyObject.transform.localPosition += meanPos;
        emptyRoot.transform.localPosition = meanPosFrom;

        //vertices = vertices.Select(vert => vert /100).ToArray();

        for (int k = 0; k < ((thickness != 0) ? 2 : 1); k++)
        {
            for (int i = 0; i < devideNumber - 1; i++)
            {
                for (int j = 0; j < devideNumber - 1; j++)
                {
                    int vertPos = newVericesFrom.Count * k + i * devideNumber + j;
                    if ((brinkNumber % 2 == 1) == (k == 0))
                    {
                        tris.Add(vertPos + 0);
                        tris.Add(vertPos + 1 + devideNumber);
                        tris.Add(vertPos + devideNumber);
                        tris.Add(vertPos + 0);
                        tris.Add(vertPos + 1);
                        tris.Add(vertPos + 1 + devideNumber);
                    }
                    else
                    {
                        tris.Add(vertPos + 1 + devideNumber);
                        tris.Add(vertPos + 1);
                        tris.Add(vertPos + 0);
                        tris.Add(vertPos + devideNumber);
                        tris.Add(vertPos + 1 + devideNumber);
                        tris.Add(vertPos + 0);
                    }
                }
            }
        }
        var uvs = new Vector2[endVerices.Count];

        for (int l = 0; l < endVerices.Count; l++)
        {
            var pos = endVerices[l] + meanPosFrom;
            var startpos = new Vector2((brinkNumber % 3) / 3.0f, (brinkNumber / 3) / 3.0f);
            if (brinkNumber < 2)
            {
                uvs[l] = startpos + new Vector2(pos.x + 1f, pos.y + 1f) / 6 / radius;
            }
            else if (brinkNumber < 4)
            {
                uvs[l] = startpos + new Vector2(pos.x + 1f, pos.z + 1f) / 6 / radius;
            }
            else if (brinkNumber < 6)
            {
                uvs[l] = startpos + new Vector2(pos.y + 1f, pos.z + 1f) / 6 / radius;
            }
        }

        mesh.vertices = endVerices.ToArray();
        mesh.triangles = tris.ToArray();
        mesh.uv = uvs;
        mesh.RecalculateNormals();
        meshFilter.mesh = mesh;
        //emptyObject.transform.LookAt(Vector3.up);
        //foreach (var vert in newVerices)
        //{
        //    var obj = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        //    obj.transform.position = vert + meanPos;                
        //}

        var vertsLen = vertsAll.Count;
        vertsAll.AddRange(endVerices);
        uvsAll.AddRange(uvs);
        for (int t = 0; t < tris.Count; t++)
        {
            trisAll.Add(tris[t] + vertsLen);
        }
        emptyRoot.transform.LookAt(Vector3.LerpUnclamped(transform.position, transform.position + meanPosFrom, 2));
        emptyObject.transform.eulerAngles = Vector3.zero;// Vector3.up - meanPos;
        //emptyObject.transform.Rotate(-emptyRoot.transform.eulerAngles);
        return emptyRoot;
    }


    private Vector3[] GenerateDeatilPolygons(Vector3[] vertices, int devideNumber, float radius)
    {
        var newVertices = new Vector3[devideNumber*devideNumber];

        for (int i = 0; i < devideNumber; i++)
        {
            for (int j = 0; j < devideNumber; j++)
            {
                var posxStart = Vector3.Lerp(vertices[0], vertices[2], j / (float)(devideNumber-1));
                var posxEnd = Vector3.Lerp(vertices[1], vertices[3], j / (float)(devideNumber - 1));
                var posEnd = Vector3.Lerp(posxStart, posxEnd, i / (float)(devideNumber - 1));

                posEnd = Vector3.LerpUnclamped(Vector3.zero, posEnd, radius / Vector3.Distance(Vector3.zero, posEnd));
                newVertices[i * devideNumber + j] = posEnd;
            }
        }

        return newVertices;
    }

}
