﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EMS.Common;
using System.ServiceModel;

namespace EMS.ServiceContracts
{
    [ServiceContract]    
    public interface ImporterContract
    {
        [OperationContract]
        bool ModelUpdate(Delta delta);
    }
}
