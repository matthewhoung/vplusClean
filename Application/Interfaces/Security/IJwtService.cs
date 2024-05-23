﻿using Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces.Security
{
    public interface IJwtService
    {
        string GenerateToken(User user);
    }
}