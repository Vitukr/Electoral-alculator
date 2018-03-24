using ElectoralСalculator.Data;
using ElectoralСalculator.Data.DTO;
using ElectoralСalculator.Data.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace ElectoralСalculator
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public List<CandidateDTO> candidates = new List<CandidateDTO>();
        public List<VoterDTO> voters = new List<VoterDTO>();
        public MainWindow()
        {
            InitializeComponent();
            using (var db = new ElectoralСalculatorContext())
            {
                //db.Database.EnsureCreated();
                db.Database.Migrate();
                GetCandidates();
                DisplayVoters(db);
            }
        }

        private void DisplayVoters(ElectoralСalculatorContext db)
        {
            Voter voter = new Voter() {Forename="Jhon", Surname="Smith", BirthDate= new DateTime(1960, 7, 1), Pesel= "1234567" };
            //db.Voters.Add(voter);
            //db.SaveChanges();
            voters = db.Voters.Select(r => new VoterDTO {Id = r.Id, Forename = r.Forename, Surname = r.Surname, BirthDate = r.BirthDate, Pesel = r.Pesel }).ToList(); // Include(r => r.FullName)
            ListBoxCandidates.DataContext = voters;
            ListBoxCandidates.DisplayMemberPath = "FullName";
        }

        private async void GetCandidates()
        {
            string page = "http://webtask.future-processing.com:8069/candidates";

            // ... Use HttpClient.
            using (HttpClient client = new HttpClient())
            using (HttpResponseMessage response = await client.GetAsync(page))
            using (HttpContent content = response.Content)
            {
                string result = await content.ReadAsStringAsync();
                LabelSorename.Content = result;
            }
        }
    }
}
