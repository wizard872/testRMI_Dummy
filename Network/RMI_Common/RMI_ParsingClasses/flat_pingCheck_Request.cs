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

public struct flat_pingCheck_Request : IFlatbufferObject
{
  private Table __p;
  public ByteBuffer ByteBuffer { get { return __p.bb; } }
  public static flat_pingCheck_Request GetRootAsflat_pingCheck_Request(ByteBuffer _bb) { return GetRootAsflat_pingCheck_Request(_bb, new flat_pingCheck_Request()); }
  public static flat_pingCheck_Request GetRootAsflat_pingCheck_Request(ByteBuffer _bb, flat_pingCheck_Request obj) { return (obj.__assign(_bb.GetInt(_bb.Position) + _bb.Position, _bb)); }
  public void __init(int _i, ByteBuffer _bb) { __p.bb_pos = _i; __p.bb = _bb; }
  public flat_pingCheck_Request __assign(int _i, ByteBuffer _bb) { __init(_i, _bb); return this; }

  public float TimeData { get { int o = __p.__offset(4); return o != 0 ? __p.bb.GetFloat(o + __p.bb_pos) : (float)0.0f; } }

  public static Offset<flat_pingCheck_Request> Createflat_pingCheck_Request(FlatBufferBuilder builder,
      float timeData = 0.0f) {
    builder.StartObject(1);
    flat_pingCheck_Request.AddTimeData(builder, timeData);
    return flat_pingCheck_Request.Endflat_pingCheck_Request(builder);
  }

  public static void Startflat_pingCheck_Request(FlatBufferBuilder builder) { builder.StartObject(1); }
  public static void AddTimeData(FlatBufferBuilder builder, float timeData) { builder.AddFloat(0, timeData, 0.0f); }
  public static Offset<flat_pingCheck_Request> Endflat_pingCheck_Request(FlatBufferBuilder builder) {
    int o = builder.EndObject();
    return new Offset<flat_pingCheck_Request>(o);
  }
    public static Offset<flat_pingCheck_Request> Createflat_pingCheck_Request(FlatBufferBuilder builder,
 pingCheck_Request data) {
        return Createflat_pingCheck_Request(builder , data.timeData);
    }

    public static byte[] Createflat_pingCheck_Request(pingCheck_Request data) {
        FlatBufferBuilder fbb = new FlatBufferBuilder(512);
        fbb.Finish(flat_pingCheck_Request.Createflat_pingCheck_Request(fbb, data).Value);
        byte[] result = fbb.SizedByteArray();
        fbb = null;
        return result;
    }

    public static pingCheck_Request GetRootAsflat_pingCheck_Request(byte[] data) {
        return new pingCheck_Request(flat_pingCheck_Request.GetRootAsflat_pingCheck_Request( new ByteBuffer(data) ) );
    }

}};