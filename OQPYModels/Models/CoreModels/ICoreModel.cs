﻿using System;
using System.Collections.Generic;
using System.Text;

namespace OQPYModels.Models.CoreModels
{
    public interface ICoreModel<T> where T : ICoreModel<T>
    {
        bool Filter(T one, T two);
    }
}
