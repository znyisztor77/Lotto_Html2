using System;
using HtmlAgilityPack;
using System.Linq;
using System.Collections.Generic;
using System.Net;
using System.ComponentModel.Design;
using System.Threading;
using System.Threading.Tasks;
using System.Text;
using System.IO;




namespace Lotto_Html
{
   
    internal class Program
    {
    
        static void Main(string[] args)
        {
         WebClient webClient = new WebClient();
         string page = webClient.DownloadString("https://bet.szerencsejatek.hu/cmsfiles/otos.html");

         HtmlAgilityPack.HtmlDocument doc = new HtmlAgilityPack.HtmlDocument();
         doc.LoadHtml(page);

         List<List<string>> lotto = doc.DocumentNode.SelectSingleNode("//table")
                            .Descendants("tr")
                            .Skip(1)
                            .Where(tr => tr.Elements("td").Count() > 1)
                            .Select(tr => tr.Elements("td").Select(td => td.InnerText.Trim()).ToList())
                            .ToList();
         Console.WriteLine($"Ennyi sorsolás volt eddig: {lotto.Count}");
         Console.WriteLine("Folytatáshoz nyomj meg egy billentyűt!");

            Console.WriteLine("Szám konvert!");

            List<Szam> lottoszamok = new List<Szam>();
                
            Szam szamok = new Szam();
            foreach (List<string> list in lotto)
            { 
                szamok.Egy = int.Parse(list[0]);
                szamok.Egy = int.Parse(list[12]);
                szamok.Egy = int.Parse(list[13]);
                szamok.Egy = int.Parse(list[14]);
                szamok.Egy = int.Parse(list[15]);

            }
            lottoszamok.Add(szamok);

            Console.WriteLine("Számok lista kiírása.");

            foreach (var item in lottoszamok)
            {
                Console.WriteLine($"{item.Egy},{item.Ketto},{item.Harom},{item.Negy},{item.Ot}");
            }

            Console.WriteLine("Folytatáshoz nyomj meg egy billentyűt!");
         Console.ReadKey();

            Menu(lotto);
         
         }

        struct Szam
        {
            public int Egy;
            public int Ketto;
            public int Harom;
            public int Negy;
            public int Ot;
        }

        private static void Menu(List<List<string>> lotto)
        {
            while (true)
            {
                Console.WriteLine("Menü: \nVálassz!");
                Console.WriteLine("|Keresés?(i)|\n|Eddig kisorsolt sorozatok(e)| \n|Kilépés(k)|\n|Utolsó sorsolás számai(u)|");
                string menu = Console.ReadLine().ToLower();

                switch (menu)
                {
                    case "i":
                        lottoszamKeres(lotto);
                        break;
                    case "k":
                            Console.WriteLine("Kilépés");
                            Thread.Sleep(50);
                            Environment.Exit(0);
                        break;
                    
                    case "e":
                            foreach (List<string> item in lotto)
                            {
                                Console.WriteLine($"{item[11]}, {item[12]}, {item[13]}, {item[14]}, {item[15]}");
                            }
                        break;

                    case "u":
                        Console.WriteLine($"{Convert.ToInt16(lotto[0])}");
                        break;

                    default:
                        Console.WriteLine("Érvénytelen bevitel!");
                        break;
                    }
                }

            }

            private static void lottoszamKeres(List<List<string>> lotto)
            {
                int[] szamok = new int[5];
                Console.WriteLine("Kérem az öt számot 1-90: ");
                for (int i = 0; i < szamok.Length; i++)
                {
                    Console.Write($"{i + 1}. szam: ");
                    szamok[i] = szamBeker();
                }
                Array.Sort(szamok);

                bool b = false;



                foreach (List<string> item in lotto)
                {
                    if (szamok[0] == Convert.ToInt32(item[11]))
                    {

                        if (szamok[1] == Convert.ToInt32(item[12]))
                        {
                            if (szamok[2] == Convert.ToInt32(item[13]))
                            {
                                if (szamok[3] == Convert.ToInt32(item[14]))
                                {
                                    if (szamok[4] == Convert.ToInt32(item[15]))
                                    {
                                        Console.WriteLine("Volt ilyen sorozat kihúzva!");
                                        Console.WriteLine($"A sorsolás dátuma {item[2]} ez a {item[1]}. héten volt.");
                                        b = true;
                                        break;
                                    }

                                }
                            }
                        }
                    }
                }
                if (!b)
                {
                    Console.WriteLine("Nem volt ilyen sorozat!");

                }

            }

            private static int szamBeker()
            {
                int szam;
                while (!int.TryParse(Console.ReadLine(), out szam))
                {
                    Console.WriteLine("Hibás adat!");

                }
                return szam;
            }
        }
    }
