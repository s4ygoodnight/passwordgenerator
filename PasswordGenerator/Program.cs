using CommandLine;
using System;
using System.Collections.Generic;
using System.Text;

namespace PasswordGenerator
{
    class Program
    {
        static void Main (string [] args)
        {
            Password pass = new Password ();
            Parser.Default.ParseArguments<Options> (args)
                  .WithParsed<Options> (o => {
                      if (o.Length != null) {
                          pass.lenght = Convert.ToInt32 (o.Length);
                      }

                      if (o.Type != null) {
                          switch (o.Type) {
                          case "simple": {
                                  pass.TypeOfPassword = new SimplePassword ();
                                  break;
                              }
                          case "medium": {
                                  pass.TypeOfPassword = new MediumPassword ();
                                  break;
                              }
                          case "strong": {
                                  pass.TypeOfPassword = new HardPassword ();
                                  break;
                              }
                          default:
                              Console.WriteLine ("Not found your type. We use Simple Password");
                              break;
                          }
                      }
                  });

            Console.WriteLine (pass.GeneratePassword ());
            Console.ReadLine ();
        }
    }

    public class Options
    {
        [Option ('l', "length", Required = false,
          HelpText = "Set length of password.")]
        public string Length { get; set; }

        [Option ('t', "type", Required = false,
         HelpText = "Set length of password.")]
        public string Type { get; set; }
    }

    public static class CommonMethods
    {
        public static string AlphabetAZ ()
        {
            string tmp = string.Empty;
            char nchar;
            for (int i = 65; i < 91; i++) {
                nchar = (char) i;
                tmp += Convert.ToString (nchar);
            }
            return tmp;
        }

        public static string Alphabetaz ()
        {
            string tmp = string.Empty;
            char nchar;
            for (int i = 97; i < 123; i++) {
                nchar = (char) i;
                tmp += Convert.ToString (nchar);
            }
            return tmp;
        }
    }
    interface IPassword
    {
        string GeneratePassword (int lenght);
    }

    class SimplePassword : IPassword
    {
        public string GeneratePassword (int lenght)
        {
            string numbers = "0123456789";
            string setOfSymbols = string.Empty;
            setOfSymbols += CommonMethods.AlphabetAZ ();
            setOfSymbols += CommonMethods.Alphabetaz ();

            string pass = "";
            Random mran = new Random ();
            for (int i = 0; i < lenght; i++) {
                int index = Convert.ToUInt16 (mran.NextDouble () * setOfSymbols.Length) % setOfSymbols.Length;
                char ScharS = setOfSymbols [index];
                pass += Convert.ToString (ScharS);
            }

            StringBuilder builder = new StringBuilder (pass);
            builder [mran.Next (0, pass.Length)] = numbers [mran.Next (0, numbers.Length)];
            pass = builder.ToString ();

            return pass;
        }
    }

    class MediumPassword : IPassword
    {
        public string GeneratePassword (int lenght)
        {
            string numbers = "0123456789";
            string specialSymbols = "-_!@#$~}";
            string setOfSymbols = string.Empty;
            setOfSymbols += CommonMethods.AlphabetAZ ();
            setOfSymbols += CommonMethods.Alphabetaz ();

            string pass = "";
            Random mran = new Random ();
            for (int i = 0; i < lenght; i++) {
                int index = Convert.ToUInt16 (mran.NextDouble () * setOfSymbols.Length) % setOfSymbols.Length;
                char ScharS = setOfSymbols [index];
                pass += Convert.ToString (ScharS);
            }

            StringBuilder builder = new StringBuilder (pass);
            builder [mran.Next (0, pass.Length)] = numbers [mran.Next (0, numbers.Length)];
            builder [mran.Next (0, pass.Length)] = specialSymbols [mran.Next (0, specialSymbols.Length)];
            pass = builder.ToString ();
            return pass;
        }
    }

    class HardPassword : IPassword
    {
        public string GeneratePassword (int lenght)
        {
            string numbers = "0123456789";
            string specialSymbols = "!@#$%^&*()~}";
            string setOfSymbols = string.Empty;
            setOfSymbols += CommonMethods.AlphabetAZ ();
            setOfSymbols += CommonMethods.Alphabetaz ();

            string pass = "";
            Random mran = new Random ();
            for (int i = 0; i < lenght; i++) {
                int index = Convert.ToUInt16 (mran.NextDouble () * setOfSymbols.Length) % setOfSymbols.Length;
                char ScharS = setOfSymbols [index];
                pass += Convert.ToString (ScharS);
            }

            StringBuilder builder = new StringBuilder (pass);
            builder [mran.Next (0, pass.Length)] = numbers [mran.Next (0, numbers.Length)];
            pass = builder.ToString ();

            List<int> ind = new List<int> ();
            while (ind.Count < 4) {
                int rndIndex = mran.Next (0, pass.Length);
                if (ind.Contains (rndIndex))
                    continue;

                builder = new StringBuilder (pass);
                builder [mran.Next (0, pass.Length)] = specialSymbols [mran.Next (0, specialSymbols.Length)];
                pass = builder.ToString ();
                ind.Add (rndIndex);
            }

            return pass;
        }
    }

    class Password
    {
        public int lenght;
        public IPassword TypeOfPassword { private get; set; }

        public Password ()
        {
            lenght = 8;
            TypeOfPassword = new SimplePassword ();
        }

        public Password (int _lenght)
        {
            lenght = _lenght;
            TypeOfPassword = new SimplePassword ();
        }

        public Password (int lenght, IPassword _TypeOfPassword)
        {
            this.lenght = lenght;
            TypeOfPassword = _TypeOfPassword;
        }

        public string GeneratePassword ()
        {
            if (TypeOfPassword.GetType () == typeof (HardPassword) && lenght < 10) {
                lenght = 10;
            }
            return TypeOfPassword.GeneratePassword (lenght);
        }
    }
}
