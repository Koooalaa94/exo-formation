using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Projet_Partie_I
{
    public class CompteBancaire
    {
        public int NumeroCompte;
        public decimal Solde;
        public decimal RetraitMax;
        public int NumeroTrans;
        public decimal MontantTrans;
        public bool StatutTrans;

        public static void TransactionBancaires(string input1, string input2, string output)
        {

            // Lecture du fichier d'entré : transaction.
            string[] lines = File.ReadAllLines(input2);

            // Ecriture du fichier dans un dictionnaire.

            Dictionary<decimal, List<decimal>> transaction = new Dictionary<decimal, List<decimal>>();
            List <decimal> Montant = new List<decimal>(2);


            for (int i = 0; i < lines.Length; i++)
            {
                Debug.WriteLine(lines[i]);
                string[] split = lines[i].Split(';');
                foreach (var item in split)
                {
                    Debug.WriteLine(item);
                }
                int numeroTrans = int.Parse(split[1]);
                decimal montant = decimal.Parse(split[2]);
                if (!transaction.ContainsKey(numeroTrans))
                {
                    transaction.Add(numeroTrans, new List<decimal>());
                }
                transaction[numeroTrans].Add(montant);
            }

            // Vérification si un dépot est possible.

            public bool DepotEstPossible(int compte)
            {
                if (transaction.Montant[compte] >= 0)
                {
                    return true;
                }
                return false;
            }


            // Vérification si un retrait est possible.

            public bool RetraitEstPossible()
            {
                if (transation.Montant[2] >=0 && transation.Montant[2] < RetraitMax && transation.Montant[2] < Solde)
                {
                    return true;
                }
                return false;
            }


            // Vérification si un virement est possible.
            public bool VirementEstPossible()
            {
                return RetraitEstPossible;
            }


            // Vérification si un prélèvement est possible.

            public bool PrelevementEstPossible()
            {
                if (DepotEstPossible && VirementEstPossible)
                {
                    return true;
                }
                return false;
            }

            // Appliquer la transaction.






            // Test statut transaction.






            // Ecriture de la transaction dans le fichier de sortie.



        }
    }
}
