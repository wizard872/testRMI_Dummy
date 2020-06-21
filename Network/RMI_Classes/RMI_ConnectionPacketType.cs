using System.Collections;
using System.Collections.Generic;

public enum RMI_ConnectionPacketType : short
{
    //고정값.
    RMI_ProtocolVersionCheck = -32768,
    RMI_RSA_PublicKey = -32767,
    RMI_Send_EncryptedAES_Key = -32766,
    RMI_Send_EncryptedAccept_Data = -32765,
    RMI_UDP_ConnectionConfirm = -32764,
    RMI_OverConnectionAnnounce = -32763
}
