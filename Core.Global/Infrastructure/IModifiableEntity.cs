﻿using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Global.Infrastructure
{
    public interface IModifiableEntity : IBaseEntity
    {
        public string CreatedBy { get; set; }
        public DateTime Created { get; set; }
        public string LastModifiedBy { get; set; }
        public DateTime? LastModified { get; set; }
    }
}
