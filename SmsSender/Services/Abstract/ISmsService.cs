﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmsSender.Services.Abstract
{
    public interface ISmsService
    {
        string Sender(string phone, string message);
    }
}
