using System;
using System.Collections.Generic;

namespace Grains
{
    [Serializable]
    public class DomainGrainState
    {
        public HashSet<string> EmailAddresses { get; } = new HashSet<string>();
    }
}