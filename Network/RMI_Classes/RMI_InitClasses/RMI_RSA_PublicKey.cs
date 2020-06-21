using Network.RMI_Common.RMI_ParsingClasses;
using System;

[Serializable]
public class RMI_RSA_PublicKey {

    public string base64Encoded_publicKey;

    public RMI_RSA_PublicKey() { }

    public RMI_RSA_PublicKey(flat_RMI_RSA_PublicKey data) {
        this.base64Encoded_publicKey = data.Base64EncodedPublicKey;
    }

    public static RMI_RSA_PublicKey CreateRMI_RSA_PublicKey(byte[] data)
    {
        return flat_RMI_RSA_PublicKey.GetRootAsflat_RMI_RSA_PublicKey( data );
    }

    public static byte[] getBytes(RMI_RSA_PublicKey data)
    {
        return flat_RMI_RSA_PublicKey.Createflat_RMI_RSA_PublicKey( data );
    }

    public byte[] getBytes()
    {
        return getBytes(this);
    }

}