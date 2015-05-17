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
        public bool Vyhovujuce { get; private set; }
        public bool Muz { get; private set; }
        public DateTime DatumNarodenia { get; private set; }

        public RodneCislo1(string rodneCislo)
        {
            RodneCislo = rodneCislo;
            if (String.IsNullOrEmpty(rodneCislo))
            {
                Console.WriteLine("daco nedobre");
                return;
            }
            Regex rx = new Regex(@"([0-9]{2})([0,1][0-9]|[5,6][0-9])(?<day>[0-3][0-9])/[0-9]{3,4}");
            Match match = rx.Match(rodneCislo);
            if (match.Success)
            {
                string[] rc = rodneCislo.Split('/');
                int s = Convert.ToInt32(rc[0]) % 10000;
                if (!OverRodneCislo(rc))
                {
                    Console.WriteLine("Rodne cislo nie je delitelne 11");
                    return;
                }
                Muz = MuzskeRodneCislo(s);
                DatumNarodenia = ZistiDatumNarodenia(rc);
                Vyhovujuce = true;
            }
            else
            {
                Console.WriteLine("Zadane rodne cislo ma zly tvar");
                Vyhovujuce = false;
            }
        }

        DateTime ZistiDatumNarodenia(string[] rc)
        {
            StringBuilder dateString = new StringBuilder();
            if ((Convert.ToInt32(rc[0][0]) - 48) > 2)
            {
                dateString.Append("19");
            }
            else
            {
                dateString.Append("20");
            }
            dateString.Append(rc[0][0]);
            dateString.Append(rc[0][1]);
            if (Muz)
                dateString.Append(rc[0][2]);
            else
                dateString.Append((Convert.ToInt32(rc[0][2]) - 48 - 5));
            for (int i = 3; i < 6; i++)
                dateString.Append(rc[0][i]);
            DateTime result = new DateTime();
            string format = "yyyyMMdd";
            CultureInfo provider = CultureInfo.InvariantCulture;
            try
            {
                result = DateTime.ParseExact(dateString.ToString(),
                    format,
                    provider
                    );
            }
            catch (FormatException)
            {
                Console.WriteLine("zly datum v rodnom cisle, neplatne rodne cislo");
            }
            return result;
        }

        bool MuzskeRodneCislo(int s)
        {
            if (s > 1200)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        bool OverRodneCislo(string[] rc)
        {
            long sum = 0;
            if (rc[1].Length == 4)
            {
                sum += Convert.ToInt32(rc[0]);
                sum *= 10000;
                sum += Convert.ToInt32(rc[1]);
            }
            else
            {
                sum += Convert.ToInt32(rc[0]);
                sum *= 1000;
                sum += Convert.ToInt32(rc[1]);
            }

            if (sum % 11 != 0)
            {
                return false;
            }
            return true;
        }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(RodneCislo + "\n");
            sb.Append("Je Vyhovujuce: " + Vyhovujuce + "\n");
            if (Vyhovujuce)
            {
                sb.Append("Datum narodenia: " + DatumNarodenia + "\n");
                sb.Append("Pohlavie: ");
                if (Muz)
                    sb.Append("muzske\n");
                else
                    sb.Append("zenske\n");
            }
            return sb.ToString();
        }
    }

    class EmailCheck
    {
        public string Email { get; set; }
        bool at = false;
        int length = 0;
        public string University { get; private set; }
        public bool GoodFormat { get; private set; }
        public string Name { get; set; }
        public string Surname { get; set; }

        public EmailCheck(string email)
        {
            try
            {

                Email = email;
                at = CheckAt();
                if (!at)
                {
                    Console.WriteLine("zla adresa");
                    return;
                }
                University = GetUniversity();
                switch (University)
                {
                    case "tuke.sk":
                        GetTukeEmail();
                        break;
                    case "upjs.sk":
                        GetUpjsEmail();
                        break;
                }
            }
            catch (Exception)
            {
                Console.WriteLine("daco nedobre");
            }
        }

        private void GetUpjsEmail()
        {
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < Email.Length - 8; i++)
            {
                sb.Append(Email[i]);
            }
            length = GetNumberLength(sb.ToString());
            Surname = CheckUpjsSurame(sb.ToString(), length);
            CheckUpjs();
        }

        private void CheckUpjs()
        {
            if (length > 5 || length < 2 || Surname.Length < 3 || Surname.Length > 20 || !at)
                return;
            GoodFormat = true;
        }

        private string CheckUpjsSurame(string p, int length)
        {
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < p.Length - length; i++)
            {
                if (!Char.IsLetter(p[i]))
                    return null;
                sb.Append(p[i]);
            }
            return sb.ToString();
        }

        private int GetNumberLength(string p)
        {
            int result = 0;
            for (int i = 1; i < p.Length; i++)
            {
                if (Char.IsDigit(p[p.Length - i]))
                {
                    result++;
                }
                else
                    return result;
            }
            return result;
        }

        private bool CheckName(string name)
        {
            for (int i = 0; i < name.Length; i++)
            {
                if (!Char.IsLetter(name[i]))
                {
                    return false;
                }
            }
            return true;
        }

        private string[] Divide(string p, int j)
        {
            string[] result = new string[j];
            int num = 0;
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < p.Length; i++)
            {
                if (p[i].Equals('.'))
                {
                    result[num] = sb.ToString();
                    num++;
                    sb.Clear();
                }
                else
                {
                    sb.Append(p[i]);
                }
            }
            result[num] = sb.ToString();
            return result;
        }

        private bool CheckDot(string email, int number)
        {
            int sum = 0;
            for (int i = 0; i < email.Length; i++)
            {
                if (email[i].Equals('.'))
                    sum++;
            }
            return sum == number ? true : false;
        }

        private void GetTukeEmail()
        {
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < Email.Length - 8; i++)
            {
                sb.Append(Email[i]);
            }
            if (!CheckDot(sb.ToString(), 2))
            {
                return;
            }
            string[] names = Divide(sb.ToString(), 3);

            if (CheckName(names[0]) && CheckName(names[1]) && CheckNumber(names[2]))
            {
                Name = names[0];
                Surname = names[1];
                CheckTuke();
            }
        }

        private void CheckTuke()
        {
            if (Name.Length > 20 || Name.Length < 3 ||
                Surname.Length > 20 || Surname.Length < 3 || !at)
                return;
            GoodFormat = true;
        }

        private bool CheckNumber(string p)
        {
            if (p.Length == 1 && Char.IsDigit(p[0]))
                return true;
            else
                return false;
        }

        private bool CheckAt()
        {
            int sum = 0;
            for (int i = 0; i < Email.Length; i++)
            {
                if (Email[i].Equals('@'))
                    sum++;
            }
            return sum == 1 ? true : false;
        }

        string GetUniversity()
        {
            if (Email.Length < 8)
                return "unknown";
            if (Email[Email.Length - 7].Equals('t') && CheckUniversity("@tuke.sk"))
                return "tuke.sk";
            else if (Email[Email.Length - 7].Equals('u') && CheckUniversity("@upjs.sk"))
                return "upjs.sk";
            else
                return "unknown";
        }

        bool CheckUniversity(string university)
        {
            for (int i = 0; i < university.Length; i++)
            {
                if (!Email[Email.Length - 8 + i].Equals(university[i]))
                    return false;
            }
            return true;
        }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("Zadany email: " + Email + "\n");
            sb.Append("Je format v poriadku: " + GoodFormat + "\n");
            if (GoodFormat)
            {
                sb.Append("Zavinac v poriadku: " + at + "\n");
                sb.Append("Univerzita: " + University + "\n");
                sb.Append("Meno: " + Name + "\n");
                sb.Append("Priezvisko: " + Surname + "\n");
            }
            return sb.ToString();
        }
    }

    class EmailCheckRx
    {
        public string Email { get; set; }
        public EmailCheckRx(string email)
        {
            Console.WriteLine(email);
            Regex rx = new Regex(@"^([a-zA-Z]{3,20}.[a-zA-Z]{3,20}.[0-9]@tuke.sk|[a-zA-Z]{3,20}[0-9]{2,5}@upjs.sk)$");
            Match match = rx.Match(email);
            if (match.Success)
            {
                Console.WriteLine("Kvalitny email");
            }
            else
            {
                Console.WriteLine("nekvalitny email");
            }
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            //RodneCislo1 rc = new RodneCislo1("930225/6524");
            //Console.WriteLine(rc);
            //RodneCislo1 rc1 = new RodneCislo1("910614/8898");
            //Console.WriteLine(rc1);
            //RodneCislo1 rc2 = new RodneCislo1("645326/7403");
            //Console.WriteLine(rc2);
            //RodneCislo1 rc3 = new RodneCislo1("930225/659");
            //Console.WriteLine(rc3);
            //RodneCislo1 rc4 = new RodneCislo1("130225/656");
            //Console.WriteLine(rc4);
            //RodneCislo1 rc5 = new RodneCislo1("030229/650");
            //Console.WriteLine(rc5);

            //EmailCheck ec = new EmailCheck("");
            //Console.WriteLine(ec);
            //ec = new EmailCheck("jaro.poru.5@tuke.sk");
            //Console.WriteLine(ec);
            //ec = new EmailCheck("jaro.poru@@tuke.sk");
            //Console.WriteLine(ec);
            //ec = new EmailCheck("jaro.poru@1@tuke.sk");
            //Console.WriteLine(ec);
            //ec = new EmailCheck("jaro.poru...1@tuke.sk");

            //EmailCheckRx ec = new EmailCheckRx("konecny25@upjs.sk");
            //ec = new EmailCheckRx("aaa.aaa.1@tuke.sk");



            Console.Read();
        }
    }
}
