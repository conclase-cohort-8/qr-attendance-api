namespace QrAttendanceApi.Application.Helpers
{
    internal class StringHelpers
    {
        public static string ConvertEachWordToUppercase(string input)
        {
            //Toba Ojo
            var words = input.ToLowerInvariant()
                    .Split(' ', StringSplitOptions.RemoveEmptyEntries)
                    .Select(word => char.ToUpperInvariant(word[0]) + word[1..]);

            return string.Join(' ', words);
        }
    }
}
