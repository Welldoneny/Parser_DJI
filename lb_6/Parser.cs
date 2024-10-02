using AngleSharp;
using AngleSharp.Dom;
using FirstEF6App;
using System;

namespace lb_6
{
    internal class Parser
    { 

        public async void Parse(string url)
        {
            IConfiguration config = Configuration.Default.WithDefaultLoader();
            IBrowsingContext context = BrowsingContext.New(config);
            IDocument doc = await context.OpenAsync(url);

            var name = doc.GetElementsByClassName("product-card__title d-lg-none");
            string droneName = string.Empty;
            foreach (var item in name)
            {
                droneName = item.TextContent;
            }

            var price = doc.GetElementsByClassName("js-product-sum");
            string dronePrice = string.Empty;
            foreach (var item in price)
            {
                dronePrice = item.TextContent;
            }


            var characteristics = doc.GetElementsByClassName("features__table")[0]
                .GetElementsByTagName("td");
            string droneCharacteristics = string.Empty;
            for (int i = 0; i < 6; i++)
            {
                droneCharacteristics += characteristics[i].TextContent;
            }
            using (DroneContext db = new DroneContext())
            {
                Drone drone = new Drone { Name = droneName, Price = dronePrice, Characteristics = droneCharacteristics };
                db.Drones.Add(drone);
                db.SaveChanges();
            }
        }

        public async Task<List<string>> GetLinks(string url)
        {
            IConfiguration config = Configuration.Default.WithDefaultLoader();
            IBrowsingContext context = BrowsingContext.New(config);
            IDocument doc = await context.OpenAsync(url);

            IEnumerable<IElement> aElements = doc.All.Where(block =>
            block.LocalName == "a"
            && block.ParentElement.LocalName == "div"
            && block.ParentElement.ClassList.Contains("product-teaser__title"));

            List<string> output = new List<string>();

            foreach (IElement a in aElements.ToList())
            {
                output.Add(a.GetAttribute("href"));
            }
            return output;
        }
    }
}
