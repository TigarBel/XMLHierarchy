using System.IO;
using System.Net.Http;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace XMLHierarchy.XMLParserLogic
{
  /// <summary>
  /// XML-Парсер
  /// </summary>
  public class XMLParser
  {
    /// <summary>
    /// Асинхронная функция по стению XML файла или URL на XML страницу
    /// </summary>
    /// <param name="path">Путь</param>
    /// <returns>XML-Документ</returns>
    public async Task<XDocument> GetDocumentAsync(string path)
    {
      XDocument document;
      if (File.Exists(path)) { // Локальный файл
        document = XDocument.Load(path);
      } else { // Загрузка через URL
        document = await GetLoadUrlAsync(path);
      }

      return document;
    }
    /// <summary>
    /// Асинхронный метод выгрузки XML страницу через URL
    /// </summary>
    /// <param name="url">Путь URL</param>
    /// <returns>XML-Документ</returns>
    private async Task<XDocument> GetLoadUrlAsync(string url)
    {
      HttpResponseMessage response = await new HttpClient().GetAsync(url);
      response.EnsureSuccessStatusCode();
      string responsContent = await response.Content.ReadAsStringAsync();
      TextReader tr = new StringReader(responsContent);
      return XDocument.Load(tr);
    }
  }
}
