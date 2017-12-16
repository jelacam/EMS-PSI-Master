using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmoothModbus
{
	public static class ModbusHelper
	{
		private static Dictionary<Type, int> typeToByteCountDictionary = new Dictionary<Type, int>()
		{
			{typeof(int),    sizeof(int)},
			{typeof(long),   sizeof(long)},
			{typeof(float),  sizeof(float)},
			{typeof(double), sizeof(double)},
			{typeof(ushort), sizeof(ushort)},
			{typeof(short),  sizeof(short)},
			{typeof(byte),   sizeof(byte) }
		};

		//Dictionary that converts type into ConversionFunction
		//ConversionFunction return converted byte[] to T 
		private static Dictionary<Type, Func<byte[], object>> typeToConversionFunction = new Dictionary<Type, Func<byte[], object>>()
		{
			{typeof(int),    (byte[] byteArray)=> { return BitConverter.ToInt32(byteArray,0); } },
			{typeof(long),   (byte[] byteArray)=> { return BitConverter.ToInt64(byteArray,0); } },
			{typeof(float),  (byte[] byteArray)=> { return BitConverter.ToSingle(byteArray,0); } },
			{typeof(double), (byte[] byteArray)=> { return BitConverter.ToDouble(byteArray,0); } },
			{typeof(ushort), (byte[] byteArray)=> { return BitConverter.ToUInt16(byteArray,0); } },
			{typeof(short),  (byte[] byteArray)=> { return BitConverter.ToInt16(byteArray,0); } },
			{typeof(byte),   (byte[] byteArray)=> { return byteArray[0]; } }
		};

		/// <summary>
		/// Generic method that converts byteArray to desired type - T
		/// </summary>
		/// <typeparam name="T">Desired type</typeparam>
		/// <param name="byteArray">Array of bytes</param>
		/// <param name="arrayLength">Array length</param>
		/// <param name="startIndex">Start index of array (optional), default is 0</param>
		/// <returns></returns>
		public static T[] GetValueFromByteArray<T>(byte[] byteArray, int arrayLength, int startIndex = 0)
		{
			int sizeofType = typeToByteCountDictionary[typeof(T)];

			int numberOfValues = arrayLength / sizeofType;
			T[] genericArray = new T[numberOfValues];
			for (int i = 0; i < numberOfValues; i++)
			{
				byte[] valueInBytes = new byte[sizeofType];
				Array.Copy(byteArray, startIndex + i * sizeofType, valueInBytes, 0, sizeofType);
			//	valueInBytes = valueInBytes.Reverse().ToArray();
				//var v = valueInBytes[2];
				//valueInBytes[2] = valueInBytes[3];
				//valueInBytes[3] = v;

				genericArray[i] = ((T)typeToConversionFunction[typeof(T)].Invoke(valueInBytes));
			}

			return genericArray;
		}
	}
}
