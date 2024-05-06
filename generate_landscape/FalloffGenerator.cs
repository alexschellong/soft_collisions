using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FalloffGenerator
{
    public static float clamped_remap(float input_min, float input_max, float output_min, float output_max, float value)
    {
        if (value < input_min)
        {
            return output_min;
        }
        else if (value > input_max)
        {
            return output_max;
        }
        else
        {
            return (value - input_min) / (input_max - input_min) * (output_max - output_min) + output_min;
        }
    }


    public static float[,] GenerateFalloffMap(int size, float planeSize, AnimationCurve transitionCurve)
    {



        float edge1 = (size - planeSize) / 2.0f;
        float edge2 = size - edge1;

        float[,] map = new float[size, size];

        float x = size / 2.0f - planeSize / 2.0f;
        float mostAwayDistance = Mathf.Sqrt((x * x) + (x * x));



        for (int i = 0; i < size; i++)
        {
            for (int j = 0; j < size; j++)
            {

                float value = 0;
                if ((j > edge1 && j < edge2) && (i > edge1 && i < edge2))
                {
                    //  Debug.Log("here");
                    map[i, j] = 1;
                }
                else
                {

                    //left
                    if (j < edge1)
                    {
                        //upper left
                        if (i < edge1)
                        {

                            float distanceHorizontal = (edge1 - j) * (edge1 - j);
                            float distanceVertical = (edge1 - i) * (edge1 - i);
                            value = clamped_remap(0, mostAwayDistance, 1, 0, Mathf.Sqrt(distanceHorizontal + distanceVertical));

                        }
                        // lower left
                        else if (i > edge2)
                        {


                            float distanceHorizontal = (edge1 - j) * (edge1 - j);
                            float distanceVertical = (i - edge2) * (i - edge2);
                            value = clamped_remap(0, mostAwayDistance, 1, 0, Mathf.Sqrt(distanceHorizontal + distanceVertical));

                        }

                        //left
                        else if (i > edge1 && i < edge2)
                        {

                            value = clamped_remap(0, mostAwayDistance, 1, 0, edge1 - j);

                        }

                    }
                    //right
                    else if (j > edge2)
                    {
                        //upper right
                        if (i < edge1)
                        {
                            float distanceHorizontal = (j - edge2) * (j - edge2);
                            float distanceVertical = (edge1 - i) * (edge1 - i);
                            value = clamped_remap(0, mostAwayDistance, 1, 0, Mathf.Sqrt(distanceHorizontal + distanceVertical));

                        }
                        // lower right
                        else if (i > edge2)
                        {
                            float distanceHorizontal = (j - edge2) * (j - edge2);
                            float distanceVertical = (i - edge2) * (i - edge2);
                            value = clamped_remap(0, mostAwayDistance, 1, 0, Mathf.Sqrt(distanceHorizontal + distanceVertical));

                        }

                        //right
                        else if (i > edge1 && i < edge2)
                        {
                            value = clamped_remap(0, mostAwayDistance, 1, 0, -(edge2 - j));
                        }
                    }


                    //up/down
                    else if (j > edge1 && j < edge2)
                    {

                        //  Debug.Log("here");

                        //up
                        if (i < edge1)
                        {
                            value = clamped_remap(0, mostAwayDistance, 1, 0, edge1 - i);

                        }
                        //down
                        else if (i > edge2)
                        {
                            value = clamped_remap(0, mostAwayDistance, 1, 0, -(edge2 - i));

                        }


                    }




                    map[i, j] = transitionCurve.Evaluate(value);

                }


            }

        }
        return map;
    }


    static float Evaluate(float value)
    {
        float a = 3;
        float b = 2.2f;

        return Mathf.Pow(value, a) / (Mathf.Pow(value, a) + Mathf.Pow(b - b * value, a));
    }
}
