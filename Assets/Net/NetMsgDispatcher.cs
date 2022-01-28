using System;
using System.Collections.Generic;
using System.IO;
using Sproto;
using UnityEngine;

public delegate SprotoTypeBase RpcReqHandler(SprotoTypeBase rpcReq);

public delegate void RpcRspHandler(SprotoTypeBase rpcRsp);

/// <summary>
/// rpc，发送消息，接收回应
/// </summary>
public class NetMsgDispatcher : Singleton<NetMsgDispatcher>
{
    private static ProtocolFunctionDictionary protocol = Protocol.Instance.Protocol;
    private static Dictionary<long, RpcRspHandler> rpcRspHandlerDict;
    private static Dictionary<long, ProtocolFunctionDictionary.typeFunc> sessionDict;
    private int session = 0;
    private int maxSession = System.Int32.MaxValue / 2; //C#端最大session为 xlua端的一半

    public void Init()
    {
        rpcRspHandlerDict = new Dictionary<long, RpcRspHandler>();
        sessionDict = new Dictionary<long, ProtocolFunctionDictionary.typeFunc>();
        NetworkManager.Instance.OnReceiveMsgCallBack += OnReceiveMsgFromNet;
    }

    private static void AddHandler(long session, RpcRspHandler rpcRspHandler)
    {
        rpcRspHandlerDict.Add(session, rpcRspHandler);
    }

    private static void RemoveHandler(long session)
    {
        if (rpcRspHandlerDict.ContainsKey(session))
        {
            rpcRspHandlerDict.Remove(session);
        }
    }

    /// <summary>
    /// 根据session回应服务器
    /// </summary>
    /// <param name="session"></param>
    /// <returns></returns>
    public static RpcRspHandler GetHandler(long session)
    {
        RpcRspHandler rpcRspHandler;
        rpcRspHandlerDict.TryGetValue(session, out rpcRspHandler);
        RemoveHandler(session);
        return rpcRspHandler;
    }

    /// <summary>
    /// 接收到消息则触发
    /// </summary>
    /// <param name="bytes">接收到的数据</param>
    public void OnReceiveMsgFromNet(byte[] bytes)
    {
        // Debug.Log("NetMsgHandler:OnReceiveMsgFromNet : "+bytes.Length.ToString());
        int content_size = bytes.Length - 5;
        if (content_size <= 0)
            return;
        char flag = BitConverter.ToChar(bytes, content_size);
        int cur_session = BitConverter.ToInt32(bytes, content_size + 1); //读取末尾4字节    session
        cur_session = Util.SwapInt32(cur_session);
        // Debug.Log("NetMsgHandler:OnReceiveMsgFromNet flag:"+flag.ToString()+" session:"+cur_session.ToString());

        RpcRspHandler rpcRspHandler = NetMsgDispatcher.GetHandler(cur_session);
        if (rpcRspHandler != null)
        {
            SprotoTypeBase receive_info = null;
            ProtocolFunctionDictionary.typeFunc GenResponse;
            sessionDict.TryGetValue(cur_session, out GenResponse);

            receive_info = GenResponse(bytes, 0, content_size);

            rpcRspHandler(receive_info);
        }
    }

    /// <summary>
    /// rpcRspHandler为空时不需要服务器回应
    /// </summary>
    /// <param name="rpcReq"></param>
    /// <param name="rpcRspHandler"></param>
    /// <typeparam name="T"></typeparam>
    public void SendMessage<T>(SprotoTypeBase rpcReq, RpcRspHandler rpcRspHandler = null)
    {
        session += 1;
        if (session >= maxSession)
            session = 0;
        if (rpcRspHandler != null)
        {
            AddHandler(session, rpcRspHandler);
        }

        byte[] message = rpcReq.encode(); //Sproto序列化
        MemoryStream ms = null;
        using (ms = new MemoryStream())
        {
            ms.Position = 0;
            BinaryWriter writer = new BinaryWriter(ms);
            UInt16 msglen = Util.SwapUInt16((UInt16) (message.Length + 8)); //大字节序
            writer.Write(msglen); //写入message长度
            int tag = protocol[typeof(T)];
            sessionDict.Add((long) session, protocol[tag].Response.Value);
            tag = Util.SwapInt32(tag);
            writer.Write(tag); //写入tag
            writer.Write(message); //写入message
            int swap_session = Util.SwapInt32(session);
            writer.Write(swap_session); //写入session
            writer.Flush();
            writer.Dispose();
            byte[] payload = ms.ToArray();
            NetworkManager.Instance.SendBytesWithoutSize(payload);
        }
    }

    // public void ListenMessage<T>(SprotoTypeBase rpcReq, RpcRspHandler rpcRspHandler = null)
    // {
    //     RpcRspHandler on_ack = null;
    //     on_ack = (SprotoTypeBase result) =>
    //     {
    //         rpcRspHandler(result);
    //         this.SendMessage<T>(rpcReq, on_ack);
    //     };
    //     this.SendMessage<T>(rpcReq, on_ack);
    // }
}