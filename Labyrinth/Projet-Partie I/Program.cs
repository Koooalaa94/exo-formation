using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Projet_Partie_I
{
    class Program
    {
        static void Main(string[] args)
        {
            string path = Directory.GetCurrentDirectory();
            string acctPath = path + @"\Comptes_1.txt";
            string trxnPath = path + @"\Transactions_1.txt";
            string sttsPath = path + @"\Statut_1.txt";
            CompteBancaire.TransactionBancaires(acctPath, trxnPath, sttsPath);
        }
    }
}
