// <auto-generated>
//  automatically generated by the FlatBuffers compiler, do not modify
// </auto-generated>
using Network.RMI_Common.RMI_ParsingClasses;
using FlatBuffers;
using System.Collections.Generic;
using System;


namespace Network.RMI_Common.RMI_ParsingClasses
{

using global::System;
using global::FlatBuffers;

public struct flat_RMI_Send_EncryptedAccept_Data : IFlatbufferObject
{
  private Table __p;
  public ByteBuffer ByteBuffer { get { return __p.bb; } }
  public static flat_RMI_Send_EncryptedAccept_Data GetRootAsflat_RMI_Send_EncryptedAccept_Data(ByteBuffer _bb) { return GetRootAsflat_RMI_Send_EncryptedAccept_Data(_bb, new flat_RMI_Send_EncryptedAccept_Data()); }
  public static flat_RMI_Send_EncryptedAccept_Data GetRootAsflat_RMI_Send_EncryptedAccept_Data(ByteBuffer _bb, flat_RMI_Send_EncryptedAccept_Data obj) { return (obj.__assign(_bb.GetInt(_bb.Position) + _bb.Position, _bb)); }
  public void __init(int _i, ByteBuffer _bb) { __p.bb_pos = _i; __p.bb = _bb; }
  public flat_RMI_Send_EncryptedAccept_Data __assign(int _i, ByteBuffer _bb) { __init(_i, _bb); return this; }

  public sbyte AESEncryptedPublicAESKey(int j) { int o = __p.__offset(4); return o != 0 ? __p.bb.GetSbyte(__p.__vector(o) + j * 1) : (sbyte)0; }
  public int AESEncryptedPublicAESKeyLength { get { int o = __p.__offset(4); return o != 0 ? __p.__vector_len(o) : 0; } }
#if ENABLE_SPAN_T
  public Span<byte> GetAESEncryptedPublicAESKeyBytes() { return __p.__vector_as_span(4); }
#else
  public ArraySegment<byte>? GetAESEncryptedPublicAESKeyBytes() { return __p.__vector_as_arraysegment(4); }
#endif
  public sbyte[] GetAESEncryptedPublicAESKeyArray() { return __p.__vector_as_array<sbyte>(4); }
  public sbyte AESEncryptedPublicAESIV(int j) { int o = __p.__offset(6); return o != 0 ? __p.bb.GetSbyte(__p.__vector(o) + j * 1) : (sbyte)0; }
  public int AESEncryptedPublicAESIVLength { get { int o = __p.__offset(6); return o != 0 ? __p.__vector_len(o) : 0; } }
#if ENABLE_SPAN_T
  public Span<byte> GetAESEncryptedPublicAESIVBytes() { return __p.__vector_as_span(6); }
#else
  public ArraySegment<byte>? GetAESEncryptedPublicAESIVBytes() { return __p.__vector_as_arraysegment(6); }
#endif
  public sbyte[] GetAESEncryptedPublicAESIVArray() { return __p.__vector_as_array<sbyte>(6); }
  public int RMIHostID { get { int o = __p.__offset(8); return o != 0 ? __p.bb.GetInt(o + __p.bb_pos) : (int)0; } }
  public int UDPInitPort { get { int o = __p.__offset(10); return o != 0 ? __p.bb.GetInt(o + __p.bb_pos) : (int)0; } }

  public static Offset<flat_RMI_Send_EncryptedAccept_Data> Createflat_RMI_Send_EncryptedAccept_Data(FlatBufferBuilder builder,
      VectorOffset AESEncrypted_PublicAESKeyOffset = default(VectorOffset),
      VectorOffset AESEncrypted_PublicAESIVOffset = default(VectorOffset),
      int RMI_HostID = 0,
      int UDP_InitPort = 0) {
    builder.StartObject(4);
    flat_RMI_Send_EncryptedAccept_Data.AddRMIHostID(builder, RMI_HostID);
    flat_RMI_Send_EncryptedAccept_Data.AddAESEncryptedPublicAESIV(builder, AESEncrypted_PublicAESIVOffset);
    flat_RMI_Send_EncryptedAccept_Data.AddAESEncryptedPublicAESKey(builder, AESEncrypted_PublicAESKeyOffset);
    flat_RMI_Send_EncryptedAccept_Data.AddUDPInitPort(builder, UDP_InitPort);
    return flat_RMI_Send_EncryptedAccept_Data.Endflat_RMI_Send_EncryptedAccept_Data(builder);
  }

