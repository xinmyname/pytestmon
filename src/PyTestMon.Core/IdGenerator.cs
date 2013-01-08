using System;
using System.Text;

namespace PyTestMon.Core
{
    public static class IdGenerator
    {
        public static string Create(string module, string name)
        {
            var idBuilder = new StringBuilder("__");

            idBuilder.Append(module.ToLower());
            idBuilder.Append(name.ToLower());

            for (int i = idBuilder.Length - 1; i >= 2; i--)
            {
                if (!Char.IsLetter(idBuilder[i]))
                    idBuilder.Remove(i, 1);
            }

            return idBuilder.ToString();
        }
    }
}