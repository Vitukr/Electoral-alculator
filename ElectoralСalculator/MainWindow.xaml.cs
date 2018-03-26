using CsvHelper;
using ElectoralСalculator.Data;
using ElectoralСalculator.Data.DTO;
using ElectoralСalculator.Data.Models;
using iTextSharp.text;
using iTextSharp.text.pdf;
using Microsoft.EntityFrameworkCore;
using Microsoft.Win32;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Windows;

namespace ElectoralСalculator
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private List<CandidateDTO> candidates = new List<CandidateDTO>();
        private List<string> pesels = new List<string>();
        public Voter Voter { get; set; }

        public MainWindow()
        {
            InitializeComponent();
            using (var db = new ElectoralСalculatorContext())
            {
                db.Database.Migrate();                
            }

            GetCandidates();
            LabelSignInInfo.Content = "Please fill all fields";
        }

        private async void GetCandidates()
        {
            string page = "http://webtask.future-processing.com:8069/candidates";

            // HttpClient.
            using (HttpClient client = new HttpClient())
            using (HttpResponseMessage response = await client.GetAsync(page))
            using (HttpContent content = response.Content)
            {
                string result = await content.ReadAsStringAsync();

                dynamic jsonCandidates = JsonConvert.DeserializeObject(result);
                JArray candidatesArray = jsonCandidates["candidates"]["candidate"];
                candidates = candidatesArray.Select(r => new CandidateDTO { Name = r["name"].ToString(), Party = r["party"].ToString() }).ToList();

                ListBoxCandidates.DataContext = candidates;
                ListBoxCandidates.DisplayMemberPath = "FullName";
            }
        }

        private async Task GetBlackList()
        {
            string page = "http://webtask.future-processing.com:8069/blocked";

            // HttpClient.
            using (HttpClient client = new HttpClient())
            using (HttpResponseMessage response = await client.GetAsync(page))
            using (HttpContent content = response.Content)
            {
                string result = await content.ReadAsStringAsync();

                dynamic jsonPesels = JsonConvert.DeserializeObject(result);
                JArray peselsArray = jsonPesels["disallowed"]["person"];
                pesels = peselsArray.Select(r => r["pesel"].ToString()).ToList();
            }
        }

        private bool CheckPesel(string pesel, DateTime birthDate)
        {
            if(pesel.Length != 11)
            {
                return false;
            }
            var date = birthDate.Year.ToString().Substring(2,2)  + birthDate.Month.ToString("D2") + birthDate.Day.ToString("D2");
            var compared = pesel.Substring(0, 6) == date;
            return compared;
        }

        private void ButtonSignUp_Click(object sender, RoutedEventArgs e)
        {
            LabelSignUpInfo.Content = "Please fill all fields";
            GridSignUp.Visibility = Visibility.Visible;
            GridSignIn.Visibility = Visibility.Collapsed;
            //ButtonVote.Visibility = Visibility.Collapsed;
            GridWelcome.Visibility = Visibility.Collapsed;
        }

        private void ButtonSignIn_Click(object sender, RoutedEventArgs e)
        {
            var forename = TextBoxSignInForename.Text.Trim();
            var surname = TextBoxSignInSurname.Text.Trim();
            var pesel = TextBoxSignInPesel.Text.Trim();
            var password = TextBoxSignInPassword.Password.Trim();
            using (var db = new ElectoralСalculatorContext())
            {
                var result = db.Voters.Where(r => r.Forename == forename && r.Surname == surname && r.Pesel == pesel.GetHashCode().ToString() && r.Password == password.GetHashCode().ToString()).FirstOrDefault();
                if(result is Voter voter)
                {
                    Voter = voter;
                    GridSignIn.Visibility = Visibility.Collapsed;
                    GridSignUp.Visibility = Visibility.Collapsed;
                    //ButtonVote.Visibility = Visibility.Visible;
                    GridWelcome.Visibility = Visibility.Visible;
                    LabelWelcome.Content = Voter.FullName;

                    TextBoxSignInForename.Text = string.Empty;
                    TextBoxSignInSurname.Text = string.Empty;
                    TextBoxSignInPesel.Text = string.Empty;
                    TextBoxSignInPassword.Password = string.Empty;
                }
                else
                {
                    LabelSignInInfo.Content = "Incorrect information";
                }
            }
        }

        private void ButtonWelcomeSignOut_Click(object sender, RoutedEventArgs e)
        {
            LabelSignInInfo.Content = "Please fill all fields";
            Voter = null;
            GridSignIn.Visibility = Visibility.Visible;
            GridSignUp.Visibility = Visibility.Collapsed;
            GridWelcome.Visibility = Visibility.Collapsed;

            GridCandidates.Visibility = Visibility.Visible;
            GridResultsNumber.Visibility = Visibility.Collapsed;
            GridResultsChart.Visibility = Visibility.Collapsed;
        }

        private void ButtonSignUpBack_Click(object sender, RoutedEventArgs e)
        {
            GridSignIn.Visibility = Visibility.Visible;
            GridSignUp.Visibility = Visibility.Collapsed;
            GridWelcome.Visibility = Visibility.Collapsed;
        }

        private void ButtonSignUpRegister_Click(object sender, RoutedEventArgs e)
        {
            var forename = TextBoxSignUpForename.Text.Trim();
            var surname = TextBoxSignUpSurname.Text.Trim();
            var birthdate = DatePickerSignUpBirthDate.DisplayDate;
            var pesel = TextBoxSignUpPesel.Text.Trim();
            var password = TextBoxSignUpPassword.Text.Trim();

            // check voter information
            if (string.IsNullOrEmpty(forename) || string.IsNullOrEmpty(surname) || string.IsNullOrEmpty(password) || string.IsNullOrEmpty(pesel) || !CheckPesel(pesel, birthdate))
            {
                LabelSignUpInfo.Content = "Incorrect information";
                return;
            }

            var newvoter = new Voter() { Forename = forename, Surname = surname, BirthDate = birthdate, Pesel = pesel.GetHashCode().ToString(), Password = password.GetHashCode().ToString() };

            using (var db = new ElectoralСalculatorContext())
            {
                var result = db.Voters.Where(r => r.Forename == forename && r.Surname == surname && r.Pesel == pesel.GetHashCode().ToString()).FirstOrDefault();
                if (result is Voter voter)
                {
                    GridSignUp.Visibility = Visibility.Collapsed;
                    GridSignIn.Visibility = Visibility.Visible;
                    //ButtonVote.Visibility = Visibility.Collapsed;
                    GridWelcome.Visibility = Visibility.Collapsed;
                }
                else
                {
                    db.Voters.Add(newvoter);
                    db.SaveChanges();
                    GridSignUp.Visibility = Visibility.Collapsed;
                    GridSignIn.Visibility = Visibility.Visible;
                    //ButtonVote.Visibility = Visibility.Collapsed;
                    GridWelcome.Visibility = Visibility.Collapsed;
                }
            }
        }

        private async void ButtonVote_Click(object sender, RoutedEventArgs e)
        {
            if(Voter != null)
            {
                await GetBlackList();
                if (ListBoxCandidates.SelectedItem is CandidateDTO candidateDTO)
                {
                    using (var db = new ElectoralСalculatorContext())
                    {
                        if(!db.Results.Any(r => r.VoterId == Voter.Id))
                        {
                            Result newresult = new Result() { Name = candidateDTO.Name, Party = candidateDTO.Party, VoterId = Voter.Id, ValidVote = true, VotingRights = CheckAge() && !pesels.Contains(Voter.Pesel) };
                            db.Results.Add(newresult);
                            db.SaveChanges();
                        }
                    }
                }
                else
                {
                    using (var db = new ElectoralСalculatorContext())
                    {
                        if (!db.Results.Any(r => r.VoterId == Voter.Id))
                        {
                            Result newresult = new Result() { Name = string.Empty, Party = string.Empty, VoterId = Voter.Id, ValidVote = false, VotingRights = CheckAge() && !pesels.Contains(Voter.Pesel) };
                            db.Results.Add(newresult);
                            db.SaveChanges();
                        }
                    }
                }
                GetResults();
                GridCandidates.Visibility = Visibility.Collapsed;
                GridResultsNumber.Visibility = Visibility.Visible;
                GridResultsChart.Visibility = Visibility.Collapsed;
                GridResultsChart.Visibility = Visibility.Collapsed;

            }
        }

        private bool CheckAge()
        {
            return Voter.BirthDate.AddYears(18).Date < DateTime.Now;
        }

        private void GetResults()
        {
            using (var db = new ElectoralСalculatorContext())
            {
                var candidatesResult = db.Results.Where(r => r.Name != string.Empty).GroupBy(r => r.Name).Select(r => new { r.Key, Count = r.Count() }).ToDictionary(r => r.Key, r => r.Count);
                var partiesResult = db.Results.Where(r => r.Party != string.Empty).GroupBy(r => r.Party).Select(r => new { r.Key, Count = r.Count() }).ToDictionary(r => r.Key, r => r.Count);
                var attemptsResult = db.Results.Where(r => r.VotingRights == false).Count();

                var cr = candidates.Select(r => r.Name + " - " + (candidatesResult.Keys.Contains(r.Name)? candidatesResult[r.Name]: 0)).ToList();
                var pr = candidates.Select(r => r.Party + " - " + (partiesResult.Keys.Contains(r.Party)? partiesResult[r.Party] : 0)).Distinct().ToList();

                ListBoxResultsCandidates.DataContext = cr;
                ListBoxResultsParties.DataContext = pr;
                LabelResultsAttempts.Content = "Number of attempts to vote by people without voting rights: " + attemptsResult;

                var crc = candidates.Select(r => new { r.Name, Votes = (candidatesResult.Keys.Contains(r.Name) ? candidatesResult[r.Name] : 0) }).ToList();
                crc.Reverse();
                var prc = candidates.Select(r => new { r.Party, Votes = (partiesResult.Keys.Contains(r.Party) ? partiesResult[r.Party] : 0) }).Distinct().ToList();
                prc.Reverse();

                ListBoxResultsCandidatesChart.DataContext = crc;
                ListBoxResultsCandidatesChart.Height = crc.Count * 25 + 100;
                ListBoxResultsPartiesChart.DataContext = prc;
                ListBoxResultsPartiesChart.Height = prc.Count * 25 + 100;
                LabelResultsAttemptsChart.Content = "Number of attempts to vote by people without voting rights: " + attemptsResult;
            }
        }

        private void ButtonCharsHide_Click(object sender, RoutedEventArgs e)
        {
            GridResultsNumber.Visibility = Visibility.Visible;
            GridResultsChart.Visibility = Visibility.Collapsed;
        }

        private void ButtonCharsShow_Click(object sender, RoutedEventArgs e)
        {
            GridResultsNumber.Visibility = Visibility.Collapsed;
            GridResultsChart.Visibility = Visibility.Visible;
        }

        private void ButtonExit_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void ButtonCSV_Click(object sender, RoutedEventArgs e)
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog()
            {
                FileName = "Results",
                DefaultExt = "csv",
                AddExtension = true,
                OverwritePrompt = true,
                Filter = "CSV files (*.csv)|*.csv"
            };
            if (saveFileDialog.ShowDialog() == true)
            {
                if (saveFileDialog.OverwritePrompt == true)
                {
                    if (File.Exists(saveFileDialog.FileName))
                    {
                        File.Delete(saveFileDialog.FileName);
                    }
                }
                SaveToCSV(saveFileDialog.FileName);
            }
        }

        private void SaveToCSV(string fileName)
        {
            using (var db = new ElectoralСalculatorContext())
            {
                var candidatesResult = db.Results.Where(r => r.Name != string.Empty).GroupBy(r => r.Name).Select(r => new { r.Key, Count = r.Count() }).ToDictionary(r => r.Key, r => r.Count);
                var partiesResult = db.Results.Where(r => r.Party != string.Empty).GroupBy(r => r.Party).Select(r => new { r.Key, Count = r.Count() }).ToDictionary(r => r.Key, r => r.Count);
                var attemptsResult = db.Results.Where(r => r.VotingRights == false).Count();

                var cr = candidates.Select(r => new { r.Name, Votes = (candidatesResult.Keys.Contains(r.Name) ? candidatesResult[r.Name] : 0) }).ToList();
                var pr = candidates.Select(r => new { r.Party, Votes = (partiesResult.Keys.Contains(r.Party) ? partiesResult[r.Party] : 0) }).Distinct().ToList();                               

                using (var sw = new StreamWriter(fileName))
                {
                    var writer = new CsvWriter(sw);

                    writer.WriteRecords(cr);
                    writer.WriteField("");
                    writer.WriteField("");
                    writer.NextRecord();
                    writer.WriteField("Party");
                    writer.WriteField("Votes");
                    writer.NextRecord();

                    writer.WriteRecords(pr);
                    writer.WriteField("");
                    writer.WriteField("");
                    writer.NextRecord();

                    var attempts = new { Name = "Number of attempts to vote by people without voting rights", Votes = attemptsResult };
                    //writer.WriteRecord(attempts);
                    writer.WriteField(attempts.Name);
                    writer.WriteField(attempts.Votes);
                    writer.NextRecord();
                }
            }
        }

        private void ButtonPDF_Click(object sender, RoutedEventArgs e)
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog()
            {
                FileName = "Results",
                DefaultExt = "pdf",
                AddExtension = true,
                OverwritePrompt = true,
                Filter = "PDF files (*.pdf)|*.pdf"
            };
            if (saveFileDialog.ShowDialog() == true)
            {
                if (saveFileDialog.OverwritePrompt == true)
                {
                    if (File.Exists(saveFileDialog.FileName))
                    {
                        File.Delete(saveFileDialog.FileName);
                    }
                }
                SaveToPDF(saveFileDialog.FileName);
            }
        }

        private void SaveToPDF(string fileName)
        {
            using (var db = new ElectoralСalculatorContext())
            {
                var candidatesResult = db.Results.Where(r => r.Name != string.Empty).GroupBy(r => r.Name).Select(r => new { r.Key, Count = r.Count() }).ToDictionary(r => r.Key, r => r.Count);
                var partiesResult = db.Results.Where(r => r.Party != string.Empty).GroupBy(r => r.Party).Select(r => new { r.Key, Count = r.Count() }).ToDictionary(r => r.Key, r => r.Count);
                var attemptsResult = db.Results.Where(r => r.VotingRights == false).Count();

                var cr = candidates.Select(r => new { r.Name, Votes = (candidatesResult.Keys.Contains(r.Name) ? candidatesResult[r.Name] : 0) }).ToList();
                var pr = candidates.Select(r => new { r.Party, Votes = (partiesResult.Keys.Contains(r.Party) ? partiesResult[r.Party] : 0) }).Distinct().ToList();

                FileStream fs = new FileStream(fileName, FileMode.Create, FileAccess.Write, FileShare.None);

                Document doc = new Document();
                PdfWriter writer = PdfWriter.GetInstance(doc, fs);
                doc.Open();

                PdfPTable table = new PdfPTable(2);

                table.AddCell("Name");
                table.AddCell("Votes");
                foreach (var record in cr)
                {
                    table.AddCell(record.Name);
                    table.AddCell(record.Votes.ToString());
                }

                table.AddCell(" ");
                table.AddCell(" ");

                table.AddCell("Party");
                table.AddCell("Votes");
                foreach (var record in pr)
                {
                    table.AddCell(record.Party);
                    table.AddCell(record.Votes.ToString());
                }

                table.AddCell(" ");
                table.AddCell(" ");
                table.AddCell("Number of attempts to vote by people without voting rights");
                table.AddCell(attemptsResult.ToString());

                doc.Add(table);
                doc.Close();
            }
        }
    }
}
