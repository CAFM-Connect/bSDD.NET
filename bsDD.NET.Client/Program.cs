using System;
using System.Collections.Generic;
using System.Linq;
using bsDD.NET.Model.Objects;


namespace bsDD.NET.Client
{
    class Program
    {
        static void Main(string[] args)
        {

            Console.WriteLine("bsDD.NET started");
            Console.WriteLine("bsDD.NET is s simple example to connect to buildingSMART Data Dictionary via C#");
            Console.WriteLine("For further information look at http://bsdd.buildingsmart.org/docs/");
            Console.WriteLine("Now, please enter your bsDD Credentials");
            Console.Write("eMail:");
            string email = Console.ReadLine();
            Console.Write("Password:");
            string password = ReadPassword('*');

            //Debug, only to debug fast, no need to put in your credentials every time in the console window
            //email = "Put here your bsDD-eMail";
            //password = "Put here your bsDD-password";
            //Debug

            if ((email.Length>1) && (password.Length>1)) //TODO Check Exceptions on empty or invalid credentials
            { 
                bsdd _bsdd = new bsdd(email, password);
                Console.WriteLine("Connected to bsDD: "+_bsdd.BaseUrl);
                Console.WriteLine("Session: " + _bsdd.Session.Guid);
                Console.WriteLine("User: " + _bsdd.Session.User.Name+ " ("+_bsdd.Session.User.PreferredOrganization+")");
                Console.WriteLine("Group: " + _bsdd.Session.User.MemberOf.Name);
                Console.WriteLine("Role: " + _bsdd.Session.User.Role);
                Console.WriteLine("+++++++++++++++++++++++++++++++++++++++++++++");
                string mode;
                do
                {
                    Console.WriteLine("What do you want to do?");
                    Console.WriteLine("Press sn to search for names");
                    Console.WriteLine("Press ex to exit");
                    mode = Console.ReadLine();
                    switch (mode)
                    {
                        case "sn":
                            Console.WriteLine("Now, you can search in the dictionary");
                            Console.Write("Please enter a search phrase for a name: ");
                            string searchstring = Console.ReadLine();
                            IfdNameList Names = _bsdd.SearchNames(searchstring);
                            if (Names== null)
                                Console.WriteLine("no result could be found in this search, please try another name");
                            else
                            {
                                Console.WriteLine(Names.IfdName.Count.ToString()+ " result(s) found");
                                Console.WriteLine("++++++++");
                                foreach (IfdName name in Names.IfdName)
                                {
                                    Console.WriteLine("IfdName.Guid:" + name.Guid);
                                    Console.WriteLine("IfdName.Language:" + name.Language.LanguageCode+" ()"+ name.Language.NameInSelf);
                                    Console.WriteLine("IfdName.LanguageFamily:" + name.LanguageFamily);
                                    Console.WriteLine("IfdName.NameType:" + name.NameType);
                                    Console.WriteLine("++++++++");
                                }
                            }
                            break;
                        case "ex":
                            Console.Write("Finished, Prese Enter to exit:");
                            break;
                        default:
                            Console.WriteLine("This Mode is not valid, please try again.");
                            break;
                    }
                } while (mode != "ex");

                Console.ReadLine();
            }
        }

        public static string ReadPassword(char mask)
        {
            const int ENTER = 13, BACKSP = 8, CTRLBACKSP = 127;
            int[] FILTERED = { 0, 27, 9, 10 /*, 32 space, if you care */ }; // const

            var pass = new Stack<char>();
            char chr = (char)0;

            while ((chr = System.Console.ReadKey(true).KeyChar) != ENTER)
            {
                if (chr == BACKSP)
                {
                    if (pass.Count > 0)
                    {
                        System.Console.Write("\b \b");
                        pass.Pop();
                    }
                }
                else if (chr == CTRLBACKSP)
                {
                    while (pass.Count > 0)
                    {
                        System.Console.Write("\b \b");
                        pass.Pop();
                    }
                }
                else if (FILTERED.Count(x => chr == x) > 0) { }
                else
                {
                    pass.Push((char)chr);
                    System.Console.Write(mask);
                }
            }

            System.Console.WriteLine();

            return new string(pass.Reverse().ToArray());
        }

    }
}
