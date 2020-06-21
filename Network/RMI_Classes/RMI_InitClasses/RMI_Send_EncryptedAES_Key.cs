using Network.RMI_Common.RMI_ParsingClasses;
using System;

[Serializable]
public class RMI_Send_EncryptedAES_Key {

    public byte[] RSAEncrypted_AESKey;
    public byte[] RSAEncrypted_AESIV;

    public RMI_Send_EncryptedAES_Key() { }

    public RMI_Send_EncryptedAES_Key(flat_RMI_Send_EncryptedAES_Key data) {
        this.RSAEncrypted_AESKey = new byte[data.RSAEncryptedAESKeyLength];
        for(int i = 0;i < data.RSAEncryptedAESKeyLength;i++) {
            this.RSAEncrypted_AESKey[i] = (byte)data.RSAEncryptedAESKey(i) ;
        }
        this.RSAEncrypted_AESIV = new byte[data.RSAEncryptedAESIVLength];
        for(int i = 0;i < data.RSAEncryptedAESIVLength;i++) {
            this.RSAEncrypted_AESIV[i] = (byte)data.RSAEncryptedAESIV(i) ;
        }
    }

    public static RMI_Send_EncryptedAES_Key CreateRMI_Send_EncryptedAES_Key(byte[] data)
    {
        return flat_RMI_Send_EncryptedAES_Key.GetRootAsflat_RMI_Send_EncryptedAES_Key( data );
    }

    public static byte[] getBytes(RMI_Send_EncryptedAES_Key data)
    {
        return flat_RMI_Send_EncryptedAES_Key.Createflat_RMI_Send_EncryptedAES_Key( data );
    }

    public byte[] getBytes()
    {
        return getBytes(this);
    }

}