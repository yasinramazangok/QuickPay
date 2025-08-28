using System.Text.RegularExpressions;

namespace QuickPay.Application.Utils
{
    public static class CardValidator
    {
        // Remove spaces and dashes
        public static string NormalizeCardNumber(string pan)
        {
            if (pan == null) return string.Empty;
            return Regex.Replace(pan, "[^0-9]", "");
        }

        // Luhn algorithm implementation
        public static bool IsValidLuhn(string cardNumber)
        {
            if (string.IsNullOrWhiteSpace(cardNumber)) return false;
            if (cardNumber.Length < 13 || cardNumber.Length > 19) return false;

            int sum = 0;
            bool alternate = false;

            for (int i = cardNumber.Length - 1; i >= 0; i--)
            {
                if (!char.IsDigit(cardNumber[i])) return false;
                int n = cardNumber[i] - '0';
                if (alternate)
                {
                    n *= 2;
                    if (n > 9) n -= 9;
                }
                sum += n;
                alternate = !alternate;
            }

            return (sum % 10) == 0;
        }

        // Expiry format MM/YY or MM/YYYY
        public static bool IsValidExpiry(string expiry)
        {
            if (string.IsNullOrWhiteSpace(expiry)) return false;

            // Accept MM/YY or MM/YYYY
            var parts = expiry.Split('/');
            if (parts.Length != 2) return false;

            if (!int.TryParse(parts[0], out var month)) return false;
            if (month < 1 || month > 12) return false;

            if (!int.TryParse(parts[1], out var year)) return false;

            if (parts[1].Length == 2)
            {
                // YY -> convert to 20YY (reasonable for demo)
                year += 2000;
            }

            var lastDay = DateTime.DaysInMonth(year, month);
            var expiryDate = new DateTime(year, month, lastDay, 23, 59, 59);

            return expiryDate >= DateTime.UtcNow;
        }

        // CVV is typically 3 or 4 digits
        public static bool IsValidCvv(string cvv)
        {
            if (string.IsNullOrWhiteSpace(cvv)) return false;
            return cvv.Length is 3 or 4 && cvv.All(char.IsDigit);
        }
    }
}
