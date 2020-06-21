using System;
using System.IO;
using System.Text;
using System.Security.Cryptography;

//암호화, 복호화를 전담할 클래스.
public class RMI_EncryptManager
{
    //공용키이다.
    public static EncryptKeyInfo Public_AES_Keys = new EncryptKeyInfo();

    public RMI_EncryptManager()
    {
        //AES 대칭키 생성.
        //Public_AES_Keys.createAES_Key();

        ////RSA 비대칭키 생성.
        //KeyPair keypair = RMI__EncryptManager.createKeyPair(1024);
        //RMI_ID.MYSELF.AESKey.keypair = keypair;
        //RMI_ID.MYSELF.AESKey.privateKey = keypair.privateKeyText;
        //RMI_ID.MYSELF.AESKey.publicKey = keypair.publicKeyText;
        //RSA_PublicKey_base64 = encodeBase64_publicKey(keypair.publicKeyText);
    }
    

    //같은데이터를 여러명의 대상에게 보내는 경우는 공용 암호화 Key를 사용할 것.
    public static byte[] RMI_EncryptMethod_Arr(RMI_ID[] rmi_id, RMI_Context rmi_ctx, bool isEncrypt, byte[] data)
    {
        EncryptKeyInfo keys = null;
        byte[] processed = null;
        
        switch (rmi_ctx)
        {
            case RMI_Context.Reliable: //plain Data
                //평문은 아무것도 안함.
                return data;
            case RMI_Context.Reliable_Public_AES_128: //AES_-128-CBC //공용 대칭키 사용. 같은데이터를 여러명에게 보낼때.
                keys = RMI_EncryptManager.Public_AES_Keys;
                if (isEncrypt)
                    processed = RMI_EncryptManager.encryptAES_128(data, keys.AES_Key, keys.AES_IV);
                else
                    processed = RMI_EncryptManager.decryptAES_128(data, keys.AES_Key, keys.AES_IV);
                break;
            case RMI_Context.Reliable_Public_AES_256: //AES_-256-CBC //공용 대칭키 사용. 같은데이터를 여러명에게 보낼때.
                keys = RMI_EncryptManager.Public_AES_Keys;
                if (isEncrypt)
                    processed = RMI_EncryptManager.encryptAES_256(data, keys.AES_Key, keys.AES_IV);
                else
                    processed = RMI_EncryptManager.decryptAES_256(data, keys.AES_Key, keys.AES_IV);
                break;

            case RMI_Context.UnReliable: //plain Data
                //평문은 아무것도 안함.
                return data;
            case RMI_Context.UnReliable_Public_AES_128: //AES_-128-CBC //공용 대칭키 사용. 같은데이터를 여러명에게 보낼때.
                keys = RMI_EncryptManager.Public_AES_Keys;
                if (isEncrypt)
                    processed = RMI_EncryptManager.encryptAES_128(data, keys.AES_Key, keys.AES_IV);
                else
                    processed = RMI_EncryptManager.decryptAES_128(data, keys.AES_Key, keys.AES_IV);
                break;
            case RMI_Context.UnReliable_Public_AES_256: //AES_-256-CBC //공용 대칭키 사용. 같은데이터를 여러명에게 보낼때.
                keys = RMI_EncryptManager.Public_AES_Keys;
                if (isEncrypt)
                    processed = RMI_EncryptManager.encryptAES_256(data, keys.AES_Key, keys.AES_IV);
                else
                    processed = RMI_EncryptManager.decryptAES_256(data, keys.AES_Key, keys.AES_IV);
                break;
            default:
                //Debug.Log("RMI_EncryptMethod_Arr, 공용 RMI_Context값 에러! => " + rmi_ctx);
                throw new ArgumentException(
                    "RMI_ID[] (범위) 로 보낼시, RMI_Context 는 Reliable/UnReliable Public_AES_128, 256만 지정 가능합니다.");
        }
        return processed;
    }

