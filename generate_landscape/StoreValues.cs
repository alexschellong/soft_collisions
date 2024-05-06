using System;
using System.Collections;
using UnityEngine;


public struct StoreValues
{
    public static string makeString<T>(T x, int type)
    {

        string string1 = "";
        if (type == 1)
        {

            Vector3[] o = x as Vector3[];
            Debug.Log(o.Length);

            for (int i = 0; i < o.Length; i++)
            {

                string1 += o[i].x;
                string1 += ",";
                string1 += o[i].y;
                string1 += ",";
                string1 += o[i].z;

            }



        }
        else if (type == 2)
        {
            int[] o = x as int[];

            for (int i = 0; i < o.Length; i++)
            {

                string1 += o[i];
                string1 += ",";
            }


        }
        else if (type == 3)
        {
            Vector2[] o = x as Vector2[];

            for (int i = 0; i < o.Length; i++)
            {

                string1 += "[";
                string1 += o[i].x;
                string1 += ",";
                string1 += o[i].y;
                string1 += "]";
                string1 += ",";

            }


        }
        else
        {
            Debug.Log("error");
        }

        return string1;
    }










    public static T stringToValue<T>(string x, int type)
    {


        //vector3
        if (type == 1)
        {
            //because 120 * 120 meshdata size
            Vector3[] vertices = new Vector3[14400];

            Vector3 result = new Vector3();
            int count = 0;
            int vectorCount = 0;
            string number = "";
            for (int i = 0; i < x.Length - 1; i++)
            {

                if (x[i].ToString() != ",")
                {
                    number += x[i];
                }
                else
                {
                    if (count == 0)
                    {
                        result.x = float.Parse(number);
                        count += 1;

                    }
                    else if (count == 1)
                    {
                        result.y = float.Parse(number);
                        count += 1;

                    }
                    else if (count == 2)
                    {
                        result.z = float.Parse(number);

                        //restart process
                        vertices[vectorCount] = result;
                        result = new Vector3();
                        count = 0;
                        vectorCount += 1;
                    }
                    else
                    {

                        Debug.Log("error");
                    }


                    number = "";
                }
            }

            return (T)Convert.ChangeType(vertices, typeof(T));

        }
        //int
        else if (type == 2)
        {
            //because 119*119*6 meshdata size
            int[] triangles = new int[84966];
            string number = "";
            int intCount = 0;

            for (int i = 0; i < x.Length - 1; i++)
            {

                if (x[i].ToString() != ",")
                {
                    number += x[i];
                }
                else
                {
                    triangles[intCount] = int.Parse(number);
                    number = "";
                    intCount += 1;
                }


            }

            return (T)Convert.ChangeType(triangles, typeof(T));

        }
        //vector2
        else if (type == 3)
        {
            Vector2[] uvs = new Vector2[14400];
            int count = 0;
            int vectorCount = 0;
            string number = "";
            Vector2 result = new Vector2();
            for (int i = 0; i < x.Length - 1; i++)
            {

                if (x[i].ToString() != ",")
                {
                    number += x[i];
                }
                else
                {

                    if (count == 0)
                    {
                        result.x = float.Parse(number);
                        count += 1;

                    }
                    else if (count == 1)
                    {
                        result.y = float.Parse(number);

                        uvs[vectorCount] = result;
                        result = new Vector2();
                        count = 0;
                        vectorCount += 1;

                    }
                    else
                    {
                        Debug.Log("error");

                    }
                    number = "";


                }

            }

            return (T)Convert.ChangeType(uvs, typeof(T));
        }
        else
        {
            Debug.Log("error");

            return (T)Convert.ChangeType(null, typeof(T)); ;

        }



    }




    //create a function that turns vector3 array into array of floats
    public static float[] vector3ArrayToFloatArray(Vector3[] x)
    {
        float[] result = new float[x.Length * 3];
        int count = 0;
        for (int i = 0; i < x.Length; i++)
        {
            result[count] = x[i].x;
            result[count + 1] = x[i].y;
            result[count + 2] = x[i].z;
            count += 3;
        }
        return result;
    }


    //create a function that turns array of floats into vector3 array

    public static Vector3[] floatArrayToVector3Array(float[] x)
    {
        Vector3[] result = new Vector3[x.Length / 3];
        int count = 0;
        for (int i = 0; i < x.Length / 3; i++)
        {
            result[i] = new Vector3(x[count], x[count + 1], x[count + 2]);
            count += 3;
        }
        return result;
    }

    //create a function that turns vector2 array into array of floats
    public static float[] vector2ArrayToFloatArray(Vector2[] x)
    {
        float[] result = new float[x.Length * 2];
        int count = 0;
        for (int i = 0; i < x.Length; i++)
        {
            result[count] = x[i].x;
            result[count + 1] = x[i].y;
            count += 2;
        }
        return result;
    }

    //create a function that turns array of floats into vector2 array
    public static Vector2[] floatArrayToVector2Array(float[] x)
    {
        Vector2[] result = new Vector2[x.Length / 2];
        int count = 0;
        for (int i = 0; i < x.Length / 2; i++)
        {
            result[i] = new Vector2(x[count], x[count + 1]);
            count += 2;
        }
        return result;
    }




}
