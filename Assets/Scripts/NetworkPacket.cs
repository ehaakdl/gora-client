using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;   //요게 바이너리 포매터임!

[Serializable]  //하나로 직렬화 묶겠다. 뜻? 바이트화 하겠다?
public class NetworkPacket       //모노비헤이비어는 싱글톤으로 만들거라서 여기서는 삭제
{
    private int type;
    
    private Object data;
    

    //쏘는거
    public static byte[] convertToByteArray(NetworkPacket packet)
    {
           //스트림생성 한다.  물흘려보내기
           MemoryStream stream = new MemoryStream();
        
            //스트림으로 건너온 패킷을 포맷으로 바이너리 묶어준다.
           BinaryFormatter formatter = new BinaryFormatter();

          formatter.Serialize(stream, packet.type);       //스트림에 담는다. 시리얼라이즈는 담는다는 뜻임.
          formatter.Serialize(stream, packet.key);
          formatter.Serialize(stream, packet.data);

        return stream.ToArray();
    }

    //받는거
    public static NetworkPacket convertToNetworkPacket(byte[] bytes)
    {
        //스트림 생성
        MemoryStream stream = new MemoryStream(bytes);
        //스트림으로 데이터 받을 때 바이너리 포매터 말고 다른거도 있는지 찾아보기
        //바이너리 포매터로 스트림에 떠내려온 데이터를 건져낸다.
        BinaryFormatter formatter = new BinaryFormatter();
        //패킷을 생성해서      //패킷 생성기에 대해 알아보기!
        NetworkPacket packet = new NetworkPacket();
        //생성한 패킷에 디이터를 디시리얼 라이즈해서 담는다.
        packet.type = formatter.Deserialize(stream);
        packet.data = formatter.Deserialize(stream);

        return packet;
    }

}