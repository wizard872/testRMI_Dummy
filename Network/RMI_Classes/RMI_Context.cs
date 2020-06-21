// <auto-generated>
//  automatically generated by the FlatBuffers compiler, do not modify
// </auto-generated>

public enum RMI_Context : short
{
    // -32768~-32763 은 사용중.

    //TCP
    Reliable = 1, //신뢰성 있는 평문 전송

    //각 클라이언트마다 가지고 있는 암호화 키 사용.
    Reliable_AES_128 = 2, //신뢰성 있는 AES-128-CBC 전송
    Reliable_AES_256 = 3, //신뢰성 있는 AES-256-CBC 전송

    //서버가 생성하고, 모든 클라이언트가 공유하고있는 암호화키 사용.
    Reliable_Public_AES_128 = 4, //신뢰성 있는 공용키 AES-128-CBC 전송
    Reliable_Public_AES_256 = 5, //신뢰성 있는 공용키 AES-256-CBC 전송

    //UDP
    UnReliable = 6, //비 신뢰성 평문 전송

    //각 클라이언트마다 가지고 있는 암호화 키 사용.
    UnReliable_AES_128 = 7, //비 신뢰성 AES-128-CBC 전송
    UnReliable_AES_256 = 8, //비 신뢰성 AES-256-CBC 전송

    //서버가 생성하고, 모든 클라이언트가 공유하고있는 암호화키 사용.
    UnReliable_Public_AES_128 = 9, //비 신뢰성 공용키 AES-128-CBC 전송
    UnReliable_Public_AES_256 = 10, //비 신뢰성 공용키 AES-256-CBC 전송
}