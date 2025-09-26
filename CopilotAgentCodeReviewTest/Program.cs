using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CopilotAgentCodeReviewTest
{
    internal class Program
    {
        static void Main(string[] args)
        {
        }

        public static float CalculateBMI(float weight, float height)
        {
            // Convert height from cm to meters
            float heightInMeters = height / 100.0f;
            
            // Calculate BMI: weight (kg) / height^2 (m^2)
            float result = weight / (heightInMeters * heightInMeters);
            
            return result;
        }

        public static void Save(string userInputFileName)
        {
            // Validate filename to prevent path traversal attacks
            if (string.IsNullOrWhiteSpace(userInputFileName) || 
                userInputFileName.Contains("..") || 
                userInputFileName.IndexOfAny(System.IO.Path.GetInvalidFileNameChars()) >= 0)
            {
                throw new ArgumentException("Invalid filename provided.", nameof(userInputFileName));
            }
            
            // Use a safe base directory and combine paths securely
            string safeDirectory = System.IO.Path.GetTempPath();
            string filePath = System.IO.Path.Combine(safeDirectory, userInputFileName);
            
            // Example: Save some data to the file
            string dataToSave = "Sample data";
            System.IO.File.WriteAllText(filePath, dataToSave);
        }

        public static DataTable GetUserData(string userInput, SqlConnection connection)
        {
            DataTable dt = new DataTable();
            
            // Use parameterized query to prevent SQL injection
            string query = "SELECT * FROM any.USERS WHERE user_name = @username";
            
            using (SqlCommand cmd = new SqlCommand(query, connection))
            {
                // Add parameter to prevent SQL injection
                cmd.Parameters.AddWithValue("@username", userInput);
                
                // Execute query and fill DataTable
                using (SqlDataAdapter adapter = new SqlDataAdapter(cmd))
                {
                    adapter.Fill(dt);
                }
            }
            
            return dt;
        }
    }
}
