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
            Assert.AreEqual(3, dataResponse.Count);

            foreach (Employee emp in dataResponse)
            {
                System.Console.WriteLine("id: " + emp.id + ", Name: " + emp.first_name + ", Salary: " + emp.salary);
            }
        }

        [TestMethod]
        public void OnCallingPostAPI_ReturnEmployeeObject()
        {
            // Arrange
            // endpoint and POST method
            RestRequest request = new RestRequest("/employees", Method.Post);
            JObject jObjectBody = new JObject();
            jObjectBody.Add("first_name", "Clark");
            jObjectBody.Add("salary", "15000");

            // Added parameters to the request object, content-type and jObjectBody with the request
            request.AddParameter("application/json", jObjectBody, ParameterType.RequestBody);

            //Act
            RestResponse response = client.ExecuteAsync(request).Result;

            //Assert
            Assert.AreEqual(response.StatusCode, HttpStatusCode.Created);
            //Employee dataResponse = JsonConvert.DeserializeObject<Employee>(response.Content);
            //Assert.AreEqual("Clark", dataResponse.first_name);
            //Assert.AreEqual("15000", dataResponse.salary);
            //System.Console.WriteLine(response.Content);
        }

        [TestMethod]
        public void OnCallingDeleteAPI_ReturnSuccessStatus()
        {
            // Arrange
            RestRequest request = new RestRequest("/employees/delete_emp/4", Method.Delete);

            // Act
            RestResponse response = client.ExecuteAsync(request).Result;

            // Assert
            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
            Console.WriteLine(response.Content);
        }
    }
}
