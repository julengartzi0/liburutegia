using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;
using frogaDB8.Models;
using frogaDB8.Services;
using Microsoft.Maui.Controls;

namespace frogaDB8.ViewModels
{
    /// <summary>
    /// Liburuak kudeatzeko ViewModel-a, liburuen datuak erakusteko eta manipulatzeko.
    /// </summary>
    public class LiburuakViewModel : BindableObject
    {
        // Zerbitzuak liburuen, egileen, argitaletxeen eta hizkuntzen datuak kudeatzeko
        private readonly LiburuakService _liburuakService;
        private readonly EgileakService _egileakService;
        private readonly ArgitaletxeakService _argitaletxeakService;
        private readonly HizkuntzakService _hizkuntzakService;

        // Barne propietateak
        private int _oraingoIndex;
        private bool _liburuBerriaEditatzenDago = false;
        private bool _aukeratutakoLiburuaBerriaDa = false;

        // Kolekzioak (ObservableCollection) liburuak eta erlazionatutako datuak gordetzeko
        public ObservableCollection<Liburuak> LiburuakCollection { get; }
        public ObservableCollection<Egileak> EgileakCollection { get; }
        public ObservableCollection<Argitaletxeak> ArgitaletxeakCollection { get; }
        public ObservableCollection<Hizkuntzak> HizkuntzakCollection { get; }

        // Publikoak diren propietateak liburuaren datuak eta erlazionatutako objektuak gordetzeko
        public Liburuak CurrentLiburua { get; private set; }

        public Egileak SelectedEgilea
        {
            get => EgileakCollection.FirstOrDefault(a => a.Id == CurrentLiburua.IdEgilea);
            set
            {
                if (value != null)
                {
                    CurrentLiburua.IdEgilea = value.Id;
                    OnPropertyChanged(nameof(SelectedEgilea));
                }
            }
        }

        public Argitaletxeak SelectedArgitaletxea
        {
            get => ArgitaletxeakCollection.FirstOrDefault(e => e.Id == CurrentLiburua.IdArgitaletxea);
            set
            {
                if (value != null)
                {
                    CurrentLiburua.IdArgitaletxea = value.Id;
                    OnPropertyChanged(nameof(SelectedArgitaletxea));
                }
            }
        }

        public Hizkuntzak SelectedHizkuntza
        {
            get => HizkuntzakCollection.FirstOrDefault(i => i.Id == CurrentLiburua.IdIHizkuntza);
            set
            {
                if (value != null)
                {
                    CurrentLiburua.IdIHizkuntza = value.Id;
                    OnPropertyChanged(nameof(SelectedHizkuntza));
                }
            }
        }

        // Komandoak: erabiltzailearen interakzioak kudeatzeko
        public ICommand HurrengoaCommand { get; }
        public ICommand AurrekoaCommand { get; }
        public ICommand BerriaCommand { get; }
        public ICommand EzabatuCommand { get; }
        public ICommand GordeCommand { get; }

        /// <summary>
        /// ViewModel-aren eraikitzailea, zerbitzuak eta datu hasierakoak kargatzeko.
        /// </summary>
        public LiburuakViewModel(
            LiburuakService liburuakService,
            EgileakService egileakService,
            ArgitaletxeakService argitaletxeakService,
            HizkuntzakService hizkuntzakService)
        {
            // Zerbitzuak esleitu
            _liburuakService = liburuakService;
            _egileakService = egileakService;
            _argitaletxeakService = argitaletxeakService;
            _hizkuntzakService = hizkuntzakService;

            // Kolekzioak hasieratu
            LiburuakCollection = new ObservableCollection<Liburuak>();
            EgileakCollection = new ObservableCollection<Egileak>();
            ArgitaletxeakCollection = new ObservableCollection<Argitaletxeak>();
            HizkuntzakCollection = new ObservableCollection<Hizkuntzak>();

            // Datuak kargatu
            EgileakKargatu();
            ArgitaletxeakKargatu();
            HizkuntzakKargatu();
            LiburuakKargatu();

            // Oraingo liburua hasieratu
            _oraingoIndex = 0;
            AktualizatuOraingoLiburua();

            // Komandoak esleitu
            HurrengoaCommand = new Command(HurrengoLiburua);
            AurrekoaCommand = new Command(AurrekoLiburua);
            BerriaCommand = new Command(LiburuBerriaSartu);
            EzabatuCommand = new Command(EzabatuOraingoLiburua);
            GordeCommand = new Command(GordeOraingoLiburua);
        }


        /// <summary>
        /// Liburuak kargatzeko metodoa zerbitzuaren bidez.
        /// </summary>
        private async void LiburuakKargatu()
        {
            LiburuakCollection.Clear();
            var liburuak = await _liburuakService.EskuratuLiburuakAsync();
            foreach (var liburua in liburuak)
                LiburuakCollection.Add(liburua);

            if (LiburuakCollection.Count > 0)
            {
                _oraingoIndex = 0;
                AktualizatuOraingoLiburua();
            }
        }

