﻿using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;

namespace PumpService
{
    public interface IServiceSettings
    {
        string FileName { get; set; }
    }
}