    public static byte[] RMI_EncryptMethod(RMI_ID rmi_id, RMI_Context rmi_ctx, bool isEncrypt, byte[] data)
    {
        //키 호출.
        EncryptKeyInfo keys;
        byte[] processed = null;

        switch (rmi_ctx)
        {
            //TCP
            case RMI_Context.Reliable: //plain Data
                //평문은 아무것도 안함.
                return data;
            case RMI_Context.Reliable_AES_128: //AES_-128-CBC
                keys = rmi_id.AESKey;
                if (rmi_id.unique_id == RMI_ID.ALL.unique_id)
                    keys = RMI_EncryptManager.Public_AES_Keys;
                if (keys != null)
                {
                    if (isEncrypt)
                        processed = RMI_EncryptManager.encryptAES_128(data, keys.AES_Key, keys.AES_IV);
                    else
                        processed = RMI_EncryptManager.decryptAES_128(data, keys.AES_Key, keys.AES_IV);
                }
                else
                    throw new Exception("AES_128 : rmi_id.AESKey_TCP 가 Null입니다.");

                break;
            case RMI_Context.Reliable_AES_256: //AES_-256-CBC
                keys = rmi_id.AESKey;
                if (rmi_id.unique_id == RMI_ID.ALL.unique_id)
                    keys = RMI_EncryptManager.Public_AES_Keys;
                if (keys != null)
                {
                    if (isEncrypt)
                        processed = RMI_EncryptManager.encryptAES_256(data, keys.AES_Key, keys.AES_IV);
                    else
                        processed = RMI_EncryptManager.decryptAES_256(data, keys.AES_Key, keys.AES_IV);
                }
                else
                    throw new Exception("AES_256 : rmi_id.AESKey_TCP 가 Null입니다.");

                break;
            case RMI_Context.Reliable_Public_AES_128: //AES_-128-CBC //공용 대칭키 사용. 같은데이터를 여러명에게 보낼때.
                keys = RMI_EncryptManager.Public_AES_Keys;
                if (isEncrypt)
                    processed = RMI_EncryptManager.encryptAES_128(data, keys.AES_Key, keys.AES_IV);
                else
                    processed = RMI_EncryptManager.decryptAES_128(data, keys.AES_Key, keys.AES_IV);
                break;
            case RMI_Context.Reliable_Public_AES_256: //AES_-256-CBC //공용 대칭키 사용. 같은데이터를 여러명에게 보낼때.
                keys = RMI_EncryptManager.Public_AES_Keys;
                if (isEncrypt)
                    processed = RMI_EncryptManager.encryptAES_256(data, keys.AES_Key, keys.AES_IV);
                else
                    processed = RMI_EncryptManager.decryptAES_256(data, keys.AES_Key, keys.AES_IV);
                break;


            //UDP
            case RMI_Context.UnReliable: //plain Data
                //평문은 아무것도 안함.
                return data;
            case RMI_Context.UnReliable_AES_128: //AES_-128-CBC
                keys = rmi_id.AESKey;
                if (rmi_id.unique_id == RMI_ID.ALL.unique_id)
                    keys = RMI_EncryptManager.Public_AES_Keys;
                if (keys != null)
                {
                    if (isEncrypt)
                        processed = RMI_EncryptManager.encryptAES_128(data, keys.AES_Key, keys.AES_IV);
                    else
                        processed = RMI_EncryptManager.decryptAES_128(data, keys.AES_Key, keys.AES_IV);
                }
                else
                    throw new Exception("AES_128 : rmi_id.AESKey_UDP 가 Null입니다.");

                break;
            case RMI_Context.UnReliable_AES_256: //AES_-256-CBC
                keys = rmi_id.AESKey;
                if (rmi_id.unique_id == RMI_ID.ALL.unique_id)
                    keys = RMI_EncryptManager.Public_AES_Keys;
                if (keys != null)
                {
                    if (isEncrypt)
                        processed = RMI_EncryptManager.encryptAES_256(data, keys.AES_Key, keys.AES_IV);
                    else
                        processed = RMI_EncryptManager.decryptAES_256(data, keys.AES_Key, keys.AES_IV);
                }
                else
                    throw new Exception("AES_256 : rmi_id.AESKey_UDP 가 Null입니다.");

                break;
            case RMI_Context.UnReliable_Public_AES_128: //AES_-128-CBC //공용 대칭키 사용. 같은데이터를 여러명에게 보낼때.
                keys = RMI_EncryptManager.Public_AES_Keys;
                if (isEncrypt)
                    processed = RMI_EncryptManager.encryptAES_128(data, keys.AES_Key, keys.AES_IV);
                else
                    processed = RMI_EncryptManager.decryptAES_128(data, keys.AES_Key, keys.AES_IV);
                break;
            case RMI_Context.UnReliable_Public_AES_256: //AES_-256-CBC //공용 대칭키 사용. 같은데이터를 여러명에게 보낼때.
                keys = RMI_EncryptManager.Public_AES_Keys;
                if (isEncrypt)
                    processed = RMI_EncryptManager.encryptAES_256(data, keys.AES_Key, keys.AES_IV);
                else
                    processed = RMI_EncryptManager.decryptAES_256(data, keys.AES_Key, keys.AES_IV);
                break;

            default:
                //Debug.Log("RMI_EncryptMethod RMI_Context값 에러! => " + rmi_ctx);
                throw new ArgumentException("올바른 RMI_Context 값을 지정하십시오 : " + rmi_ctx);
        }
        return processed;
    }


