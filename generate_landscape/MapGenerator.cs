using UnityEngine;
using UnityEngine.Networking;
using System.Collections;
using UnityEngine;



public class MapGenerator : MonoBehaviour
{




    private float speed = 0.0003f;
    //private float speed = 0;

    public bool lines;
    public int textureMultiplier = 1;
    public int numOfLines;
    private int thicknessOfLines;
    public Color colorOfLine;
    public bool lineCurve;
    public int normalizationStepLimit = 10;
    public int elevationDeviationStepLimit = 4;
    [Range(0.0f, 1)]
    public float elevationDeviationLimit = 0.3f;
    [Range(0.0f, 1)]
    public float slopeAmount;


    public Color colorOfTerrain;

    //	public enum DrawMode { NoiseMap, ColourMap, Mesh };
    //public DrawMode drawMode;

    const int mapChunkSize = 241;
    [Range(0, 6)]
    // public int levelOfDetail;
    public float noiseScale;

    public int octaves;
    [Range(0, 1)]
    public float persistance;
    public float lacunarity;

    public int seed;
    //public Vector2 offset;
    public float interpolPerl = 0;
    public int skipFrames = 1;

    public float meshHeightMultiplier;

    public AnimationCurve meshHeightCurve;
    public AnimationCurve transitionCurve;

    public bool autoUpdate;

    public MeshFilter meshFilter;
    private MeshData[] meshData2;
    //  private MeshData meshData;
    public Renderer textureRender;


    //    public float increment = 0.01f;

    public bool useFalloff;

    float[,] falloffMap;

    public MeshRenderer plane;
    float planeSize;

    private int countOfTurns = 0;
    public int renderLoop = 100;
    private bool switch1 = false;


    public bool switchForFile = true;



    //XmlSerializer xml;

    MeshDataArray meshDataArrayPart;

    void Awake()
    {
        falloffMap = FalloffGenerator.GenerateFalloffMap(mapChunkSize, planeSize, transitionCurve);




    }





    //public TerrainType[] regions;

    private void Start()
    {


        //  meshDataArray.CreateArray(renderLoop);

        //  xml = new XmlSerializer(typeof(MeshDataArray));




        planeSize = plane.bounds.size.x;



        if (speed != 0)
        {
            if (switchForFile)
            {

                //tw = new StreamWriter("Assets/scripts/generate landscape/p0.txt");



                meshDataArrayPart = new MeshDataArray();
                meshDataArrayPart.CreateArray(100);

                int batchCount = 0;
                for (int i = 0; i < renderLoop; i++)
                {
                    interpolPerl += speed;



                    GenerateMap(i - (100 * batchCount));

                    if (i == 99 + (100 * batchCount))
                    {
                        string json = JsonUtility.ToJson(meshDataArrayPart);

                        System.IO.File.WriteAllText("Assets/StreamingAssets/playerData" + (batchCount + 1) + ".json", json);
                        meshDataArrayPart = new MeshDataArray();
                        meshDataArrayPart.CreateArray(100);

                        batchCount += 1;
                    }

                }


                meshDataArrayPart = null;

                Debug.Log("done");




                meshData2 = new MeshData[renderLoop];
                for (int i = 0; i < renderLoop; i++)
                {
                    meshData2[i] = new MeshData(mapChunkSize, mapChunkSize);

                }

                for (int i = 0; i < batchCount; i++)
                {
                    string json = System.IO.File.ReadAllText("Assets/StreamingAssets/playerData" + (i + 1) + ".json");
                    meshDataArrayPart = JsonUtility.FromJson<MeshDataArray>(json);

                    for (int j = 0; j < meshDataArrayPart.meshDataArrayArray.Length; j++)
                    {

                        meshData2[j + (i * 100)].vertices = StoreValues.floatArrayToVector3Array(meshDataArrayPart.meshDataArrayArray[j].vertices);
                        meshData2[j + (i * 100)].triangles = meshDataArrayPart.meshDataArrayArray[j].triangles;
                        meshData2[j + (i * 100)].uvs = StoreValues.floatArrayToVector2Array(meshDataArrayPart.meshDataArrayArray[j].uv);
                    }

                }
                meshDataArrayPart = null;





            }
            else
            {
                GenerateMap(0);


                int batchCount = renderLoop / 100;


                meshData2 = new MeshData[renderLoop];
                for (int i = 0; i < renderLoop; i++)
                {
                    meshData2[i] = new MeshData(mapChunkSize, mapChunkSize);

                }

                for (int i = 0; i < batchCount; i++)
                {

                    if (!System.IO.File.Exists(Application.persistentDataPath + "/playerData" + (i + 1) + ".json"))
                    {
                        string path = "";
#if (UNITY_EDITOR || UNITY_STANDALONE_LINUX)
                        path = "file://" + Application.streamingAssetsPath + "/playerData" + (i + 1) + ".json";
#endif
#if (UNITY_ANDROID && !UNITY_EDITOR && !UNITY_STANDALONE_LINUX)
                         path = Application.streamingAssetsPath + "/playerData" + (i + 1) + ".json";
#endif

                        UnityWebRequest www = UnityWebRequest.Get(path);

                        www.SendWebRequest();

                        while (!www.isDone) { }

                        byte[] loadBytes = www.downloadHandler.data;

                        System.IO.File.WriteAllBytes(Application.persistentDataPath + "/playerData" + (i + 1) + ".json", loadBytes);
                    }


                    string json = System.IO.File.ReadAllText(Application.persistentDataPath + "/playerData" + (i + 1) + ".json");
                    meshDataArrayPart = JsonUtility.FromJson<MeshDataArray>(json);

                    for (int j = 0; j < meshDataArrayPart.meshDataArrayArray.Length; j++)
                    {


                        meshData2[j + (i * 100)].vertices = StoreValues.floatArrayToVector3Array(meshDataArrayPart.meshDataArrayArray[j].vertices);
                        meshData2[j + (i * 100)].triangles = meshDataArrayPart.meshDataArrayArray[j].triangles;
                        meshData2[j + (i * 100)].uvs = StoreValues.floatArrayToVector2Array(meshDataArrayPart.meshDataArrayArray[j].uv);
                    }

                }
                meshDataArrayPart = null;





            }

            //changes by 30 frames per second
            InvokeRepeating("ChangeMesh", 0.0f, 0.03f);

        }
    }

