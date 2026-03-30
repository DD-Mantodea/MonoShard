using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonoShard.Contents.Attributes
{
    [AttributeUsage(AttributeTargets.Class, Inherited = true)]
    public class AutoRegisterAttribute : Attribute { }
}