  public static void Startflat_RMI_Send_EncryptedAccept_Data(FlatBufferBuilder builder) { builder.StartObject(4); }
  public static void AddAESEncryptedPublicAESKey(FlatBufferBuilder builder, VectorOffset AESEncryptedPublicAESKeyOffset) { builder.AddOffset(0, AESEncryptedPublicAESKeyOffset.Value, 0); }
  public static VectorOffset CreateAESEncryptedPublicAESKeyVector(FlatBufferBuilder builder, sbyte[] data) { builder.StartVector(1, data.Length, 1); for (int i = data.Length - 1; i >= 0; i--) builder.AddSbyte(data[i]); return builder.EndVector(); }
  public static VectorOffset CreateAESEncryptedPublicAESKeyVectorBlock(FlatBufferBuilder builder, sbyte[] data) { builder.StartVector(1, data.Length, 1); builder.Add(data); return builder.EndVector(); }
  public static void StartAESEncryptedPublicAESKeyVector(FlatBufferBuilder builder, int numElems) { builder.StartVector(1, numElems, 1); }
  public static void AddAESEncryptedPublicAESIV(FlatBufferBuilder builder, VectorOffset AESEncryptedPublicAESIVOffset) { builder.AddOffset(1, AESEncryptedPublicAESIVOffset.Value, 0); }
  public static VectorOffset CreateAESEncryptedPublicAESIVVector(FlatBufferBuilder builder, sbyte[] data) { builder.StartVector(1, data.Length, 1); for (int i = data.Length - 1; i >= 0; i--) builder.AddSbyte(data[i]); return builder.EndVector(); }
  public static VectorOffset CreateAESEncryptedPublicAESIVVectorBlock(FlatBufferBuilder builder, sbyte[] data) { builder.StartVector(1, data.Length, 1); builder.Add(data); return builder.EndVector(); }
  public static void StartAESEncryptedPublicAESIVVector(FlatBufferBuilder builder, int numElems) { builder.StartVector(1, numElems, 1); }
  public static void AddRMIHostID(FlatBufferBuilder builder, int RMIHostID) { builder.AddInt(2, RMIHostID, 0); }
  public static void AddUDPInitPort(FlatBufferBuilder builder, int UDPInitPort) { builder.AddInt(3, UDPInitPort, 0); }
  public static Offset<flat_RMI_Send_EncryptedAccept_Data> Endflat_RMI_Send_EncryptedAccept_Data(FlatBufferBuilder builder) {
    int o = builder.EndObject();
    return new Offset<flat_RMI_Send_EncryptedAccept_Data>(o);
  }
    public static Offset<flat_RMI_Send_EncryptedAccept_Data> Createflat_RMI_Send_EncryptedAccept_Data(FlatBufferBuilder builder,
 RMI_Send_EncryptedAccept_Data data) {
        var AESEncrypted_PublicAESKeyOffset = CreateAESEncryptedPublicAESKeyVector(builder, (sbyte[])(Array)data.AESEncrypted_PublicAESKey);
        var AESEncrypted_PublicAESIVOffset = CreateAESEncryptedPublicAESIVVector(builder, (sbyte[])(Array)data.AESEncrypted_PublicAESIV);
        return Createflat_RMI_Send_EncryptedAccept_Data(builder , AESEncrypted_PublicAESKeyOffset, AESEncrypted_PublicAESIVOffset, data.RMI_HostID, data.UDP_InitPort);
    }

public static byte[] Createflat_RMI_Send_EncryptedAccept_Data(RMI_Send_EncryptedAccept_Data data) {
        FlatBufferBuilder fbb = new FlatBufferBuilder(512);
        fbb.Finish(flat_RMI_Send_EncryptedAccept_Data.Createflat_RMI_Send_EncryptedAccept_Data(fbb, data).Value);
        byte[] result = fbb.SizedByteArray();
        fbb = null;
        return result;
    }

        public static RMI_Send_EncryptedAccept_Data GetRootAsflat_RMI_Send_EncryptedAccept_Data(byte[] data) {
        return new RMI_Send_EncryptedAccept_Data(flat_RMI_Send_EncryptedAccept_Data.GetRootAsflat_RMI_Send_EncryptedAccept_Data( new ByteBuffer(data) ) );
    }
}};