namespace StoreManagementSystem.API.Services
{
    public interface IBarcodeService
    {
        string GenerateEan13();
    }

    public class BarcodeService : IBarcodeService
    {
        private static readonly Random _random = new Random();

        public string GenerateEan13()
        {
            // Bulgarian country code range is 380
            string code = "380" + _random.Next(100000000, 999999999).ToString("D9");
            
            // Calculate Check Digit
            int sum = 0;
            for (int i = 0; i < 12; i++)
            {
                int digit = int.Parse(code[i].ToString());
                sum += (i % 2 == 0) ? digit : digit * 3;
            }
            int checkDigit = (10 - (sum % 10)) % 10;
            
            return code + checkDigit.ToString();
        }
    }
}
