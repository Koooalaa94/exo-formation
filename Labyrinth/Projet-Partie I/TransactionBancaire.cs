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
        public Dictionary<int, List<decimal>> transaction;
        public Dictionary<int, decimal> compte;

        public TransactionBancaire(string acctPath, string trxnPath)
        {

            // Lecture du fichier d'entré : transaction.
            string[] transactionLines = File.ReadAllLines(trxnPath);

            // Ecriture du fichier dans un dictionnaire.

            transaction = new Dictionary<int, List<decimal>>();
            List<decimal> transactionInfo = new List<decimal>();


            for (int i = 0; i < transactionLines.Length; i++)
            {
                string[] split = transactionLines[i].Split(';');
                int numeroTrans = int.Parse(split[0]);
                decimal montant = decimal.Parse(split[1]);
                decimal compteExpediteur = decimal.Parse(split[2]);
                decimal compteDestinataire = decimal.Parse(split[3]);

                //Ajout de chaque champs dans la liste du dictionnaire
                if (!transaction.ContainsKey(numeroTrans))
                {
                    transaction.Add(numeroTrans, new List<decimal>());
                    transaction[numeroTrans].Add(montant);
                    transaction[numeroTrans].Add(compteExpediteur);
                    transaction[numeroTrans].Add(compteDestinataire);
                }
               }

            // Lecture du fichier d'entré : compte.
            string[] compteLines = File.ReadAllLines(acctPath);

            // Ecriture du fichier dans un dictionnaire.

            compte = new Dictionary<int, decimal>();
            for (int i = 0; i < compteLines.Length; i++)
            {
                string[] split = compteLines[i].Split(';');
                int numeroCompte = int.Parse(split[0]);
                decimal solde = !string.IsNullOrEmpty(split[1]) ? decimal.Parse(split[1].Replace('.',',')) : 0;
                if (!compte.ContainsKey(numeroCompte))
                {
                    compte.Add(numeroCompte, solde);
                }
            }

        }


        // Vérification si un dépot est possible.
        public bool DepotEstPossible(int numeroTrans)
        {
            int numeroCompte = (int)transaction[numeroTrans][2];
            decimal soldeCompte = decimal.MinValue;

            if (compte.ContainsKey(numeroCompte))
            {
                soldeCompte = compte[numeroCompte];
            }
            if (transaction[numeroTrans][0] > 0 && soldeCompte >= 0 )
            {
                return true;
            }
            return false;
        }


        // Vérification si un retrait est possible.

        public bool RetraitEstPossible(int numeroTrans)
        {
            int numeroCompte = (int)transaction[numeroTrans][1];
            decimal soldeCompte;

            if (compte.ContainsKey(numeroCompte))
            {
                soldeCompte = compte[numeroCompte];
            }
            else
            {
                return false;
            }
            if (transaction[numeroTrans][0] > 0 && transaction[numeroTrans][0] <= RetraitMax && transaction[numeroTrans][0] <= soldeCompte)
            {
                return true;
            }
            return false;
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
        public void StatutTransaction(string sttsPath)
        {
            using (StreamWriter writer = new StreamWriter(sttsPath))
                foreach (var item in transaction)
                {

                    int numeroTrans = item.Key;
                    int numeroCompteExp = (int)transaction[numeroTrans][1];
                    int numeroCompteDesti = (int)transaction[numeroTrans][2];
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

                    if (transaction[numeroTrans][1] == 0 && transaction[numeroTrans][2] != 0)
                    {
                        if (!DepotEstPossible(numeroTrans))
                        {
                            writer.WriteLine($"{numeroTrans};KO");
                        }
                        else
                        {
                            writer.WriteLine($"{numeroTrans};OK");
                            soldeCompteDesti += transaction[numeroTrans][0];
                        }
                    }

                    // Vérification pour un retrait.
                    if (transaction[numeroTrans][1] != 0 && transaction[numeroTrans][2] == 0)
                    {
                        if (!RetraitEstPossible(numeroTrans))
                        {
                            writer.WriteLine($"{numeroTrans};KO");
                        }
                        else
                        {
                            writer.WriteLine($"{numeroTrans};OK");
                            soldeCompteExp -= transaction[numeroTrans][0];
                        }

                    }

                    // Vérification pour un virement/prélèvement.

                    if (transaction[numeroTrans][1] != 0 && transaction[numeroTrans][2] != 0)
                    {
                        if (DepotEstPossible(numeroTrans) && RetraitEstPossible(numeroTrans))
                        {
                            writer.WriteLine($"{numeroTrans};OK");
                            soldeCompteDesti += transaction[numeroTrans][0];
                            soldeCompteExp -= transaction[numeroTrans][0];
                        }
                        else
                        {
                            writer.WriteLine($"{numeroTrans};KO");
                        }
                    }
                    if (transaction[numeroTrans][1] == 0 && transaction[numeroTrans][2] == 0)
                    {
                        writer.WriteLine($"{numeroTrans};KO");
                    }
                }
        }
    }



}