    public static byte[] encryptRSA_public(byte[] plainData, string publicKey_Text)
    {
        byte[] encryptData = null;

        //RSA 암호화 객체 생성
        using (RSACryptoServiceProvider rsa = new RSACryptoServiceProvider())
        {
            //privateKey로 암호화!
            rsa.FromXmlString(publicKey_Text);

            //암호화할 plainData를 세팅후 암호화!
            encryptData = rsa.Encrypt(plainData, false);
        }
        return encryptData;
    }

    public static byte[] decryptRSA_public(byte[] encryptData, string publicKey_Text)
    {
        byte[] plainData = null;

        //RSA 암호화 객체 생성
        using (RSACryptoServiceProvider rsa = new RSACryptoServiceProvider())
        {
            //privateKey로 암호화!
            rsa.FromXmlString(publicKey_Text);

            //암호화할 plainData를 세팅후 암호화!
            encryptData = rsa.Decrypt(encryptData, false);
        }
        return plainData;
    }

    public static byte[] encryptRSA_private(byte[] plainData, string privateKey_Text)
    {
        byte[] encryptData = null;

        //RSA 암호화 객체 생성
        using (RSACryptoServiceProvider rsa = new RSACryptoServiceProvider())
        {
            //privateKey로 암호화!
            rsa.FromXmlString(privateKey_Text);

            //암호화할 plainData를 세팅후 암호화!
            encryptData = rsa.Encrypt(plainData, false);
        }
        return encryptData;
    }

    public static byte[] decryptRSA_private(byte[] encryptData, string privateKey_Text)
    {
        byte[] plainData = null;

        //RSA 복호화 객체 생성
        using (RSACryptoServiceProvider rsa = new RSACryptoServiceProvider())
        {
            //privateKey로 복호화!
            rsa.FromXmlString(privateKey_Text);

            //복호화할 encryptData를 세팅후 복호화!
            plainData = rsa.Decrypt(encryptData, false);
        }
        return plainData;
    }


    public static KeyPair createKeyPair(int keySize)
    {
        //키 페어 객체
        KeyPair keypair = null;

        // 암호화 개체 생성
        using (RSACryptoServiceProvider rsa = new RSACryptoServiceProvider(keySize))
        {
            // 개인키 & 공개키 생성
            RSAParameters privateKey = RSA.Create().ExportParameters(true); //만약, ExportParameters 에 false 를 주면 공개키만을 반환! 

            //rsa.ImportParameters(privateKey);
            string privateKeyText = rsa.ToXmlString(true);
            string publicKeyText = rsa.ToXmlString(false);

            keypair = new KeyPair();

            keypair.privateKeyText = privateKeyText;
            keypair.publicKeyText = publicKeyText;
        }
        return keypair;
    }

    public static string encodeBase64_publicKey(string publicKey_text)
    {
        string publicKey_base64Text = null;

        //RSA 암호화 객체
        using (RSACryptoServiceProvider rsa = new RSACryptoServiceProvider())
        {
            //privateKey_Text로부터 RSACryptoServiceProvider를 세팅.
            rsa.FromXmlString(publicKey_text);

            //RSAParameters privateKey = rsa.ExportParameters(true);
            RSAParameters publicKey = rsa.ExportParameters(false);

            publicKey_base64Text = Convert.ToBase64String(publicKey.Modulus) + "§" + Convert.ToBase64String(publicKey.Exponent);

            //Debug.Log("encodeBase64_publicKey=" + publicKey_base64Text);
        }
        return publicKey_base64Text;
    }

