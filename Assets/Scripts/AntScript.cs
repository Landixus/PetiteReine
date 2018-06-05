using UnityEngine;
using ANT_Managed_Library;
using System;
using Dynastream.Fit;

/**
 *  This script provides a simple Interface for the Ant Manager
 *  it needs a CyclistMovement script and will update continuously its forward value
 *  It is simple to set the resistance through this interface
 *  /!\ this script is dependant of the SetForward() method of CyclistMovement
 **/
public class AntScript : MonoBehaviour {

    CyclistMovement cyclistScript;

    AntChannel backgroundScanChannel;
    AntChannel FECChannel;

    byte transType;

    //variable use for speed display
    int prevRev2;
    int prevMeasTime2 = 0;
    int stoppedCounter2 = 0;

    ushort deviceNumber;
 
    public bool isOn = false;
    public float dt;
    private float rpm;
    private float previousrpm=0f;
    private float acceleration = 0f;
    private float pt=0;
    private float t;
    public void Activate()
    {
        
        print("init device and start background scan");

        cyclistScript = GetComponent<CyclistMovement>();
        if(cyclistScript == null)
        {
            Debug.Log("no cyclist found, unable to start");
            return;
        }
        AntManager.Instance.Init();
        backgroundScanChannel = AntManager.Instance.OpenBackgroundScanChannel(0);
        backgroundScanChannel.onReceiveData += ReceivedBackgroundScanData;

        isOn = true;
    }

    public void Desactivate()
    {
        if (backgroundScanChannel)
        {
            backgroundScanChannel.Close();
            backgroundScanChannel = null;
        }
        if (FECChannel)
        {
            FECChannel.Close();
            FECChannel = null;        
        }

        isOn = false;
    }

    /** Set the resistance of the HomeTrainer to simulate a slope
     * */
    public void SetResistance(byte res)
    {
        Debug.Log("setting resistance to"+res*(0.5f/100f)+"%");
        byte[] pageToSend = new byte[8] { 0x30, 0xFF, 0xFF, 0xFF, 0xFF, 4, 55, res };//unit is 0.50%
        FECChannel.sendAcknowledgedData(pageToSend);
    }

    void ReceivedBackgroundScanData(Byte[] data)
    {

        byte deviceType = (data[12]); // extended info Device Type byte
                                      //use the Extended Message Formats to identify nodes

        switch (deviceType)
        {

            case AntplusDeviceType.FitnessEquipment:
                {
                    backgroundScanChannel.Close();

                    deviceNumber = (ushort)((data[10]) | data[11] << 8);

                    if (deviceNumber != (ushort)48550)
                    {
                        print("init device and start background scan");
                        AntManager.Instance.Init();
                        backgroundScanChannel = AntManager.Instance.OpenBackgroundScanChannel(0);
                        backgroundScanChannel.onReceiveData += ReceivedBackgroundScanData;
                    }

                    transType = data[13];
                    Debug.Log("found FEC trainer, opening channel, device number is " + deviceNumber);

                    FECChannel = AntManager.Instance.OpenChannel(ANT_ReferenceLibrary.ChannelType.BASE_Slave_Receive_0x00, 1, deviceNumber, 17, transType, 57, 8192, false);
                    FECChannel.onReceiveData += FECData;
                    FECChannel.hideRXFAIL = true;
                    break;
                }

            default:
                {

                    break;
                }
        }

    }

    public void FECData(Byte[] data)
    {

        if (data[0] == 16) //General FE Data Page
        {

            int distanceTraveled = data[3];
            //Debug.Log("distanceTraveled:" + distanceTraveled+"m");
            rpm = GetSpeed(data);
            t = Time.time;
            if (t - pt >= dt)
            {
                acceleration = (rpm - previousrpm) / (t-pt);
                previousrpm = rpm;
                pt = t;
                cyclistScript.SetForward(acceleration);
            }
            
        }
    }

    public float GetSpeed(Byte[] data)
    {
        float speed = 0;
        //speed formula as described in the ant+ device profile doc
        int currentRevCount = (data[6]) | data[7] << 8;
        int currentMeasTime = (data[4]) | data[5] << 8;
        //print (prevRev2);
        //print (currentRevCount);
        //print (currentMeasTime);
        if (prevRev2 > 0)
        {

            if (currentMeasTime != prevMeasTime2 || currentRevCount != prevRev2)
            {
                float s = (2.070f * (currentRevCount - prevRev2) * 1024) / (currentMeasTime - prevMeasTime2);
                s *= 3.6f;
                //print (s);

                speed = currentMeasTime * 1.0f / 1024 * 3.6f * 2.070f;
                stoppedCounter2 = 0;
            }
            else
            {

                stoppedCounter2++;
            }

        }

        if (stoppedCounter2 > 5 || speed < 1)

            speed = 0;


        prevRev2 = currentRevCount;
        prevMeasTime2 = currentMeasTime;


        return speed;
    }
}
