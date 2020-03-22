using System;
using System.Collections.Generic;
using System.Text;

namespace Hera.Common.Core
{
    public static class HeraConstants
    {
        public const string APP_SETTING__ENCRYPTION = "Encryption";
        public const string APP_SETTING__OAUTH_KEY = "OAuthKey";
        public const string APP_SETTING__JWT_TOKEN_DESCRIPTOR = "JwtTokenDescriptor";
        public const string CONNECTION_STRINGS__POSTGRES_SQL_CONNECTION = "PostgresSqlConnection";

        public const string POLICY_BASED_ROLE = "POLICY_BASED_ROLE";
        public const string POLICY_ADMIN_ROLE = "POLICY_ADMIN_ROLE";

        public const string CLAIM_TYPE_ROLES = "Roles";
        public const string CLAIM_HERA_USER = "HERA_USER";
        public const string CLAIM_HERA_USER_ADMIN = "HERA_USER_ADMIN";
        public const string CLAIM_TYPE_BAND = "band";
        public const string CLAIM_TYPE_ONBOARDING = "onboarding";
    }
}