    public static string getPublicKey(string publicKey_base64, int keySize)
    {
        string publicKey_Text = null;

        using (RSACryptoServiceProvider rsa = new RSACryptoServiceProvider(keySize))
        {
            // 공개키 생성 (개인키로부터 생성)
            RSAParameters publicKey = new RSAParameters();

            char regex = '§';
            string[] key_str = publicKey_base64.Split(regex);

            publicKey.Modulus = Convert.FromBase64String(key_str[0]);
            publicKey.Exponent = Convert.FromBase64String(key_str[1]);

            //RSAParameters publicKey로부터 RSACryptoServiceProvider rsa객체 세팅!
            rsa.ImportParameters(publicKey);

            publicKey_Text = rsa.ToXmlString(false);
        }
        return publicKey_Text;
    }


    public static byte[] encryptAES_256(byte[] plainData, string plainSessionKey, string plainInitialization_Vector)
    {
        //byte[] plainData = compressLZ4(plainData1);

        byte[] encryptData;

        //Rijndael 알고리즘을 사용하여 AES_알고리즘 암호화 준비.
        using (RijndaelManaged AES_ = new RijndaelManaged())
        {
            //CBC(Cipher-Block Chaining) 암호화 이므로 CipherMode는 CBC.
            AES_.Mode = CipherMode.CBC;

            //AES_256-CBC 암호화 이므로 256 비트(32 바이트)
            AES_.KeySize = 256;

            //암호화 블록의 크기. 128 비트 (16 바이트)
            AES_.BlockSize = 128;

            //입력받은 평문 대칭키, 초기화 벡터를 솔팅 & 키 스트레칭 처리를 하여 복호화를 좀더 복잡하게 함 
            using (Rfc2898DeriveBytes salting_key = MakeKey(plainSessionKey, 1))
            {
                //솔팅 & 키 스트레칭 처리된 key를 세팅한다.
                AES_.Key = salting_key.GetBytes(32);
            }

            using (Rfc2898DeriveBytes salting_vector = MakeInitializationVector(plainInitialization_Vector, 1))
            {
                //솔팅 & 키 스트레칭 처리된 vector를 세팅한다.
                AES_.IV = salting_vector.GetBytes(16);
            }

            //Padding

            #region Padding

            /*        
            블록 암호는 고정된 블록 크기에서 작동하지만, 메세지는 다양한 길이로 나타난다.
            좀 더 쉽게 얘기를 하자면, 데이터(메세지)를 블럭으로 암호화 할 때 평문이 항상 블럭 크기(일반적으로 64비트 / 128비트)의 배수가 되지 않을 경우가 존재한다.
            패딩은 어떻게 평문의 마지막 블록이 암호화 되기 전에 데이터로 채워지는가를 확실히 지정하는 방법 이다. 복호화 과정에서는 패딩을 제거하고, 평문의 실제 길이를 지정하게 된다. 
            간단하게 설명하자면 암호 블록 사이즈와 데이터 사이즈가 맞지 않을 경우에 배수에 맞춰 빈공간을 채워주는 방식이라고 볼 수가 있다.
             */

            #endregion

            AES_.Padding = PaddingMode.PKCS7;

            //암호화된 byte[]를 저장하기 위한 메모리 스트림. using을 통해, 범위를 제한함!
            using (MemoryStream memoryStream = new MemoryStream())
            {
                //processing Encrypt 암호화 처리 부분.
                using (ICryptoTransform encryptor = AES_.CreateEncryptor(AES_.Key, AES_.IV))
                {
                    //암호화를 진행하는 스트림.
                    using (CryptoStream cryptoStream = new CryptoStream(memoryStream, encryptor, CryptoStreamMode.Write))
                    {
                        //plainData 데이터로부터 읽어들여, memoryStream에 암호화된 결과를 기록하는 부분.
                        cryptoStream.Write(plainData, 0, plainData.Length);
                    }
                }
                
                //암호화된 byte[]를 memoryStream로부터 불러온다.
                encryptData = memoryStream.ToArray();
            }
        }
        //암호화된 byte[]를 반환!
        return encryptData;
    }

