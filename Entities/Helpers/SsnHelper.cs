namespace IS_Proj_HIT.Entities.Helpers
{
    public static class SsnHelper
    {
        public static string FormatSsn(string ssn)
        {
            if (string.IsNullOrWhiteSpace(ssn)) return ssn;

            return ssn.Replace("-", "");
        }
    }
}