using AzureFunctionTangyWeb.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace AzureFunctionTangyWeb.Controllers;
public class GroceryController : Controller
{
    static readonly HttpClient client = new HttpClient();
    string GroceryAPIUrl = "http://localhost:7071/api/GroceryList";
    // GET: GroceryController
    public async Task<ActionResult> Index()
    {
        HttpResponseMessage response = await client.GetAsync(GroceryAPIUrl);
        string returnValue = response.Content.ReadAsStringAsync().Result;
        List<GroceryItem> groceryListToReturn = JsonConvert.DeserializeObject<List<GroceryItem>>(returnValue);
        return View(groceryListToReturn);
    }


    // GET: GroceryController/Create
    public ActionResult Create()
    {
        return View();
    }

    // POST: GroceryController/Create
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<ActionResult> Create(GroceryItem obj)
    {
        try
        {
            var jsonContent = JsonConvert.SerializeObject(obj);
            using (var content = new StringContent(jsonContent, System.Text.Encoding.UTF8, "application/json"))
            {
                HttpResponseMessage response = await client.PostAsync(GroceryAPIUrl, content);
                string returnValue = response.Content.ReadAsStringAsync().Result;
            }
            return RedirectToAction(nameof(Index));
        }
        catch
        {
            return View();
        }
    }

    // GET: GroceryController/Edit/5
    public async Task<ActionResult> Edit(string id)
    {
        HttpResponseMessage response = await client.GetAsync(GroceryAPIUrl + "/" + id);
        string returnValue = response.Content.ReadAsStringAsync().Result;
        GroceryItem groceryItem = JsonConvert.DeserializeObject<GroceryItem>(returnValue);
        return View(groceryItem);
    }

    // POST: GroceryController/Edit/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<ActionResult> Edit(GroceryItem obj)
    {
        try
        {
            var jsonContent = JsonConvert.SerializeObject(obj);
            using (var content = new StringContent(jsonContent, System.Text.Encoding.UTF8, "application/json"))
            {
                HttpResponseMessage response = await client.PutAsync(GroceryAPIUrl + "/" + obj.Id, content);
                string returnValue = response.Content.ReadAsStringAsync().Result;
            }
            return RedirectToAction(nameof(Index));
        }
        catch
        {
            return View();
        }
    }

    // GET: GroceryController/Delete/5
    public async Task<ActionResult> Delete(string id)
    {
        HttpResponseMessage response = await client.GetAsync(GroceryAPIUrl + "/" + id);
        string returnValue = response.Content.ReadAsStringAsync().Result;
        GroceryItem groceryItem = JsonConvert.DeserializeObject<GroceryItem>(returnValue);
        return View(groceryItem);
    }

    // POST: GroceryController/Delete/5
    [HttpPost]
    [ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<ActionResult> DeletePOST(string id)
    {
        try
        {
            HttpResponseMessage response = await client.DeleteAsync(GroceryAPIUrl + "/" + id);
            string returnValue = response.Content.ReadAsStringAsync().Result;
            return RedirectToAction(nameof(Index));
        }
        catch
        {
            return View();
        }
    }
}
