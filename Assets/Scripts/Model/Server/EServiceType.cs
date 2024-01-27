using System;

namespace NetowrkServiceType
{
    public enum EServiceType
    {
        PlayerCoordinate = 1,
        Test = 2,
        HealthCheck = 3
    }

    public static class EServiceTypeExtensions
    {
        public static EServiceType? Convert(int type)
        {
            if (Enum.IsDefined(typeof(EServiceType), type))
            {
                return (EServiceType)type;
            }

            return null;
        }
    }
}
