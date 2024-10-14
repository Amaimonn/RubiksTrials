using System;

namespace EnumPro
{
    public static class EnumPro
    {
        public static int GetFlagsCount(long lValue)
        {
            int iCount = 0;

            while (lValue != 0)
            {
                lValue &= lValue - 1;

                iCount++;
            }

            return iCount;
        }

        public static int GetFlagsCount<T>(this T definedEnum) where T : Enum
        {
            int totalCount = 0;
            foreach (T enumValue in Enum.GetValues(typeof(T)))
            {
                if (definedEnum.HasFlag(enumValue))
                {
                    totalCount++;
                }
            }
            return totalCount;
        }
    }
}