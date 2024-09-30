using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
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

namespace SimpleDBAccess
    {
        public partial class MainWindow : Window
        {

            private string connectionString = @"SERVER=localhost;DATABASE=patenwoche;UID=root;PASSWORD=sqlpassword;";

            public MainWindow()
            {
                InitializeComponent();
            }

            // Event-Handler für den Button-Klick (Spielersuche)
            private void Button_get_data(object sender, RoutedEventArgs e)
            {
                // Rückennummer aus der TextBox abrufen
                if (int.TryParse(RückennummerEingabe.Text, out int rueckennummer))
                {
                    // Spieler in der Datenbank suchen
                    SucheSpielerInDatenbank(rueckennummer);
                }
                else
                {
                    // Fehlermeldung anzeigen, wenn die Eingabe keine Zahl ist
                    MessageBox.Show("Bitte geben Sie eine gültige Rückennummer ein.");
                }
            }

            // Methode, um den Spieler aus der Datenbank zu suchen und anzuzeigen
            private void SucheSpielerInDatenbank(int rueckennummer)
            {
                try
                {
                    // Verbindung zur MySQL-Datenbank herstellen
                    using (MySqlConnection connection = new MySqlConnection(connectionString))
                    {
                        connection.Open(); // Verbindung öffnen

                        // SQL-Abfrage zum Abrufen des Spielers basierend auf der Rückennummer
                        string query = "SELECT `namen`, `altere` FROM `spieler` WHERE `rueckennummer` = @Rueckennummer";
                    using (MySqlCommand cmd = new MySqlCommand(query, connection))
                        {
                            // Parameter für die SQL-Abfrage hinzufügen
                            cmd.Parameters.AddWithValue("@Rueckennummer", rueckennummer);

                            // SQL-Abfrage ausführen und das Ergebnis lesen
                            using (MySqlDataReader reader = cmd.ExecuteReader())
                            {
                                if (reader.Read())
                                {
                                    // Name und Alter des Spielers aus der Datenbank lesen
                                    string name = reader.GetString("namen");
                                    int alter = reader.GetInt32("altere");

                                    // Spielerinformationen in der Ausgabe-TextBox anzeigen
                                    AusgabeSpieler.Text = $"Name: {name}, Alter: {alter}";
                                }
                                else
                                {
                                    // Spieler nicht gefunden, Textfeld leeren oder Fehlermeldung anzeigen
                                    AusgabeSpieler.Text = "Kein Spieler mit dieser Rückennummer gefunden.";
                                }
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    // Fehlerbehandlung, falls bei der Datenbankverbindung oder -abfrage ein Problem auftritt
                    MessageBox.Show($"Fehler: {ex.Message}");
                }
            }
        }
    }