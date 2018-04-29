using System;
using HtmlAgilityPack;
using System.Net.Http;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Text.RegularExpressions;

namespace chemistwarehourse
{
    public class Scraper
    {
        public async static Task<List<String>> ListA2BabyFormulas()//其中Task是泛型
        {
            String url = @"https://www.chemistwarehouse.com.au/search/go?w=a2%20stage%";
            //httpclient 释放空间
            using (HttpClient httpClient = new HttpClient())
            {
                var reponse = await httpClient.GetAsync(url);//需要在static前加async

                var html = await reponse.Content.ReadAsStringAsync();

                HtmlDocument document = new HtmlDocument();

                document.LoadHtml(html);

                var products = document.DocumentNode.DescendantsAndSelf()
                    .Where(node => node.Name.ToLower() == "div" && node.HasClass("Product")) //里面的名字不能跟局部变量冲突
                    .Select(node => node.ParentNode)
                    .Select(node => node.GetAttributeValue("href",""))
                    .Where(link => link != null && link !="")
                    .ToList();

                return products;

            }
        }

        public async static Task<decimal?> GetPriceForProduct(String url)
        {//?等价于Nullable()
            using (HttpClient client = new HttpClient())
            {
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
                        if(decimal.TryParse(match.Groups[1].Value, out price))//关键字out,回作为输出结果显示
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
