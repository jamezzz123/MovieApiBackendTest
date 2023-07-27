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
        // GetMovieItems()
        // {
        //     // if (_context.MovieItems == null)
        //     // {
        //     //     return NotFound();
        //     // }

        //     // return await _context.MovieItems.ToListAsync();

        //     HttpResponseMessage response = client.GetAsync(urlParameters).Result;

        //      if (response.IsSuccessStatusCode){
        //         var dataObjects = response.Content.ReadAsAsync<IEnumerable<MovieItem>>().Result;  //Make sure to add a reference to System.Net.Http.Formatting.dll
        //             return (Task<ActionResult<IEnumerable<MovieItem>>>)dataObjects;

        //      }


        // }



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

        // PUT: api/MovieItems/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutMovieItem(long id, MovieItem movieItem)
        {
            if (id != movieItem.Id)
            {
                return BadRequest();
            }

            _context.Entry(movieItem).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!MovieItemExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/MovieItems
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<MovieItem>> PostMovieItem(MovieItem movieItem)
        {
            if (_context.MovieItems == null)
            {
                return Problem("Entity set 'MovieContext.MovieItems'  is null.");
            }
            _context.MovieItems.Add(movieItem);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetMovieItem", new { id = movieItem.Id }, movieItem);
        }

        // DELETE: api/MovieItems/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteMovieItem(long id)
        {
            if (_context.MovieItems == null)
            {
                return NotFound();
            }
            var movieItem = await _context.MovieItems.FindAsync(id);
            if (movieItem == null)
            {
                return NotFound();
            }

            _context.MovieItems.Remove(movieItem);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool MovieItemExists(long id)
        {
            return (_context.MovieItems?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
