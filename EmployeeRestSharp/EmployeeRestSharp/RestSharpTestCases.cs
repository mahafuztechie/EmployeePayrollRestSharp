using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Net;

namespace EmployeeRestSharp
{
 
        public class Employee
        {
            public int id { get; set; }
            public string first_name { get; set; }
            public string salary { get; set; }
      
        }
        [TestClass]
        public class RestSharpTestCase
        {
            RestClient client;
            [TestInitialize]
            public void SetUp()
            {
                client = new RestClient("http://localhost:4000");
            }

            private RestResponse GetEmployeeList()
            {
                // arrange
                RestRequest request = new RestRequest("/employees", Method.Get);

            // act
            RestResponse response = client.ExecuteAsync(request).Result;

            return response;
            }

        [TestMethod]
        public void onCallingGETApi_ReturnEmployeeList()
        {
            RestResponse response = GetEmployeeList();

            // Assert
            Assert.AreEqual(response.StatusCode, HttpStatusCode.OK);
            List<Employee> dataResponse = JsonConvert.DeserializeObject<List<Employee>>(response.Content);
            Assert.AreEqual(5, dataResponse.Count);

            foreach (Employee emp in dataResponse)
            {
                System.Console.WriteLine("id: " + emp.id + ", Name: " + emp.first_name + ", Salary: " + emp.salary);
            }
        }

        //UC2
        [TestMethod]
        public void OnCallingPostAPI_ReturnEmployeeObject()
        {
            // Arrange
            // endpoint and POST method
            RestRequest request = new RestRequest("/employees/add_emp", Method.Post);
            //JObject jObjectBody = new JObject();
            //jObjectBody.Add("name", "Clark");
            //jObjectBody.Add("salary", "15000");
            request.AddHeader("Content-type", "application/json");
            request.AddJsonBody(
            new
            {
                first_name = "Clark",
                salary = "15000"
            });

            // Added parameters to the request object, content-type and jObjectBody with the request
            //request.AddParameter("application/json", jObjectBody, ParameterType.RequestBody);

            //Act
            RestResponse response = client.PostAsync(request).Result;

            //Assert
            Assert.AreEqual(response.StatusCode, HttpStatusCode.Created);
            Employee dataResponse = JsonConvert.DeserializeObject<Employee>(response.Content);
            Assert.AreEqual("Clark", dataResponse.first_name);
            Assert.AreEqual("15000", dataResponse.salary);
            System.Console.WriteLine(response.Content);
        }

        //UC3
        [TestMethod]
        public void GivenMultipleEmployee_OnPost_ThenShouldReturnEmployeeList()
        {
            // Arrange
            List<Employee> employeeList = new List<Employee>();
            employeeList.Add(new Employee { first_name = "sahil", salary = "15000" });
            employeeList.Add(new Employee { first_name = "ajit", salary = "10000" });

            foreach (var emp in employeeList)
            {
                // POST method and endpoint
                RestRequest request = new RestRequest("/employees/add_emp", Method.Post);
              
                request.AddHeader("Content-type", "application/json");
                request.AddJsonBody(
                new
                {
                    first_name = emp.first_name,
                    salary = emp.salary
                });
                //Act
                RestResponse response = client.ExecuteAsync(request).Result;

                //Assert
                Assert.AreEqual(response.StatusCode, HttpStatusCode.Created);
                Employee employee = JsonConvert.DeserializeObject<Employee>(response.Content);
                Assert.AreEqual(emp.first_name, employee.first_name);
                Assert.AreEqual(emp.salary, employee.salary);
                System.Console.WriteLine(response.Content);
            }
        }

        //UC4
        [TestMethod]
        public void OnCallingPutAPI_ReturnEmployeeObject()
        {
            // Arrange
            RestRequest request = new RestRequest("/employees/update_emp/7", Method.Put);
            request.AddHeader("Content-type", "application/json");
            request.AddJsonBody(
            new
            {
                first_name = "Michael Clark",
                salary = "45000"
            });
            // Act
            RestResponse response = client.ExecuteAsync(request).Result;

            // Assert
            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
            Employee employee = JsonConvert.DeserializeObject<Employee>(response.Content);
            Assert.AreEqual("Michael Clark", employee.first_name);
            Assert.AreEqual("45000", employee.salary);
            Console.WriteLine(response.Content);
        }


        [TestMethod]
        public void OnCallingDeleteAPI_ReturnSuccessStatus()
        {
            // Arrange
            RestRequest request = new RestRequest("/employees/delete_emp/2", Method.Delete);

            // Act
            RestResponse response = client.ExecuteAsync(request).Result;

            // Assert
            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
            Console.WriteLine(response.Content);
        }
    }
}
