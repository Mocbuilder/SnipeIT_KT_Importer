using System.IO;

namespace SnipeIT_KT_Importer
{
    internal class Program
    {
        static void Main(string[] args)
        {
            string pathToExistingCSV = @"";
            string pathToSourceCSV = @"";
            string pathToOutputCSV = @"";

            Console.WriteLine("Enter path to CSV with existing KT's:");
            pathToExistingCSV  = Console.ReadLine();

            Console.WriteLine("Enter path to source CSV:");
            pathToSourceCSV = Console.ReadLine();

            Console.WriteLine("Enter path to Output CSV:");
            pathToOutputCSV = Console.ReadLine();

            WriteOutputCSV(pathToOutputCSV, ReadSourceCSV(pathToSourceCSV, ReadExistingKT(pathToExistingCSV)));

            Console.ReadLine();
        }

        static public List<string> ReadExistingKT(string path)
        {
            List<string> result = new List<string>();
            string[] lines;

            lines = File.ReadLines(path).ToArray();
            lines[0] = "";

            foreach (string line in lines)
            {
                string[] lineValues = line.Split(",");

                if (lineValues[0] == "")
                {
                    continue;
                }

                result.Add(lineValues[0]);
            }

            return result;
        }

        static public List<(string, string)> ReadSourceCSV(string path, List<string> existingKT)
        {
            List<(string, string)> result = new List<(string, string)>();
            string[] lines;

            lines = File.ReadLines(path).ToArray();
            lines[0] = "";

            foreach (string line in lines)
            {
                string[] lineValues = line.Split(",");

                if (lineValues[0] == "" || existingKT.Contains(lineValues[0]))
                {
                    continue;
                }

                string name = lineValues[0];
                string expirationDate = lineValues[1];

                expirationDate = FormatDateStringToYear(expirationDate);

                var tempResult = (name, expirationDate);
                result.Add(tempResult);
            }

            return result;
        }

        public static void WriteOutputCSV(string outputPath, List<(string, string)> output)
        {
            if (string.IsNullOrEmpty(outputPath))
            {
                throw new ArgumentException("Target path cannot be null or empty.");
            }

            if(!Path.Exists(outputPath))
            {
                throw new ArgumentException("Target path is either invalid or inaccesible.");
            }

            using (StreamWriter writer = new StreamWriter(outputPath))
            {
                foreach (var item in output)
                {
                    writer.WriteLine($"{item.Item1},{item.Item2}");
                }
            }
        }

        public static string FormatDateStringToYear(string fullDate)
        {
            string[] dateParts = fullDate.Split(".");

            return dateParts[2];
        }
    }
}
