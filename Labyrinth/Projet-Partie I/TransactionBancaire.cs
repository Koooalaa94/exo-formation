using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Projet_Partie_I
{
    public class TransactionBancaire
    {
        public int RetraitMax = 1000;
        public int NombreRetraitMax = 9;
        public List<int> transaction;
        public Dictionary<int, decimal> compte;

        public TransactionBancaire(string acctPath, string trxnPath)
        {

            // Lecture du fichier d'entré : transaction.
            string[] transactionLines = File.ReadAllLines(trxnPath);

            // Ecriture du fichier dans un dictionnaire.

            transaction = new List<int>();


            for (int i = 0; i < transactionLines.Length; i++)
            {
                string[] split = transactionLines[i].Split(';');
                int numeroTrans = int.Parse(split[0]);
                int montant = int.Parse(split[1].Replace('.', ','));
                int compteExpediteur = int.Parse(split[2]);
                int compteDestinataire = int.Parse(split[3]);

                //Ajout de chaque champs dans la liste du dictionnaire
                //if (!transaction.ContainsKey(numeroTrans))
                //{
                transaction.Add(numeroTrans);
                transaction.Add(montant);
                transaction.Add(compteExpediteur);
                transaction.Add(compteDestinataire);
                //}
            }

            // Lecture du fichier d'entré : compte.
            string[] compteLines = File.ReadAllLines(acctPath);

            // Ecriture du fichier dans un dictionnaire.

            compte = new Dictionary<int, decimal>();
            for (int i = 0; i < compteLines.Length; i++)
            {
                string[] split = compteLines[i].Split(';');
                int numeroCompte = int.Parse(split[0]);
                decimal solde = !string.IsNullOrEmpty(split[1]) ? decimal.Parse(split[1].Replace('.', ',')) : 0;
                if (!compte.ContainsKey(numeroCompte))
                {
                    compte.Add(numeroCompte, solde);
                }
            }

        }


        // Vérification si un dépot est possible.
        public bool DepotEstPossible(int numeroTrans)
        {
            int numeroCompte = (int)transaction[3];
            decimal soldeCompte = decimal.MinValue;

            if (compte.ContainsKey(numeroCompte))
            {
                soldeCompte = compte[numeroCompte];
            }
            if (transaction[1] > 0 && soldeCompte >= 0)
            {
                return true;
            }
            return false;
        }


        // Vérification si un retrait est possible.

        public bool RetraitEstPossible(int numeroTrans)
        {
            int numeroCompte = (int)transaction[2];
            decimal soldeCompte;

            if (compte.ContainsKey(numeroCompte))
            {
                soldeCompte = compte[numeroCompte];
            }
            else
            {
                return false;
            }
            if (transaction[1] > 0 && transaction[1] <= RetraitMax && transaction[1] <= soldeCompte)
            {
                return true;
            }
            return false;
        }

        // Vérification du retrait maximum pour un virement.
         
        public bool RetraitMaxCumule(int numeroCompte)
        {
            int compteurRetrait = 0;
            decimal cumulRetrait = 0;

            for (int i = 0; i < compte.Count; i++)
            {

            }
        }

        // Vérification si un virement est possible.
        public bool VirementEstPossible(int numeroTrans)
        {
            return RetraitEstPossible(numeroTrans);
        }


        // Vérification si un prélèvement est possible.

        public bool PrelevementEstPossible(int numeroTrans)
        {
            return RetraitEstPossible(numeroTrans);
        }

        // Test du statut de la transaction et ecriture du fichier de sortie.
        public void StatutTransaction(int numeroTrans , string sttsPath)
        {
            using (StreamWriter writer = new StreamWriter(sttsPath))
                foreach (var item in transaction)
                {

                    int numeroCompteExp = (int)transaction[2];
                    int numeroCompteDesti = (int)transaction[3];
                    decimal soldeCompteExp = decimal.MinValue;
                    decimal soldeCompteDesti = decimal.MinValue;
                    if (compte.ContainsKey(numeroCompteExp))
                    {
                        soldeCompteExp = compte[numeroCompteExp];
                    }
                    if (compte.ContainsKey(numeroCompteDesti))
                    {
                        soldeCompteDesti = compte[numeroCompteDesti];
                    }
                    // Vérification pour un dépôt.

                    if (transaction[2] == 0 && transaction[3] != 0)
                    {
                        if (!DepotEstPossible(numeroTrans))
                        {
                            writer.WriteLine($"{numeroTrans};KO");
                        }
                        else
                        {
                            writer.WriteLine($"{numeroTrans};OK");
                            soldeCompteDesti += transaction[1];
                        }
                    }

                    // Vérification pour un retrait.
                    if (transaction[2] != 0 && transaction[3] == 0)
                    {
                        if (!RetraitEstPossible(numeroTrans))
                        {
                            writer.WriteLine($"{numeroTrans};KO");
                        }
                        else
                        {
                            writer.WriteLine($"{numeroTrans};OK");
                            soldeCompteExp -= transaction[1];
                        }

                    }

                    // Vérification pour un virement/prélèvement.

                    if (transaction[2] != 0 && transaction[3] != 0)
                    {
                        if (DepotEstPossible(numeroTrans) && RetraitEstPossible(numeroTrans))
                        {
                            writer.WriteLine($"{numeroTrans};OK");
                            soldeCompteDesti += transaction[1];
                            soldeCompteExp -= transaction[1];
                        }
                        else
                        {
                            writer.WriteLine($"{numeroTrans};KO");
                        }
                    }
                    if (transaction[2] == 0 && transaction[3] == 0)
                    {
                        writer.WriteLine($"{numeroTrans};KO");
                    }
                }
        }
    }



}
