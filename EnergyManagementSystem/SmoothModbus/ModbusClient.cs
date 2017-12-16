﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using NModbus;

namespace SmoothModbus
{
	public class ModbusClient
	{
		private TcpClient client;
		private byte[] receiveData = new byte[100];
		private byte[] sendData = new byte[5];

		private byte[] header = new byte[7]
		{
			9,	//Transaction Identifier (2 bytes)
			0,
			0,	//Protocol Identifier 
			0,
			0,	//Length (2 bytes)
			5,
			0	//Unit identifier
		};
		#region Constructors

		public ModbusClient()
		{
			client = new TcpClient();
		}

		public ModbusClient(string ipAddress, int port)
		{
			IPAddress = ipAddress;
			Port = port;
			client = new TcpClient(ipAddress, port);
		}
		#endregion

		#region Properties

		public bool Connected
		{
			get
			{
				return client.Connected;
			}
		}

		public string IPAddress { get; set; }

		public int Port { get; set; }

		#endregion

		public void Connect()
		{
			if (Connected)
			{
				return;
			}

			try
			{
				client.Connect(IPAddress, Port);
			}
			catch (Exception e)
			{
				Console.WriteLine("Connection failed! Reason: " + e.Message);
			}
		}
		public void Connect(string ipAddress, int port)
		{
			IPAddress = ipAddress;
			Port = port;
			Connect();
		}

		public void Disconnect()
		{
			client.Client.Disconnect(true);
		}

		public bool[] ReadCoils(ushort startingAddress, ushort quantity)
		{
			byte[] result = SendAndReceive(startingAddress, quantity, FunctionCode.ReadCoils);
			return byteToBoolArray(result[9], quantity);
		}

		public bool[] ReadDiscreteInputs(ushort startingAddress, ushort quantity)
		{
			byte[] result = SendAndReceive(startingAddress, quantity, FunctionCode.ReadDiscreteInputs);
			return byteToBoolArray(result[9], quantity);
		}

		public byte[] ReadInputRegisters(ushort startingAddress, ushort quantity)
		{
			byte[] result = SendAndReceive(startingAddress, quantity, FunctionCode.ReadInputRegisters);
			return StripHeader(result);
		}

		public byte[] ReadHoldingRegisters(ushort startingAddress, ushort quantity)
		{
			byte[] result = SendAndReceive(startingAddress, quantity, FunctionCode.ReadHoldingRegisters);
			return StripHeader(result);
		}

		public void WriteSingleCoil(ushort startingAddress, bool value)
		{
			ushort shortValue = value == true ? (ushort)0xFF00 : (ushort)0x0000;

			SendForWrite(startingAddress, shortValue, FunctionCode.WriteSingleCoil);
		}

		public void WriteSingleRegister(ushort startingAddress, ushort value)
		{
			SendForWrite(startingAddress, value, FunctionCode.WriteSingleRegister);
		}

		public void WriteSingleRegister(ushort startingAddress, float value)
		{
			byte[] bytes = BitConverter.GetBytes(value);
			ushort upper = BitConverter.ToUInt16(bytes, 0);
			ushort lower = BitConverter.ToUInt16(bytes, 2);

			SendForWrite(startingAddress, upper, FunctionCode.WriteSingleRegister);
			SendForWrite((ushort)(startingAddress + 1), lower, FunctionCode.WriteSingleRegister);
		}

		private byte[] SendAndReceive(ushort startingAddress, ushort quantity, FunctionCode functionCode)
		{
			byte[] data = PreparePackageForRead(startingAddress, quantity, functionCode);

			int numberOfBytes = client.Client.Send(data);
			Console.WriteLine("Sent {0} bytes", numberOfBytes);

			numberOfBytes = client.Client.Receive(receiveData);
			Console.WriteLine("Received {0} bytes", numberOfBytes);
			return receiveData;
		}

		private byte[] SendForWrite(ushort outputAddress, ushort outputValue, FunctionCode functionCode)
		{
			byte[] data = PreparePackageForWrite(outputAddress, outputValue, functionCode);

			int numberOfBytes = client.Client.Send(data);
			Console.WriteLine("Sent {0} bytes", numberOfBytes);

			numberOfBytes = client.Client.Receive(receiveData);
			Console.WriteLine("Received {0} bytes", numberOfBytes);
			return receiveData;
		}

		private byte[] StripHeader(byte[] data)
		{
			byte[] returnData = new byte[data.Length - header.Length];

			Array.Copy(data, header.Length, returnData, 0, returnData.Length);
			return returnData;
		}

		private byte[] PreparePackageForRead(ushort startingAddress, ushort quantity, FunctionCode functionCode)
		{
			byte[] startAddressBytes = BitConverter.GetBytes(startingAddress);
			byte[] quanityBytes = BitConverter.GetBytes(quantity);

			sendData[0] = (byte)functionCode;
			sendData[1] = startAddressBytes[1];
			sendData[2] = startAddressBytes[0];
			sendData[3] = quanityBytes[1];
			sendData[4] = quanityBytes[0];

			byte[] data = new byte[header.Length + sendData.Length + 1];

			header.CopyTo(data, 0);
			sendData.CopyTo(data, header.Length);
			return data;
		}

		private byte[] PreparePackageForWrite(ushort outputAddress, ushort outputValue, FunctionCode functionCode)
		{
			byte[] startAddressBytes = BitConverter.GetBytes(outputAddress);
			byte[] outValue = BitConverter.GetBytes(outputValue);

			sendData[0] = (byte)functionCode;
			sendData[1] = startAddressBytes[1];
			sendData[2] = startAddressBytes[0];
			sendData[3] = outValue[0];
			sendData[4] = outValue[1];

			byte[] data = new byte[header.Length + sendData.Length + 1];

			header.CopyTo(data, 0);
			sendData.CopyTo(data, header.Length);
			return data;
		}

		private bool[] byteToBoolArray(byte bitArray, int arrayCount)
		{
			// prepare the return result
			bool[] result = new bool[8];

			// check each bit in the byte. if 1 set to true, if 0 set to false
			for (int i = 0; i < 8; i++)
				result[i] = (bitArray & (1 << i)) == 0 ? false : true;

			bool[] retList = new bool[arrayCount];
			Array.Copy(result, retList, arrayCount);
			return retList;
		}

	}
}
