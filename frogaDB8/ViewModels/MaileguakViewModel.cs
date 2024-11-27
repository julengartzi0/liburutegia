using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;
using frogaDB8.Models;
using frogaDB8.Services;
using Microsoft.Maui.Controls;

namespace frogaDB8.ViewModels
{
    /// <summary>
    /// Maileguak kudeatzeko ViewModel-a.
    /// Liburuen, bazkideen eta maileguen informazioa kudeatzen du.
    /// </summary>
    public class MaileguakViewModel : BindableObject
    {
        // Zerbitzuak datuak eskuratzeko eta gordetzeko
        private readonly LiburuakService _liburuakService;
        private readonly BazkideakService _bazkideakService;
        private readonly MaileguakService _maileguakService;

        // Kolekzioak liburuak eta bazkideak gordetzeko
        public ObservableCollection<Liburuak> LiburuakCollection { get; }
        public ObservableCollection<Bazkideak> BazkideakCollection { get; }
        public ObservableCollection<Liburuak> MailegukoLiburuakCollection { get; }

        // Barne propietateak
        private Bazkideak _selectedBazkidea;
        private Liburuak _selectedLiburua;
        private Liburuak _selectedMailegukoLiburua;
        private int _mailegukoLiburuak;
        private int _kopiaEskuragarriak;
        private int _geratzenDirenEgunak;
        private bool _mailegatuDezake;

        // Propietate publikoak
        /// <summary>
        /// Aukeratutako bazkidea.
        /// </summary>
        public Bazkideak SelectedBazkidea
        {
            get => _selectedBazkidea;
            set
            {
                _selectedBazkidea = value;
                OnPropertyChanged(nameof(SelectedBazkidea));
                MailegukoLiburuakKargatu();
                EskuragarriDaudenKopiakAktualizatu();
                MailegurakoRestrikzioakEgiaztatu();
            }
        }
        /// <summary>
        /// Aukeratutako liburua.
        /// </summary>
        public Liburuak SelectedLiburua
        {
            get => _selectedLiburua;
            set
            {
                _selectedLiburua = value;
                OnPropertyChanged(nameof(SelectedLiburua));
                EskuragarriDaudenKopiakAktualizatu();
                MailegurakoRestrikzioakEgiaztatu();
            }
        }
        /// <summary>
        /// Aukeratutako maileguko liburua.
        /// </summary>
        public Liburuak SelectedMailegukoLiburua
        {
            get => _selectedMailegukoLiburua;
            set
            {
                _selectedMailegukoLiburua = value;
                OnPropertyChanged(nameof(SelectedMailegukoLiburua));
                GeratzenDirenEgunakKalkulatu();
            }
        }
        /// <summary>
        /// AMaileguko Liburuak.
        /// </summary>
        public int MailegukoLiburuak
        {
            get => _mailegukoLiburuak;
            set
            {
                _mailegukoLiburuak = value;
                OnPropertyChanged(nameof(MailegukoLiburuak));
                GeratzenDirenEgunakKalkulatu();
            }
        }
        /// <summary>
        /// Kopia eskuragarriak.
        /// </summary>
        public int KopiaEskuragarriak
        {
            get => _kopiaEskuragarriak;
            set
            {
                _kopiaEskuragarriak = value;
                OnPropertyChanged(nameof(KopiaEskuragarriak));
            }
        }
        /// <summary>
        /// Geratzen diren egunak.
        /// </summary>
        public int GeratzenDirenEgunak
        {
            get => _geratzenDirenEgunak;
            set
            {
                _geratzenDirenEgunak = value;
                OnPropertyChanged(nameof(GeratzenDirenEgunak));
            }
        }
        /// <summary>
        /// Mailegatu dezake.
        /// </summary>
        public bool MailegatuDezake
        {
            get => _mailegatuDezake;
            set
            {
                _mailegatuDezake = value;
                OnPropertyChanged(nameof(MailegatuDezake));
            }
        }

        // Komandoak: erabiltzailearen ekintzak kudeatzen dituzte
        public ICommand MaileguCommand { get; }
        public ICommand BueltatuCommand { get; }
        public ICommand InformazioaCommand { get; }

