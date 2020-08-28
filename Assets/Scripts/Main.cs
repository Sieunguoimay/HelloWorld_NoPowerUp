using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BLINDED_AM_ME;
using UnityEngine.UI;
using Assets.Misc;

public class Main : MonoBehaviour
{
    public GameObject canvas;
    
    public GameObject cuttingTarget;
    public Material innerMaterial;
    Bounds bounds;

    public float per;
    public float percentage { set {
            per = value;
            if(onPercentageChanged!=null)
                onPercentageChanged(value);
        }
        get { return per; }
    }

    //public InputField UiPercentageInputField;
    public Slider UiPercentageSlider;

    public delegate void OnPercentageChanged(float value);
    public OnPercentageChanged onPercentageChanged  =null;

    private GameObject[] _blocks;
    private int nullBlockCounter = 0;

    private int vectorFieldSize = 10;
    private Vector3[] vectorField = null;
    private GameObject[] vectorFieldUi = null;
    public Vector3 boundPos;
    public Vector3 boundSize;

    public List<Boid> boids = new List<Boid>();
    public Boid boidTemplate;


    private float deltaTime;
    public Text UiFps;
    // Start is called before the first frame update
    void Start()
    {
        canvas.SetActive(true);

        bounds = cuttingTarget.GetComponent<MeshRenderer>().bounds;

        _blocks = GridCutter.Cut(cuttingTarget, innerMaterial, 10, out int nullBlockCounter);
        this.nullBlockCounter = nullBlockCounter;
        Debug.Log("_blocks.Length - nullBlockCounter: " + (_blocks.Length - nullBlockCounter));
        //UiPercentageInputField.onEndEdit.AddListener((text) => { float.TryParse(text, out float p); percentage = Mathf.Min(p,100.0f); });
        UiPercentageSlider.onValueChanged.AddListener((value) => { percentage = value * 100f; });
        onPercentageChanged = (value) =>
        {
            int upperBoundIndex = (int)((1.0f - value / 100.0f) * (float)(_blocks.Length - nullBlockCounter));
            ShowBlocks(upperBoundIndex);
        };

        upperBoundIndexToDisplay = _blocks.Length - nullBlockCounter;

        boundPos = new Vector3(0, 3.5f, 0);
        boundSize = new Vector3(3, 3, 3);
        vectorFieldSize = 10;
        vectorField = new Vector3[vectorFieldSize * vectorFieldSize * vectorFieldSize];
        vectorFieldUi = new GameObject[vectorFieldSize * vectorFieldSize * vectorFieldSize];

        //DisplayBound();

        for (int i = 0; i < 20; i++)
        {

            Vector3 boidPos = new Vector3(
                Random.Range(-boundPos.x*0.5f, boundPos.x * 0.5f),
                Random.Range(-boundPos.y * 0.5f, boundPos.y * 0.5f),
                Random.Range(-boundPos.z * 0.5f, boundPos.z * 0.5f));
            Vector3 boidVel = new Vector3(
                Random.Range(-0.05f, 0.05f),
                Random.Range(-0.05f, 0.05f),
                Random.Range(-0.05f, 0.05f));
            Vector3 boidAcc = new Vector3(
                Random.Range(-0.01f, 0.01f),
                Random.Range(-0.01f, 0.01f),
                Random.Range(-0.01f, 0.01f));
            Vector3 boidRot = new Vector3(0, 0, 0);
            Boid newBoid = Instantiate(boidTemplate, boidPos, Quaternion.Euler(boidRot));
            newBoid.SetUp(boidPos, boidVel, boidVel);
            newBoid.SetPercepts(boundPos, boundSize);
            boids.Add(newBoid);
        }

        deltaTime = Time.deltaTime;
    }
    private void ShowBlocks(int upperBoundIndex)
    {
        Debug.Log("upperBoundIndex:" + upperBoundIndex);
        for (int i = 0; i < _blocks.Length - nullBlockCounter; i++)
        {
            if (_blocks[i] != null)
            {
                if (i > upperBoundIndex)
                {
                    _blocks[i].SetActive(true);
                }
                else
                {
                    _blocks[i].SetActive(false);
                }
            }
            else
            {
                //Debug.Log("Null " + i+" "+ upperBoundIndex);
            }
        }
    }
    private int upperBoundIndexToDisplay = 0;
    public void OnPrevButtonClicked()
    {
        if (upperBoundIndexToDisplay < _blocks.Length - nullBlockCounter)
        {
            upperBoundIndexToDisplay++;
            ShowBlocks(upperBoundIndexToDisplay);
        }
    }
    public void OnNextButtonClicked()
    {
        if (upperBoundIndexToDisplay >0)
        {
            upperBoundIndexToDisplay--;
            ShowBlocks(upperBoundIndexToDisplay);
        }
    }
    private void OnDrawGizmos()
    {
        if (_blocks != null)
        {
            for (int i = 0; i < _blocks.Length - nullBlockCounter; i++)
            {
                if (_blocks[i] != null)
                {
                    if (_blocks[i].activeSelf)
                    {
                        var bound = _blocks[i].GetComponent<MeshRenderer>().bounds;
                        Gizmos.DrawWireCube(bound.center, bound.size);
                    }
                }
            }

        }
        //if(vectorField!=null)
    }
    // Update is called once per frame
    void Update()
    {

        //Vector3 pos = Input.mousePosition;
        //pos.z = 10;
        //var mouseWorldPos = UnityEngine.Camera.main.ScreenToWorldPoint(pos);
        //Vector3 target = new Vector3(mouseWorldPos.x, 2, -mouseWorldPos.y);
        //boids[0].Arrive(target);

        //deltaTime += (Time.deltaTime - deltaTime) * 0.1f;
        float fps = 1.0f / Time.deltaTime;
        UiFps.text = ((int)fps).ToString();

        if (Input.GetMouseButtonUp(0))
        {
            //Debug.Log("Cut " + transform.forward);


            //for(int i =0; i<pieces.Count; i++)
            //{
            //GameObject piece = pieces[i];
            //Debug.Log("piece");
            //if (!piece.GetComponent<Rigidbody>())
            //{
            //    piece.AddComponent<Rigidbody>();
            //    piece.AddComponent<MeshCollider>();
            //    piece.GetComponent<MeshCollider>().sharedMesh = piece.GetComponent<MeshFilter>().sharedMesh;
            //    piece.GetComponent<MeshCollider>().convex = true;
            //}
            //Destroy(piece, 1);
            //}

            //GameObject[] pieces = MeshCutter.Cut(cuttingTarget, transform.position, transform.up, innerMaterial);
            //if (!pieces[1].GetComponent<Rigidbody>())
            //{
            //    pieces[1].AddComponent<Rigidbody>();
            //    Debug.Log("AddComponent");
            //}
            //Destroy(pieces[1], 1);
        }

        foreach (Boid boid in boids)
        {
            int index = GetIndexFromVectorField(boid.Pos);
            if (index>0&&index < vectorFieldUi.Length)
            {
                if (vectorFieldUi[index] != null)
                {
                    boid.ApplyForce(boid.Steer(vectorFieldUi[index].transform.forward)*0.5f);
                }
            }
            boid.Flock(boids);
            boid.update();
        }
        CreateVectorFieldUi(boundSize.x * 2.0f, boundPos, vectorField);
    }

