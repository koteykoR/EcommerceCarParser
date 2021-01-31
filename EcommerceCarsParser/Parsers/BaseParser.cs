using System.Collections.Generic;
using System.Text;
using System.Collections.Concurrent;
using HtmlAgilityPack;

namespace EcommerceCarsParser.Parsers
{
    public abstract class BaseParser
    {
        protected HtmlWeb web = new();
        protected HtmlDocument htmlDoc;
        protected IEnumerable<HtmlDocument> paginationDocuments;

        protected readonly string company;
        protected readonly string model;
        protected string url;

        public BaseParser(string _company = "volkswagen", string _model = "polo")
        {
            company = _company;
            model = _model;
        }

        public abstract IEnumerable<ParseCar> Parse();

        protected abstract IEnumerable<HtmlDocument> LoadPaginationDocuments();
    }
}
