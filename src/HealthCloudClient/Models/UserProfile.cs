
namespace HealthCloudClient
{
    using System;

    /// <summary>
    /// The user's profile record
    /// </summary>
    public class UserProfile
    {
        /// <value>The user's first name</value>
        public string FirstName { get; set; }
        /// <value>The user's middle name</value>
        public string MiddleName { get; set; }
        /// <value>The user's last name</value>
        public string LastName { get; set; }
        /// <value>The user's birth date</value>
        public DateTime? Birthdate { get; set; }
        /// <value>The user's postal code</value>
        public string PostalCode { get; set; }
        /// <value>The user's gender</value>
        public string Gender { get; set; }
        /// <value>The user's current height</value>
        public int? Height { get; set; }
        /// <value>The user's current weight</value>
        public int? Weight { get; set; }
        /// <value>The user's preferred locale</value>
        public string PreferredLocale { get; set; }
        /// <value>The last update time of the user's profile record</value>
        public DateTime? LastUpdateTime { get; set; }
        /// <value>The date the user profile record was created</value>
        public DateTime? CreatedTime { get; set; }
    }
}
