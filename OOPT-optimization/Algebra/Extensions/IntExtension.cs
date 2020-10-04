namespace OOPT.Optimization.Algebra.Extensions
{
    public static class IntExtension
    {
        public static int FastPower(this int source, int pow)
        {
            switch (pow)
            {
                case 0: return 1;
                case 1: return source;
                case 2: return source * source;
                case 3: return source * source * source;
                case 4: return source * source * source * source;
                case 5: return source * source * source * source * source;
                case 6: return source * source * source * source * source * source;
                case 7: return source * source * source * source * source * source * source;
                case 8: return source * source * source * source * source * source * source * source;
                case 9: return source * source * source * source * source * source * source * source * source;
                case 10: return source * source * source * source * source * source * source * source * source * source;
                case 11: return source * source * source * source * source * source * source * source * source * source * source;
                // up to 32 can be added 
                default: // Vilx's solution is used for default
                    int ret = 1;

                    while (pow != 0)
                    {
                        if ((pow & 1) == 1)
                            ret *= source;

                        source *= source;
                        pow >>= 1;
                    }

                    return ret;
            }
        }
    }
}