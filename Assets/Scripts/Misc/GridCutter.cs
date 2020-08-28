using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BLINDED_AM_ME;

public class GridCutter/*: MonoBehaviour*/
{
    //int _gridNumberInEachDimension;

    //// Start is called before the first frame update
    //void Start()
    //{
    //}

    public static GameObject[] Cut(GameObject victim,Material innerMaterial,int gridNumberInEachDimension, out int nullCounter)
    {
        Vector3 pos = victim.GetComponent<MeshRenderer>().bounds.center;// victim.transform.position;
        int n = gridNumberInEachDimension;
        Vector3 size = victim.GetComponent<MeshRenderer>().bounds.size;

        GameObject[] allPieces = new GameObject[n* n* n];
        nullCounter = 0;
        for (int i = 0; i < n - 1; i++)
        {
            float cuttingYoffset = ((float)((float)(n-2) / 2.0f - (float)i) / (float)n);
            float sy = size.y;
            Vector3 cuttingPos1 = pos + new Vector3(0, cuttingYoffset * sy, 0);
            Vector3 up = new Vector3(0, 1, 0);

            GameObject[] pieces = MeshCutter.Cut(victim, cuttingPos1, up, innerMaterial);

            //var plane = GameObject.CreatePrimitive(PrimitiveType.Plane);
            //plane.transform.position = cuttingPos1;
            //plane.transform.up = up;

            //allPieces.Add(pieces[1]);
            if (pieces[1] != null)
            {
                for (int j = 0; j < n - 1; j++)
                {
                    float cuttingZOffset = ((float)((float)(n - 2) / 2.0f - (float)j) / (float)n);
                    float sz = size.z;

                    Vector3 cuttingPos2 = pos + new Vector3(0, 0, cuttingZOffset * sz);
                    Vector3 forward = new Vector3(0, 0, 1);

                    GameObject[] pieces2 = MeshCutter.Cut(pieces[1], cuttingPos2, forward, innerMaterial);
                    //allPieces.Add(pieces2[1]);
                    if (pieces2[1] != null)
                    {

                        for (int k = 0; k < n - 1; k++)
                        {
                            float cuttingXOffset = ((float)((float)(n - 2) / 2.0f - (float)k) / (float)n);
                            float sx = size.x;
                            Vector3 cuttingPos3 = pos + new Vector3(cuttingXOffset * sx, 0, 0);
                            Vector3 right = new Vector3(1, 0, 0);

                            GameObject[] pieces3 = MeshCutter.Cut(pieces2[1], cuttingPos3, right, innerMaterial);
                            //allPieces.Add(pieces3[1]);
                            if (pieces2[1] != null)
                            {
                                allPieces[i * n * n + j * n + k - nullCounter] = pieces3[1];
                                Debug.Log("y: " + (i * n * n + j * n + k - nullCounter));
                            }
                            else
                            {
                                nullCounter++;
                            }
                        }
                        allPieces[i * n * n + j * n + n - 1 - nullCounter] = pieces2[1];
                        Debug.Log("z: " + (i * n * n + j * n + n - 1 - nullCounter));
                    }
                    else
                    {
                        nullCounter += n;
                    }
                }
                for (int k = 0; k < n - 1; k++)
                {
                    float cuttingXOffset = ((float)((float)(n - 2) / 2.0f - (float)k) / (float)n);
                    float sx = size.x;
                    Vector3 cuttingPos3 = pos + new Vector3(cuttingXOffset * sx, 0, 0);
                    Vector3 right = new Vector3(1, 0, 0);

                    GameObject[] pieces3 = MeshCutter.Cut(pieces[1], cuttingPos3, right, innerMaterial);
                    if (pieces3[1] != null)
                    {
                        allPieces[i * n * n + (n - 1) * n + k - nullCounter] = pieces3[1];
                        //allPieces.Add(pieces3[1]);
                        Debug.Log("y: " + (i * n * n + (n - 1) * n + k - nullCounter));
                    }
                    else
                    {
                        nullCounter++;
                    }
                }
                allPieces[i * n * n + (n - 1) * n + n - 1 - nullCounter] = pieces[1];
                Debug.Log("x: " + (i * n * n + (n - 1) * n + n - 1 - nullCounter));
            }
            else
            {
                nullCounter+=n*n;
            }
        }

        for (int j = 0; j < n - 1; j++)
        {
            float cuttingZOffset = ((float)((float)(n - 2) / 2.0f - (float)j) / (float)n);
            float sz = size.z;
            Vector3 cuttingPos2 = pos + new Vector3(0, 0, cuttingZOffset * sz);
            Vector3 normal = new Vector3(0, 0, 1);

            GameObject[] pieces2 = MeshCutter.Cut(victim, cuttingPos2, normal, innerMaterial);
            if (pieces2[1] != null)
            {
                for (int k = 0; k < n - 1; k++)
                {
                    float cuttingXOffset = ((float)((float)(n - 2) / 2.0f - (float)k) / (float)n);
                    float sx = size.x;
                    Vector3 cuttingPos3 = pos + new Vector3(cuttingXOffset * sx, 0, 0);
                    Vector3 right = new Vector3(1, 0, 0);

                    GameObject[] pieces3 = MeshCutter.Cut(pieces2[1], cuttingPos3, right, innerMaterial);
                    if (pieces3[1]!=null)
                    {
                        allPieces[(n - 1) * n * n + j * n + k - nullCounter] = pieces3[1];
                        Debug.Log("y: " + ((n - 1) * n * n + j * n + k - nullCounter));
                    }
                    else
                    {
                        nullCounter++;
                    }
                }

                allPieces[(n - 1) * n * n + j * n + n - 1 - nullCounter] = pieces2[1];
                Debug.Log("z: " + ((n - 1) * n * n + j * n + n - 1 - nullCounter));
            }
            else
            {
                nullCounter+=n;
            }
        }

        for (int k = 0; k < n - 1; k++)
        {
            float cuttingXOffset = ((float)((float)(n - 2) / 2.0f - (float)k) / (float)n);
            float sx = size.x;
            Vector3 cuttingPos3 = pos + new Vector3(cuttingXOffset * sx, 0, 0);
            Vector3 right = new Vector3(1, 0, 0);

            GameObject[] pieces3 = MeshCutter.Cut(victim, cuttingPos3, right, innerMaterial);
            if (pieces3[1] != null)
            {
                allPieces[(n - 1) * n * n + (n - 1) * n + k - nullCounter] = pieces3[1];
                //allPieces.Add(pieces3[1]);
                Debug.Log("y: " + ((n - 1) * n * n + (n - 1) * n + k - nullCounter));
            }
            else
            {
                nullCounter++;
            }
        }

        allPieces[(n - 1)*n*n +  (n-1)*n + n-1 - nullCounter] = victim;
        Debug.Log("z: " + ((n - 1) * n * n + (n - 1) * n + n - 1 - nullCounter));
        //allPieces.Add(victim);
        return allPieces;
    }
    //// Update is called once per frame
    //void Update()
    //{
        
    //}
}
