﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinancialSupport.Domain.Entities
{
    public abstract class Entity
    {   
        public int? Id { get; protected set; }
    }
}
