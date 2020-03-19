using System;
using NaughtyAttributes;

namespace MIT.SamtleGame.Attributes
{
    [AttributeUsage(AttributeTargets.Field, AllowMultiple = false, Inherited = true)]
    public class GameAudioAttribute : DrawerAttribute
    {
        public GameAudioAttribute()
        {
            
        }
    }
}