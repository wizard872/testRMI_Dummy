// <auto-generated>
//  automatically generated by the FlatBuffers compiler, do not modify
// </auto-generated>
using Network.RMI_Common.RMI_ParsingClasses;
using System.Collections.Generic;
using System;


namespace Network.RMI_Common.RMI_ParsingClasses
{

using global::System;
using global::FlatBuffers;

public struct flat_heartBeatCheck_Response : IFlatbufferObject
{
  private Table __p;
  public ByteBuffer ByteBuffer { get { return __p.bb; } }
  public static flat_heartBeatCheck_Response GetRootAsflat_heartBeatCheck_Response(ByteBuffer _bb) { return GetRootAsflat_heartBeatCheck_Response(_bb, new flat_heartBeatCheck_Response()); }
  public static flat_heartBeatCheck_Response GetRootAsflat_heartBeatCheck_Response(ByteBuffer _bb, flat_heartBeatCheck_Response obj) { return (obj.__assign(_bb.GetInt(_bb.Position) + _bb.Position, _bb)); }
  public void __init(int _i, ByteBuffer _bb) { __p.bb_pos = _i; __p.bb = _bb; }
  public flat_heartBeatCheck_Response __assign(int _i, ByteBuffer _bb) { __init(_i, _bb); return this; }

  public float TimeData { get { int o = __p.__offset(4); return o != 0 ? __p.bb.GetFloat(o + __p.bb_pos) : (float)0.0f; } }

  public static Offset<flat_heartBeatCheck_Response> Createflat_heartBeatCheck_Response(FlatBufferBuilder builder,
      float timeData = 0.0f) {
    builder.StartObject(1);
    flat_heartBeatCheck_Response.AddTimeData(builder, timeData);
    return flat_heartBeatCheck_Response.Endflat_heartBeatCheck_Response(builder);
  }

  public static void Startflat_heartBeatCheck_Response(FlatBufferBuilder builder) { builder.StartObject(1); }
  public static void AddTimeData(FlatBufferBuilder builder, float timeData) { builder.AddFloat(0, timeData, 0.0f); }
  public static Offset<flat_heartBeatCheck_Response> Endflat_heartBeatCheck_Response(FlatBufferBuilder builder) {
    int o = builder.EndObject();
    return new Offset<flat_heartBeatCheck_Response>(o);
  }
    public static Offset<flat_heartBeatCheck_Response> Createflat_heartBeatCheck_Response(FlatBufferBuilder builder,
 heartBeatCheck_Response data) {
        return Createflat_heartBeatCheck_Response(builder , data.timeData);
    }

    public static byte[] Createflat_heartBeatCheck_Response(heartBeatCheck_Response data) {
        FlatBufferBuilder fbb = new FlatBufferBuilder(512);
        fbb.Finish(flat_heartBeatCheck_Response.Createflat_heartBeatCheck_Response(fbb, data).Value);
        byte[] result = fbb.SizedByteArray();
        fbb = null;
        return result;
    }

    public static heartBeatCheck_Response GetRootAsflat_heartBeatCheck_Response(byte[] data) {
        return new heartBeatCheck_Response(flat_heartBeatCheck_Response.GetRootAsflat_heartBeatCheck_Response( new ByteBuffer(data) ) );
    }

}};