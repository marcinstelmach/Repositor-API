using System;
using System.Linq;

namespace RepositoryApp.Service.Helpers
{
    public static class StringExtension
    {
        public static string RandomString(this string str, int length)
        {
            const string pool = "abcdefghijklmnopqrstuvwxyz0123456789";
            var rnd = new Random();
            var chars = Enumerable.Range(0, length)
                .Select(x => pool[rnd.Next(0, pool.Length)]);
            return new string(chars.ToArray());
        }
    }
}