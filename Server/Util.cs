//using Amazon.Runtime.Internal;
using eamuse.KBin;
using eamuse.Compression;
using System.Security.Cryptography;
using System.Xml.Linq;
//using ZstdSharp;
using eamuse.KBin;

namespace eamuse
{
    public class Util
    {
        public static string randomCard { get; set; }
        public static string randomCode { get; set; }

        //public static Option tempCard { get; set; }//for new card cases


        private static readonly byte[] konamiCode =
            new byte[]{ 0x69, 0xd7, 0x46, 0x27, 0xd9, 0x85, 0xee, 0x21, 0x87, 0x16, 0x15, 0x70, 0xd0,
                        0x8d, 0x93, 0xb1, 0x24, 0x55, 0x03, 0x5b, 0x6d, 0xf0, 0xd8, 0x20, 0x5d, 0xf5 };
        public static byte[] ExtractRequest(HttpRequest r, string amuse, string compress)
        {
            if (compress == null)
                compress = "";

            //byte[] olddata;
            byte[] byteData;
            using (var ms = new MemoryStream((int)(r.ContentLength ?? 512L)))
            {
                r.Body.CopyTo(ms);
                byteData = ms.ToArray();
            }
            if (amuse != null)
                ApplyEAmuseInfo(amuse, byteData);

            if (compress.ToLower() == "lz77")
                byteData = LZ77.Decompress(byteData);
            //else
            //    newdata = olddata;

            return byteData;
        }

        public static byte[] CompressRequest(XDocument d, string amuse, string algo)
        {
            byte[] resData = new KBinXML(d).Bytes;
            //if (algo == "lz77")
            //{
            //resData = LZ77.Compress(resData, 32);
            resData = LZ77.Compress(resData, 32);
            //}

            if (amuse != null)            
                ApplyEAmuseInfo(amuse, resData);
            
            return resData;
        }

        public static void ApplyEAmuseInfo(string info, byte[] data)
        {
            if (!info.StartsWith("1-") || info.Count(c => c == '-') != 2 || info.Length != 15)
                throw new ArgumentException("Unknown E-Amuse-Info format.", "info");
            info = info.Substring(2).Replace("-", "");
            byte[] key = Enumerable.Range(0, info.Length / 2).Select(i => Convert.ToByte(info.Substring(i * 2, 2), 16)).ToArray();
            ApplyEAmuse(key, data);
        }
        public static void ApplyEAmuse(byte[] key, byte[] data)
        {
            if (key.Length != 6)
                throw new ArgumentException("Key length has to be exactly 6 bytes.", "key");

            using (MD5 md5 = MD5.Create())
            {
                byte[] realKey = md5.ComputeHash(key.Concat(konamiCode).ToArray());
                EncryptOutput(realKey, data);
            }
        }
        private static void EncryptOutput(byte[] key, byte[] data)
        {
            byte[] s = EncryptInitalize(key);

            uint i = 0;
            uint j = 0;

            for (int k = 0; k < data.Length; ++k)
            {
                i = (i + 1) & 0xff;
                j = (j + s[i]) & 0xff;

                Swap(s, i, j);

                data[k] ^= s[(s[i] + s[j]) & 0xff];
            };
        }
        private static byte[] EncryptInitalize(byte[] key)
        {
            byte[] s = Enumerable.Range(0, 0x100).Select(i => (byte)i).ToArray();

            for (uint i = 0, j = 0; i < 0x100; i++)
            {
                j = (j + key[i % key.Length] + s[i]) & 0xff;
                Swap(s, i, j);
            }

            return s;
        }

        private static void Swap(byte[] s, uint i, uint j)
        {
            byte c = s[i];

            s[i] = s[j];
            s[j] = c;
        }

        public static void RandomCardString()
        {
            var chars = "ABCDEF0123456789";
            var stringChars = new char[16];
            var random = new Random();

            for (int i = 0; i < stringChars.Length; i++)            
                stringChars[i] = chars[random.Next(chars.Length)];            

            randomCard = new String(stringChars);
        }
        public static string RandomSeqCode()
        {
            var chars = "123456789";//removed the '0'
            var stringChars = new char[4];
            var random = new Random();

            for (int i = 0; i < stringChars.Length; i++)            
                stringChars[i] = chars[random.Next(chars.Length)];            

            return new String(stringChars);
        }

        public static XElement FindMusicBymcode(int mcode)
        {
            var filesPath = Directory.GetCurrentDirectory() + @"\musicdb.xml";
            XDocument xml = XDocument.Load(filesPath);
            XElement mdb = xml.Document.Element("mdb");
            IEnumerable<XElement> music = mdb.Elements("music");
            return music.FirstOrDefault(x => (int)x.Element("mcode") == mcode);
            
        }
    }
}
