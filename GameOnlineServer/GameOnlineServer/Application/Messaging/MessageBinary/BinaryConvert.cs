using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameOnlineServer.Application.Messaging.MessageBinary
{
    public static class BinaryConvert
    {
        public static byte[] Add(byte[] array1, byte[] array2)
        {
            byte[] result = new byte[array1.Length + array2.Length];
            Array.Copy(array1, 0, result, 0, array1.Length);
            Array.Copy(array2, 0, result, array1.Length, array2.Length);
            return result;
        }
        public static byte[] Append(byte[] array, byte value)
        {
            byte[] result = new byte[array.Length + 1];
            Array.Copy(array, result, array.Length);
            result[result.Length - 1] = value;
            return result;
        }
        public static byte[] AddFirst(byte newByte, byte[] originalArray)
        {
            byte[] newArray = new byte[originalArray.Length + 1];
            newArray[0] = newByte;
            Array.Copy(originalArray, 0, newArray, 1, originalArray.Length);
            return newArray;
        }
        public static float ConvertFloat(byte[] bytes,byte startIndex,byte length) 
        {
            byte[] result = new byte[length];
            Array.Copy(bytes, startIndex, result, 0, length);
            return BitConverter.ToSingle(result);
        }
        public static string ConvertString(byte[] bytes)
        {
            string result = Encoding.UTF8.GetString(bytes, 1, bytes.Length - 1);
            return result;
        }
    }
}
