using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEditor;


public class RandomPattern : MonoBehaviour
{
    public Texture2D ImageInput;
    private Texture2D ImageOutput = null;

    public int DivX = 10;
    public int DivY = 10;
    public float NoiseAmplitudeRatio = 0.1f;
    private int width = 0;
    private int height = 0;

    private int xx = 0;
    private int yy = 0;


    private string ImageInputName = "";
    private string PrevImageInputName = "";
    private int PrevDivX = 10;
    private int PrevDivY = 10;
    private float PrevNoiseAmplitudeRatio = 0.1f;


    // Start is called before the first frame update
    void Start()
    {
        Color32 c;
        width = ImageInput.width;
        height = ImageInput.height;
        xx = width / DivX;
        yy = height / DivY;

        PrevImageInputName = ImageInput.name.ToString();
        PrevDivX = DivX;
        PrevDivY = DivY;
        PrevNoiseAmplitudeRatio = NoiseAmplitudeRatio;


        //Debug.Log("xx,yy = " + xx +"," +yy );

        if ((DivX>0) && (DivX<=512) && (DivY>0) && (DivY<=512) && (width>0) && (height>0) && (xx>0) && (yy>0) ) {

            ImageOutput = new Texture2D(width, height, TextureFormat.RGBA32, false);
            //ImageOutput.wrapMode = TextureWrapMode.Clamp;

            for (int j=0; j<(DivY+2); j++ )
            {
                for(int i=0; i<(DivX+2); i++ )
                {
                    float CorrectionRatio = 1.0f - Random.value * NoiseAmplitudeRatio;

                    //Debug.Log("CorrectionRatio: "+ CorrectionRatio);

                    for (int x=0; x<xx; x++)
                    {
                        for (int y = 0; y < yy; y++)
                        {
                            float cc1;
                            int xxx = x + i * xx;
                            int yyy = y + j * yy;
                            //Debug.Log("xxx,yyy = " + xxx + "," + yyy);

                            if ((xxx<width) && (yyy<height)) { 
                                c = ImageInput.GetPixel(xxx, yyy);

                                cc1 = (float)c.r * CorrectionRatio;
                                c.r = (byte)cc1;

                                cc1 = (float)c.g * CorrectionRatio;
                                c.g = (byte)cc1;

                                cc1 = (float)c.b * CorrectionRatio;
                                c.b = (byte)cc1;

                                // c.a = c.a;
                                ImageOutput.SetPixel(xxx, yyy, c);
                            }
                        }

                    }
                }
            }
            
            ImageOutput.Apply();
            GetComponent<Renderer>().material.mainTexture = ImageOutput;
            onSave();

        } else
        {
            Debug.Log( "Parameter: Out of Range! SKIP" );

        }
       
    }

    // Update is called once per frame
    void Update()
    {
        Color32 c;

        ImageInputName = ImageInput.name.ToString();
        if (
                (PrevDivX != DivX) || 
                (PrevDivY != DivY) || 
                (PrevNoiseAmplitudeRatio != NoiseAmplitudeRatio) 
                || (!PrevImageInputName.Equals(ImageInputName))
            ) { 
            PrevImageInputName = ImageInputName;
            PrevDivX = DivX;
            PrevDivY = DivY;
            PrevNoiseAmplitudeRatio = NoiseAmplitudeRatio;

            width = ImageInput.width;
            height = ImageInput.height;
            xx = width / DivX;
            yy = height / DivY;


            if ((DivX > 0) && (DivX <= 512) && (DivY > 0) && (DivY <= 512) && (width > 0) && (height > 0) && (xx > 0) && (yy > 0))
             {

                for (int j = 0; j < (DivY+2); j++)
                {
                    for (int i = 0; i < (DivX+2); i++ )
                    {
                        float CorrectionRatio = 1.0f - Random.value * NoiseAmplitudeRatio;
                    

                        for (int x = 0; x < xx; x++)
                        {
                            for (int y = 0; y < yy; y++)
                            {
                                float cc2;
                                int xxx = x + i * xx;
                                int yyy = y + j * yy;
                                //Debug.Log("xxx,yyy = " + xxx + "," + yyy);

                                if ((xxx < width) && (yyy < height))
                                {
                                    c = ImageInput.GetPixel(xxx, yyy);

                                    cc2 = (float)c.r * CorrectionRatio;
                                    c.r = (byte)cc2;

                                    cc2 = (float)c.g * CorrectionRatio;
                                    c.g = (byte)cc2;

                                    cc2 = (float)c.b * CorrectionRatio;
                                    c.b = (byte)cc2;

                                    // c.a = c.a;
                                    ImageOutput.SetPixel(xxx, yyy, c);
                                }
                            }

                        }
                    }
                }
                ImageOutput.Apply();
                GetComponent<Renderer>().material.mainTexture = ImageOutput;
                onSave();
            }
            else
            {
                Debug.Log("Parameter: Out of Range! SKIP");

            }
        }

    }

    void onSave()
    {
        string FileName = "test";

        var path2 = string.Format("Export/{0}.png", FileName);

        string path = Path.Combine(Application.dataPath, path2);
               
        Directory.CreateDirectory(Path.GetDirectoryName(path));
        File.WriteAllBytes(path, ImageOutput.EncodeToPNG());
        AssetDatabase.Refresh();

    }


}
