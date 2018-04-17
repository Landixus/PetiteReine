using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using ANT_Managed_Library;
using System;
using Dynastream.Fit;

/*
 * FitnessEquipmentExample
 *
 * Start a background scan 
 * and auto connect to an FE with the deviceNumber and transmission type
 * send resitance % to Data Page 48 (0x30) – Basic Resistance
 */

public class FitnessEquipmentExample : MonoBehaviour
{
    AntChannel backgroundScanChannel;
    AntChannel FECChannel;
  
    ushort deviceNumber;
    byte transType;
   

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

                    transType = data[13];
                    Debug.Log("found FEC trainer, opening channel, device number is "+ deviceNumber);

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

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.S))
        {
            Debug.Log("init device and start background scan");
            AntManager.Instance.Init();
            backgroundScanChannel = AntManager.Instance.OpenBackgroundScanChannel(0);
            backgroundScanChannel.onReceiveData += ReceivedBackgroundScanData;
        }
       
        if (Input.GetKeyDown(KeyCode.D)) 
        {
            Debug.Log("setting resistance to 0 %");
            byte[] pageToSend = new byte[8] { 0x30, 0xFF, 0xFF, 0xFF, 0xFF, 4, 55, 0 };
            FECChannel.sendAcknowledgedData(pageToSend);
        }
        if (Input.GetKeyDown(KeyCode.F)) 
        {
            Debug.Log("setting resistance to 100 %");
            byte[] pageToSend = new byte[8] { 0x30, 0xFF, 0xFF, 0xFF, 0xFF, 4, 55, 200 };//unit is 0.50%
            FECChannel.sendAcknowledgedData(pageToSend);
        }
		if (Input.GetKeyDown(KeyCode.E)) 
		{
			Debug.Log("setting resistance to 75 %");
			byte[] pageToSend = new byte[8] { 0x30, 0xFF, 0xFF, 0xFF, 0xFF, 4, 55, 150 };//unit is 0.50%
			FECChannel.sendAcknowledgedData(pageToSend);
		}



    }

    public void FECData(Byte[] data)
    {

        if (data[0] == 16) //General FE Data Page
        {

            int distanceTraveled = data[3];
            Debug.Log("distanceTraveled:" + distanceTraveled+"m");

        }
    }

}
