using System;
using System.Text;

public static class CodeGenerator
{
    private static readonly string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";

    public static string GenerateRandomCode(int length = 5)
    {
        StringBuilder result = new StringBuilder();
        Random random = new Random();

        for (int i = 0; i < length; i++)
        {
            result.Append(chars[random.Next(chars.Length)]);
        }

        return result.ToString();
    }
}
