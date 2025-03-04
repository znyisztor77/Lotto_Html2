﻿using System;
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
            Console.Write("Luxor(l) vagy ötös lottó?(o):");
            string fomenu = Console.ReadLine().ToLower();

            WebClient webClient = new WebClient();
            string page_lotto = webClient.DownloadString("https://bet.szerencsejatek.hu/cmsfiles/otos.html");
            string page_luxor = webClient.DownloadString("https://bet.szerencsejatek.hu/cmsfiles/luxor.html");
            HtmlAgilityPack.HtmlDocument doc = new HtmlAgilityPack.HtmlDocument();
            bool kapcsolo = false;
            switch (fomenu)
            {
                case "l":
                    doc.LoadHtml(page_luxor);
                    Console.WriteLine("Luxor kiválasztva.");
                    break;

                case "o":
                    doc.LoadHtml(page_lotto);
                    Console.WriteLine("Ötös lottó kiválasztva.");
                    kapcsolo = true;
                    break;

                default:
                    Console.WriteLine("Nincs ilyen menüpont");
                    break;
            }
            //doc.LoadHtml(page);

            List<List<string>> lotto = doc.DocumentNode.SelectSingleNode("//table")
                            .Descendants("tr")
                            .Skip(1)
                            .Where(tr => tr.Elements("td").Count() > 1)
                            .Select(tr => tr.Elements("td").Select(td => td.InnerText.Trim()).ToList())
                            .ToList();
            if (kapcsolo==true)
            {
                Console.WriteLine($"Ennyi lotto sorsolás volt eddig: {lotto.Count}");
                Console.WriteLine($"Az legutolsó sorsolás számai: Dátum:{lotto[0][2]} {lotto[0][11]},{lotto[0][12]},{lotto[0][13]},{lotto[0][14]},{lotto[0][15]}");

            }
            else
            {
                Console.WriteLine($"Ennyi luxor sorsolás volt eddig: {lotto.Count}");

                int[] luxor = new int[51];
                int cc = 7;
                Console.WriteLine($"Az legutolsó sorsolás dátum:{lotto[0][2]}");
                for (int i = 0; i < luxor.Length; i++)
                {
                    //Console.WriteLine($"{lotto[0][cc]}");
                    if (int.TryParse(lotto[0][cc], out int a))
                    {
                        luxor[i] = a;
                        cc++;
                    }
                }

                Console.WriteLine("Az utolsó luxor számok:\n");
                for (int i = 0; i < luxor.Length; i++)
                {
                    if (luxor[i]>0 && luxor[i]<=15)
                    {
                        Console.Write("L "+luxor[i]+",");
                    }
                    if (luxor[i] >= 16 && luxor[i] <= 30)
                    {
                        Console.Write("U " + luxor[i]+",");
                    }
                    if (luxor[i] >= 31 && luxor[i] <= 45)
                    {
                        Console.Write("X " + luxor[i]+",");
                    }
                    if (luxor[i] >= 46 && luxor[i] <= 60)
                    {
                        Console.Write("O " + luxor[i]+",");
                    }
                    if (luxor[i] >= 61 && luxor[i] <= 75)
                    {
                        Console.Write("R " + luxor[i]+",");
                    }

                }
                Console.WriteLine("");
                Array.Sort(luxor);
                int db = 0;
                Console.WriteLine("\nEmelkedő sorrendben:\n");
                for (int i = 0; i < luxor.Length; i++)
                {
                    if (luxor[i]>0)
                    {

                        Console.Write(luxor[i]+",");
                        db++;
                    }
                }
                Console.WriteLine("");
                Console.WriteLine($"\n{db} db számot húztak ki\n");
                Console.WriteLine("Kilépéshez nyomj meg egy billentyűt.");
                Console.ReadKey();
                Environment.Exit(0);
            }

            Console.WriteLine("\nTovábbi műveletekhez nyomj meg egy billentyűt!");

            Console.ReadKey();

            Menu(lotto);

        }


        private static void Menu(List<List<string>> lotto)
        {
            while (true)
            {
                Console.WriteLine("Ötös lottó.\n");
                Console.WriteLine("Menü: \nVálassz!\n");
                Console.WriteLine("| Keresés?(i) |\n| Eddig kisorsolt sorozatok(e) | \n| Kilépés(k) |\n| Utolsó sorsolás számai(u) |");
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
                        Console.WriteLine($"Az legutolsó sorsolás számai: Dátum:{lotto[0][2]} {lotto[0][11]},{lotto[0][12]},{lotto[0][13]},{lotto[0][14]},{lotto[0][15]}");
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
            Console.WriteLine("Számok ellenörzése.");
            Console.WriteLine("\nKérem az öt számot 1-90: ");
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
