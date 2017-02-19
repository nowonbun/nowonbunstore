using System;

namespace HouseholdORM
{
    class HouseholdORMException : Exception
    {
        public HouseholdORMException(String message)
            : base(message)
        { }
    }
}
