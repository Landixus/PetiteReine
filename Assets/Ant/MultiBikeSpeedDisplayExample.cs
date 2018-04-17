using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using ANT_Managed_Library;
using System;
using Dynastream.Fit;


/*
 * MultiBikeSpeedDisplayExample
 *
 * Start a background scan 
 * and create a new BikeDisplay object
 * open a channel for every bike speed sensor found
 */

public class BikeDisplay
{
    public int deviceNumber;

    public float speed;
    //variable use for speed display
    int prevRev;
    int prevMeasTime = 0;

    int stoppedCounter = 0;
    public void GetSpeed(Byte[] data)
    {

        //speed formula as described in the ant+ device profile doc
        int currentRevCount = (data[6]) | data[7] << 8;
        int currentMeasTime = (data[4]) | data[5] << 8;

        if (prevRev > 0)
        {

            if (currentMeasTime != prevMeasTime || currentRevCount != prevRev)
            {
                float s = (2.070f * (currentRevCount - prevRev) * 1024) / (currentMeasTime - prevMeasTime);
                s *= 3.6f;

                speed = s;
                stoppedCounter = 0;
            }
            else {

                stoppedCounter++;
            }

        }

        if (stoppedCounter > 5 || speed < 1)

            speed = 0;


        prevRev = currentRevCount;
        prevMeasTime = currentMeasTime;


    }

}

public class MultiBikeSpeedDisplayExample : MonoBehaviour
{

    AntChannel backgroundScanChannel;
    List<BikeDisplay> bikeDisplayList;
    int lastChannelUsed = 0;
    // Use this for initialization
    void Start()
    {
        bikeDisplayList = new List<BikeDisplay>();

        AntManager.Instance.Init();


    }

    void StartScan()
    {
        Debug.Log("starting scan");
        backgroundScanChannel = AntManager.Instance.OpenBackgroundScanChannel(0);
        backgroundScanChannel.onReceiveData += ReceivedBackgroundScanData;

    }

    void StartDisplay()
    {
        Debug.Log("starting Speed Display");
        StartCoroutine("DisplaySpeed");

    }

    IEnumerator DisplaySpeed()
    {
        while (true)
        {
            foreach (BikeDisplay b in bikeDisplayList)
                Debug.Log("speed display #" + b.deviceNumber + ":      " + b.speed.ToString("F2") + "km/h");

            yield return new WaitForSeconds(0.5f);
        }

    }


    void ReceivedBackgroundScanData(Byte[] data)
    {

        byte deviceType = (data[12]); // extended info Device Type byte
                                      //use the Extended Message Formats to identify nodes

        switch (deviceType)
        {

            case AntplusDeviceType.BikeSpeed:
                {

                    BikeDisplay b = new BikeDisplay();
                    bikeDisplayList.Add(b);
                    b.deviceNumber = (data[10]) | data[11] << 8;
                    lastChannelUsed++;
                    AntChannel speedChannel = AntManager.Instance.OpenChannel(ANT_ReferenceLibrary.ChannelType.BASE_Slave_Receive_0x00, (byte)lastChannelUsed, 0, 123, 0, 57, 8118, false); //bike speed Display
                    speedChannel.onReceiveData += b.GetSpeed;
                    speedChannel.hideRXFAIL = true;
                    Debug.Log("found bike speed #" + bikeDisplayList.Count);

                    break;
                }

            default:
                {

                    break;
                }
        }

    }


    // Update is called once per frame
    void Update()
    {

        if (Input.GetKeyDown(KeyCode.S))
        {
            StartScan();

        }
        if (Input.GetKeyDown(KeyCode.Space))
        {
            backgroundScanChannel.Close();
            StartDisplay();

        }
    }
}
