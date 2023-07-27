using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using System.Net.Http;
using System.Net.Http.Headers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MovieApi.Models;

namespace MovieApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MovieItemsController : ControllerBase
    {
        private readonly MovieContext _context;
        private const string URL = "http://www.omdbapi.com/";
        // private string urlParameters = "?s=fast&apikey=804d6ce9";
        private string apikey = "804d6ce9";

        private readonly HttpClient client;



        public MovieItemsController(MovieContext context)
        {
            _context = context;

            client = new HttpClient();
            client.BaseAddress = new Uri(URL);
            // Add an Accept header for JSON format.
            client.DefaultRequestHeaders.Accept.Add(
            new MediaTypeWithQualityHeaderValue("application/json"));
        }

        // GET: api/MovieItems?search=Fast
        [HttpGet]
        public async Task<ActionResult> Get([FromQuery] string search)
        {

            // Console.WriteLine(search);
            string urlParameters = $"?apikey={apikey}&s={search}";
            // Create a string variable and get the data from the web server.
            // string result = "";
            HttpResponseMessage response = client.GetAsync(urlParameters).Result;

            // Check that the response is successful or not.
            if (response.IsSuccessStatusCode)
            {
                // Read the data from the response.

                string responseData = await response.Content.ReadAsStringAsync();

                // Return the response data to the client
                return Content(responseData, "application/json");
            }

             return Content($"Error: {response.StatusCode} - {response.ReasonPhrase}");
        }
        

        
        // GET: api/MovieItems/tt0088457 - using imdbID 
        [HttpGet("{id}")]
        public async Task<ActionResult> GetAMovieItem(string id)
        {
            string urlParameters = $"?apikey={apikey}&i={id}";
            // Create a string variable and get the data from the web server.
            // string result = "";
            HttpResponseMessage response = client.GetAsync(urlParameters).Result;

            // Check that the response is successful or not.
            if (response.IsSuccessStatusCode)
            {
                // Read the data from the response.
                string responseData = await response.Content.ReadAsStringAsync();
                // Return the response data to the client
                return Content(responseData, "application/json");
            }

            return Content($"Error: {response.StatusCode} - {response.ReasonPhrase}");
        }
       
    }
}