    public static byte[] decryptAES_256(byte[] encryptData, string plainSessionKey, string plainInitialization_Vector)
    {
        byte[] plainData;

        //Rijndael 알고리즘을 사용하여 AES_알고리즘 암호화 준비.
        using (RijndaelManaged AES_ = new RijndaelManaged())
        {
            //CBC(Cipher-Block Chaining) 암호화 이므로 CipherMode는 CBC.
            AES_.Mode = CipherMode.CBC;

            //AES_256-CBC 암호화 이므로 256 비트(32 바이트)
            AES_.KeySize = 256;

            //암호화 블록의 크기. 128 비트 (16 바이트)
            AES_.BlockSize = 128;

            //솔팅 & 키 스트레칭 처리된 key & vector를 세팅한다.
            //입력받은 평문 대칭키, 초기화 벡터를 솔팅 & 키 스트레칭 처리를 하여 복호화를 좀더 복잡하게 함 
            using (Rfc2898DeriveBytes salting_key = MakeKey(plainSessionKey, 1))
            {
                //솔팅 & 키 스트레칭 처리된 key & vector를 세팅한다.
                AES_.Key = salting_key.GetBytes(32);
            }

            using (Rfc2898DeriveBytes salting_vector = MakeInitializationVector(plainInitialization_Vector, 1))
            {
                AES_.IV = salting_vector.GetBytes(16);
            }

            //Padding

            #region Padding

            /*        
            블록 암호는 고정된 블록 크기에서 작동하지만, 메세지는 다양한 길이로 나타난다.
            좀 더 쉽게 얘기를 하자면, 데이터(메세지)를 블럭으로 암호화 할 때 평문이 항상 블럭 크기(일반적으로 64비트 / 128비트)의 배수가 되지 않을 경우가 존재한다.
            패딩은 어떻게 평문의 마지막 블록이 암호화 되기 전에 데이터로 채워지는가를 확실히 지정하는 방법 이다. 복호화 과정에서는 패딩을 제거하고, 평문의 실제 길이를 지정하게 된다. 
            간단하게 설명하자면 암호 블록 사이즈와 데이터 사이즈가 맞지 않을 경우에 배수에 맞춰 빈공간을 채워주는 방식이라고 볼 수가 있다.
             */

            #endregion

            AES_.Padding = PaddingMode.PKCS7;

            //복호화된 byte[]를 저장하기 위한 메모리 스트림. using을 통해, 범위를 제한함!
            using (MemoryStream memoryStream = new MemoryStream())
            {
                //processing Encrypt 암호화 처리 부분.
                using (ICryptoTransform decryptor = AES_.CreateDecryptor(AES_.Key, AES_.IV))
                {
                    //복호화를 진행하는 스트림.
                    using (CryptoStream cryptoStream =
                        new CryptoStream(memoryStream, decryptor, CryptoStreamMode.Write))
                    {
                        //encryptData 데이터로부터 읽어들여, memoryStream에 복호화된 결과를 기록하는 부분.
                        cryptoStream.Write(encryptData, 0, encryptData.Length);
                    }
                }

                //복호화된 byte[]를 memoryStream로부터 불러온다.
                plainData = memoryStream.ToArray();
            }
        }
        //암호화된 byte[]를 반환!
        return plainData;
    }