    public void CreateVectorFieldUi(float size, Vector3 position, Vector3[] vectorField)
    {
        int n = vectorFieldSize;
        float distance = size / (float)n;
        float vectorLength = distance;
        Vector3 originPos = new Vector3(distance / 2.0f, distance / 2.0f, distance / 2.0f);
        Vector3 pos = new Vector3();
        Vector3 pivot = new Vector3(0.5f, 0.5f, 0.5f)*size;
        for (int i = 0; i<n; i++)
        {
            pos.y = i * distance;
            for (int j = 0; j < n; j++)
            {
                pos.z = j * distance;
                for (int k = 0; k < n; k++)
                {
                    pos.x = k * distance;

                    int index = i * n * n + j * n + k;


                    //new Vector3 Qua(Mathf.PerlinNoise(pos.x, pos.y));// vectorField[index];
                    float t = 4.0f*Time.time;
                    float scale = 0.1f;
                    float pitch = Mathf.Lerp(0,Mathf.PI*2.0f,Mathf.PerlinNoise((pos.y+t)* scale, (pos.z+t) * scale));
                    float yaw = Mathf.Lerp(0, Mathf.PI * 2.0f, Mathf.PerlinNoise((pos.z+t) * scale, (pos.x+t) * scale));

                    //float roll = Mathf.Rad2Deg*Mathf.PerlinNoise(pos.x * scale, pos.y * scale);
                    //Vector3 vector = Utils.Instance.YawPitchToVector(Random.Range(0,Mathf.PI*2.0f), Random.Range(0, Mathf.PI * 2.0f));
                    Vector3 vector = Utils.Instance.YawPitchToVector(yaw,pitch);
                    //Vector3 pos2 = ;

                    //Debug.Log(pitch + " " + yaw + " " + vector);

                    if (vectorFieldUi[index] == null)
                    {
                        vectorFieldUi[index] = GameObject.CreatePrimitive(PrimitiveType.Cube);
                        vectorFieldUi[index].transform.localScale = new Vector3(0.01f, 0.01f, distance);
                        vectorFieldUi[index].SetActive(false);
                    }
                    vectorFieldUi[index].transform.position = pos + position - pivot;
                    vectorFieldUi[index].transform.forward = vector * vectorLength;

                    //Gizmos.DrawLine(pos+ position- pivot, pos2+ position- pivot);
                }
            }
        }
    }
    public int GetIndexFromVectorField(Vector3 pos)
    {
        Vector3 relativePos = pos - (boundPos-boundSize);
        float ratioX = relativePos.x / (boundSize.x * 2.0f);
        float ratioY = relativePos.y / (boundSize.y * 2.0f);
        float ratioZ = relativePos.z / (boundSize.z * 2.0f);
        
        int xIndex = (int)(ratioX * vectorFieldSize);
        int yIndex = (int)(ratioY * vectorFieldSize);
        int zIndex = (int)(ratioZ * vectorFieldSize);
        
        int index = yIndex* vectorFieldSize* vectorFieldSize+zIndex* vectorFieldSize+xIndex;
        return index;
    }
    public GameObject GetVectorFromFieldUi(Vector3 pos)
    {
        Vector3 relativePos = pos - (boundPos - boundSize);
        float ratioX = relativePos.x / (boundSize.x * 2.0f);
        float ratioY = relativePos.y / (boundSize.y * 2.0f);
        float ratioZ = relativePos.z / (boundSize.z * 2.0f);

        int xIndex = (int)(ratioX * vectorFieldSize);
        int yIndex = (int)(ratioY * vectorFieldSize);
        int zIndex = (int)(ratioZ * vectorFieldSize);

        int index = yIndex * vectorFieldSize * vectorFieldSize + zIndex * vectorFieldSize + xIndex;

        if (index > vectorFieldUi.Length)
            return null;
        return vectorFieldUi[index];
    }
    public void DisplayBound()
    {
        {
            var plane = GameObject.CreatePrimitive(PrimitiveType.Cube);
            plane.transform.position = boundPos + new Vector3(boundSize.x, 0, boundSize.z); ;
            plane.transform.localScale = new Vector3(0.1f, boundSize.y*2.0f, 0.1f);
        }
        {
            var plane = GameObject.CreatePrimitive(PrimitiveType.Cube);
            plane.transform.position = boundPos + new Vector3(-boundSize.x, 0, boundSize.z); ;
            plane.transform.localScale = new Vector3(0.1f, boundSize.y * 2.0f, 0.1f);
        }
        {
            var plane = GameObject.CreatePrimitive(PrimitiveType.Cube);
            plane.transform.position = boundPos + new Vector3(boundSize.x, 0, -boundSize.z); ;
            plane.transform.localScale = new Vector3(0.1f, boundSize.y * 2.0f, 0.1f);
        }
        {
            var plane = GameObject.CreatePrimitive(PrimitiveType.Cube);
            plane.transform.position = boundPos + new Vector3(-boundSize.x, 0, -boundSize.z); ;
            plane.transform.localScale = new Vector3(0.1f, boundSize.y * 2.0f, 0.1f);
        }
    }
}
