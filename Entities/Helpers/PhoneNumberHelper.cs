namespace IS_Proj_HIT.Entities.Helpers
{
    public static class PhoneNumberHelper
    {
        public static string FormatPhoneNumber(string phoneNumber)
        {
            if (string.IsNullOrWhiteSpace(phoneNumber)) return phoneNumber;

            return phoneNumber
                    .Replace("(", "")
                    .Replace("-", "")
                    .Replace(")", "")
                    .Trim();
        }
    }
}
