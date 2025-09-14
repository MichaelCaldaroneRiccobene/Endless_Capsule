using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using UnityEngine;

// Ho Usata AI per criptarlo perché non ho ancora chiaramente capito come farlo
public static class SaveSystem
{
    private const string FileName = "porn.dat"; // file binario (Base64 text)
    private static string GetPath() => Path.Combine(Application.persistentDataPath, FileName);

    // ---------------------------------------------------------
    // IMPORTANT: cambia questa passphrase con una tua frase
    // forte e segreta. NON committarla pubblicamente.
    // ---------------------------------------------------------
    private const string KeyString = "InserisciQuiUnaPassphraseMoltoLungaESicura!"; // cambiala
    private static readonly byte[] Key = DeriveKeyFromPassword(KeyString, 32); // 32 bytes = 256 bit

    // PBKDF2 per derivare la chiave in modo più sicuro che usare directly UTF8.GetBytes
    private static byte[] DeriveKeyFromPassword(string password, int keyBytes)
    {
        // Usare un salt fisso qui mantiene compatibilità tra esecuzioni;
        // per maggiore sicurezza potresti combinare salt dinamico, ma poi devi salvarlo.
        // Qui usiamo un salt costante definito (meglio comunque non hardcodarlo in produzione).
        byte[] salt = Encoding.UTF8.GetBytes("SomeFixedSaltValueShouldBeChanged");
        using (var kdf = new Rfc2898DeriveBytes(password, salt, 10000, HashAlgorithmName.SHA256))
        {
            return kdf.GetBytes(keyBytes);
        }
    }

    // --------------------- API ---------------------
    public static bool Save(SaveData saveData)
    {
        try
        {
            string json = JsonUtility.ToJson(saveData);
            byte[] encrypted = EncryptToBytes(json, Key); // iv + ciphertext
            string b64 = Convert.ToBase64String(encrypted);
            File.WriteAllText(GetPath(), b64);
            return true;
        }
        catch (Exception ex)
        {
            Debug.LogError($"[SaveSystem] Save error: {ex.Message}");
            return false;
        }
    }

    public static bool Exists() => File.Exists(GetPath());

    public static SaveData Load()
    {
        try
        {
            if (!Exists()) return null;
            string b64 = File.ReadAllText(GetPath());
            byte[] data = Convert.FromBase64String(b64);
            string json = DecryptFromBytes(data, Key);
            if (string.IsNullOrEmpty(json)) return null;
            return JsonUtility.FromJson<SaveData>(json);
        }
        catch (Exception ex)
        {
            Debug.LogError($"[SaveSystem] Load error: {ex.Message}");
            return null;
        }
    }

    public static void Delete()
    {
        try
        {
            if (Exists()) File.Delete(GetPath());
        }
        catch (Exception ex)
        {
            Debug.LogError($"[SaveSystem] Delete error: {ex.Message}");
        }
    }

    // ------------------ Encryption helpers ------------------
    // encrypt => output = IV (16 bytes) || ciphertext
    private static byte[] EncryptToBytes(string plainText, byte[] key)
    {
        using (Aes aes = Aes.Create())
        {
            aes.Key = key;
            aes.Mode = CipherMode.CBC;
            aes.Padding = PaddingMode.PKCS7;

            // IV random per salvataggio
            aes.GenerateIV();
            byte[] iv = aes.IV;

            using (ICryptoTransform encryptor = aes.CreateEncryptor(aes.Key, iv))
            {
                byte[] plainBytes = Encoding.UTF8.GetBytes(plainText);
                byte[] cipherBytes = encryptor.TransformFinalBlock(plainBytes, 0, plainBytes.Length);

                // concat IV + cipher
                byte[] result = new byte[iv.Length + cipherBytes.Length];
                Buffer.BlockCopy(iv, 0, result, 0, iv.Length);
                Buffer.BlockCopy(cipherBytes, 0, result, iv.Length, cipherBytes.Length);
                return result;
            }
        }
    }

    // decrypt expects input = IV (16) || ciphertext
    private static string DecryptFromBytes(byte[] data, byte[] key)
    {
        if (data == null || data.Length < 16) return null; // invalid

        byte[] iv = new byte[16];
        Buffer.BlockCopy(data, 0, iv, 0, iv.Length);

        byte[] cipher = new byte[data.Length - iv.Length];
        Buffer.BlockCopy(data, iv.Length, cipher, 0, cipher.Length);

        using (Aes aes = Aes.Create())
        {
            aes.Key = key;
            aes.IV = iv;
            aes.Mode = CipherMode.CBC;
            aes.Padding = PaddingMode.PKCS7;

            using (ICryptoTransform decryptor = aes.CreateDecryptor(aes.Key, aes.IV))
            {
                try
                {
                    byte[] plainBytes = decryptor.TransformFinalBlock(cipher, 0, cipher.Length);
                    return Encoding.UTF8.GetString(plainBytes);
                }
                catch (CryptographicException e)
                {
                    Debug.LogError($"[SaveSystem] Decryption failed: {e.Message}");
                    return null;
                }
            }
        }
    }
}
