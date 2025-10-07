using Microsoft.AspNetCore.Mvc;
using veeb.Models;

namespace veeb.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class ToodeController : ControllerBase
    {
        private static Toode _toode = new Toode(1, "Koola", 1.5, true);

        // GET: toode
        [HttpGet]
        public Toode GetToode()
        {
            return _toode;
        }

        // GET: toode/suurenda-hinda
        [HttpGet("suurenda-hinda")]
        public Toode SuurendaHinda()
        {
            _toode.Price = _toode.Price + 1;
            return _toode;
        }

        // GET: toode/muuda-aktiivsus
        [HttpGet("muuda-aktiivsus")]
        public Toode MuudaAktiivsus()
        {
            _toode.IsActive = !_toode.IsActive; // toggle true <-> false
            return _toode;
        }

        // GET: toode/muuda-nimi/UusNimi
        [HttpGet("muuda-nimi/{uusNimi}")]
        public Toode MuudaNimi(string uusNimi)
        {
            _toode.Name = uusNimi;
            return _toode;
        }

        // GET: toode/korruta-hind/3
        [HttpGet("korruta-hind/{kordaja}")]
        public Toode KorrutaHind(double kordaja)
        {
            _toode.Price = _toode.Price * kordaja;
            return _toode;
        }
    }
}