    public static byte[] encryptAES_128(byte[] plainData, string plainSessionKey, string plainInitialization_Vector)
    {
        //byte[] plainData = compressLZ4(plainData1);

        byte[] encryptData;

        //Rijndael 알고리즘을 사용하여 AES_알고리즘 암호화 준비.
        using (RijndaelManaged AES_ = new RijndaelManaged())
        {
            //CBC(Cipher-Block Chaining) 암호화 이므로 CipherMode는 CBC.
            AES_.Mode = CipherMode.CBC;

            //AES_128-CBC 암호화 이므로 128 비트(16 바이트)
            AES_.KeySize = 128;

            //암호화 블록의 크기. 128 비트 (16 바이트)
            AES_.BlockSize = 128;

            //입력받은 평문 대칭키, 초기화 벡터를 솔팅 & 키 스트레칭 처리를 하여 복호화를 좀더 복잡하게 함 
            using (Rfc2898DeriveBytes salting_key = MakeKey(plainSessionKey, 1))
            {
                //솔팅 & 키 스트레칭 처리된 key를 세팅한다.
                AES_.Key = salting_key.GetBytes(16);
            }

            using (Rfc2898DeriveBytes salting_vector = MakeInitializationVector(plainInitialization_Vector, 1))
            {
                //솔팅 & 키 스트레칭 처리된 vector를 세팅한다.
                AES_.IV = salting_vector.GetBytes(16);
            }

            //Padding

            #region Padding

            /*        
            블록 암호는 고정된 블록 크기에서 작동하지만, 메세지는 다양한 길이로 나타난다.
            좀 더 쉽게 얘기를 하자면, 데이터(메세지)를 블럭으로 암호화 할 때 평문이 항상 블럭 크기(일반적으로 64비트 / 128비트)의 배수가 되지 않을 경우가 존재한다.
            패딩은 어떻게 평문의 마지막 블록이 암호화 되기 전에 데이터로 채워지는가를 확실히 지정하는 방법 이다. 복호화 과정에서는 패딩을 제거하고, 평문의 실제 길이를 지정하게 된다. 
            간단하게 설명하자면 암호 블록 사이즈와 데이터 사이즈가 맞지 않을 경우에 배수에 맞춰 빈공간을 채워주는 방식이라고 볼 수가 있다.
             */

            #endregion

            AES_.Padding = PaddingMode.PKCS7;

            //암호화된 byte[]를 저장하기 위한 메모리 스트림. using을 통해, 범위를 제한함!
            using (MemoryStream memoryStream = new MemoryStream())
            {
                //processing Encrypt 암호화 처리 부분.
                using (ICryptoTransform encryptor = AES_.CreateEncryptor(AES_.Key, AES_.IV))
                {
                    //암호화를 진행하는 스트림.
                    using (CryptoStream cryptoStream =
                        new CryptoStream(memoryStream, encryptor, CryptoStreamMode.Write))
                    {
                        //plainData 데이터로부터 읽어들여, memoryStream에 암호화된 결과를 기록하는 부분.
                        cryptoStream.Write(plainData, 0, plainData.Length);
                    }
                }

                //암호화된 byte[]를 memoryStream로부터 불러온다.
                encryptData = memoryStream.ToArray();;
            }
        }
        //암호화된 byte[]를 반환!
        return encryptData;
    }

