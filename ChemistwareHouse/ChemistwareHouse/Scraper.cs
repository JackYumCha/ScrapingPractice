using System;
using HtmlAgilityPack;
using System.Collections.Generic;
using System.Net.Http;
using System.Linq;
using System.Threading.Tasks;
using System.Text.RegularExpressions;

namespace ChemistwareHouse
{
    public class Scraper
    {
        public async static Task<List<string>> ListA2BabyFormulas()
        {
            string url = @"https://www.chemistwarehouse.com.au/search/go?w=A2%20stage";

            using (HttpClient httpClient = new HttpClient())
            {
                var response = await httpClient.GetAsync(url);

                var html = await response.Content.ReadAsStringAsync();

                // create a new htmldocument with agility pack
                HtmlDocument document = new HtmlDocument();

                //load the html
                document.LoadHtml(html);

                // .where() function  
                var products = document.DocumentNode.DescendantsAndSelf()
                    .Where(node => node.Name.ToLower() == "div" && node.HasClass("Product"))
                    .Select(node => node.ParentNode.GetAttributeValue("href", ""))
                    .Where(link => link != null && link != "")
                    .ToList();
                return products;
            }

        }

        public async static Task<decimal?> GetPriceForProduct(string url) {
            using (HttpClient client = new HttpClient()) {

                var response = await client.GetAsync(url);

                var html = await response.Content.ReadAsStringAsync();


                HtmlDocument document = new HtmlDocument();

                document.LoadHtml(html);

                var first = document.DocumentNode.DescendantsAndSelf()
                    .Where(node => node.Name.ToLower() == "div" && node.HasClass("Price"))
                    .Select(node => node.InnerText)
                    .FirstOrDefault();

                if(first != null)
                {
                    Regex rgxPrice = new Regex(@"\$([\d\.]+)");
                    var match = rgxPrice.Match(first);
                    if (match.Success)
                    {
                        decimal price = 0;
                        if (decimal.TryParse(match.Groups[1].Value, out price))
                        {
                            return price;
                        }
                    }
                                        
                }
                return null;
             
            }
        }



    }
}
