using System;
using Xunit;
using ChemistwareHouse;
using Shouldly;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;



namespace Chemist.Tests
{
    public class Units
    {
        [Fact (DisplayName  = "get list of a2 baby formulars")]
        public async void GetListofBabyFormulars()
        {
           var urls = await ChemistwareHouse.Scraper.ListA2BabyFormulas();
            Debugger.Break();
            urls.Count.ShouldBe(4);
                 
        }

        [Fact(DisplayName = "get proces of a2 baby formulars")]
        public async void GetPriceofBabyFormulars()
        {
            var urls = await ChemistwareHouse.Scraper.ListA2BabyFormulas();

            var tasks = urls.Select( url=>ChemistwareHouse.Scraper.GetPriceForProduct(url)).ToList();

            await Task.WhenAll(tasks);

            var prices = tasks.Select(task => task.Result).ToList();
            Debugger.Break();
            prices.Count.ShouldBe(4);




        }


    }
}
