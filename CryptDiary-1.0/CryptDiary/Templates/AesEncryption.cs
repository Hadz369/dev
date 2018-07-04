using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace CryptDiary
{
    static class AesEncryption
    {
        //Encrypting a string
        public static string passwordEncrypt(string inText, string password)
        {
            string outText;
            // Der Eingabetext wird in ein Byte-Array geschrieben
            byte[] bytesBuff = Encoding.Unicode.GetBytes(inText);
            // Ein AES-Objekt wird kreiert
            using (Aes aes = Aes.Create())
            {
                // Aus dem Passwort wird durch 1000-fache (Standard) Iteration ein Key generiert, ein fester Salt (Ivan Medvedev) wird verwendet
                Rfc2898DeriveBytes crypto = new Rfc2898DeriveBytes(password, new byte[] { 0x49, 0x76, 0x61, 0x6e, 0x20, 0x4d, 0x65, 0x64, 0x76, 0x65, 0x64, 0x65, 0x76 });
                // aus diesem Iterations-Dingens wird nun ein 32 byte (256 bit) langer Key generiert
                aes.Key = crypto.GetBytes(32);
                // und ein 16 byte (128 bit) langer Initialisation Vector (IV)
                aes.IV = crypto.GetBytes(16);
                // ein Memorystream wird instanziiert (disposable)
                using (MemoryStream mStream = new MemoryStream())
                {
                    // ein Cryptostream wird instanziiert, dieser bekommt den Memorystream, einen AES-Encryptor(ICryptoTransform)
                    // und einen Zugriffsmodus (Write) mitgegeben. Auch dieser Cryptostream ist disposable, weil sicherheitskritisch
                    using (CryptoStream cStream = new CryptoStream(mStream, aes.CreateEncryptor(), CryptoStreamMode.Write))
                    {
                        // Der PlainText wird vom CryptoStream verschlüsselt und in den MemoryStream geschrieben
                        cStream.Write(bytesBuff, 0, bytesBuff.Length);
                        // Der CryptoStream wird geschlossen
                        cStream.Close();
                    } // CryptoStream wird zerstört
                    // Der verschlüsselte Ausgabetext wird aus dem Memory-Stream geholt und dieses Byte-Array in
                    // Text umgewandelt (Base64-Codierung)
                    outText = Convert.ToBase64String(mStream.ToArray());
                } // MemoryStream wird zerstört
            } // AES-Objekt wird zerstört
            return outText;
        }
        //Decrypting a string
        public static string passwordDecrypt(string cryptTxt, string password)
        {
            string plainText;
            // Im verschlüsselten Text werden Leerzeichen durch + ersetzt, keine Ahnung, wozu...
            cryptTxt = cryptTxt.Replace(" ", "+");
            // Text wird in byteArray geschrieben
            byte[] bytesBuff = Convert.FromBase64String(cryptTxt);
            // AES-Objekt wird erstellt (disposable)
            using (Aes aes = Aes.Create())
            {
                // Ivan Medvedev kommt wieder als Salt zum Einsatz und es wird iteriert (mit Standardgröße)
                Rfc2898DeriveBytes crypto = new Rfc2898DeriveBytes(password, new byte[] { 0x49, 0x76, 0x61, 0x6e, 0x20, 0x4d, 0x65, 0x64, 0x76, 0x65, 0x64, 0x65, 0x76 });
                // aus diesem Salat wird ein 256 bit Key generiert
                aes.Key = crypto.GetBytes(32);
                // und ein 128 bit Initilisation Vector
                aes.IV = crypto.GetBytes(16);
                // neueer MemoryStream wird instanziiert (disposable)
                using (MemoryStream mStream = new MemoryStream())
                {
                    try
                    {

                        // ein Cryptostream wird instanziiert, dieser bekommt den Memorystream, einen AES-Decryptor(ICryptoTransform)
                        // und einen Zugriffsmodus (Write) mitgegeben. Auch dieser Cryptostream ist disposable, weil sicherheitskritisch
                        using (CryptoStream cStream = new CryptoStream(mStream, aes.CreateDecryptor(), CryptoStreamMode.Write))
                        {
                            // der CipherText wird in den CryptoStream geschrieben
                            cStream.Write(bytesBuff, 0, bytesBuff.Length);
                            // CryptoStream wird geschlossen
                            cStream.Close();
                        } // CryptoStream wird zerstört
                    }
                    catch
                    {
                        throw;
                    }
                    // Plaintext wird aus MemoryStream geholt (Byte-Array wird Unicode-codiert)
                    plainText = Encoding.Unicode.GetString(mStream.ToArray());
                } // Memory-Stream wird zerstört
            } // AES-Objekt wird zerstört
            return plainText;
        }
    }
}
