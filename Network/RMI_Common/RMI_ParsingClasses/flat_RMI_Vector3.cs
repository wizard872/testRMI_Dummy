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

public struct flat_RMI_Vector3 : IFlatbufferObject
{
  private Table __p;
  public ByteBuffer ByteBuffer { get { return __p.bb; } }
  public static flat_RMI_Vector3 GetRootAsflat_RMI_Vector3(ByteBuffer _bb) { return GetRootAsflat_RMI_Vector3(_bb, new flat_RMI_Vector3()); }
  public static flat_RMI_Vector3 GetRootAsflat_RMI_Vector3(ByteBuffer _bb, flat_RMI_Vector3 obj) { return (obj.__assign(_bb.GetInt(_bb.Position) + _bb.Position, _bb)); }
  public void __init(int _i, ByteBuffer _bb) { __p.bb_pos = _i; __p.bb = _bb; }
  public flat_RMI_Vector3 __assign(int _i, ByteBuffer _bb) { __init(_i, _bb); return this; }

  public short XPos { get { int o = __p.__offset(4); return o != 0 ? __p.bb.GetShort(o + __p.bb_pos) : (short)0; } }
  public short YPos { get { int o = __p.__offset(6); return o != 0 ? __p.bb.GetShort(o + __p.bb_pos) : (short)0; } }
  public short ZPos { get { int o = __p.__offset(8); return o != 0 ? __p.bb.GetShort(o + __p.bb_pos) : (short)0; } }

  public static Offset<flat_RMI_Vector3> Createflat_RMI_Vector3(FlatBufferBuilder builder,
      short xPos = 0,
      short yPos = 0,
      short zPos = 0) {
    builder.StartObject(3);
    flat_RMI_Vector3.AddZPos(builder, zPos);
    flat_RMI_Vector3.AddYPos(builder, yPos);
    flat_RMI_Vector3.AddXPos(builder, xPos);
    return flat_RMI_Vector3.Endflat_RMI_Vector3(builder);
  }

  public static void Startflat_RMI_Vector3(FlatBufferBuilder builder) { builder.StartObject(3); }
  public static void AddXPos(FlatBufferBuilder builder, short xPos) { builder.AddShort(0, xPos, 0); }
  public static void AddYPos(FlatBufferBuilder builder, short yPos) { builder.AddShort(1, yPos, 0); }
  public static void AddZPos(FlatBufferBuilder builder, short zPos) { builder.AddShort(2, zPos, 0); }
  public static Offset<flat_RMI_Vector3> Endflat_RMI_Vector3(FlatBufferBuilder builder) {
    int o = builder.EndObject();
    return new Offset<flat_RMI_Vector3>(o);
  }
    public static Offset<flat_RMI_Vector3> Createflat_RMI_Vector3(FlatBufferBuilder builder,
 RMI_Vector3 data) {
        return Createflat_RMI_Vector3(builder , data.xPos, data.yPos, data.zPos);
    }

    public static byte[] Createflat_RMI_Vector3(RMI_Vector3 data) {
        FlatBufferBuilder fbb = new FlatBufferBuilder(512);
        fbb.Finish(flat_RMI_Vector3.Createflat_RMI_Vector3(fbb, data).Value);
        byte[] result = fbb.SizedByteArray();
        fbb = null;
        return result;
    }

    public static RMI_Vector3 GetRootAsflat_RMI_Vector3(byte[] data) {
        return new RMI_Vector3(flat_RMI_Vector3.GetRootAsflat_RMI_Vector3( new ByteBuffer(data) ) );
    }

}};