    public static byte[] decryptAES_128(byte[] encryptData, string plainSessionKey, string plainInitialization_Vector)
    {
        byte[] plainData;

        //Rijndael 알고리즘을 사용하여 AES_알고리즘 암호화 준비.
        using (RijndaelManaged AES_ = new RijndaelManaged())
        {
            //CBC(Cipher-Block Chaining) 암호화 이므로 CipherMode는 CBC.
            AES_.Mode = CipherMode.CBC;

            //AES_128-CBC 암호화 이므로 128 비트(16 바이트)
            AES_.KeySize = 128;

            //암호화 블록의 크기. 128 비트 (16 바이트)
            AES_.BlockSize = 128;

            //솔팅 & 키 스트레칭 처리된 key & vector를 세팅한다.
            //입력받은 평문 대칭키, 초기화 벡터를 솔팅 & 키 스트레칭 처리를 하여 복호화를 좀더 복잡하게 함 
            using (Rfc2898DeriveBytes salting_key = MakeKey(plainSessionKey, 1))
            {
                //솔팅 & 키 스트레칭 처리된 key & vector를 세팅한다.
                AES_.Key = salting_key.GetBytes(16);
            }

            using (Rfc2898DeriveBytes salting_vector = MakeInitializationVector(plainInitialization_Vector, 1))
            {
                AES_.IV = salting_vector.GetBytes(16);
            }

            //Padding

            #region Padding

            /*        
            블록 암호는 고정된 블록 크기에서 작동하지만, 메세지는 다양한 길이로 나타난다.
            좀 더 쉽게 얘기를 하자면, 데이터(메세지)를 블럭으로 암호화 할 때 평문이 항상 블럭 크기(일반적으로 64비트 / 128비트)의 배수가 되지 않을 경우가 존재한다.
            패딩은 어떻게 평문의 마지막 블록이 암호화 되기 전에 데이터로 채워지는가를 확실히 지정하는 방법 이다. 복호화 과정에서는 패딩을 제거하고, 평문의 실제 길이를 지정하게 된다. 
            간단하게 설명하자면 암호 블록 사이즈와 데이터 사이즈가 맞지 않을 경우에 배수에 맞춰 빈공간을 채워주는 방식이라고 볼 수가 있다.
             */

            #endregion

            AES_.Padding = PaddingMode.PKCS7;

            //복호화된 byte[]를 저장하기 위한 메모리 스트림. using을 통해, 범위를 제한함!
            using (MemoryStream memoryStream = new MemoryStream())
            {
                //processing Encrypt 암호화 처리 부분.
                using (ICryptoTransform decryptor = AES_.CreateDecryptor(AES_.Key, AES_.IV))
                {
                    //복호화를 진행하는 스트림.
                    using (CryptoStream cryptoStream =
                        new CryptoStream(memoryStream, decryptor, CryptoStreamMode.Write))
                    {
                        //encryptData 데이터로부터 읽어들여, memoryStream에 복호화된 결과를 기록하는 부분.
                        cryptoStream.Write(encryptData, 0, encryptData.Length);
                    }
                }

                //복호화된 byte[]를 memoryStream로부터 불러온다.
                plainData = memoryStream.ToArray();;
            }
        }
        //암호화된 byte[]를 반환!
        return plainData;
    }

    //솔팅 및 Key 스트레칭을 통해 좀더 복잡화 함.
    public static Rfc2898DeriveBytes MakeKey(string key_data, int count)
    {
        byte[] keyBytes = Encoding.UTF8.GetBytes(key_data);
        byte[] saltBytes;

        //keyBytes 로부터 256 비트 길이의 단방향 해시 다이제스트 생성 
        using (SHA1 SHA1 = SHA1.Create())
        {
            saltBytes = SHA1.ComputeHash(keyBytes);
        }

        //count 번 반복
        Rfc2898DeriveBytes result = new Rfc2898DeriveBytes(keyBytes, saltBytes, count);

        return result;
    }

    //솔팅 및 Key 스트레칭을 통해 좀더 복잡화 함.
    public static Rfc2898DeriveBytes MakeInitializationVector(string vector_data, int count)
    {
        byte[] vectorBytes = Encoding.UTF8.GetBytes(vector_data);
        byte[] saltBytes;

        //vectorBytes 로부터 256 비트 길이의 단방향 해시 다이제스트 생성
        using (SHA1 SHA1 = SHA1.Create())
        {
            saltBytes = SHA1.ComputeHash(vectorBytes);
        }

        //count 번 반복
        Rfc2898DeriveBytes result = new Rfc2898DeriveBytes(vectorBytes, saltBytes, count);

        return result;
    }

    public class EncryptKeyInfo
    {
        public KeyPair keypair;

        //암호화 중인 Key 종류.
        public string privateKey;
        public string publicKey;
        public string AES_Key;
        public string AES_IV;

        public EncryptKeyInfo()
        {
            this.createAES_Key();
        }

        public EncryptKeyInfo setAES_Key(string receivedKey)
        {
            string[] received_data = receivedKey.Split('§');
            this.AES_Key = received_data[0];
            this.AES_IV = received_data[1];

            return this;
        }

        public void createAES_Key()
        {
            RijndaelManaged AES_ = new RijndaelManaged();
            AES_.GenerateKey();
            AES_.GenerateIV();
            byte[] key = AES_.Key;
            byte[] iv = AES_.IV;

            //Debug.Log("createAES_Key key.Length=" + key.Length+ "/iv.Length=" + iv.Length);           

            this.AES_Key = Convert.ToBase64String(key);
            this.AES_IV = Convert.ToBase64String(iv);

            AES_.Dispose();
        }
    }

    public class KeyPair
    {
        public string privateKeyText;
        public string publicKeyText;
    }
}