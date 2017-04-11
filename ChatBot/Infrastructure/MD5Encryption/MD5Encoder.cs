using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Security.Cryptography;
using System.Text;

namespace ChatBot.Infrastructure.MD5Encryption
{
    public  class MD5Encoder
    {
        public static string MD5Hash(string text)
        {
            //MD5 md5 = MD5.Create();
            //md5.ComputeHash(ASCIIEncoding.ASCII.GetBytes(text));
            //byte[] result = md5.ComputeHash();
            //StringBuilder builder = new StringBuilder();
            //for (int i = 0; i < result.Length; i++)
            //{
            //    builder.Append(result[i].ToString("x2"));
            //}
            //return builder.ToString();
            using (var md5 = MD5.Create())
            {
                var result = md5.ComputeHash(Encoding.ASCII.GetBytes(text));
                StringBuilder builder = new StringBuilder();
                for (int i = 0; i < result.Length; i++)
                {
                    builder.Append(result[i].ToString("x2"));
                }
                return builder.ToString();
            }
        }
    }
}
