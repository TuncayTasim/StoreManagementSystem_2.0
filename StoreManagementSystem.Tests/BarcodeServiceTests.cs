using StoreManagementSystem.API.Helpers;
using StoreManagementSystem.API.Services;
using Xunit;

namespace StoreManagementSystem.Tests
{
    public class BarcodeServiceTests
    {

        [Fact]
        public void GenerateEan13_ReturnsValidLength()
        {
            // Act
            string barcode = BarcodeGenerator.GenerateEan13();

            // Assert
            Assert.Equal(13, barcode.Length);
            Assert.StartsWith("380", barcode);
        }

        [Fact]
        public void GenerateEan13_CalculatesValidCheckDigit()
        {
            // Act
            string barcode = BarcodeGenerator.GenerateEan13();
            string data = barcode.Substring(0, 12);
            int expectedCheck = int.Parse(barcode[12].ToString());

            // Recalculate check digit
            int sum = 0;
            for (int i = 0; i < 12; i++)
            {
                int digit = int.Parse(data[i].ToString());
                sum += (i % 2 == 0) ? digit : digit * 3;
            }
            int calculatedCheck = (10 - (sum % 10)) % 10;

            // Assert
            Assert.Equal(calculatedCheck, expectedCheck);
        }
    }
}
