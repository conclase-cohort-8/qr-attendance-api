using System.Text.RegularExpressions;

namespace QrAttendanceApi.Application.Helpers
{
    public class ValidatorHelper
    {
        public static bool IsAValidName(string name)
        {
            var nameCount = name.Split(' ').Length;
            if (nameCount <= 1 && nameCount > 3)
            {
                return false;
            }

            var match = Regex.Match(name, @"^[A-Za-z]+(?: [A-Za-z]+)*$");
            return match.Success;
        }

        public static bool PasswordMatch(string password, string comparePassword)
        {
            return !string.IsNullOrWhiteSpace(password) && !string.IsNullOrWhiteSpace(comparePassword)
                && password.Equals(comparePassword);
        }

        public static bool IsAValidFullName(string fullName)
        {
            var splitted = fullName.Split(' ');
            return splitted.Length > 1;
        }

        public static bool IsAValidPhoneNumber(string? phoneNumber)
        {
            if (string.IsNullOrWhiteSpace(phoneNumber)) return true;

            var match = Regex.Match(phoneNumber, @"^\+(\d{1,4})[-\s]?(\d{6,14})$");

            return match.Success;
        }

        public static bool VaidDate(DateTime date)
        {
            return date > DateTime.UtcNow;
        }
    }
}
