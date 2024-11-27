using frogaDB8.Models;
using frogaDB8.Services;
using Microsoft.Maui.Controls;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;

namespace frogaDB8.ViewModels
{
    /// <summary>
    /// Epez kanpo dauden maileguak kudeatzeko ViewModel-a.
    /// </summary>
    public class EpezKanpoViewModel : BindableObject
    {
        // Zerbitzuak datuak kudeatzeko
        private readonly MaileguakService _maileguakService; // Maileguen zerbitzua
        private readonly LiburuakService _liburuakService;  // Liburuen zerbitzua
        private readonly BazkideakService _bazkideakService; // Bazkideen zerbitzua

        /// <summary>
        /// ObservableCollection epez kanpo dauden maileguak gordetzeko.
        /// </summary>
        public ObservableCollection<MaileguakInfo> EpezKanpoCollection { get; }

        /// <summary>
        /// ViewModel-aren eraikitzailea.
        /// </summary>
        /// <param name="maileguakService">Maileguen zerbitzua.</param>
        /// <param name="liburuakService">Liburuen zerbitzua.</param>
        /// <param name="bazkideakService">Bazkideen zerbitzua.</param>
        public EpezKanpoViewModel(
            MaileguakService maileguakService,
            LiburuakService liburuakService,
            BazkideakService bazkideakService)
        {
            _maileguakService = maileguakService; // Maileguen zerbitzua esleitu
            _liburuakService = liburuakService;   // Liburuen zerbitzua esleitu
            _bazkideakService = bazkideakService; // Bazkideen zerbitzua esleitu

            // ObservableCollection hasieratu
            EpezKanpoCollection = new ObservableCollection<MaileguakInfo>();

            // Epez kanpo dauden datuak kargatu
            KargatuEpezKanpokoDatuak();
        }

        /// <summary>
        /// Epez kanpo dauden maileguen datuak kargatzeko metodoa.
        /// </summary>
        private async void KargatuEpezKanpokoDatuak()
        {
            try
            {
                // Zerbitzutik datuak eskuratu
                var maileguak = await _maileguakService.EskuratuMaileguakAsync(); // Maileguen datuak eskuratu
                var liburuak = await _liburuakService.EskuratuLiburuakAsync();   // Liburuen datuak eskuratu
                var bazkideak = await _bazkideakService.EskuratuBazkideakAsync(); // Bazkideen datuak eskuratu

                // Epez kanpo dauden maileguak filtratu
                var epezKanpo = maileguak.Where(p =>
                {
                    // Itzuli gabe eta 15 egun baino gehiago pasa diren maileguak aurkitu
                    if (p.Itzulita == 0 && DateTime.TryParse(p.MaileguData, out DateTime maileguData))
                    {
                        return (DateTime.Now - maileguData).TotalDays > 15;
                    }
                    return false;
                });

                // Kolekzioa datuekin bete
                foreach (var mailegua in epezKanpo)
                {
                    var liburua = liburuak.FirstOrDefault(l => l.Id == mailegua.IdLiburua); // Liburua aurkitu
                    var bazkidea = bazkideak.FirstOrDefault(b => b.Id == mailegua.IdBazkidea);
                    var data = maileguak.FirstOrDefault(b => b.Id == mailegua.Id);

                    // Liburua eta bazkidea badaude, kolekzioan gehitu
                    if (liburua != null && bazkidea != null)
                    {
                        EpezKanpoCollection.Add(new MaileguakInfo
                        {
                            BazkideaIzena = bazkidea.Izena, // Bazkidearen izena
                            LiburuaIzena = liburua.Izenburua,
                            MaileguData = data.MaileguData
                        });
                    }
                }
            }
            catch (Exception ex)
            {
                // Erroreak kudeatu (aukerazko log-a)
                Console.WriteLine($"Errorea kargatzen: {ex.Message}");
            }
        }
    }

    /// <summary>
    /// Epez kanpo dauden maileguak erakusteko modeloa.
    /// </summary>
    public class MaileguakInfo
    {
        public string BazkideaIzena { get; set; } // Bazkidearen izena
        public string LiburuaIzena { get; set; } // Liburuaren izenburua
        public string MaileguData { get; set; }
    }
}
