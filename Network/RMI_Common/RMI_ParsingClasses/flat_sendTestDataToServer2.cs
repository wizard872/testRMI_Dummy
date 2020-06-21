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

public struct flat_sendTestDataToServer2 : IFlatbufferObject
{
  private Table __p;
  public ByteBuffer ByteBuffer { get { return __p.bb; } }
  public static flat_sendTestDataToServer2 GetRootAsflat_sendTestDataToServer2(ByteBuffer _bb) { return GetRootAsflat_sendTestDataToServer2(_bb, new flat_sendTestDataToServer2()); }
  public static flat_sendTestDataToServer2 GetRootAsflat_sendTestDataToServer2(ByteBuffer _bb, flat_sendTestDataToServer2 obj) { return (obj.__assign(_bb.GetInt(_bb.Position) + _bb.Position, _bb)); }
  public void __init(int _i, ByteBuffer _bb) { __p.bb_pos = _i; __p.bb = _bb; }
  public flat_sendTestDataToServer2 __assign(int _i, ByteBuffer _bb) { __init(_i, _bb); return this; }

  public float TimeData { get { int o = __p.__offset(4); return o != 0 ? __p.bb.GetFloat(o + __p.bb_pos) : (float)0.0f; } }
  public string TestData { get { int o = __p.__offset(6); return o != 0 ? __p.__string(o + __p.bb_pos) : null; } }
#if ENABLE_SPAN_T
  public Span<byte> GetTestDataBytes() { return __p.__vector_as_span(6); }
#else
  public ArraySegment<byte>? GetTestDataBytes() { return __p.__vector_as_arraysegment(6); }
#endif
  public byte[] GetTestDataArray() { return __p.__vector_as_array<byte>(6); }
  public flat_RMI_Vector3? TestDataArray(int j) { int o = __p.__offset(8); return o != 0 ? (flat_RMI_Vector3?)(new flat_RMI_Vector3()).__assign(__p.__indirect(__p.__vector(o) + j * 4), __p.bb) : null; }
  public int TestDataArrayLength { get { int o = __p.__offset(8); return o != 0 ? __p.__vector_len(o) : 0; } }

  public static Offset<flat_sendTestDataToServer2> Createflat_sendTestDataToServer2(FlatBufferBuilder builder,
      float timeData = 0.0f,
      StringOffset testDataOffset = default(StringOffset),
      VectorOffset testDataArrayOffset = default(VectorOffset)) {
    builder.StartObject(3);
    flat_sendTestDataToServer2.AddTestDataArray(builder, testDataArrayOffset);
    flat_sendTestDataToServer2.AddTestData(builder, testDataOffset);
    flat_sendTestDataToServer2.AddTimeData(builder, timeData);
    return flat_sendTestDataToServer2.Endflat_sendTestDataToServer2(builder);
  }

  public static void Startflat_sendTestDataToServer2(FlatBufferBuilder builder) { builder.StartObject(3); }
  public static void AddTimeData(FlatBufferBuilder builder, float timeData) { builder.AddFloat(0, timeData, 0.0f); }
  public static void AddTestData(FlatBufferBuilder builder, StringOffset testDataOffset) { builder.AddOffset(1, testDataOffset.Value, 0); }
  public static void AddTestDataArray(FlatBufferBuilder builder, VectorOffset testDataArrayOffset) { builder.AddOffset(2, testDataArrayOffset.Value, 0); }
  public static VectorOffset CreateTestDataArrayVector(FlatBufferBuilder builder, Offset<flat_RMI_Vector3>[] data) { builder.StartVector(4, data.Length, 4); for (int i = data.Length - 1; i >= 0; i--) builder.AddOffset(data[i].Value); return builder.EndVector(); }
  public static VectorOffset CreateTestDataArrayVectorBlock(FlatBufferBuilder builder, Offset<flat_RMI_Vector3>[] data) { builder.StartVector(4, data.Length, 4); builder.Add(data); return builder.EndVector(); }
  public static void StartTestDataArrayVector(FlatBufferBuilder builder, int numElems) { builder.StartVector(4, numElems, 4); }
  public static Offset<flat_sendTestDataToServer2> Endflat_sendTestDataToServer2(FlatBufferBuilder builder) {
    int o = builder.EndObject();
    return new Offset<flat_sendTestDataToServer2>(o);
  }
    public static Offset<flat_sendTestDataToServer2> Createflat_sendTestDataToServer2(FlatBufferBuilder builder,
 sendTestDataToServer2 data) {
        var testDataOffset = builder.CreateString(data.testData);
        int size2 = data.testDataArray.Count;
        Offset<flat_RMI_Vector3>[] testDataArray_ = new Offset<flat_RMI_Vector3>[size2];
        for (int x = 0; x < size2; x++) {
        RMI_Vector3 aa = data.testDataArray[x];
        testDataArray_[x] = flat_RMI_Vector3.Createflat_RMI_Vector3(builder, aa);
        }
        var testDataArrayOffset = flat_sendTestDataToServer2.CreateTestDataArrayVector(builder, testDataArray_);
        return Createflat_sendTestDataToServer2(builder , data.timeData, testDataOffset, testDataArrayOffset);
    }

    public static byte[] Createflat_sendTestDataToServer2(sendTestDataToServer2 data) {
        FlatBufferBuilder fbb = new FlatBufferBuilder(512);
        fbb.Finish(flat_sendTestDataToServer2.Createflat_sendTestDataToServer2(fbb, data).Value);
        byte[] result = fbb.SizedByteArray();
        fbb = null;
        return result;
    }

    public static sendTestDataToServer2 GetRootAsflat_sendTestDataToServer2(byte[] data) {
        return new sendTestDataToServer2(flat_sendTestDataToServer2.GetRootAsflat_sendTestDataToServer2( new ByteBuffer(data) ) );
    }

}};