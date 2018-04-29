using System;
using Xunit;
using chemistwarehourse;
using Shouldly;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Linq;
using System.Collections.Generic;


namespace ChemistwarehouseTest
{
    public class Units
    {
        [Fact(DisplayName ="Get List of A2 baby Formulars")]
        public async void GetListOfA2BabyFormulars()
        {
           var urls = await chemistwarehourse.Scraper.ListA2BabyFormulas();

            Debugger.Break();

            urls.Count.ShouldBe(4);
        }

        [Fact(DisplayName = "Get price")]
        public async void GetPrice()
        {
            var urls = await chemistwarehourse.Scraper.ListA2BabyFormulas();

            var tasks = urls.Select(url => chemistwarehourse.Scraper.GetPriceForProduct(url)).ToList();
            await Task.WhenAll(tasks);
            var prices = tasks.Select(task => task.Result).ToList();

            Debugger.Break();

            prices.Count.ShouldBe(4);
        }
    }
}