        /// <summary>
        /// ViewModel-aren eraikitzailea. Zerbitzuak esleitzen ditu eta hasierako datuak kargatzen ditu.
        /// </summary>
        public MaileguakViewModel(LiburuakService liburuakService, BazkideakService bazkideakService, MaileguakService maileguakService)
        {
            // Zerbitzuak esleitu
            _liburuakService = liburuakService;
            _bazkideakService = bazkideakService;
            _maileguakService = maileguakService;

            // Kolekzioak hasieratu
            LiburuakCollection = new ObservableCollection<Liburuak>();
            BazkideakCollection = new ObservableCollection<Bazkideak>();
            MailegukoLiburuakCollection = new ObservableCollection<Liburuak>();

            // Komandoak esleitu
            MaileguCommand = new Command(MaileguaEgin);
            BueltatuCommand = new Command(LiburuaBueltatu);
            InformazioaCommand = new Command(InformazioaBueltatu);

            // Datuak kargatu
            LiburuakKargatu();
            BazkideakKargatu();
        }

        /// <summary>
        /// Liburu guztiak zerbitzutik kargatzen ditu eta kolekzioan gordetzen ditu.
        /// </summary>
        private async void LiburuakKargatu()
        {
            LiburuakCollection.Clear();
            var Liburuak = await _liburuakService.EskuratuLiburuakAsync();
            foreach (var Liburua in Liburuak)
                LiburuakCollection.Add(Liburua);

            EskuragarriDaudenKopiakAktualizatu();
        }
        /// <summary>
        /// Bazkide guztiak zerbitzutik kargatzen ditu eta kolekzioan gordetzen ditu.
        /// </summary>
        private async void BazkideakKargatu()
        {
            var bazkideak = await _bazkideakService.EskuratuBazkideakAsync();
            foreach (var bazkidea in bazkideak)
                BazkideakCollection.Add(bazkidea);
        }
        /// <summary>
        /// Aukeratutako bazkidearen mailegatutako liburuak kargatzen ditu eta kolekzioan gordetzen ditu.
        /// </summary>
        private async void MailegukoLiburuakKargatu()
        {
            if (SelectedBazkidea == null) return;

            MailegukoLiburuakCollection.Clear();
            var MailegukoLiburua = await _liburuakService.EskuratuLibrosPrestadosPorBazkideaAsync(SelectedBazkidea.Id);
            foreach (var Liburua in MailegukoLiburua)
                MailegukoLiburuakCollection.Add(Liburua);

            MailegukoLiburuak = MailegukoLiburuakCollection.Count;
            MailegurakoRestrikzioakEgiaztatu();
        }

        /// <summary>
        /// Aukeratutako liburu bat bazkide bati maileguan emateko metodoa.
        /// </summary>
        /// <param name="parameter">Maileguan emango den liburua.</param>
        private async void MaileguaEgin(object parameter)
        {
            var AukeratutakoLiburua = parameter as Liburuak;
            if (AukeratutakoLiburua == null || SelectedBazkidea == null)
            {
                await Application.Current.MainPage.DisplayAlert("Errorea", "Mesedez, Aukeratu liburu bat eta bazkide bat.", "OK");
                return;
            }

            var MaileguBerria = new Maileguak
            {
                IdLiburua = AukeratutakoLiburua.Id,
                IdBazkidea = SelectedBazkidea.Id,
                MaileguData = DateTime.Now.ToString("yyyy-MM-dd"),
                Itzulita = 0
            };

            await _maileguakService.GehituMaileguakAsync(MaileguBerria);
            MailegukoLiburuakCollection.Add(AukeratutakoLiburua);
            EskuragarriDaudenKopiakAktualizatu();
            MailegurakoRestrikzioakEgiaztatu();

            await Application.Current.MainPage.DisplayAlert("Mailegua ondo egina", $"'{AukeratutakoLiburua.Izenburua}' liburua '{SelectedBazkidea.Izena}'-ri eman zaio", "OK");
        }
        /// <summary>
        /// Aukeratutako liburua bazkide batek bueltatzen du.
        /// </summary>
        /// <param name="parameter">Bueltatuko den liburua.</param>
        private async void LiburuaBueltatu(object parameter)
        {
            var AukeratutakoLiburua = parameter as Liburuak;
            if (AukeratutakoLiburua == null || SelectedBazkidea == null)
            {
                await Application.Current.MainPage.DisplayAlert("Errorea", "Mesedez, aukeratu liburu bat bueltatzeko", "OK");
                return;
            }

            var Maileguak = await _maileguakService.EskuratuMaileguakAsync();
            var Mailegua = Maileguak.FirstOrDefault(p => p.IdLiburua == AukeratutakoLiburua.Id && p.IdBazkidea == SelectedBazkidea.Id && p.Itzulita == 0);

            if (Mailegua != null)
            {
                Mailegua.Itzulita = 1;
                await _maileguakService.AktualizatuMaileguakAsync(Mailegua);
                MailegukoLiburuakCollection.Remove(AukeratutakoLiburua);
                EskuragarriDaudenKopiakAktualizatu();
                MailegurakoRestrikzioakEgiaztatu();
                MailegukoLiburuakKargatu();

                await Application.Current.MainPage.DisplayAlert("Liburuaren informazio", $"'{AukeratutakoLiburua.Izenburua}' liburua ondo bueltatu da,", "OK");
            }
            else
            {
                await Application.Current.MainPage.DisplayAlert("Errorea", "Ez da aurkitu mailegua bueltatzeko", "OK");
            }
        }

