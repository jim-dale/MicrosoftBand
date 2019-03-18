
namespace DataWrangler
{
    using System.Linq;
    using System.Xml.Linq;
    using HealthCloudClient;

    public static partial class MapPointExtensions
    {
        public static void SaveToXmlFile(this MapPoint[] items, string path)
        {
            var query = from item in items
                        where item.Location != null
                        orderby item.Ordinal
                        select item;

            var gpx = XNamespace.Get("http://www.topografix.com/GPX/1/0");
            var xsi = XNamespace.Get("http://www.w3.org/2001/XMLSchema-instance");
            var location = xsi.GetName("schemaLocation");

            var trkseg = new XElement(gpx + "trkseg");

            foreach (var item in query)
            {
                var lat = (double)item.Location.Latitude / 10_000_000;
                var lon = (double)item.Location.Longitude / 10_000_000;
                var elev = (double)item.Location.ElevationFromMeanSeaLevel / 1000;  // convert mm to meters
                var node = new XElement(gpx + "trkpt", new XAttribute("lat", lat), new XAttribute("lon", lon), new XElement("ele", elev));

                trkseg.Add(node);
            }

            var xml =
                    //new XElement("gpx", new XAttribute("xmlns", gpx), new XAttribute(XNamespace.Xmlns + "xsi", xsi),
                    new XElement(gpx + "gpx",
                        new XAttribute("version", "1.0"),
                        new XAttribute("creator", "Data Wrangler"),
                        new XAttribute(XNamespace.Xmlns + "xsi", xsi.NamespaceName),
                        new XAttribute(location, "http://www.topografix.com/GPX/1/0 http://www.topografix.com/GPX/1/0/gpx.xsd"),
                        new XElement(gpx + "trk", new XElement(gpx + "name", "My Track"), trkseg)
                );

            xml.Save(path);
        }
    }
}
