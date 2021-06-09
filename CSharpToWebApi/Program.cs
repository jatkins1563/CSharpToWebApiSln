using System;
using System.ComponentModel.DataAnnotations;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;

namespace CSharpToWebApi
{
    class Employee
    {
        public int Id { get; set; }
        [Required, StringLength(30)]
        public string Login { get; set; }
        [Required, StringLength(30)]
        public string Password { get; set; }
        [Required, StringLength(30)]
        public string Firstname { get; set; }
        [Required, StringLength(30)]
        public string Lastname { get; set; }
        public bool IsManager { get; set; }

        public Employee() { }
    }
    class Program
    {
        async Task Run()
        {
            var http = new HttpClient();
            var jsonSerializerOptions = new JsonSerializerOptions()
            {
                PropertyNameCaseInsensitive = true,
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            };
            var url = "http://localhost:59780/api/employees";
            
            var newEmpl = new Employee()
            {
                Id = 0,
                Firstname = "Noah",
                Lastname = "Phence",
                Login = "nphence",
                Password = "password",
                IsManager = true
            };
            var json = JsonSerializer.Serialize<Employee>(newEmpl, jsonSerializerOptions);
            var httpContent2 = new StringContent(json, System.Text.Encoding.UTF8, "application/json");

            var httpMessageResponse = await http.GetAsync(url);
            var httpContent = await httpMessageResponse.Content.ReadAsStringAsync();
            var employees = JsonSerializer.Deserialize<Employee[]>(httpContent, jsonSerializerOptions);
            var httpMessageResponse2 = await http.PostAsync(url, httpContent2);

            foreach(var e in employees)
            {
                Console.WriteLine($"{e.Id} | {e.Lastname}");
            }
        }
        async static Task Main(string[] args)
        {
            var pgm = new Program();
            await pgm.Run();
        }
    }
}