        /// <summary>
        /// Egileak kargatzeko metodoa zerbitzuaren bidez.
        /// </summary>
        private async void EgileakKargatu()
        {
            var egileak = await _egileakService.EskuratuEgileakAsync();
            foreach (var egilea in egileak)
                EgileakCollection.Add(egilea);
        }
        /// <summary>
        /// Argitaletxeak kargatzeko metodoa zerbitzuaren bidez.
        /// </summary>
        private async void ArgitaletxeakKargatu()
        {
            var argitaletxeak = await _argitaletxeakService.EskuratuArgitaletxeakAsync();
            foreach (var argitaletxea in argitaletxeak)
                ArgitaletxeakCollection.Add(argitaletxea);
        }
        /// <summary>
        /// Hizkuntzak kargatzeko metodoa zerbitzuaren bidez.
        /// </summary>
        private async void HizkuntzakKargatu()
        {
            var hizkuntzak = await _hizkuntzakService.EskuratuHizkuntzakAsync();
            foreach (var hizkuntza in hizkuntzak)
                HizkuntzakCollection.Add(hizkuntza);
        }

        /// <summary>
        /// Oraingo liburua eguneratzeko metodoa.
        /// </summary>
        private void AktualizatuOraingoLiburua()
        {
            if (LiburuakCollection.Count > 0)
                CurrentLiburua = LiburuakCollection[_oraingoIndex];
            else
                CurrentLiburua = new Liburuak();

            OnPropertyChanged(nameof(CurrentLiburua));
            OnPropertyChanged(nameof(SelectedEgilea));
            OnPropertyChanged(nameof(SelectedArgitaletxea));
            OnPropertyChanged(nameof(SelectedHizkuntza));
        }

        /// <summary>
        /// Hurrengo liburura pasatzeko metodoa.
        /// </summary>
        private void HurrengoLiburua()
        {
            if (_oraingoIndex < LiburuakCollection.Count - 1)
            {
                _oraingoIndex++;
                AktualizatuOraingoLiburua();
            }
        }

        /// <summary>
        /// Aurreko liburura pasatzeko metodoa.
        /// </summary>
        private void AurrekoLiburua()
        {
            if (_oraingoIndex > 0)
            {
                _oraingoIndex--;
                AktualizatuOraingoLiburua();
            }
        }

        /// <summary>
        /// Liburu berria sartzeko metodoa.
        /// </summary>
        private void LiburuBerriaSartu()
        {
            if (_liburuBerriaEditatzenDago)
            {
                MessagingCenter.Send(this, "Liburua ez da gorde");
                return;
            }

            int idLibreTxikiena = 1;
            var idOkupatuak = LiburuakCollection.Select(liburua => liburua.Id).OrderBy(id => id).ToList();
            foreach (var id in idOkupatuak)
            {
                if (id == idLibreTxikiena)
                    idLibreTxikiena++;
                else
                    break;
            }

            var liburuBerria = new Liburuak { Id = idLibreTxikiena };
            LiburuakCollection.Add(liburuBerria);
            _oraingoIndex = LiburuakCollection.Count - 1;
            AktualizatuOraingoLiburua();

            _liburuBerriaEditatzenDago = true;
            _aukeratutakoLiburuaBerriaDa = true;
        }

        /// <summary>
        /// Oraingo liburua ezabatzeko metodoa.
        /// </summary>
        private async void EzabatuOraingoLiburua()
        {
            if (CurrentLiburua == null)
                return;

            bool baieztatuDa = await Application.Current.MainPage.DisplayAlert(
                "Ezabatzea konfirmatu",
                $"Zihur zaude liburua ezabatu nahi duzula? ID: {CurrentLiburua.Id} eta Izenburua: {CurrentLiburua.Izenburua}",
                "Ezabatu", "Atzera egin");

            if (baieztatuDa)
            {
                await _liburuakService.EzabatuLiburuakAsync(CurrentLiburua);
                LiburuakCollection.Remove(CurrentLiburua);

                if (_oraingoIndex >= LiburuakCollection.Count)
                    _oraingoIndex = LiburuakCollection.Count - 1;

                AktualizatuOraingoLiburua();
                await Application.Current.MainPage.DisplayAlert("Ezabatzea burutua", "Liburua ondo ezabatu da.", "OK");
            }
        }

        /// <summary>
        /// Oraingo liburua gordetzeko metodoa.
        /// </summary>
        private async void GordeOraingoLiburua()
        {
            if (CurrentLiburua.ISBN == null || CurrentLiburua.Kopiak == null)
            {
                await Application.Current.MainPage.DisplayAlert("Errorea", "ISBN eta Kopia balore numerikoa izan behar dute eta ezin dute hutsik egon.", "OK");
                return;
            }

            if (_aukeratutakoLiburuaBerriaDa)
            {
                await _liburuakService.GehituLiburuakAsync(CurrentLiburua);
                _aukeratutakoLiburuaBerriaDa = false;
            }
            else
            {
                await _liburuakService.AktualizatuLiburuakAsync(CurrentLiburua);
            }

            _liburuBerriaEditatzenDago = false;
            MessagingCenter.Send(this, "DatuakGordeta");
        }
    }
}
