namespace ReBook.Utilities
{
    public static class ISBN10
    {
        public static string Convert(string ISBN10)
        {
            string ISBN13 = "978" + ISBN10.Substring(0, 9);

            int[] split = ISBN13.Where(x => char.IsNumber(x)).Select(x => x - 48).ToArray();

            int sum = split[0] * 1 +
                        split[1] * 3 +
                        split[2] * 1 +
                        split[3] * 3 +
                        split[4] * 1 +
                        split[5] * 3 +
                        split[6] * 1 +
                        split[7] * 3 +
                        split[8] * 1 +
                        split[9] * 3 +
                        split[10] * 1 +
                        split[11] * 3;

            int modulo = sum % 10;

            if (modulo == 0)
            {
                ISBN13 += (0).ToString();
            }
            else
            {
                ISBN13 += (10 - modulo).ToString();
            }

            return ISBN13;
        }
    }
}