    public void GenerateMap(int turn)
    {


        float[,] noiseMap = Noise.GenerateNoiseMap(mapChunkSize, mapChunkSize, seed, noiseScale, octaves, persistance, lacunarity, interpolPerl);




        if (useFalloff)
        {
            for (int i = 0; i < mapChunkSize; i++)
            {
                for (int x = 0; x < mapChunkSize; x++)
                {

                    noiseMap[x, i] = Mathf.Clamp01(noiseMap[x, i] - falloffMap[x, i]);


                }
            }
        }



        if (lines & turn == 0)
        {

            elevationDeviationStepLimit = elevationDeviationStepLimit * textureMultiplier;

            int mapChunkSizeEnlarged = mapChunkSize * textureMultiplier;

            Texture2D noiseTexture = TextureGenerator.TextureFromHeightMap(noiseMap);

            //Texture2D noiseTextureScaled = TextureScaler.scaled(noiseTexture, mapChunkSize * textureMultiplier, mapChunkSize * textureMultiplier);
            Texture2D scaled = new Texture2D(mapChunkSize * textureMultiplier, mapChunkSize * textureMultiplier);
            //might not work on android
            Graphics.ConvertTexture(noiseTexture, scaled);
            Texture2D noiseTextureScaled = new Texture2D(mapChunkSize * textureMultiplier, mapChunkSize * textureMultiplier);
            Rect regionToReadFrom = new Rect(0, 0, mapChunkSize * textureMultiplier, mapChunkSize * textureMultiplier);
            noiseTextureScaled.ReadPixels(regionToReadFrom, 0, 0);
            noiseTextureScaled.Apply();

            Color[] colorMap = Lines.linesTexture(mapChunkSizeEnlarged, noiseTextureScaled, numOfLines, thicknessOfLines, colorOfLine, colorOfTerrain, elevationDeviationStepLimit, elevationDeviationLimit, normalizationStepLimit, slopeAmount, lineCurve);
            Texture g = TextureGenerator.TextureFromColourMap(colorMap, mapChunkSizeEnlarged, mapChunkSizeEnlarged);

            textureRender.transform.localScale = new Vector3(noiseTexture.width, 1, noiseTexture.height);
            textureRender.sharedMaterial.mainTexture = g;
        }



        //Texture g = TextureGenerator.TextureFromHeightMap(falloffMap);


        //textureRender.transform.localScale = new Vector3(noiseTexture.width, 1, noiseTexture.height);
        MeshData meshData = MeshGenerator.GenerateTerrainMesh(noiseMap, meshHeightMultiplier, meshHeightCurve);
        if (switchForFile)
        {

            MeshData3 meshData3 = new MeshData3();
            meshData3.vertices = StoreValues.vector3ArrayToFloatArray(meshData.vertices);
            meshData3.triangles = meshData.triangles;
            meshData3.uv = StoreValues.vector2ArrayToFloatArray(meshData.uvs);


            meshDataArrayPart.meshDataArrayArray[turn] = meshData3;
        }

        // tw.WriteLine(StoreValues.makeString<Vector3[]>(meshData2[turn].vertices,1));



        //tw.WriteLine(StoreValues.makeString<int[]>(meshData2[turn].triangles,2));
        //tw.WriteLine(StoreValues.makeString<Vector2[]>(meshData2[turn].uvs,3));





        if (turn == 0)
        {
            meshFilter.sharedMesh = meshData.CreateMesh();

        }









    }

    void OnValidate()
    {
        if (lacunarity < 1)
        {
            lacunarity = 1;
        }
        if (octaves < 0)
        {
            octaves = 0;
        }

        falloffMap = FalloffGenerator.GenerateFalloffMap(mapChunkSize, planeSize, transitionCurve);
    }



    private void ChangeMesh()
    {



        meshFilter.sharedMesh.vertices = meshData2[countOfTurns].vertices;
        meshFilter.sharedMesh.triangles = meshData2[countOfTurns].triangles;
        meshFilter.sharedMesh.uv = meshData2[countOfTurns].uvs;

        if (switch1 == false)
        {



            meshFilter.sharedMesh.RecalculateNormals();
            countOfTurns += 1;
            if (countOfTurns > renderLoop - 1)
            {
                switch1 = true;
                countOfTurns -= 1;
            }
        }
        else
        {

            //meshFilter.sharedMesh.vertices = meshData2[countOfTurns].vertices;
            //meshFilter.sharedMesh.triangles = meshData2[countOfTurns].triangles;
            //meshFilter.sharedMesh.uv = meshData2[countOfTurns].uvs;
            meshFilter.sharedMesh.RecalculateNormals();
            countOfTurns -= 1;
            if (countOfTurns < 1)
            {
                switch1 = false;
            }

        }

    }








}



[System.Serializable]
public class MeshData3
{
    public float[] vertices;
    public int[] triangles;
    public float[] uv;
}
public class MeshDataArray
{
    //public MeshData3[] meshDataArrayArray = new MeshData3[10];
    public MeshData3[] meshDataArrayArray;

    public void CreateArray(int length)
    {
        meshDataArrayArray = new MeshData3[length];
    }




}









