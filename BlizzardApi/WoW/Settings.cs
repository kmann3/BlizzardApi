namespace BlizzardApi.WoW
{
    /// <summary>
    /// All WoW API settings will reference this file.
    /// This is only to save from having to set the same three settings on everything.
    /// Assigning these settings is not required but recommended to make your life easier.
    /// </summary>
    public static class Settings
    {
        private static Base.Locale? locale = null;

        /// <summary>
        /// The locale to use.
        /// Base.Locale.EN_US is used if no value is assigned.
        /// </summary>
        public static Base.Locale Locale { 
            get
            {
                if (locale == null)
                {
                    return Base.Locale.EN_US;
                } else
                {
                    return (Base.Locale)locale;
                }
            }
            set
            {
                locale = value;
            }
        }

        private static Base.Region? region = null;

        /// <summary>
        /// The region to use.
        /// Base.Region.US is used if no value is assigned.
        /// </summary>
        public static Base.Region Region
        {
            get
            {
                if(region == null)
                {
                    return Base.Region.US;
                } else
                {
                    return (Base.Region)region;
                }
            }
            set
            {
                region = value;
            }
        }

        /// <summary>
        /// Token to use for all queries. This absolutely must be set.
        /// </summary>
        public static string Token { get; set; }

        /// <summary>
        /// Method to help assigning all the suggested values.
        /// </summary>
        /// <param name="_locale"></param>
        /// <param name="_region"></param>
        /// <param name="_token"></param>
        public static void AssignSettings(Base.Locale _locale, Base.Region _region, string _token )
        {
            Locale = _locale;
            Region = _region;
            Token = _token;
        }

    }
}
