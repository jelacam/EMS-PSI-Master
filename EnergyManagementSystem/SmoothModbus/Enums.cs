﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmoothModbus
{
	public enum FunctionCode : byte
	{
		ReadCoils				= 0x01,
		ReadDiscreteInputs		= 0x02,
		ReadHoldingRegisters    = 0x03,
		ReadInputRegisters      = 0x04,
		WriteSingleCoil         = 0x05,
		WriteSingleRegister     = 0x06
	}
}
