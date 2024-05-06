using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Mathematics;

public static class Lines
{

    public static Color[] linesTexture(int mapChunkSizeEnlarged, Texture2D noiseMap, int numOfLines, int thicknessOfLines, Color colorOfLine, Color colorOfTerrain, int elevationDeviationStepLimit, float elevationDeviationLimit, int normalizationStepLimit, float slopeAmount, bool lineCurve)
    {
        Color[] colorMap = new Color[mapChunkSizeEnlarged * mapChunkSizeEnlarged];


        Color[] mapEnlarged = noiseMap.GetPixels();




        int spacingDifferenceHalf = 0;


        bool stop = false;


        float spacing = ((mapChunkSizeEnlarged - numOfLines) - (numOfLines * (thicknessOfLines * 2f))) / (numOfLines + 1);

        if (spacing != (int)(spacing))
        {
            //get the difference that is rounded down and lost in every space between lines and multiple it to get total amount and then divided it by two to add to the sides of the map
            float spacingDifference = (spacing - math.floor(spacing));
            spacing = spacing - spacingDifference;
            spacingDifference = spacingDifference * (numOfLines + 1);


            if (spacingDifference >= 2)
            {
                spacingDifferenceHalf = (int)math.floor(spacingDifference / 2);
            }

        }


        int spacingCount = 0;
        int lineCount = 0;



        for (int i = spacingDifferenceHalf; i < mapChunkSizeEnlarged; i++)
        {






            spacingCount += 1;






            if ((spacingCount >= (spacing + 1)) && (spacingCount <= spacing + (1 + (thicknessOfLines * 2))) && (lineCount < numOfLines))
            {
                float previousVal = 0;
                bool deviation = false;
                int deviationCount = 0;

                //1 normal, 2  searching for peak, 3 rotating around
                int mode = 1;


                if (lineCurve)

                {

                    for (int x = 0; x < mapChunkSizeEnlarged; x++)
                    {

                        if (stop)
                        {

                            break;
                        }


                        switch (mode)
                        {

                            case 1:




                                //compare previous values to current ones if current one exceeds elevation limits for certain amount of steps without getting back to the base value start moving around 
                                if (x == 0)
                                {

                                    previousVal = (float)mapEnlarged[i * mapChunkSizeEnlarged + x].grayscale;
                                    colorMap[i * mapChunkSizeEnlarged + x] = colorOfLine;
                                }
                                else
                                {
                                    if (deviation == false)
                                    {

                                        float currentVal = (float)mapEnlarged[i * mapChunkSizeEnlarged + x].grayscale; ;
                                        Debug.Log(x);
                                        Debug.Log("previous" + previousVal);
                                        Debug.Log("current" + currentVal);



                                        if (currentVal > previousVal + elevationDeviationLimit)
                                        {

                                            deviation = true;
                                            deviationCount = deviationCount + 1;

                                            colorMap[i * mapChunkSizeEnlarged + x] = Color.red;

                                            Debug.Log("deviating");
                                        }
                                        else
                                        {

                                            previousVal = currentVal;
                                            colorMap[i * mapChunkSizeEnlarged + x] = colorOfLine;

                                        }




                                    }
                                    else
                                    {
                                        float currentVal = (float)mapEnlarged[i * mapChunkSizeEnlarged + x].grayscale; ;

                                        if (previousVal + slopeAmount < currentVal)
                                        {




                                            //start rotating around and backtrack 

                                            deviation = false;
                                            x = x - deviationCount;
                                            deviationCount = 0;
                                            // check the highest value of the peak
                                            mode = 2;
                                            Debug.Log("deviated");
                                            colorMap[i * mapChunkSizeEnlarged + x] = Color.green;
                                            Debug.Log("yes");
                                            stop = true;
                                            break;







                                        }
                                        else
                                        {

                                            if (deviationCount == elevationDeviationStepLimit)
                                            {


                                                deviationCount = 0;
                                                deviation = false;

                                                colorMap[i * mapChunkSizeEnlarged + x] = colorOfLine;
                                                x = x - elevationDeviationStepLimit;

                                                previousVal = (float)mapEnlarged[i * mapChunkSizeEnlarged + x].grayscale;
                                                Debug.Log("returned");

                                            }
                                            else
                                            {

                                                deviationCount = deviationCount + 1;

                                                colorMap[i * mapChunkSizeEnlarged + x] = colorOfLine;

                                            }

                                        }
                                    }
                                }
                                break;
                            //check for the peak coordinate
                            case 2:

                                float peakValue = previousVal;
                                ArrayList peakCoordinates = new ArrayList();
                                float previousVal2 = previousVal;
                                bool goingDownDeviation = false;
                                bool actuallyGoingDown = false;
                                int goingDownCount = 0;
                                bool bottom = false;
                                int normalizationCount = 0;


                                int old = x;



                                for (int o = x; o < mapChunkSizeEnlarged; o++)
                                {
                                    //if ( old  > o - 5  || old +1 < o  )
                                    //{

                                    //    Debug.Log(goingDownCount);
                                    //    break;
                                    //}

                                    Debug.Log(o);

                                    float currentVal2 = (float)mapEnlarged[i * mapChunkSizeEnlarged + o].grayscale;




                                    if (goingDownDeviation == false)
                                    {



                                        if (currentVal2 < previousVal2)
                                        {
                                            goingDownDeviation = true;
                                            goingDownCount = goingDownCount + 1;



                                        }
                                        else
                                        {

                                            previousVal2 = currentVal2;

                                        }

                                        colorMap[i * mapChunkSizeEnlarged + o] = Color.green;
                                        ArrayList t = getMax(peakValue, currentVal2, peakCoordinates, o, i);
                                        peakValue = (float)t[0];
                                        peakCoordinates = (ArrayList)t[1];


                                    }

                                    //check for when going actually down over the peak
                                    else if (actuallyGoingDown == false)
                                    {


                                        if (previousVal2 - slopeAmount > currentVal2)
                                        {

                                            actuallyGoingDown = true;
                                            o = o - goingDownCount;

                                            goingDownCount = 0;


                                        }
                                        else
                                        {
                                            if (goingDownCount >= elevationDeviationStepLimit)
                                            {

                                                goingDownCount = 0;
                                                goingDownDeviation = false;
                                                previousVal2 = currentVal2;
                                                o = o - elevationDeviationStepLimit;
                                            }
                                            else
                                            {
                                                goingDownCount = goingDownCount + 1;
                                            }

                                        }



                                    }

                                    //we are going down now we need to check for when we are leveled
                                    else
                                    {




                                        if (bottom == false)
                                        {
                                            if (currentVal2 >= previousVal2)
                                            {
                                                bottom = true;
                                                normalizationCount = normalizationCount + 1;
                                                colorMap[i * mapChunkSizeEnlarged + o] = Color.green;
                                                Debug.Log("yes");
                                                break;
                                            }
                                            else
                                            {
                                                previousVal2 = currentVal2;
                                                colorMap[i * mapChunkSizeEnlarged + o] = Color.green;

                                            }
                                        }
                                        else
                                        {



                                            if (currentVal2 + elevationDeviationLimit >= previousVal2)
                                            {
                                                normalizationCount = normalizationCount + 1;
                                                colorMap[i * mapChunkSizeEnlarged + o] = Color.green;



                                                if (normalizationCount >= normalizationStepLimit)
                                                {
                                                    //we are at the bottom

                                                    // set color to normal because we colored over 
                                                    for (int g = o - normalizationStepLimit; g < o; g++)
                                                    {
                                                        colorMap[i * mapChunkSizeEnlarged + o] = colorOfLine;
                                                    }

                                                    // set color of peak to red
                                                    for (int g = 0; g < peakCoordinates.Count; g++)
                                                    {

                                                        int[] coord = (int[])(peakCoordinates[g]);

                                                        colorMap[coord[1] * mapChunkSizeEnlarged + coord[0]] = Color.red; ;
                                                    }

                                                    o = o - normalizationStepLimit;
                                                    mode = 1;
                                                    x = o;

                                                }


                                            }
                                            else
                                            {
                                                bottom = false;
                                                normalizationCount = 0;
                                                colorMap[i * mapChunkSizeEnlarged + o] = Color.green;
                                            }

                                        }
                                    }


                                }
                                mode = 1;
                                stop = true;
                                Debug.Log("break!");
                                break;



                        }
                    }

                }
                //end of the line thickness
                if (spacingCount == spacing + (1 + (thicknessOfLines * 2)))
                {
                    lineCount += 1;
                    spacingCount = 0;
                }




            }

            // not colored line fill with diffeent color
            else
            {
                for (int x = 0; x < mapChunkSizeEnlarged; x++)
                {

                    colorMap[i * mapChunkSizeEnlarged + x] = colorOfTerrain;
                }
            }




        }


        return colorMap;

    }

    static ArrayList getMax(float maxVal, float currentVal, ArrayList coordinateList, int x, int y)
    {
        if (currentVal > maxVal)
        {

            maxVal = currentVal;
            coordinateList = new ArrayList();
            int[] coordinates = { x, y };



            coordinateList.Add(new int[] { x, y });
        }
        else if (currentVal == maxVal)
        {
            coordinateList.Add(new int[] { x, y });

        }




        return new ArrayList { maxVal, coordinateList };
    }
}
