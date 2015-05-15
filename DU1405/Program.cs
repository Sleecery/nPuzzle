using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;
using System.Globalization;


namespace DU1405
{
    class RodneCislo1
    {
        public string RodneCislo { get; set; }

        public RodneCislo1(string rodneCislo)
        {
            bool muz = false;
            if (String.IsNullOrEmpty(rodneCislo))
            {
                Console.WriteLine("daco nedobre");
                return;
            }
            Regex rx = new Regex(@"(?<year>[0-9]{2})(?<month>[0,1][0-9]|[5,6][0,9])(?<day>[0-3][0-9])/[0-9]{3,4}");
            Match match = rx.Match(rodneCislo);
            if (match.Success)
            {
                RodneCislo = rodneCislo;
                long sum = 0;
                string[] rc = rodneCislo.Split('/');

                if (rc[1].Length == 4)
                {
                    sum += Convert.ToInt32(rc[0]);
                    sum *= 10000;
                    sum += Convert.ToInt32(rc[1]);
                }
                else
                {
                    sum += Convert.ToInt32(rc[0]) * 1000;
                    sum += Convert.ToInt32(rc[1]);
                }

                if (sum % 11 != 0)
                {
                    Console.WriteLine("Nevyhovujuce rodne cislo, nie je delitelne 11");
                    return;
                }
                int s = Convert.ToInt32(rc[0]) % 10000;

                if (s > 12000)
                {
                    Console.WriteLine("zena");
                }
                else
                {
                    Console.WriteLine("muz");
                    muz = true;
                }
                string dateString;
                if ((Convert.ToInt32(rc[0][0]) - 48) > 2)
                {
                    dateString = "19" + rc[0];
                }
                else
                {
                    dateString = "20" + rc[0];
                }
                Console.WriteLine(dateString);

                DateTime result;
                string format = "yyyyMMdd";
                CultureInfo provider = CultureInfo.InvariantCulture;
                try
                {
                    result = DateTime.ParseExact(dateString,
                        format,
                        provider
                        );
                }
                catch (FormatException)
                {
                    Console.WriteLine("zle datum v rodnom cisle, neplatne rodne cislo");
                    return;
                }
                //DateTime dt = new DateTime(Convert.ToInt32(match.Groups["year"].Value), 
                //    Convert.ToInt32(match.Groups["month"].Value), 
                //    Convert.ToInt32(match.Groups["day"].Value));


                Console.WriteLine("Vyhovujuce rodne cislo");
            }
            else
            {
                Console.WriteLine("Nevyhovujuce rodne cislo");
            }
        }

    }

    class EmailCheck
    {
        public string Email { get; set; }
        bool tuke = false;

        public EmailCheck(string email)
        {
            Email = email;
            //Console.WriteLine(email[email.Length - 7] + " " + email[email.Length - 7].Equals('t'));
            if (email[email.Length - 7].Equals('t') && check("tuke.sk"))
            {
                tuke = true;
            }
            else if (email[email.Length - 7].Equals('u') && check("upjs.sk"))
            {
                tuke = false;
            }
            else
            {
                Console.WriteLine("wrong email");
                return;
            }

            string meno = null;
            string priezvisko = null;
            int poz = 0;
            for (int i = 0; i < email.Length; i++)
            {
                Console.WriteLine(email[i] + " " + email[i].Equals('.'));
                if (email[i].Equals('.') && tuke)
                {
                    if (String.IsNullOrEmpty(meno))
                    {
                        poz = i;
                        StringBuilder sb = new StringBuilder();
                        for (int j = 0; j < i; j++)
                        {
                            sb.Append(email[j]);
                        }
                        meno = sb.ToString();
                        Console.WriteLine("meno: " + meno );
                    }
                    else if (String.IsNullOrEmpty(priezvisko))
                    {
                        StringBuilder sb = new StringBuilder();
                        for (int j = 1; j < i - poz; j++)
                        {
                            sb.Append(email[j + poz]);
                        }
                        priezvisko = sb.ToString();
                        Console.WriteLine("priezvisko: " + priezvisko);
                        poz = i;
                        char next = email[i + 1];
                        char nextOne = email[i + 2];
                        if (Char.IsDigit(next) && nextOne.Equals('@'))
                        {
                            Console.WriteLine(next + " "+ nextOne);
                            //i += 2;
                            if (checkEnd(i+3))
                            {
                                Console.WriteLine("spravny email!");
                                return;
                            }
                        }
                        //i += 2;
                    }
                }
            }
            Console.WriteLine("Nespravny email");

        }

        bool check(string checker)
        {
            //Console.WriteLine(Email + " " + checker);
            for (int i = 0; i < 7; i++)
            {
              //  Console.WriteLine(Email[Email.Length - 7 + i] + " " + checker[i]);
                if (!Email[Email.Length - 7 + i].Equals(checker[i]))
                {
                //    Console.WriteLine("wrong email");
                  //  Console.WriteLine(Email[Email.Length - 7 + i] + " " + checker[i]);
                    return false;
                }
            }
            return true;
        }

        bool checkEnd(int i)
        {
            string checker = "upjs.sk";
            if (tuke)
                checker = "tuke.sk";
            for (int j = 0; j < 7; j++)
            {
                Console.WriteLine(Email[i + j] + " " + checker[j]);
                if (!Email[i + j].Equals(checker[j]))
                {
                    return false;
                }
            }
            return true;
        }
    }


    class Program
    {
        static void Main(string[] args)
        {
            //RodneCislo1 rc = new RodneCislo1("930225/6524");
            //Console.WriteLine(rc.RodneCislo);
            //RodneCislo1 rc1 = new RodneCislo1("910614/8898");
            //Console.WriteLine(rc1.RodneCislo);

            EmailCheck ec = new EmailCheck("jaro.poru.1@tuke.sk");
            ec = new EmailCheck("jaro.poru@tuke.sk");
            ec = new EmailCheck("jaro.poru.1@tuke.sk");
            ec = new EmailCheck("jaro.poru.1@tuke.sk");
            ec = new EmailCheck("jaro.poru.1@tuke.sk");

            Console.Read();
        }
    }
}
