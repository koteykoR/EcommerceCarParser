using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EcommerceCarsParser.Extension;
using HtmlAgilityPack;

namespace EcommerceCarsParser.Parsers
{
    public sealed class AutoParser : BaseParser
    {
        #region xPathProperty
        private readonly string lastPaginationUrlDocuments = @"//a[@class='Button Button_color_whiteHoverBlue Button_size_s Button_type_link Button_width_default ListingPagination-module__page'][last()]";
        private readonly string carDescXPath = @".//div[contains(@class, 'ListingItem-module__description')]";
        private readonly string yearXPath = @".//div[contains(@class, 'ListingItem-module__year')]";
        private readonly string priceXPath = @".//div[contains(@class, 'ListingItemPrice-module__content')]//span";
        private readonly string mileageXPath = @".//div[@class='ListingItem-module__kmAge']";
        private readonly string enginePowerVolumeXPath = @".//div[@class='ListingItemTechSummaryDesktop__column'][1]/div[1]";
        private readonly string transmissionXPath = @".//div[@class='ListingItemTechSummaryDesktop__column'][1]//div[2]";
        #endregion xPathProperty

        #region specialWord
        private readonly string failResultGetNode = "";
        private readonly string newCarMileage = "новый";
        private readonly string transmissionMechanic = "механика";
        private readonly int failResultTryParseNumber = -1;
        #endregion specialWord

        public AutoParser(string _company = "lifan", string _model = "breez") : base() 
        {
            url = $"https://auto.ru/cars/{company}/{model}/all/";

            web.OverrideEncoding = Encoding.UTF8;

            htmlDoc = web.Load(url);

            paginationDocuments = LoadPaginationDocuments();
        }

        protected override IEnumerable<HtmlDocument> LoadPaginationDocuments()
        {
            var maybeCountPaginationUrls = htmlDoc.DocumentNode.SelectSingleNode(lastPaginationUrlDocuments)
                                                               .With(n => n.InnerText, failResultGetNode);

            if (!int.TryParse(maybeCountPaginationUrls, out var countPaginationUrls)) return null;
        
            var paginationDocuments = new ConcurrentQueue<HtmlDocument>();
            paginationDocuments.Enqueue(htmlDoc);

            Parallel.ForEach(Enumerable.Range(2, countPaginationUrls), (number => 
            {
                var sb = new StringBuilder();

                sb.Append(url);
                sb.Append($"?page={number}");

                paginationDocuments.Enqueue(web.Load(sb.ToString()));
            }));

            return paginationDocuments;
        }

        public override IEnumerable<ParseCar> Parse()
        {
            if (paginationDocuments is null)
            {
                yield break;
            }

            foreach (var paginationDocument in paginationDocuments)
            {
                var nodesCar = paginationDocument.DocumentNode
                                                 .SelectNodes(carDescXPath);

                if (nodesCar is null)
                {
                    yield break;
                }

                foreach (var nodeCar in nodesCar)
                {
                    var maybeCar = GetCar(nodeCar);
                    if (maybeCar is not null)
                    {
                        yield return maybeCar;
                    }
                }
            }
        }

        private ParseCar GetCar(HtmlNode nodeCar)
        {
            var year = ParseYear(nodeCar);
            if (year == failResultGetNode) return null;

            var price = ParsePrice(nodeCar);
            if (price == failResultTryParseNumber) return null;

            var mileage = ParseMileage(nodeCar);
            if (mileage == failResultTryParseNumber) return null;

            var transmission = ParseTransmission(nodeCar);

            var (power, volume) = ParseEnginePowerVolume(nodeCar);
            if (power == failResultTryParseNumber) return null;
            if (volume == failResultTryParseNumber) return null;

            return new(company, model, year, price, mileage, transmission, power, volume);
        }

        private string ParseYear(HtmlNode nodeCar)
        {
            return nodeCar.SelectSingleNode(yearXPath)
                          .With(n => n.InnerText, failResultGetNode);
        }

        private int ParsePrice(HtmlNode nodeCar)
        {
            var maybePrice = nodeCar.SelectSingleNode(priceXPath)
                                    .With(n => n.InnerText, failResultGetNode)
                                    .Where(c => c is >= '0' and <= '9')
                                    .GetString();

            if (!int.TryParse(maybePrice, out var price)) return failResultTryParseNumber; 

            return price;
        }

        private int ParseMileage(HtmlNode nodeCar)
        {
            var maybeMileage = nodeCar.SelectSingleNode(mileageXPath)
                                      .With(n => n.InnerText, failResultGetNode);

            if (maybeMileage == newCarMileage) return 0;

            maybeMileage = maybeMileage.Where(c => c is >= '0' and <= '9')
                                       .GetString();

            if (!int.TryParse(maybeMileage, out var mileage)) return failResultTryParseNumber;

            return mileage;
        }

        private bool ParseTransmission(HtmlNode nodeCar)
        {
            var maybeTransmission = nodeCar.SelectSingleNode(transmissionXPath)
                                           .With(n => n.InnerText, failResultGetNode);

            if (maybeTransmission == transmissionMechanic) return false;

            return true;
        }

        private (int, double) ParseEnginePowerVolume(HtmlNode nodeCar)
        {
            var maybeEnginePowerVolume = nodeCar.SelectSingleNode(enginePowerVolumeXPath)
                                                .With(n => n.InnerText, failResultGetNode);

            var maybeVolume = maybeEnginePowerVolume[0..3];
            var maybePower = maybeEnginePowerVolume[8..10];

            if (!int.TryParse(maybePower, out var enginePower))
            {
                enginePower = failResultTryParseNumber;
            }

            if (!double.TryParse(maybeVolume, NumberStyles.Any, CultureInfo.InvariantCulture, out var engineVolume))
            {
                engineVolume = failResultTryParseNumber;
            }

            return (enginePower, engineVolume);
        }
    }
}
