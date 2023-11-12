using System.Text;

namespace KerryCoAdmin.Modules
{
    public class Generate
    {

        //generate username
        public static string GenerateUsername(string firstName, string lastName, string email)
        {
            string subFirst = firstName.Substring(0, 3);
            string subLast = lastName.Substring(0, 3);
            string subEmail = email.Substring(3, 1);

            string newName = $"{subLast.ToLower()}{subFirst.ToLower()}{subEmail.ToLower()}";

            return newName;
        }



        // generate a random password
        public static string GeneratePassword(int size)
        {
            StringBuilder builder = new StringBuilder();
            builder.Append(RandomString(size, true));
            builder.Append(RandomNumber(1000, 9999));
            builder.Append(RandomString(size, false));
            return builder.ToString();
        }


        // Generate a random number between two numbers
        private static int RandomNumber(int min, int max)
        {
            Random random = new Random();
            return random.Next(min, max);
        }


        // Generate a random string with a given size and case.
        // If second parameter is true, the return string is lowercase
        private static string RandomString(int size, bool lowerCase)
        {
            StringBuilder builder = new StringBuilder();
            Random random = new Random();
            char ch;
            for (int i = 0; i < size; i++)
            {
                ch = Convert.ToChar(Convert.ToInt32(Math.Floor(26 * random.NextDouble() + 65)));
                builder.Append(ch);
            }
            if (lowerCase)
                return builder.ToString().ToLower();
            return builder.ToString();
        }


    }
}
