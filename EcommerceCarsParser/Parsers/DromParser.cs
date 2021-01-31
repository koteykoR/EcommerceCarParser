using EcommerceCarsParser;
using EcommerceCarsParser.Parsers;
using HtmlAgilityPack;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace EcommerceCarsParser
{
    public sealed class DromParser : BaseParser
    {
        #region XpathProperty
        private static readonly string carPath =
            @"//a[contains(@data-ftid, 'bulls-list') and not (./descendant::*[ (contains(@data-ftid, 'broken'))])]";
        private static readonly string firmModelYearPath =
            @"//a[contains(@data-ftid, 'bulls-list') and not (./descendant::*[ (contains(@data-ftid, 'broken'))])] //span[contains(@data-ftid,'bull_title')]";
        private static readonly string pricePath =
            @"//a[contains(@data-ftid, 'bulls-list') and not (./descendant::*[ (contains(@data-ftid, 'broken'))])] //span[contains(@data-ftid,'bull_price')]";
        private static readonly string descriptionPath =
            @"//a[contains(@data-ftid, 'bulls-list') and not (./descendant::*[ (contains(@data-ftid, 'broken'))])]  //div[contains(@data-ftid,'bull_description')]";
        private static readonly string volumeAndPowerPath =
            @"//a[contains(@data-ftid, 'bulls-list') and not (./descendant::*[ (contains(@data-ftid, 'broken'))])]  //span[contains(text(),'л') and (contains(@data-ftid, 'item'))  ]";
        private static readonly string MielagePath =
            @"//a[contains(@data-ftid, 'bulls-list') and not (./descendant::*[ (contains(@data-ftid, 'broken'))])]  //span[contains(text(),'тыс.км') ]";
        private static readonly string carsCounPath = @"//button[contains(@data-ftid,'component_select_button') ]";
        #endregion XpathProperty
        HtmlNodeCollection titles;
        const int carsOnPage = 20;

        public DromParser(string firm = "audi", string model = "a8") :base()
        {
            url = $"https://auto.drom.ru/{firm}/{model}";
            htmlDoc= web.Load(url);
            paginationDocuments = LoadPaginationDocuments();

        }
        protected override IEnumerable<HtmlDocument> LoadPaginationDocuments()
        {
            var carsCount = GetCarsCount();
            var paginationCount = CalculatePagesCount(carsCount);
            var paginationDocuments = new ConcurrentQueue<HtmlDocument>();
            paginationDocuments.Enqueue(htmlDoc);
            Parallel.ForEach(Enumerable.Range(1, paginationCount), (number =>
            {
                var sb = new StringBuilder();

                sb.Append(url);
                sb.Append($"/page{number}");

                paginationDocuments.Enqueue(web.Load(sb.ToString()));
            }));

            return paginationDocuments;
        }
        int CalculatePagesCount(int carsCount)
        {
            if (carsCount % carsOnPage != 0)
                return (carsCount / carsOnPage) +1;
            else return carsCount / carsOnPage;
        }
        int GetCarsCount()
        {
            var carsString = htmlDoc.DocumentNode.SelectNodes(carsCounPath)[1].InnerHtml;
            //[1] чтобы вернуть нужную кнопку потому что этот Xpath возвращает несколько
            int pFrom = carsString.IndexOf("(") +1;
            int pTo = carsString.LastIndexOf(")") ;
            var str = carsString.Substring(pFrom, pTo-pFrom);
            return Convert.ToInt32(str);
        }

        public override IEnumerable<ParseCar> Parse()
        {
            foreach (var paginationDocument in paginationDocuments)
            {
                htmlDoc = paginationDocument;
                titles = GetTitles();
                List<int> mielages, enginePowers = Enumerable.Empty<int>().ToList();
                List<double> engineVolumes = Enumerable.Empty<double>().ToList();
                List<bool> gearBoxes = Enumerable.Empty<bool>().ToList();
                var size = ParseFirm().ToList().Count();
                var firms = ParseFirm().ToList();
                var models = ParseModel().ToList();
                var dates = ParseDate().ToList();
                var prices = ParsePrice().ToList();
                ParseDescription(out mielages, out enginePowers,
                out engineVolumes, out gearBoxes);

                for (int i = 0; i < size; i++)
                {
                    yield return new(firms[i], models[i], dates[i], prices[i], mielages[i], gearBoxes[i], enginePowers[i], engineVolumes[i]);
                }

            }
        }


    internal IEnumerable<string> ParseFirm()
    {
        return GetNodesByXpath(firmModelYearPath).
            Select(c => { string str = c.InnerHtml; return str.Split(" ").First(); });
    }

    internal IEnumerable<string> ParseModel()
    {
        return GetNodesByXpath(firmModelYearPath).
     Select(c =>
     {
         string str = c.InnerHtml;
         int pFrom = str.IndexOf(" ") + 1;
         int pTo = str.LastIndexOf(",");
         return str.Substring(pFrom, pTo - pFrom);
     });
    }

    internal IEnumerable<string> ParseDate()
    {
        return titles.Select(n => n.InnerText);
    }

    internal IEnumerable<int> ParsePrice()
    {
        return GetNodesByXpath(pricePath).Select(c =>
        {
            var str = c.InnerHtml;
            return Convert.ToInt32(str.Remove(str.IndexOf('<')).Replace(" ", ""));
        });
    }

    internal int ParseMielage(string description)
    {
        if (description.Contains("���. ��"))
        {
            int pFrom = description.IndexOf("���. ��") - 4;  //"тыс. км"
            int pTo = description.LastIndexOf("���. ��") - 1;
                var str = description.Substring(pFrom, pTo - pFrom).Replace(" ", "").Replace(",", "");
            return Convert.ToInt32(str); //если число двухзначное-отбрасываем пробел
        }
        else
        {
            return 0;
        }
    }

    internal int ParseEnginePower(string description)
    {
        try
        {
            int pFrom = description.IndexOf("(") + 1;
            int pTo = description.LastIndexOf(")") - 5;
            return Convert.ToInt32(description.Substring(pFrom, pTo - pFrom));
        }
        catch
        {
            return 0;
        }

    }

    internal double ParseEngineVolume(string description)
    {
        try
        {
            int pTo = description.IndexOf(" � "); // " л "
            return Convert.ToDouble(description.Substring(0, pTo).Replace("\"", ""),
            CultureInfo.InvariantCulture.NumberFormat);
        }
        catch
        {
            return 0;
        }
    }

    internal void ParseDescription(out List<int> Mielages, out List<int> EnginePowers,
        out List<double> EngineVolumes, out List<bool> GearBoxes)
    {
        Mielages = Enumerable.Empty<int>().ToList();
        EnginePowers = Enumerable.Empty<int>().ToList();
        EngineVolumes = Enumerable.Empty<double>().ToList();
        GearBoxes = Enumerable.Empty<bool>().ToList();
        var descrriptions = GetNodesByXpath(descriptionPath).Select(x => x.InnerText);
        foreach (var description in descrriptions)
        {
            Mielages.Add(ParseMielage(description));
            EnginePowers.Add(ParseEnginePower(description));
            EngineVolumes.Add(ParseEngineVolume(description));
            GearBoxes.Add(ParseGearbox(description));
        }
    }
    internal bool ParseGearbox(string description)        //КПП
    {
        if (description.Contains("��������"))
            return true;
        else return false;
    }
    internal  HtmlNodeCollection GetTitles()
    {
        return GetNodesByXpath(firmModelYearPath);
    }
     HtmlNodeCollection GetNodesByXpath(string XPath)
    {
        return htmlDoc.DocumentNode.SelectNodes(XPath);
    }
}
}
