﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileCabinetApp.Readers
{
    public interface IFileCabinetRecordReader
    {
        IList<FileCabinetRecord> ReadAll();
    }
}