        private async void InformazioaBueltatu(object parameter) {

            var AukeratutakoLiburua = parameter as Liburuak;
            await Application.Current.MainPage.DisplayAlert("Liburuaren informazioa", $"Izenburua: '{AukeratutakoLiburua.Izenburua}', ISBN: '{AukeratutakoLiburua.ISBN}', Egilea: '{AukeratutakoLiburua.IdEgilea}?", "OK");

        }

        /// <summary>
        /// Aukeratutako liburuaren eskuragarri dauden kopiak eguneratzen ditu.
        /// </summary>
        private async void EskuragarriDaudenKopiakAktualizatu()
        {
            if (SelectedLiburua == null)
            {
                KopiaEskuragarriak = 0;
                MailegatuDezake = false;
                return;
            }

            var Maileguak = await _maileguakService.EskuratuMaileguakAsync();
            var LiburuMaileguak = Maileguak.Count(p => p.IdLiburua == SelectedLiburua.Id && p.Itzulita == 0);

            KopiaEskuragarriak = (SelectedLiburua.Kopiak ?? 0) - LiburuMaileguak;
            MailegurakoRestrikzioakEgiaztatu();
        }
        /// <summary>
        /// Mailegurako baldintzak betetzen diren egiaztatzen du.
        /// </summary>
        private async void MailegurakoRestrikzioakEgiaztatu()
        {
            if (SelectedBazkidea == null)
            {
                MailegatuDezake = false;
                return;
            }

            var Maileguak = await _maileguakService.EskuratuMaileguakAsync();
            var BazkidearenMaileguak = Maileguak.Where(p => p.IdBazkidea == SelectedBazkidea.Id && p.Itzulita == 0).ToList();

            bool tienePrestamoVencido = BazkidearenMaileguak.Any(p =>
            {
                if (DateTime.TryParse(p.MaileguData, out DateTime MaileguData))
                {
                    return (DateTime.Now - MaileguData).TotalDays > 15;
                }
                return false;
            });

            MailegatuDezake = KopiaEskuragarriak > 0 && MailegukoLiburuak < 3 && !tienePrestamoVencido;
        }
        /// <summary>
        /// Aukeratutako maileguaren itzultzeko geratzen diren egunak kalkulatzen ditu.
        /// </summary>
        private async void GeratzenDirenEgunakKalkulatu()
        {
            if (SelectedMailegukoLiburua == null || SelectedBazkidea == null)
            {
                GeratzenDirenEgunak = 0;
                return;
            }

            var Maileguak = await _maileguakService.EskuratuMaileguakAsync();
            var Mailegua = Maileguak.FirstOrDefault(p => p.IdLiburua == SelectedMailegukoLiburua.Id && p.IdBazkidea == SelectedBazkidea.Id && p.Itzulita == 0);

            if (Mailegua != null && DateTime.TryParse(Mailegua.MaileguData, out DateTime fechaPrestamo))
            {
                GeratzenDirenEgunak = Math.Max(0, 15 - (int)(DateTime.Now - fechaPrestamo).TotalDays);
            }
            else
            {
                GeratzenDirenEgunak = 0;
            }
        }
    }
}
