﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImageServiceCommunication
{
    interface IClientCommunication: IClientChannel
    {
        int send(string data);
    }
}
