using Microsoft.AspNetCore.Mvc;
using veeb.Models;
using System.Linq;

namespace veebirakendus.Controllers;

[ApiController]
[Route("[controller]")]
public class TootedController : ControllerBase
{
    private static List<Toode> _tooted = new List<Toode>{
        new Toode(1,"Koola", 1.5, true),
        new Toode(2,"Fanta", 1.0, false),
        new Toode(3,"Sprite", 1.7, true),
        new Toode(4,"Vichy", 2.0, true),
        new Toode(5,"Vitamin well", 2.5, true)
        };
    

    // https://localhost:7052/tooted
    [HttpGet]
    public List<Toode> Get()
    {
        return _tooted;
    }
    
    [HttpGet("kustuta/{index}")]
    public List<Toode> Delete(int index)
    {
        _tooted.RemoveAt(index);
        return _tooted;
    }
    
    [HttpGet("kustuta2/{index}")]
    public string Delete2(int index)
    {
        _tooted.RemoveAt(index);
        return "Kustutatud!";
    }
    
    [HttpGet("lisa/{id}/{nimi}/{hind}/{aktiivne}")]
    public List<Toode> Add(int id, string nimi, double hind, bool aktiivne)
    {
        Toode toode = new Toode(id, nimi, hind, aktiivne);
        _tooted.Add(toode);
        return _tooted;
    }
    
    [HttpGet("lisa")] // GET /tooted/lisa?id=1&nimi=Koola&hind=1.5&aktiivne=true
    public List<Toode> Add2([FromQuery] int id, [FromQuery] string nimi, [FromQuery] double hind, [FromQuery] bool aktiivne)
    {
        Toode toode = new Toode(id, nimi, hind, aktiivne);
        _tooted.Add(toode);
        return _tooted;
    }
    
    [HttpGet("hind-dollaritesse/{kurss}")] // GET /tooted/hind-dollaritesse/1.5
    public List<Toode> Dollaritesse(double kurss)
    {
        for (int i = 0; i < _tooted.Count; i++)
        {
            _tooted[i].Price = _tooted[i].Price * kurss;
        }
        return _tooted;
    }

    // või foreachina:

    [HttpGet("hind-dollaritesse2/{kurss}")] // GET /tooted/hind-dollaritesse2/1.5
    public List<Toode> Dollaritesse2(double kurss)
    {
        foreach (var t in _tooted)
        {
            t.Price = t.Price * kurss;
        }

        return _tooted;
    }
    // kustuta korraga kõik tooted
    // GET: /tooted/kustuta-koik
    [HttpGet("kustuta-koik")]
    public List<Toode> KustutaKoik()
    {
        _tooted.Clear();
        return _tooted; // tühi list
    }

    // kõigi aktiivsus vääraks (false)
    // GET: /tooted/muuda-koik-vale
    [HttpGet("muuda-koik-vale")]
    public List<Toode> MuudaKoikVale()
    {
        foreach (var t in _tooted)
            t.IsActive = false;
        return _tooted;
    }

    // tagasta üks toode järjekorranumbri (indeksi) järgi
    // GET: /tooted/yks/0
    [HttpGet("yks/{index}")]
    public ActionResult<Toode> YksToode(int index)
    {
        if (index < 0 || index >= _tooted.Count)
            return NotFound($"Indeks {index} on väljas vahemikust 0..{_tooted.Count - 1}");
        return _tooted[index];
    }

    // tagasta kõige suurema hinnaga toode 
    // GET: /tooted/korgeim-hind
    [HttpGet("korgeim-hind")]
    public ActionResult<Toode> KorgeimHind()
    {
        if (_tooted.Count == 0)
            return NotFound("Toodete nimekiri on tühi.");
        var maxToode = _tooted.OrderByDescending(t => t.Price).First();
        return maxToode;
    }
}