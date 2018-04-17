using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;
using ANT_Managed_Library;
using System.Text;
using System.Collections.Generic;


/*
 * AntManager
 *
 * Call the Init method from your script and then open a channel. 
 * Ant responses and received DATA are queued when received
 * and dequeued in the update loop by triggering events you can register on your script
 */

public class SerialError
{
    public ANT_Device sender;
    public ANT_Device.serialErrorCode error;
    public bool isCritical;

    public SerialError(ANT_Device sender, ANT_Device.serialErrorCode error, bool isCritical)
    {
        this.sender = sender;
        this.error = error;
        this.isCritical = isCritical;
    }
}

public class AntManager : MonoBehaviour
{
    static AntManager _instance;

    public static AntManager Instance
    {
        get
        {
            if (_instance == null)
                return _instance = (new GameObject("AntManager")).AddComponent<AntManager>();
            else
                return _instance;
        }
    }

   
	readonly byte[] NETWORK_KEY = { 0xB9, 0xA5, 0x21, 0xFB, 0xBD, 0x72, 0xC3, 0x45 }; // COPY THE CORRECT NETWORK KEY HERE

    /*
    * To obtain the network key:
    * register on https://www.thisisant.com/register/
    * Once your basic user account is activated, login and go to your MyANT+ page  https://www.thisisant.com/my-ant to add ANT+ Adopter,
    * search "Network Keys" on thisisant.com, we want the first key on the txt
    */


    public ANT_Device device;
    Queue<ANT_Response> messageQueue;
    public delegate void OnDeviceResponse(ANT_Response response);
    public event OnDeviceResponse onDeviceResponse; //ant response event
    bool logResponse = false;
    public delegate void OnSerialError(SerialError error);
    public event OnSerialError onSerialError;
    Queue<SerialError> errorQueue;
    public List<AntChannel> channelList;

    void Awake()
    {
        // Sets this to not be destroyed when reloading scene
        DontDestroyOnLoad(gameObject);
    }
    void Update()
    {

        if (messageQueue != null && messageQueue.Count > 0)
        {
            if (onDeviceResponse != null)
                onDeviceResponse(messageQueue.Dequeue());
        }

        if (onSerialError != null && errorQueue.Count > 0)
            onSerialError(errorQueue.Dequeue());

    }


    public void Init()
    {
        messageQueue = new Queue<ANT_Response>(16);
        errorQueue = new Queue<SerialError>(16);
        channelList = new List<AntChannel>();

        //init the device
        if (device == null)
        {
            device = new ANT_Device();
            device.deviceResponse += new ANT_Device.dDeviceResponseHandler(DeviceResponse);
            device.serialError += new ANT_Device.dSerialErrorHandler(SerialErrorHandler);
            device.ResetSystem();
            device.setNetworkKey(0, NETWORK_KEY, 500);
        }

    }

    public void Reconnect(ANT_Device previousDevice)
    {
        device = previousDevice;
        device.deviceResponse += new ANT_Device.dDeviceResponseHandler(DeviceResponse);
        device.serialError += new ANT_Device.dSerialErrorHandler(SerialErrorHandler);
        device.ResetSystem();
        device.setNetworkKey(0, NETWORK_KEY, 500);

    }
    public AntChannel OpenChannel(ANT_ReferenceLibrary.ChannelType channelType, byte userChannel, ushort deviceNum, byte deviceType, byte transType, byte radioFreq, ushort channelPeriod, bool pairing)
    {
        AntChannel channel = this.gameObject.AddComponent<AntChannel>();
        channelList.Add(channel);
        channel.ConfigureAnt(channelType, userChannel, deviceNum, deviceType, transType, radioFreq, channelPeriod, pairing);
        return channel;
    }

    public AntChannel OpenBackgroundScanChannel(byte userChannel)
    {
        AntChannel channel = this.gameObject.AddComponent<AntChannel>();
        channelList.Add(channel);
        channel.ConfigureScan(userChannel);
        return channel;
    }



    /*
    * DeviceResponse
    * Called whenever a message is received from ANT unless that message is a 
    * channel event message. 
    * response: ANT message
    */

    void DeviceResponse(ANT_Response response)
    {
        if (response.responseID == (byte)ANT_ReferenceLibrary.ANTMessageID.RESPONSE_EVENT_0x40)
            messageQueue.Enqueue(response);
    }

    void SerialErrorHandler(ANT_Device sender, ANT_Device.serialErrorCode error, bool isCritical)
    {
        if (onSerialError != null)
        {
            SerialError serialError = new SerialError(sender, error, isCritical);
            errorQueue.Enqueue(serialError);


        }

    }

    void OnApplicationQuit()
    {
        //dispose the device on app quit or the application will freeze
        if (device != null)
            device.Dispose();
    }
}
